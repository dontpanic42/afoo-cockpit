using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFooCockpit.App.Core.FlightData
{
    /// <summary>
    /// Thise are standard events for our app. Every hardware, aircraft etc. is
    /// expected to convert their internal event names to these standardized
    /// event types before sending it to the event bus
    /// </summary>
    public enum FlightDataEvent
    {
        ECAM_ENG_Button,
        ECAM_APU_Button,
        ECAM_CLR_Left_Button,
        ECAM_TO_CONFIG_Button,
        ECAM_BLEED_Button,
        ECAM_COND_Button,
        ECAM_PRESS_Button,
        ECAM_DOOR_Button,
        ECAM_STS_Button,
        ECAM_ELEC_Button,
        ECAM_WHEEL_Button,
        ECAM_RCL_Button,
        ECAM_EMER_CANC_Button,
        ECAM_HYD_Button,
        ECAM_FCTL_Button,
        ECAM_FUEL_Button,
        ECAM_ALL_Button,
        ECAM_CLR_Right_Button,

        ECAM_APU_Button_Annunciator,
        ECAM_BLEED_Button_Annunciator,
        ECAM_CLR_Left_Button_Annunciator,
        ECAM_CLR_Right_Button_Annunciator,
        ECAM_COND_Button_Annunciator,

        Switching_ATT_HDG_Knob,
        Switching_AIR_DATA_Knob,
        Switching_EIS_DMC_Knob,
        Switching_ECAM_ND_XFR_Knob,
    }

    /// <summary>
    /// Some standardized event values. Every hardware, aircraft etc. is
    /// expected to convert their internal event names to these standardized
    /// event types before sending an event to the event bus
    /// </summary>
    public struct FlightDataEventValue
    {
        public const double ButtonPress = 1;
        public const double ButtonRelease = 0;

        public const double IndicatorOn = 1;
        public const double IndicatorOff = 0;

        public const double SelectorSwitchPos1 = 0;
        public const double SelectorSwitchPos2 = 1;
        public const double SelectorSwitchPos3 = 2;
        public const double SelectorSwitchPos4 = 3;
        public const double SelectorSwitchPos5 = 4;
        public const double SelectorSwitchPos6 = 5;
        public const double SelectorSwitchPos7 = 6;
        public const double SelectorSwitchPos8 = 7;
        public const double SelectorSwitchPos9 = 8;

    }
}
