using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AFooCockpit.App.Core.Utils.ArduinoGenericFirmwareUtils.ArduinoGenericFirmwareUtils;

namespace AFooCockpit.App.Core.DataSource.DataSources.GenericArduino
{
    internal class GenericArduinoDataSourceData : DataSourceData
    {
        public required double Value;
        public required string PinId;
    }
}
