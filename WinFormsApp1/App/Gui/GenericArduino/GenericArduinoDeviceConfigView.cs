using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AFooCockpit.App.Core.DataSource.DataSources.Arinc429TranscieverDataSource;
using AFooCockpit.App.Core.DataSource.DataSources.GenericArduino;
using AFooCockpit.App.Core.Device;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using static AFooCockpit.App.Gui.GenericArduinoDeviceView;

namespace AFooCockpit.App.Gui.DeviceConfigForms
{
    public partial class GenericArduinoDeviceConfigView : Form
    {
        private static readonly string DefaultDeviceName = "Arduino Device";

        private BindingList<DataSourceItem> _dataSourceItems = new BindingList<DataSourceItem>();

        public BindingList<DataSourceItem> DataSourceItems
        {
            get => _dataSourceItems;
            set
            {
                _dataSourceItems = value;
                cmbDataSources.Items.Clear();
                cmbDataSources.DataSource = _dataSourceItems;
                cmbDataSources.DisplayMember = "DisplayName";
                cmbDataSources.ValueMember = "SourceConfig";

                ConfigureDeviceList();
            }
        }

        public GenericArduinoDeviceConfigView()
        {
            InitializeComponent();

            Load += GenericArduinoDeviceConfigView_Load;
        }

        private void GenericArduinoDeviceConfigView_Load(object? sender, EventArgs e)
        {
            btnOk.Enabled = false;
            cmbDevices.SelectedValueChanged += (_, _) => UpdateOkBtn();
            cmbDataSources.SelectedValueChanged += (_, _) => UpdateOkBtn();

            txtDisplayName.Text = GenerateRandomDisplayName();
        }

        private void UpdateOkBtn()
        {
            btnOk.Enabled = cmbDevices.SelectedItem != null; // && cmbDataSources.SelectedValue != null
        }

        private void ConfigureDeviceList()
        {
            var deviceList = DeviceManager.GetDeviceTypesByDataSourceType<GenericArduinoDataSource>();
            cmbDevices.Items.Clear();
            cmbDevices.Items.AddRange(deviceList.ToArray());
        }

        /// <summary>
        /// Generates a random and hopefully unique default name for new devices
        /// </summary>
        /// <returns></returns>
        private string GenerateRandomDisplayName()
        {
            return $"{DefaultDeviceName} {Guid.NewGuid()}";
        }

        /// <summary>
        /// Checks if the device can be created from the current inputs
        /// </summary>
        /// <returns></returns>
        private bool ValidateForm()
        {
            if (txtDisplayName.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please enter a unique name for this device");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns the selected device. Should be called after closing the dialog, and 
        /// after verifying that the dialog result is OK
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when either selected device
        /// or selected datasource is null. This shouldn't happen if the dialog result
        /// is OK.</exception>
        public DeviceItem GetSelectedDevice()
        {
            var deviceName = cmbDevices.SelectedItem as string;
            if (deviceName == null)
            {
                throw new ArgumentNullException("Cannot return selected device - selected device is null");
            }

            var dataSourceItem = (DataSourceItem?)cmbDataSources.SelectedItem;
            if (dataSourceItem == null)
            {
                throw new ArgumentNullException("Cannot return selected devices - selected data source is null");
            }

            return new DeviceItem
            {
                UniqueDisplayName = txtDisplayName.Text,
                DeviceName = deviceName,
                DataSourceGuid = dataSourceItem.Guid
            };
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
            Dispose();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
