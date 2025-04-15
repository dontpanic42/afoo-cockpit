using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using AFooCockpit.App.Core.DataSource.DataSources.Arinc429TranscieverDataSource;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AFooCockpit.App.Gui.DataSourceConfigForms
{
    internal partial class Arinc429TranscieverDataSourceConfigView :
        DataSourceConfigView,
        IDataSourceConfigView<Arinc429TranscieverDataSourceConfig>
    {
        private Arinc429TranscieverDataSourceConfig? _config;

        public Arinc429TranscieverDataSourceConfigView()
        {
            InitializeComponent();
        }

        public Arinc429TranscieverDataSourceConfig? DataSourceConfig
        {
            get => _config;
            set
            {
                _config = value;

                if (value != null)
                {
                    txtProductId.Text = HexValueToString(_config!.Pid);
                    txtVendorId.Text = HexValueToString(_config!.Vid);
                }
            }
        }

        private int HexStringToInt(System.Windows.Forms.TextBox textBox)
        {
            Regex validInput = new Regex(@"^(\$|0x)?[0-9a-f]*", RegexOptions.IgnoreCase);
            if (validInput.IsMatch(textBox.Text))
            {
                return Convert.ToInt32(textBox.Text, 16);
            }

            MessageBox.Show($"Invalid input - input needs to be a hex string (example: 0xDEAD), is {textBox.Text}", "Value Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            throw new Exception("Invalid value");
        }

        private string HexValueToString(int value, int minLength = 4)
        {
            return "0x" + Convert.ToString(value, 16).PadLeft(minLength, '0');
        }


        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                var vid = HexStringToInt(txtVendorId);
                var pid = HexStringToInt(txtProductId);

                DataSourceConfig!.Vid = vid;
                DataSourceConfig!.Pid = pid;

                this.Close();
            }
            catch (Exception) 
            { 
                // Error handling done by the conversion method
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
