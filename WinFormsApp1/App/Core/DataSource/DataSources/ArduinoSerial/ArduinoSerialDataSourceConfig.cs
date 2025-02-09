using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.DataSource.DataSources.Serial;

namespace AFooCockpit.App.Core.DataSource.DataSources.ArduinoSerial
{
    internal class ArduinoSerialDataSourceConfig : DataSourceConfig
    {
        public required string Port;

        public int BaudRate = 115200;
        public int ReadTimeout = 2000;
        public string NewLine = "\r\n";
        public Handshake Handshake = Handshake.None;
        public Parity Parity = Parity.None;
        public int DataBits = 8;
        public bool DtrEnable = true;
        public bool RtsEnable = true;

        // Timeout after connecting, to give the firmware time to load...
        public int AfterConnectTimeout = 2000;
        // Delay between polling for values
        public int PollIntervalMs = 500;
    }
}
