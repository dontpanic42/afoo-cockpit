using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using AFooCockpit.App.Core.DataSource.DataSources.Serial;

namespace AFooCockpit.App.Gui
{
    public partial class DeviceGridView : UserControl
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public class DeviceConfig
        {
            public string SerialPort { get; set; }
            public string DeviceType { get; set; }
        }

        private static readonly string SETTINGS_PATH = "AFooCockpit";
        private static readonly string SETTINGS_FILE = "devices.json";

        private BindingSource BindingSource = new BindingSource();

        public List<DeviceConfig> Devices { get; private set; } = new List<DeviceConfig>();

        public DeviceGridView()
        {
            InitializeComponent();

            SetUpDataGrid();

        }

        /// <summary>
        /// Data grid set up, for general setup that doesn't change
        /// Also loads the last config from file if it exsits
        /// </summary>
        private void SetUpDataGrid()
        {
            dgDeviceGrid.AutoGenerateColumns = false;
            dgDeviceGrid.AutoSize = true;
            dgDeviceGrid.DataSource = BindingSource;

            dgDeviceGrid.Columns.Add(CreateComboboxWithDeviceTypes());
            dgDeviceGrid.Columns.Add(CreateCombobooxWithComPorts());

            dgDeviceGrid.DataError += DgDeviceGrid_DataError;

            LoadFromFile();
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
            combo.DataSource = SerialDataSource.GetPortList();
            combo.DataPropertyName = "SerialPort";
            combo.Name = "Port";
            combo.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            return combo;
        }

        /// <summary>
        /// Create combobox column for device types
        /// </summary>
        /// <returns></returns>
        private DataGridViewComboBoxColumn CreateComboboxWithDeviceTypes()
        {
            var combo = new DataGridViewComboBoxColumn();
            combo.DataSource = (string[])["JavaSimulations Ecam", "JavaSimulations Switching"];
            combo.DataPropertyName = "DeviceType";
            combo.Name = "Device Type";
            combo.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            return combo;
        }

        private void DeviceGridView_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Saves the current config to file
        /// </summary>
        private void SaveToFile()
        {
            var baseFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var appFolder = Path.Join(baseFolder, SETTINGS_PATH);
            var fileName = Path.Combine(appFolder, SETTINGS_FILE);

            Directory.CreateDirectory(appFolder);

            logger.Debug($"Saving config to {fileName}");

            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(Devices, options);

                using (StreamWriter outputFile = new StreamWriter(fileName))
                {
                    outputFile.Write(jsonString);
                }

                logger.Debug($"Wrote config file {fileName}");
            }
            catch (Exception e)
            {
                logger.Error(e);
                MessageBox.Show($"Error while saving configuration to {fileName} \r\n {e.Message}");
            }
        }

        /// <summary>
        /// Attempts to load config from file (if config exists)
        /// </summary>
        private void LoadFromFile()
        {
            var baseFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var appFolder = Path.Join(baseFolder, SETTINGS_PATH);
            var fileName = Path.Combine(appFolder, SETTINGS_FILE);

            Directory.CreateDirectory(appFolder);

            if (Path.Exists(fileName))
            {
                logger.Debug($"Loading config from {fileName}");

                try
                {
                    string jsonString = File.ReadAllText(fileName);
                    var devices = JsonSerializer.Deserialize<List<DeviceConfig>>(jsonString);


                    if (devices != null)
                    {
                        logger.Debug($"Read {jsonString.Length} characters from config, resulting in {devices.Count} devices");
                        Devices = devices;
                        UpdateGridValues();
                    }
                    else
                    {
                        logger.Debug("Loaded config file, but list of devices is empty");
                    }
                }
                catch (Exception e)
                {
                    logger.Error(e);
                    MessageBox.Show($"Error while loading configuration to {fileName} \r\n {e.Message}");
                }
            }
        }

        /// <summary>
        /// Add a new (default) device
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddDevice_Click(object sender, EventArgs e)
        {
            Devices.Add(new DeviceConfig { SerialPort = "COM1", DeviceType = "JavaSimulations Ecam" });
            UpdateGridValues();
        }

        /// <summary>
        /// Force update of all grid values
        /// </summary>
        private void UpdateGridValues()
        {
            dgDeviceGrid.DataSource = null;
            dgDeviceGrid.DataSource = Devices;
            dgDeviceGrid.ResetBindings();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveToFile();
        }

        /// <summary>
        /// Removes the currently selected device
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemoveDevice_Click(object sender, EventArgs e)
        {
            if (dgDeviceGrid.SelectedRows.Count > 0) {
                var item = (DeviceConfig) dgDeviceGrid.SelectedRows[0].DataBoundItem;
                Devices.Remove(item);
                UpdateGridValues();
            }
        }
    }
}
