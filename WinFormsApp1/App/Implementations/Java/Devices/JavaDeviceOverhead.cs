using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.FlightData;
using AFooCockpit.App.Implementations.Java.DeviceFeatures;

namespace AFooCockpit.App.Implementations.Java.Devices
{
    internal class JavaDeviceOverhead : JavaDevice
    {
        private static readonly (FlightDataEvent flightDataEvent, string serialEvent, bool is3posSwitch)[] SelectorSwitches = [
            (FlightDataEvent.ExteriorLight_Strobe_Switch, "T01", true),
            (FlightDataEvent.ExteriorLight_Beacon_Switch, "T02", false),
            (FlightDataEvent.ExteriorLight_Wing_Switch, "T03", false),
            (FlightDataEvent.ExteriorLight_Navigation_Switch, "T04", true),
            (FlightDataEvent.ExteriorLight_Runway_Turnoff_Switch, "T05", false),
            (FlightDataEvent.ExteriorLight_Landing_L_Switch, "T06", true),
            (FlightDataEvent.ExteriorLight_Landing_R_Switch, "T07", true),
            (FlightDataEvent.ExteriorLight_Nose_Switch, "T08", true),

            (FlightDataEvent.Sign_Seat_Belts_Switch, "T09", false),
            (FlightDataEvent.Sign_No_Smoking_Switch, "T10", true),
            (FlightDataEvent.Sign_Emergency_Exit_Switch, "T11", true)
        ];

        public JavaDeviceOverhead(FlightDataEventBus flightDataEventBus) : base("Java Switching Panel", flightDataEventBus)
        {
            AddDeviceFeatures<JavaDeviceFeatureSelectorSwitch>(SelectorSwitches);
        }
    }
}
