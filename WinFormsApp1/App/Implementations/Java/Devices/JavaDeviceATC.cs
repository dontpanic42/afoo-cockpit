using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.DataSource.DataSources.Serial;
using AFooCockpit.App.Core.Device;
using AFooCockpit.App.Core.FlightData;
using AFooCockpit.App.Implementations.Java.DeviceFeatures;

namespace AFooCockpit.App.Implementations.Java.Devices
{
    internal class JavaDeviceATC : JavaDevice
    {
        private static readonly (FlightDataEvent, string)[] MomentaryButtons = [
            //(FlightDataEvent.ECAM_ENG_Button,                       "EB01"),
            //(FlightDataEvent.ECAM_APU_Button,                       "EB02"),
            //(FlightDataEvent.ECAM_CLR_Left_Button,                  "EB03"),
            //(FlightDataEvent.ECAM_TO_CONFIG_Button,                 "EB04"),
            //(FlightDataEvent.ECAM_BLEED_Button,                     "EB05"),
            //(FlightDataEvent.ECAM_COND_Button,                      "EB06"),
            //(FlightDataEvent.ECAM_PRESS_Button,                     "EB07"),
            //(FlightDataEvent.ECAM_DOOR_Button,                      "EB08"),
            //(FlightDataEvent.ECAM_STS_Button,                       "EB09"),
            //(FlightDataEvent.ECAM_ELEC_Button,                      "EB10"),
            //(FlightDataEvent.ECAM_WHEEL_Button,                     "EB11"),
            //(FlightDataEvent.ECAM_RCL_Button,                       "EB12"),
            //(FlightDataEvent.ECAM_EMER_CANC_Button,                 "EB13"),
            //(FlightDataEvent.ECAM_HYD_Button,                       "EB14"),
            //(FlightDataEvent.ECAM_FCTL_Button,                      "EB15"),
            //(FlightDataEvent.ECAM_FUEL_Button,                      "EB16"),
            //(FlightDataEvent.ECAM_ALL_Button,                       "EB17"),
            //(FlightDataEvent.ECAM_CLR_Right_Button,                 "EB18")
        ];

        private static readonly (FlightDataEvent, string)[] IndicatorLights = [
            //(FlightDataEvent.ECAM_ENG_Button_Annunciator,           "EL01"),
            //(FlightDataEvent.ECAM_APU_Button_Annunciator,           "EL02"),
            //(FlightDataEvent.ECAM_CLR_Left_Button_Annunciator,      "EL03"),
            //(FlightDataEvent.ECAM_BLEED_Button_Annunciator,         "EL04"),
            //(FlightDataEvent.ECAM_COND_Button_Annunciator,          "EL05"),
            //(FlightDataEvent.ECAM_PRESS_Button_Annunciator,         "EL06"),
            //(FlightDataEvent.ECAM_DOOR_Button_Annunciator,          "EL07"),
            //(FlightDataEvent.ECAM_STS_Button_Annunciator,           "EL08"),
            //(FlightDataEvent.ECAM_ELEC_Button_Annunciator,          "EL09"),
            //(FlightDataEvent.ECAM_WHEEL_Button_Annunciator,         "EL10"),
            //(FlightDataEvent.ECAM_HYD_Button_Annunciator,           "EL11"),
            //(FlightDataEvent.ECAM_FCTL_Button_Annunciator,          "EL12"),
            //(FlightDataEvent.ECAM_FUEL_Button_Annunciator,          "EL13"),
            //(FlightDataEvent.ECAM_CLR_Right_Button_Annunciator,     "EL14"),
            //// Backlight
            (FlightDataEvent.Elect_Bus_Power_AC_1,                  "PDLV"),
        ];

        private static readonly (FlightDataEvent flightDataEvent, string serialEvent, int multiplier)[] NumericDisplays = [
            (FlightDataEvent.TestEvent_DoNotUse                  , "ATSQ", 1),
        ];

        public JavaDeviceATC(FlightDataEventBus flightDataEventBus) : base("Java ATC Panel", flightDataEventBus)
        {
            AddDeviceFeatures<JavaDeviceFeatureMomentaryButton>(MomentaryButtons);
            AddDeviceFeatures<JavaDeviceFeatureIndicatorLight>(IndicatorLights);
            AddDeviceFeatures<JavaDeviceFeatureNumericDisplay>(NumericDisplays);
        }
    }
}
