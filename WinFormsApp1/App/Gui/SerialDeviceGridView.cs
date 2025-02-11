using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using AFooCockpit.App.Core.DataSource.DataSources.Serial;
using AFooCockpit.App.Core.Device;
using AFooCockpit.App.Core.Settings;

namespace AFooCockpit.App.Gui
{
    public partial class SerialDeviceGridView : UserControl
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public class DeviceConfig : INotifyPropertyChanged
        {
            private string? _SerialPort;
            private string? _DeviceName;
            private string? _DeviceType;
            private bool? _Enabled;

            public required string SerialPort
            {
                get => _SerialPort!;
                set { _SerialPort = value; NotifyPropertyChanged("SerialPort"); }
            }
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

            public event PropertyChangedEventHandler? PropertyChanged;

            private void NotifyPropertyChanged(string name)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(name));
                }
            }
        }

        public BindingList<DeviceConfig> Devices = new BindingList<DeviceConfig>();

        public SerialDeviceGridView()
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

            Dictionary<string, SerialDataSource> dataSources = new Dictionary<string, SerialDataSource>();

            foreach (DeviceConfig deviceConfig in Devices)
            {
                if (!deviceConfig.Enabled)
                {
                    continue;
                }

                if (!dataSources.ContainsKey(deviceConfig.SerialPort))
                {
                    dataSources.Add(deviceConfig.SerialPort, new SerialDataSource(new SerialDataSourceConfig { Port = deviceConfig.SerialPort }));
                }

                deviceManager.CreateDeviceInstance(deviceConfig.DeviceType, deviceConfig.DeviceName);
                deviceManager.ConnectDataSource(deviceConfig.DeviceName, dataSources[deviceConfig.SerialPort]);
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
            Devices = Settings.App.GetOrDefault("serialDevices", new BindingList<DeviceConfig>());

            dgDeviceGrid.AutoGenerateColumns = false;
            dgDeviceGrid.AutoSize = true;
            dgDeviceGrid.DataSource = Devices;

            dgDeviceGrid.Columns.Add(CreateEnabledCheckBox());
            dgDeviceGrid.Columns.Add(CreateComboboxWithDeviceTypes());
            dgDeviceGrid.Columns.Add(CreateNameTextBox());
            dgDeviceGrid.Columns.Add(CreateCombobooxWithComPorts());

            dgDeviceGrid.DataError += DgDeviceGrid_DataError;
            // If we don't add this, changes made in the data grid (such as clicking a checkbox) will not be propagated
            // before pressing enter or similar
            dgDeviceGrid.CellContentClick += DgDeviceGrid_CellContentClick;
        }

        private void DgDeviceGrid_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            dgDeviceGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void DgDeviceGrid_DataError(object? sender, DataGridViewDataErrorEventArgs e)
        {
            logger.Error(e);
        }

        /// <summary>
        /// Create combobox column for com ports
        /// </summary>
        /// <returns></returns>
        private DataGridViewComboBoxColumn CreateCombobooxWithComPorts()
        {
            var combo = new DataGridViewComboBoxColumn();
            combo.DataSource = SerialDataSource.GetPortList().ToArray();
            combo.DataPropertyName = "SerialPort";
            combo.Name = "Port";
            combo.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            combo.Width = 200;
            return combo;
        }

        /// <summary>
        /// Create combobox column for device types
        /// </summary>
        /// <returns></returns>
        private DataGridViewComboBoxColumn CreateComboboxWithDeviceTypes()
        {
            var combo = new DataGridViewComboBoxColumn();
            combo.DataSource = DeviceManager.GetDeviceTypesByDataSourceType<SerialDataSource>().ToArray();
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

        private void tsbtnAddDevice_Click(object sender, EventArgs e)
        {
            var deviceTypes = DeviceManager.GetDeviceTypesByDataSourceType<SerialDataSource>();
            if (deviceTypes.Count == 0)
            {
                MessageBox.Show("Cannot add device - no serial devices available");
                return;
            }

            var portList = SerialDataSource.GetPortList();
            if (portList.Count == 0)
            {
                MessageBox.Show("Cannot add device - no serial ports available");
                return;
            }

            Debug.WriteLine($"Adding element to list, size is {Devices.Count}");
            Devices.Add(new DeviceConfig
            {
                SerialPort = portList[0],
                DeviceType = deviceTypes[0],
                DeviceName = $"New Device {Devices.Count}",
                Enabled = true
            });
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
