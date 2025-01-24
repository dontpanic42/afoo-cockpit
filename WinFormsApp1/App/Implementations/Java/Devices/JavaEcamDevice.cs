using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.DataSource.DataSources.Serial;
using AFooCockpit.App.Core.Device;
using AFooCockpit.App.Core.FlightData;

namespace AFooCockpit.App.Implementations.Java.Devices
{
    internal class JavaEcamDevice : JavaDevice
    {
        public JavaEcamDevice(FlightDataEventBus flightDataEventBus) : base("Java Ecam Panel", flightDataEventBus)
        {
            AddPushButton(FlightDataEvent.ECAM_ENG_Button, "EB01");
            AddPushButton(FlightDataEvent.ECAM_APU_Button, "EB02");
            AddPushButton(FlightDataEvent.ECAM_CLR_Left_Button, "EB03");
            AddPushButton(FlightDataEvent.ECAM_TO_CONFIG_Button, "EB04");
            AddPushButton(FlightDataEvent.ECAM_BLEED_Button, "EB05");
            AddPushButton(FlightDataEvent.ECAM_COND_Button, "EB06");
            AddPushButton(FlightDataEvent.ECAM_PRESS_Button, "EB07");
            AddPushButton(FlightDataEvent.ECAM_DOOR_Button, "EB08");
            AddPushButton(FlightDataEvent.ECAM_STS_Button, "EB09");
            AddPushButton(FlightDataEvent.ECAM_ELEC_Button, "EB10");
            AddPushButton(FlightDataEvent.ECAM_WHEEL_Button, "EB11");
            AddPushButton(FlightDataEvent.ECAM_RCL_Button, "EB12");
            AddPushButton(FlightDataEvent.ECAM_EMER_CANC_Button, "EB13");
            AddPushButton(FlightDataEvent.ECAM_HYD_Button, "EB14");
            AddPushButton(FlightDataEvent.ECAM_FCTL_Button, "EB15");
            AddPushButton(FlightDataEvent.ECAM_FUEL_Button, "EB16");
            AddPushButton(FlightDataEvent.ECAM_ALL_Button, "EB17");
            AddPushButton(FlightDataEvent.ECAM_CLR_Right_Button, "EB18");
        }
    }
}
