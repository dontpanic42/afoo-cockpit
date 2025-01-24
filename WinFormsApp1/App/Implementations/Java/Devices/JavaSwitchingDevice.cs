using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.FlightData;

namespace AFooCockpit.App.Implementations.Java.Devices
{
    internal class JavaSwitchingDevice : JavaDevice
    {
        public JavaSwitchingDevice(FlightDataEventBus flightDataEventBus) : base("Java Switching Panel", flightDataEventBus)
        {
            AddSelectorSwitch3Pos(FlightDataEvent.Switching_ATT_HDG_Knob, "SS01");
            AddSelectorSwitch3Pos(FlightDataEvent.Switching_AIR_DATA_Knob, "SS02");
            AddSelectorSwitch3Pos(FlightDataEvent.Switching_EIS_DMC_Knob, "SS03");
            AddSelectorSwitch3Pos(FlightDataEvent.Switching_ECAM_ND_XFR_Knob, "SS04");
        }
    }
}
