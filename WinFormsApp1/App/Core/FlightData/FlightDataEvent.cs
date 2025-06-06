﻿using System;
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
        /// <summary>
        /// Ecam
        /// </summary>
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
        ECAM_ELEC_Button_Annunciator,
        ECAM_ENG_Button_Annunciator,
        ECAM_FCTL_Button_Annunciator,
        ECAM_FUEL_Button_Annunciator,
        ECAM_HYD_Button_Annunciator,
        ECAM_PRESS_Button_Annunciator,
        ECAM_STS_Button_Annunciator,
        ECAM_WHEEL_Button_Annunciator,
        ECAM_DOOR_Button_Annunciator,

        /// <summary>
        /// Switching
        /// </summary>
        Switching_ATT_HDG_Knob,
        Switching_AIR_DATA_Knob,
        Switching_EIS_DMC_Knob,
        Switching_ECAM_ND_XFR_Knob,

        /// <summary>
        /// OVHD
        /// </summary>
        /// 
        // Indicators
        // Ext Lt
        ExteriorLight_Strobe_Switch,
        ExteriorLight_Beacon_Switch,
        ExteriorLight_Wing_Switch,
        ExteriorLight_Navigation_Switch,
        ExteriorLight_Runway_Turnoff_Switch,
        ExteriorLight_Landing_L_Switch,
        ExteriorLight_Landing_R_Switch,
        ExteriorLight_Nose_Switch,

        // Signs
        Sign_Seat_Belts_Switch,
        Sign_No_Smoking_Switch,
        Sign_Emergency_Exit_Switch,

        // ADIRS
        ADIRS_IR_1_Annunciator_Lower,
        ADIRS_IR_1_Annunciator_Upper,
        ADIRS_IR_2_Annunciator_Lower,
        ADIRS_IR_2_Annunciator_Upper,
        ADIRS_IR_3_Annunciator_Lower,
        ADIRS_IR_3_Annunciator_Upper,
        ADIRS_On_Bat_Annunciator,

        // APU
        APU_Master_Button_Annunciator_Lower,
        APU_Master_Button_Annunciator_Upper,
        APU_Start_Button_Annunciator_Lower,
        APU_Start_Button_Annunciator_Upper,

        // Anti Ice
        Icing_Engine_1_Button_Annunciator_Lower,
        Icing_Engine_1_Button_Annunciator_Upper,
        Icing_Engine_2_Button_Annunciator_Lower,
        Icing_Engine_2_Button_Annunciator_Upper,
        Icing_Wing_Button_Annunciator_Lower,
        Icing_Wing_Button_Annunciator_Upper,

        // Oxygen
        Oxygen_Crew_Button_Annunciator,

        // Air Cond
        Pneumatic_Pack_1_Button_Annunciator_Lower,
        Pneumatic_Pack_1_Button_Annunciator_Upper,
        Pneumatic_Pack_2_Button_Annunciator_Lower,
        Pneumatic_Pack_2_Button_Annunciator_Upper,
        Pneumatic_APU_Bleed_Button_Annunciator_Lower,
        Pneumatic_APU_Bleed_Button_Annunciator_Upper,

        // Elec
        Electrical_Battery_1_Button_Annunciator_Lower,
        Electrical_Battery_1_Button_Annunciator_Upper,
        Electrical_Battery_2_Button_Annunciator_Lower,
        Electrical_Battery_2_Button_Annunciator_Upper,
        Electrical_External_Power_Button_Annunciator_Lower,
        Electrical_External_Power_Button_Annunciator_Upper,

        // GPWS
        GPWS_Landing_Flap_3_Button_Annunciator,

        // Fuel
        Fuel_Right_Tank_Pump_1_Button_Annunciator_Lower,
        Fuel_Right_Tank_Pump_1_Button_Annunciator_Upper,
        Fuel_Right_Tank_Pump_2_Button_Annunciator_Lower,
        Fuel_Right_Tank_Pump_2_Button_Annunciator_Upper,
        Fuel_Center_Tank_Pump_1_Button_Annunciator_Lower,
        Fuel_Center_Tank_Pump_1_Button_Annunciator_Upper,
        Fuel_Center_Tank_Pump_2_Button_Annunciator_Lower,
        Fuel_Center_Tank_Pump_2_Button_Annunciator_Upper,
        Fuel_Crossfeed_Button_Annunciator_Lower,
        Fuel_Crossfeed_Button_Annunciator_Upper,
        Fuel_Left_Tank_Pump_1_Button_Annunciator_Lower,
        Fuel_Left_Tank_Pump_1_Button_Annunciator_Upper,
        Fuel_Left_Tank_Pump_2_Button_Annunciator_Lower,
        Fuel_Left_Tank_Pump_2_Button_Annunciator_Upper,

        //3 pos switches 
        InteriorLight_Dome_Switch,
        ADIRS_IR_1_Knob,
        ADIRS_IR_2_Knob,
        ADIRS_IR_3_Knob,
        Wiper_Captain_Knob,

        Call_All_Button,
        Fire_Engine_1_Test_Button,
        Fire_APU_Test_Button,
        Fire_Engine_2_Test_Button,
        APU_Start_Button,
        APU_Master_Button,
        Icing_Wing_Button,
        Icing_Engine_1_Button,
        Icing_Engine_2_Button,
        Oxygen_Crew_Button,
        Pneumatic_Pack_1_Button,
        Pneumatic_APU_Bleed_Button,
        Pneumatic_Pack_2_Button,
        ADIRS_IR_1_Button,
        ADIRS_IR_3_Button,
        ADIRS_IR_2_Button,
        GPWS_Landing_Flap_3_Button,
        Electrical_Battery_1_Button,
        Electrical_Battery_2_Button,
        Electrical_External_Power_Button,
        Fuel_Wing_Tank_Pump_Left_1_Button,
        Fuel_Wing_Tank_Pump_Left_2_Button,
        Fuel_Center_Tank_Pump_1_Button,
        Fuel_Crossfeed_Button,
        Fuel_Center_Tank_Pump_2_Button,
        Fuel_Wing_Tank_Pump_Right_1_Button,
        Fuel_Wing_Tank_Pump_Right_2_Button,

        Elect_Bus_Power_AC_1,

        //Numeric displays
        Battery_Voltage_1,
        Battery_Voltage_2,

        /// <summary>
        /// RMP
        /// </summary>
        /// 
        //Momentary
        RMP1_VHF_1_Button,
        RMP1_VHF_2_Button,
        RMP1_VHF_3_Button,
        RMP1_HF_1_Button,
        RMP1_HF_2_Button,
        RMP1_AM_Button,
        RMP1_NAV_Button,
        RMP1_VOR_Button,
        RMP1_LS_Button,
        RMP1_GLS_Button,
        RMP1_ADF_Button,
        RMP1_BFO_Button,
        RMP1_Transfer_Button,

        //Displays
        RMP1_ActiveFreq,
        RMP1_StandbyFreq,

        //Programs
        RMP1_Frequency_Inner_Knob_Inc,
        RMP1_Frequency_Inner_Knob_Dec,
        RMP1_Frequency_Outer_Knob_Inc,
        RMP1_Frequency_Outer_Knob_Dec,

        // Latch
        RMP1_Power_Switch,

        /// <summary>
        /// Engine Start Panel
        /// </summary>
        /// 
        Throttle_Engine_1_Master_Switch,
        Throttle_Engine_2_Master_Switch,
        Throttle_Engine_Mode_Knob,
        Throttle_Engine_Fire_1_Annunciator, 
        Throttle_Engine_Fire_2_Annunciator,
        Throttle_Engine_Fault_1_Annunciator,
        Throttle_Engine_Fault_2_Annunciator,

        TestEvent_DoNotUse
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

        public const double ButtonLatch = 1;
        public const double ButtonUnlatch = 0;

        public const double On = 1;
        public const double Off = 0;

        public const double None = 0;

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
