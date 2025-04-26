using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.Device.DeviceFeatures;

namespace AFooCockpit.App.Implementations.GenericArduinoDevice.DeviceFeatures
{
    internal class GenericArduinoDeviceFeatureConfig : DeviceFeatureConfig
    {
        public string PinId = "";

        public Dictionary<string, double> PinIds = new Dictionary<string, double>();
    }
}
