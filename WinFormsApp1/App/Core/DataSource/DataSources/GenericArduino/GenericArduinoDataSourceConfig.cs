using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.Utils.ArduinoGenericFirmwareUtils;
using AFooCockpit.App.Core.Utils.SerialUtils;

namespace AFooCockpit.App.Core.DataSource.DataSources.GenericArduino
{
    public class GenericArduinoDataSourceConfig : DataSourceConfig
    {
        public class PinConfig
        {
            /// <summary>
            /// This the name that is displayed in the GUI etc.
            /// </summary>
            public required string PinName;
            /// <summary>
            /// Number of the pin on the actual arduino
            /// </summary>
            public required int PinNumber;
            /// <summary>
            /// Direction of the pin, i.e. is it an input or an output
            /// </summary>
            public ArduinoGenericFirmwareUtils.PinDirection Direction = ArduinoGenericFirmwareUtils.PinDirection.In;
            /// <summary>
            /// Should the pull up be activated. Only valid for inputs, ignored for outputs
            /// </summary>
            public ArduinoGenericFirmwareUtils.Pullup Pullup = ArduinoGenericFirmwareUtils.Pullup.Disable;
            /// <summary>
            /// If false, the pin is ignored
            /// </summary>
            public bool Enabled = false;
            /// <summary>
            /// Pin ID is a name that will be put as "sender" in the data field. It doesn't need to be unique.
            /// It's used to decouple devices from actual pin usage on arduinos.
            /// A device can, for example, listen for "Button XY" data events, but doesn't need to know
            /// what pin the button is configured on. That way, we can also have multiple instances of the same
            /// device using different Arduinos
            /// </summary>
            public string PinId = "";
        }

        public string Port = "COM0";
        public int BaudRate = 115200;

        public List<PinConfig> PinConfigs = new List<PinConfig>
        {
            new PinConfig { PinName = "D0", PinId = "Pin D0", PinNumber = 0 },
            new PinConfig { PinName = "D1", PinId = "Pin D1", PinNumber = 1 },
            new PinConfig { PinName = "D2", PinId = "Pin D2", PinNumber = 2 },
            new PinConfig { PinName = "D3", PinId = "Pin D3", PinNumber = 3 },
            new PinConfig { PinName = "D4", PinId = "Pin D4", PinNumber = 4 },
            new PinConfig { PinName = "D5", PinId = "Pin D5", PinNumber = 5 },
            new PinConfig { PinName = "D6", PinId = "Pin D6", PinNumber = 6 },
            new PinConfig { PinName = "D7", PinId = "Pin D7", PinNumber = 7 },
            new PinConfig { PinName = "D8", PinId = "Pin D8", PinNumber = 8 },
            new PinConfig { PinName = "D9", PinId = "Pin D9", PinNumber = 9 },
            new PinConfig { PinName = "D10", PinId = "Pin D10", PinNumber = 10 },
            new PinConfig { PinName = "D11", PinId = "Pin D11", PinNumber = 11 },
            new PinConfig { PinName = "D12", PinId = "Pin D12", PinNumber = 12 },
            new PinConfig { PinName = "D13", PinId = "Pin D13", PinNumber = 13 },
        };

        /// <summary>
        /// True, when at least one pin with the given pin id exists
        /// </summary>
        /// <param name="pinId"></param>
        /// <returns></returns>
        public bool HasActivePinsWithId(string pinId)
        {
            return GetActivePinsWithId(pinId).Count() > 0;
        }

        /// <summary>
        /// Returns all pins with a given PinId
        /// </summary>
        /// <param name="pinId"></param>
        /// <returns></returns>
        public List<PinConfig> GetActivePinsWithId(string pinId)
        {
            return PinConfigs.Where(p => p.PinId == pinId && p.Enabled == true).ToList();
        }
    }
}
