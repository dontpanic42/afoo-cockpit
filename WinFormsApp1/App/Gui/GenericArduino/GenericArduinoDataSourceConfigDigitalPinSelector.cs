using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AFooCockpit.App.Core.Utils.ArduinoGenericFirmwareUtils;
using NLog;
using static AFooCockpit.App.Core.DataSource.DataSources.GenericArduino.GenericArduinoDataSourceConfig;

namespace AFooCockpit.App.Gui.DataSourceConfigForms
{
    public partial class GenericArduinoDataSourceConfigDigitalPinSelector : UserControl
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private string _PinName = "Pin Name";
        
        private class InputOutputItem
        {
            public required string Name { get; set; }
            public required ArduinoGenericFirmwareUtils.PinDirection Direction { get; set; }
        }

        private class PullupItem
        {
            public required string Name { get; set; }
            public required ArduinoGenericFirmwareUtils.Pullup Pullup { get; set; }
        }

        private BindingList<InputOutputItem> InputOutputItems = new BindingList<InputOutputItem>
        {
            new InputOutputItem { Name = "Input", Direction = ArduinoGenericFirmwareUtils.PinDirection.In},
            new InputOutputItem { Name = "Output", Direction = ArduinoGenericFirmwareUtils.PinDirection.Out},
        };

        private BindingList<PullupItem> PullupItems = new BindingList<PullupItem>
        {
            new PullupItem { Name = "Enabled", Pullup = ArduinoGenericFirmwareUtils.Pullup.Enable },
            new PullupItem { Name = "Disabled", Pullup = ArduinoGenericFirmwareUtils.Pullup.Disable },
        };

        [Description("Pin Name"), Category("Data")]
        public string PinName
        {
            get
            {
                return _PinName;
            }
            set
            {
                if (_PinName != value)
                {
                    _PinName = value;
                    chkPinEnabled.Text = _PinName;
                }
            }
        }

        private PinConfig? _PinConfig;

        public PinConfig? PinConfig
        {
            private get
            {
                return _PinConfig;
            }
            set
            {
                if (value != null)
                {
                    _PinConfig = value;
                    cmbInputOutput.SelectedValue = _PinConfig.Direction;
                    cmbPullUp.SelectedValue = _PinConfig.Pullup;
                    chkPinEnabled.Checked = _PinConfig.Enabled;
                    txtPinId.Text = _PinConfig.PinId;
                    UpdateConditionalEnable();
                }
            }
        }

        public GenericArduinoDataSourceConfigDigitalPinSelector()
        {
            InitializeComponent();

            Load += GenericArduinoDataSourceConfigDigitalPinSelector_Load;

            UpdateConditionalEnable();
        }

        public PinConfig GetSavePin()
        {
            var oldConfig = _PinConfig!;

            oldConfig.PinName = PinName;
            oldConfig.Enabled = chkPinEnabled.Checked;
            oldConfig.PinId = txtPinId.Text;

            var dirValue = cmbInputOutput.SelectedValue as ArduinoGenericFirmwareUtils.PinDirection?;
            dirValue = dirValue ?? ArduinoGenericFirmwareUtils.PinDirection.In;
            oldConfig.Direction = (ArduinoGenericFirmwareUtils.PinDirection) dirValue!;

            var pullValue = cmbPullUp.SelectedValue as ArduinoGenericFirmwareUtils.Pullup?;
            pullValue = pullValue ?? ArduinoGenericFirmwareUtils.Pullup.Disable;
            oldConfig.Pullup = (ArduinoGenericFirmwareUtils.Pullup) pullValue!;

            return oldConfig;
        }

        private void GenericArduinoDataSourceConfigDigitalPinSelector_Load(object? sender, EventArgs e)
        {
            cmbInputOutput.Items.Clear();
            cmbInputOutput.DataSource = InputOutputItems;
            cmbInputOutput.ValueMember = "Direction";
            cmbInputOutput.DisplayMember = "Name";
            cmbInputOutput.SelectedValueChanged += (object? sender, EventArgs args) => UpdateConditionalEnable();

            cmbPullUp.Items.Clear();
            cmbPullUp.DataSource = PullupItems;
            cmbPullUp.ValueMember = "Pullup";
            cmbPullUp.DisplayMember = "Name";
        }

        private void chkPinEnabled_CheckedChanged(object sender, EventArgs e)
        {
            UpdateConditionalEnable();
        }

        private void UpdateConditionalEnable()
        {
            // Depending on the configuration of the direction, we want to enable or disable the pullup selector
            if (chkPinEnabled.Checked)
            {
                // When first opening, the value might not be selected yet. In that case, we're using the value
                // from the pin configuration
                var value = cmbInputOutput.SelectedValue as ArduinoGenericFirmwareUtils.PinDirection? ?? _PinConfig?.Direction;
                cmbInputOutput.Enabled = true;
                cmbPullUp.Enabled = value == ArduinoGenericFirmwareUtils.PinDirection.In;
                txtPinId.Enabled = true;
            } 
            else
            {
                cmbInputOutput.Enabled = false;
                cmbPullUp.Enabled = false;
                txtPinId.Enabled = false;
            }
        }
    }
}
