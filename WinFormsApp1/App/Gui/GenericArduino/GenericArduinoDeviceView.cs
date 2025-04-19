using System.ComponentModel;
using System.Windows.Forms;
using AFooCockpit.App.Core.DataSource;
using AFooCockpit.App.Core.DataSource.DataSources.Arinc429TranscieverDataSource;
using AFooCockpit.App.Core.DataSource.DataSources.GenericArduino;
using AFooCockpit.App.Core.Device;
using AFooCockpit.App.Core.Settings;
using AFooCockpit.App.Gui.DeviceConfigForms;

namespace AFooCockpit.App.Gui
{
    public partial class GenericArduinoDeviceView : UserControl
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public class DataSourceItem
        {
            public required string DisplayName { get; set; }
            public required GenericArduinoDataSourceConfig SourceConfig { get; set; }
            // We need to keep references between device and datasource by using a GUID,
            // since our settings framework doesn't support restoring object references...
            public required Guid Guid;
        }

        public class DeviceItem
        {
            public required string UniqueDisplayName { get; set; }
            public required string DeviceName { get; set; }

            public required Guid DataSourceGuid;
        }

        private BindingList<DataSourceItem> dataSourceItems = new BindingList<DataSourceItem>();
        private BindingList<DeviceItem> deviceItems = new BindingList<DeviceItem>();

        public GenericArduinoDeviceView()
        {
            InitializeComponent();


            dataSourceItems = Settings.App.GetOrDefault($"{GetType().Name}-datasources", new BindingList<DataSourceItem>());
            deviceItems = Settings.App.GetOrDefault($"{GetType().Name}-devices", new BindingList<DeviceItem>());

            Load += GenericArduinoDeviceView_Load;
        }

        private void GenericArduinoDeviceView_Load(object? sender, EventArgs e)
        {
            InitializeDataSourceList();
            InitializeDeviceList();
        }

        private void InitializeDataSourceList()
        {
            lsbDataSources.ClearSelected();
            lsbDataSources.DisplayMember = "DisplayName";
            lsbDataSources.ValueMember = "SourceConfig";
            lsbDataSources.DataSource = dataSourceItems;
        }

        private void InitializeDeviceList()
        {
            lsbDevices.ClearSelected();
            lsbDevices.DataSource = deviceItems;
            lsbDevices.DisplayMember = "UniqueDisplayName";
        }

        /// <summary>
        /// Returns wether or not the device name already exists
        /// </summary>
        /// <param name="deviceName"></param>
        /// <returns></returns>
        private bool DeviceNameExists(string deviceName)
        {
            return deviceItems.FirstOrDefault(d => d.UniqueDisplayName == deviceName, null) != null;
        }

        /// <summary>
        /// Add a device
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void tsbAddDevice_Click(object sender, EventArgs e)
        {
            if (dataSourceItems.Count == 0)
            {
                MessageBox.Show("Before creating a device you need to create a datasource.");
                return;
            }

            using (var form = new GenericArduinoDeviceConfigView())
            {
                form.DataSourceItems = dataSourceItems;
                var tcs = new TaskCompletionSource();

                form.FormClosed += (s, e) =>
                {
                    if (form.DialogResult == DialogResult.OK)
                    {
                        var device = form.GetSelectedDevice();

                        // Check if the device name already exists. In that case show an error
                        // and return
                        if (DeviceNameExists(device.UniqueDisplayName))
                        {
                            MessageBox.Show(
                                $"Device Name {device.UniqueDisplayName} already exists, cannot create device.",
                                $"Device Name exists",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            return;
                        }

                        deviceItems.Add(device);
                    }
                };

                form.Show(); // non-blocking
                await tcs.Task; // awaits until the form is closed
            }
        }

        /// <summary>
        /// Delete a datasource
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbDeleteDataSource_Click(object sender, EventArgs e)
        {
            var item = lsbDataSources.SelectedItem as DataSourceItem;
            if (item == null)
            {
                logger.Warn("Trying to delete an arduino data source, but no data source selected");
                return;
            }

            //var devices = new List<DeviceItem>();
            //// We also need to delete dependend devices. Here we're getting all the devices
            //// that depend on the to-be-deleted datasource
            var devices = deviceItems.ToList().Where(d => d.DataSourceGuid == item.Guid).ToList();

            var result = MessageBox.Show(
                $"This will delete the datasource {item.DisplayName}, its configuration and any associated devices. Are you sure you want to delete it?\n\n" +
                $"This will also delete {devices.Count()} device(s) that are currently configured for this data source.",
                $"Delete?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                dataSourceItems.Remove(item);
                devices.ForEach((d) => deviceItems.Remove(d));
            }
        }

        /// <summary>
        /// Edit a datasource
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void tsbEditDataSource_Click(object sender, EventArgs e)
        {
            var config = lsbDataSources.SelectedValue as GenericArduinoDataSourceConfig;
            if (config == null)
            {
                logger.Warn("Trying to edit an arduino data source, but no data source selected");
                return;
            }

            try
            {
                var sourceConfig = await DataSourceConfigFactory.CreateDataSourceConfig(config);
                if (sourceConfig == null)
                {
                    logger.Warn("Dialog returned a null configuration");
                    return;
                }

                Settings.App.Taint();
            }
            catch (DataSourceConfigCreationCanceledException)
            {
                logger.Warn("User canceled configuration");
            }
        }

        /// <summary>
        /// Delete a data source
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void tsbAddDataSource_Click(object sender, EventArgs e)
        {
            try
            {
                var sourceConfig = await DataSourceConfigFactory.CreateDataSourceConfig<GenericArduinoDataSourceConfig>();
                if (sourceConfig == null)
                {
                    logger.Warn("Dialog returned a null configuration");
                    return;
                }

                Invoke(() =>
                {
                    lsbDataSources.ClearSelected();
                    var item = new DataSourceItem
                    {
                        DisplayName = $"Data Source {sourceConfig.Port}",
                        SourceConfig = sourceConfig,
                        Guid = Guid.NewGuid()
                    };

                    try
                    {
                        dataSourceItems.Add(item);
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        // Looks like there's a bug in the form designer/control when debugging:
                        // When debugging, it throws an Argument out of Range exception when addint the first item
                        // (see https://stackoverflow.com/questions/36487128/binding-combobox-to-data-source-with-0-items-initially-items-added-later-causes)
                        // You can safely ignore the exception.
                        logger.Error(ex);
                    }

                });

            }
            catch (DataSourceConfigCreationCanceledException)
            {
                logger.Warn("User canceled configuration");
            }
        }

        /// <summary>
        /// Delete a device
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbRemoveDevice_Click(object sender, EventArgs e)
        {
            var device = lsbDevices.SelectedValue as DeviceItem;
            if (device == null)
            {
                logger.Warn("Trying to delete an arduino device, but no device selected");
                return;
            }

            var result = MessageBox.Show(
                $"This will delete the device {device.UniqueDisplayName} and its configuration. Are you sure you want to delete it?",
                $"Delete?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                deviceItems.Remove(device);
            }
        }

        private DataSourceItem? GetDataSourceItem(Guid guid)
        {
            return dataSourceItems.FirstOrDefault(i => i.Guid == guid, null);
        }

        /// <summary>
        /// Creates all configured devices using the given device manager
        /// </summary>
        /// <param name="deviceManager"></param>
        public void CreateDevices(DeviceManager deviceManager)
        {
            var dsHashMap = new Dictionary<Guid, GenericArduinoDataSource>();

            foreach (DeviceItem deviceItem in deviceItems)
            {
                GenericArduinoDataSource? dataSource;
                if (dsHashMap.ContainsKey(deviceItem.DataSourceGuid))
                {
                    dataSource = dsHashMap[deviceItem.DataSourceGuid];
                }
                else
                {
                    var dataSourceItem = GetDataSourceItem(deviceItem.DataSourceGuid);

                    if (dataSourceItem == null)
                    {
                        MessageBox.Show(
                            $"Error while creating devices - device {deviceItem.UniqueDisplayName} datasource could not be found, continuing without it.",
                            $"Error while creating {deviceItem.UniqueDisplayName}",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);

                        continue;
                    }

                    dataSource = new GenericArduinoDataSource(dataSourceItem.SourceConfig);
                    dsHashMap.Add(deviceItem.DataSourceGuid, dataSource);
                }

                deviceManager.CreateDeviceInstance(deviceItem.DeviceName, deviceItem.UniqueDisplayName);
                deviceManager.ConnectDataSource(deviceItem.UniqueDisplayName, dataSource);
            }
        }
    }
}
