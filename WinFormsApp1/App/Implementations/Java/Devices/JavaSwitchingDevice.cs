using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.FlightData;
using AFooCockpit.App.Implementations.Java.DeviceFeatures;

namespace AFooCockpit.App.Implementations.Java.Devices
{
    internal class JavaSwitchingDevice : JavaDevice
    {
        private static readonly (FlightDataEvent flightDataEvent, string serialEvent, bool is3posSwitch)[] SelectorSwitches = [
            (FlightDataEvent.Switching_ATT_HDG_Knob, "SS01", true),
            (FlightDataEvent.Switching_ATT_HDG_Knob, "SS02", true),
            (FlightDataEvent.Switching_ATT_HDG_Knob, "SS03", true),
            (FlightDataEvent.Switching_ATT_HDG_Knob, "SS04", true),
        ];

        public JavaSwitchingDevice(FlightDataEventBus flightDataEventBus) : base("Java Switching Panel", flightDataEventBus)
        {
            AddDeviceFeatures<JavaDeviceFeatureSelectorSwitch>(SelectorSwitches);
        }
    }
}
