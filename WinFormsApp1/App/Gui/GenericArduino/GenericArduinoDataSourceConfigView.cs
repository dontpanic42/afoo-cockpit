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
using AFooCockpit.App.Core.Utils.SerialUtils;
using FSUIPC;

namespace AFooCockpit.App.Gui.DataSourceConfigForms
{
    public partial class GenericArduinoDataSourceConfigView
        : DataSourceConfigView
        , IDataSourceConfigView<GenericArduinoDataSourceConfig>
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private GenericArduinoDataSourceConfig? _config;

        public GenericArduinoDataSourceConfig? DataSourceConfig
        {
            get => _config;
            set
            {
                _config = value;

                if (value != null)
                {
                    UpdatePortComboBox();
                    UpdateBaudrateNumberBox();
                    UpdatePinSelectors();
                }
            }
        }

        public GenericArduinoDataSourceConfigView()
        {
            InitializeComponent();

            Load += GenericArduinoDataSourceConfigView_Load;
        }

        private void GenericArduinoDataSourceConfigView_Load(object? sender, EventArgs e)
        {
            UpdatePortComboBox();
            UpdateBaudrateNumberBox();
            UpdatePinSelectors();
        }

        private void UpdatePortComboBox()
        {
            List<string> ports = SerialUtils.GetPortList();
            cmbSerialPort.Items.Clear();
            cmbSerialPort.Items.AddRange(ports.ToArray());
            if (DataSourceConfig != null && ports.Contains(DataSourceConfig.Port))
            {
                cmbSerialPort.SelectedIndex = ports.IndexOf(DataSourceConfig.Port);
            }
        }

        private void UpdateBaudrateNumberBox()
        {
            if (DataSourceConfig != null)
            {
                numBaudRate.Value = Convert.ToInt32(DataSourceConfig.BaudRate);
            }
        }

        private void SaveToConfigObject()
        {
            if (_config == null)
            {
                logger.Error("Cannot save configuration - configuration object is null");
                return;
            }

            _config.Port = cmbSerialPort.Text;
            _config.BaudRate = Convert.ToInt32(numBaudRate.Value);

            _config.PinConfigs[0] = selPinD0.GetSavePin();
            _config.PinConfigs[1] = selPinD1.GetSavePin();
            _config.PinConfigs[2] = selPinD2.GetSavePin();
            _config.PinConfigs[3] = selPinD3.GetSavePin();
            _config.PinConfigs[4] = selPinD4.GetSavePin();
            _config.PinConfigs[5] = selPinD5.GetSavePin();
            _config.PinConfigs[6] = selPinD6.GetSavePin();
            _config.PinConfigs[7] = selPinD7.GetSavePin();
            _config.PinConfigs[8] = selPinD8.GetSavePin();
            _config.PinConfigs[9] = selPinD9.GetSavePin();
            _config.PinConfigs[10] = selPinD10.GetSavePin();
            _config.PinConfigs[11] = selPinD11.GetSavePin();
            _config.PinConfigs[12] = selPinD12.GetSavePin();
            _config.PinConfigs[13] = selPinD13.GetSavePin();
        }

        private void UpdatePinSelectors()
        {
            if (_config == null)
            {
                logger.Error("Cannot update configuration - configuration object is null");
                return;
            }

            selPinD0.PinConfig = _config.PinConfigs[0];
            selPinD1.PinConfig = _config.PinConfigs[1];
            selPinD2.PinConfig = _config.PinConfigs[2];
            selPinD3.PinConfig = _config.PinConfigs[3];
            selPinD4.PinConfig = _config.PinConfigs[4];
            selPinD5.PinConfig = _config.PinConfigs[5];
            selPinD6.PinConfig = _config.PinConfigs[6];
            selPinD7.PinConfig = _config.PinConfigs[7];
            selPinD8.PinConfig = _config.PinConfigs[8];
            selPinD9.PinConfig = _config.PinConfigs[9];
            selPinD10.PinConfig = _config.PinConfigs[10];
            selPinD11.PinConfig = _config.PinConfigs[11];
            selPinD12.PinConfig = _config.PinConfigs[12];
            selPinD13.PinConfig = _config.PinConfigs[13];
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            SaveToConfigObject();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnRefreshComPorts_Click(object sender, EventArgs e)
        {
            UpdatePortComboBox();
        }
    }
}
