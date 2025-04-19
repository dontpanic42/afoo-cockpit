using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Windows.Forms;
using AFooCockpit.App.Core.DataSource;
using AFooCockpit.App.Core.DataSource.DataSources.Arinc429TranscieverDataSource;
using AFooCockpit.App.Core.DataSource.DataSources.Serial;
using AFooCockpit.App.Core.Device;
using AFooCockpit.App.Core.Settings;
using AFooCockpit.App.Gui.DataSourceConfigForms;

namespace AFooCockpit.App.Gui
{
    internal partial class Arinc429TranscieverGridView : UserControl
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public class DeviceConfig : INotifyPropertyChanged
        {
            private string? _DeviceName;
            private string? _DeviceType;
            private bool? _Enabled;
            private Arinc429TranscieverDataSourceConfig _DataSourceConfig;

            public required string DeviceName
            {
                get => _DeviceName!;
                set { _DeviceName = value; NotifyPropertyChanged("DeviceName"); }
            }
            public required string DeviceType
            {
                get => _DeviceType!;
                set { _DeviceType = value; NotifyPropertyChanged("DeviceType"); }
            }

            public required bool Enabled
            {
                get => (bool)_Enabled!;
                set { _Enabled = value; NotifyPropertyChanged(nameof(Enabled)); }
            }

            public Arinc429TranscieverDataSourceConfig DataSourceConfig
            {
                get => _DataSourceConfig;
                set { _DataSourceConfig = value; NotifyPropertyChanged(nameof(DataSourceConfig)); }
            }

            public event PropertyChangedEventHandler? PropertyChanged;

            private void NotifyPropertyChanged(string name)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(name));
                }
            }
        }

        private BindingList<DeviceConfig> Devices = new BindingList<DeviceConfig>();

        public Arinc429TranscieverGridView()
        {
            InitializeComponent();

            SetUpDataGrid();

        }

        public List<DeviceConfig> GetSerialDevices()
        {
            return Devices.ToList();
        }

        /// <summary>
        /// Creates all configured devices using the given device manager
        /// </summary>
        /// <param name="deviceManager"></param>
        public void CreateDevices(DeviceManager deviceManager)
        {
            ValidateForm();


            foreach (DeviceConfig deviceConfig in Devices)
            {
                if (!deviceConfig.Enabled)
                {
                    continue;
                }

                var dataSource = new Arinc429TranscieverDataSource(deviceConfig.DataSourceConfig);

                deviceManager.CreateDeviceInstance(deviceConfig.DeviceType, deviceConfig.DeviceName);
                deviceManager.ConnectDataSource(deviceConfig.DeviceName, dataSource);
            }
        }

        /// <summary>
        /// Validate that the form is valid, i.e. that all devices can be created
        /// </summary>
        /// <exception cref="FormValidationException"></exception>
        private void ValidateForm()
        {
            HashSet<string> deviceNames = new HashSet<string>();
            foreach (DeviceConfig deviceConfig in Devices)
            {
                if (deviceNames.Contains(deviceConfig.DeviceName))
                {
                    throw new FormValidationException("Cannot have two serial devices with same name!");
                }

                deviceNames.Add(deviceConfig.DeviceName);
            }
        }

        /// <summary>
        /// Data grid set up, for general setup that doesn't change
        /// Also loads the last config from file if it exsits
        /// </summary>
        private void SetUpDataGrid()
        {
            Devices = Settings.App.GetOrDefault(GetType().Name, new BindingList<DeviceConfig>());

            dgDeviceGrid.AutoGenerateColumns = false;
            dgDeviceGrid.AutoSize = true;
            dgDeviceGrid.DataSource = Devices;

            dgDeviceGrid.Columns.Add(CreateEnabledCheckBox());
            dgDeviceGrid.Columns.Add(CreateComboboxWithDeviceTypes());
            dgDeviceGrid.Columns.Add(CreateNameTextBox());
            dgDeviceGrid.Columns.Add(CreateSourceConfigEditButtonColumn());

            dgDeviceGrid.DataError += DgDeviceGrid_DataError;
            // If we don't add this, changes made in the data grid (such as clicking a checkbox) will not be propagated
            // before pressing enter or similar
            dgDeviceGrid.CellContentClick += DgDeviceGrid_CellContentClick;
        }

        private void DgDeviceGrid_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            // Check dgDeviceGrid the click was on the button column
            if (dgDeviceGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                // Access the clicked row
                var row = dgDeviceGrid.Rows[e.RowIndex];

                EditConfiguration(Devices[e.RowIndex].DataSourceConfig);
            }

            dgDeviceGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void DgDeviceGrid_DataError(object? sender, DataGridViewDataErrorEventArgs e)
        {
            logger.Error(e);
        }

        /// <summary>
        /// Create combobox column for device types
        /// </summary>
        /// <returns></returns>
        private DataGridViewComboBoxColumn CreateComboboxWithDeviceTypes()
        {
            var combo = new DataGridViewComboBoxColumn();
            combo.DataSource = DeviceManager.GetDeviceTypesByDataSourceType<Arinc429TranscieverDataSource>().ToArray();
            combo.DataPropertyName = "DeviceType";
            combo.Name = "Device Type";
            combo.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            combo.Width = 200;
            return combo;
        }

        private DataGridViewTextBoxColumn CreateNameTextBox()
        {
            var column = new DataGridViewTextBoxColumn();
            column.Name = "Device Name";
            column.DataPropertyName = "DeviceName";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            return column;
        }

        private DataGridViewCheckBoxColumn CreateEnabledCheckBox()
        {
            var column = new DataGridViewCheckBoxColumn();
            column.Name = "Enabled";
            column.DataPropertyName = "Enabled";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            column.Width = 100;
            return column;
        }

        private DataGridViewButtonColumn CreateSourceConfigEditButtonColumn()
        {
            var column = new DataGridViewButtonColumn();
            column.Name = "Configuration";
            column.Text = "Edit";
            column.UseColumnTextForButtonValue = true;
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            column.Width = 80;
            return column;
        }

        private async void EditConfiguration(Arinc429TranscieverDataSourceConfig baseConfig)
        {
            try { 
                await DataSourceConfigFactory.CreateDataSourceConfig(baseConfig);
                // In case there's changes, we need to enable the save button
                Settings.App.Taint();
            } 
            catch (DataSourceConfigCreationCanceledException)
            {
                logger.Warn("User canceled data source configuration");
            }
        }

        private async void tsbtnAddDevice_Click(object sender, EventArgs e)
        {
            var deviceTypes = DeviceManager.GetDeviceTypesByDataSourceType<Arinc429TranscieverDataSource>();
            if (deviceTypes.Count == 0)
            {
                MessageBox.Show("Cannot add device - no Arinc4 Arinc429TranscieverDataSource available");
                return;
            }

            try { 
                var sourceConfig = await DataSourceConfigFactory.CreateDataSourceConfig<Arinc429TranscieverDataSourceConfig>();

                Debug.WriteLine($"Adding element to list, size is {Devices.Count}");
                Devices.Add(new DeviceConfig
                {
                    DeviceType = deviceTypes[0],
                    DeviceName = $"New Device {Devices.Count}",
                    DataSourceConfig = sourceConfig,
                    Enabled = true
                });
            } 
            catch(DataSourceConfigCreationCanceledException)
            {
                logger.Warn("User canceled configuration");
            }
        }

        private void tsbtnRemove_Click(object sender, EventArgs e)
        {
            if (dgDeviceGrid.SelectedRows.Count > 0)
            {
                var item = (DeviceConfig)dgDeviceGrid.SelectedRows[0].DataBoundItem;
                Devices.Remove(item);
            }
        }
    }
}
