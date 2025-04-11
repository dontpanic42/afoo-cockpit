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
    internal class JavaDeviceRMP : JavaDevice
    {
        private static readonly (FlightDataEvent, string)[] MomentaryButtons = [
            (FlightDataEvent.RMP1_VHF_1_Button,                     "RM02"),
            (FlightDataEvent.RMP1_VHF_2_Button,                     "RM03"),
            (FlightDataEvent.RMP1_VHF_3_Button,                     "RM04"),
            (FlightDataEvent.RMP1_HF_1_Button,                      "RM05"),
            (FlightDataEvent.RMP1_HF_2_Button,                      "RM06"),
            (FlightDataEvent.RMP1_AM_Button,                        "RM07"),
            (FlightDataEvent.RMP1_NAV_Button,                       "RM08"),
            (FlightDataEvent.RMP1_VOR_Button,                       "RM09"),
            (FlightDataEvent.RMP1_LS_Button,                        "RM10"),
            (FlightDataEvent.RMP1_GLS_Button,                       "RM11"),
            (FlightDataEvent.RMP1_ADF_Button,                       "RM12"),
            (FlightDataEvent.RMP1_BFO_Button,                       "RM13"),
            (FlightDataEvent.RMP1_Transfer_Button,                  "RM01"),
        ];

        private static readonly (FlightDataEvent, string)[] IndicatorLights = [
            //(FlightDataEvent.RMP1_VHF_1_Button,           "EL01"),
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

        private static readonly (FlightDataEvent, string)[] Dummies = [
            (FlightDataEvent.RMP1_Frequency_Inner_Knob_Dec, "RMBL"),
            (FlightDataEvent.RMP1_Frequency_Inner_Knob_Inc, "RMBR"),
            (FlightDataEvent.RMP1_Frequency_Outer_Knob_Dec, "RMAL"),
            (FlightDataEvent.RMP1_Frequency_Outer_Knob_Inc, "RMAR"),
        ];

        private static readonly (FlightDataEvent flightDataEvent, string serialEvent, int multiplier)[] NumericDisplays = [
            (FlightDataEvent.RMP1_ActiveFreq                  , "RMAT", 1),
            (FlightDataEvent.RMP1_StandbyFreq                  , "RMST", 1),
        ];

        public JavaDeviceRMP(FlightDataEventBus flightDataEventBus) : base("Java RMP Panel", flightDataEventBus)
        {
            AddDeviceFeatures<JavaDeviceFeatureMomentaryButton>(MomentaryButtons);
            AddDeviceFeatures<JavaDeviceFeatureIndicatorLight>(IndicatorLights);
            AddDeviceFeatures<JavaDeviceFeatureNumericDisplay>(NumericDisplays);
            AddDeviceFeatures<JavaDeviceFeatureDummy>(Dummies);
        }
    }
}
