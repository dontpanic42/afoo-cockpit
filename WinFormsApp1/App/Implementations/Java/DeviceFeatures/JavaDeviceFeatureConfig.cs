using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.Device.DeviceFeatures;

namespace AFooCockpit.App.Implementations.Java.DeviceFeatures
{
    internal class JavaDeviceFeatureConfig : DeviceFeatureConfig
    {
        public required string SerialEvent;

        public bool Is3PosSwitch = false;
    }
}
