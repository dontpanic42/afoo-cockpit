using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.Aircraft;
using AFooCockpit.App.Core.DataSource.DataSources.Sim.VariableSource;
using AFooCockpit.App.Core.FlightData;

namespace AFooCockpit.App.Implementations.Aircraft
{
    internal class FenixA3XX : Core.Aircraft.Aircraft
    {
        public FenixA3XX(FlightDataEventBus eventBus, FlightSimVariableDataSource variableDataSource) : base("Fenix Simulations A3xx", eventBus, variableDataSource)
        {

            // Variables that we are interested in, coming from the flight simulator
            Source2BusMap.Add([
                ("I_ECAM_ENGINE",       FlightDataEvent.ECAM_ENG_Button_Annunciator),
                ("I_ECAM_APU",          FlightDataEvent.ECAM_APU_Button_Annunciator),
                ("I_ECAM_CLR_LEFT",     FlightDataEvent.ECAM_CLR_Left_Button_Annunciator),
                ("I_ECAM_BLEED",        FlightDataEvent.ECAM_BLEED_Button_Annunciator),
                ("I_ECAM_COND",         FlightDataEvent.ECAM_COND_Button_Annunciator),
                ("I_ECAM_CAB_PRESS",    FlightDataEvent.ECAM_PRESS_Button_Annunciator),
                ("I_ECAM_DOOR",         FlightDataEvent.ECAM_DOOR_Button_Annunciator),
                ("I_ECAM_STATUS",       FlightDataEvent.ECAM_STS_Button_Annunciator),
                ("I_ECAM_ELEC",         FlightDataEvent.ECAM_ELEC_Button_Annunciator),
                ("I_ECAM_WHEEL",        FlightDataEvent.ECAM_WHEEL_Button_Annunciator),
                ("I_ECAM_HYD",          FlightDataEvent.ECAM_HYD_Button_Annunciator),
                ("I_ECAM_FCTL",         FlightDataEvent.ECAM_FCTL_Button_Annunciator),
                ("I_ECAM_FUEL",         FlightDataEvent.ECAM_FUEL_Button_Annunciator),
                ("I_ECAM_CLR_RIGHT",    FlightDataEvent.ECAM_CLR_Right_Button_Annunciator),

                ("I_OH_NAV_IR1_SWITCH_L", FlightDataEvent.ADIRS_IR_1_Annunciator_Lower),
                ("I_OH_NAV_IR1_SWITCH_U", FlightDataEvent.ADIRS_IR_1_Annunciator_Upper),
                ("I_OH_NAV_IR2_SWITCH_L", FlightDataEvent.ADIRS_IR_2_Annunciator_Lower),
                ("I_OH_NAV_IR2_SWITCH_U", FlightDataEvent.ADIRS_IR_2_Annunciator_Upper),
                ("I_OH_NAV_IR3_SWITCH_L", FlightDataEvent.ADIRS_IR_3_Annunciator_Lower),
                ("I_OH_NAV_IR3_SWITCH_U", FlightDataEvent.ADIRS_IR_3_Annunciator_Upper),

                ("I_OH_ELEC_APU_MASTER_L", FlightDataEvent.APU_Master_Button_Annunciator_Lower),
                ("I_OH_ELEC_APU_MASTER_U", FlightDataEvent.APU_Master_Button_Annunciator_Upper),
                ("I_OH_ELEC_APU_START_L", FlightDataEvent.APU_Start_Button_Annunciator_Lower),
                ("I_OH_ELEC_APU_START_U", FlightDataEvent.APU_Start_Button_Annunciator_Upper),

                ("I_OH_PNEUMATIC_ENG1_ANTI_ICE_L", FlightDataEvent.Icing_Engine_1_Button_Annunciator_Lower),
                ("I_OH_PNEUMATIC_ENG1_ANTI_ICE_U", FlightDataEvent.Icing_Engine_1_Button_Annunciator_Upper),
                ("I_OH_PNEUMATIC_ENG2_ANTI_ICE_L", FlightDataEvent.Icing_Engine_2_Button_Annunciator_Lower),
                ("I_OH_PNEUMATIC_ENG2_ANTI_ICE_U", FlightDataEvent.Icing_Engine_2_Button_Annunciator_Upper),
                ("I_OH_PNEUMATIC_WING_ANTI_ICE_L", FlightDataEvent.Icing_Wing_Button_Annunciator_Lower),
                ("I_OH_PNEUMATIC_WING_ANTI_ICE_U", FlightDataEvent.Icing_Wing_Button_Annunciator_Upper),

                ("I_OH_OXYGEN_CREW_OXYGEN_L", FlightDataEvent.Oxygen_Crew_Button_Annunciator),

                ("I_OH_PNEUMATIC_PACK_1_L", FlightDataEvent.Pneumatic_Pack_1_Button_Annunciator_Lower),
                ("I_OH_PNEUMATIC_PACK_1_U", FlightDataEvent.Pneumatic_Pack_1_Button_Annunciator_Upper),
                ("I_OH_PNEUMATIC_PACK_2_L", FlightDataEvent.Pneumatic_Pack_2_Button_Annunciator_Lower),
                ("I_OH_PNEUMATIC_PACK_2_U", FlightDataEvent.Pneumatic_Pack_2_Button_Annunciator_Upper),
                ("I_OH_PNEUMATIC_APU_BLEED_L", FlightDataEvent.Pneumatic_APU_Bleed_Button_Annunciator_Lower),
                ("I_OH_PNEUMATIC_APU_BLEED_U", FlightDataEvent.Pneumatic_APU_Bleed_Button_Annunciator_Upper),

                ("I_OH_GPWS_LDG_FLAP3_L", FlightDataEvent.GPWS_Landing_Flap_3_Button_Annunciator),

                ("I_OH_ELEC_BAT1_L", FlightDataEvent.Electrical_Battery_1_Button_Annunciator_Lower),
                ("I_OH_ELEC_BAT1_U", FlightDataEvent.Electrical_Battery_1_Button_Annunciator_Upper),
                ("I_OH_ELEC_BAT2_L", FlightDataEvent.Electrical_Battery_2_Button_Annunciator_Lower),
                ("I_OH_ELEC_BAT2_U", FlightDataEvent.Electrical_Battery_2_Button_Annunciator_Upper),
                ("I_OH_ELEC_EXT_PWR_L", FlightDataEvent.Electrical_External_Power_Button_Annunciator_Lower),
                ("I_OH_ELEC_EXT_PWR_U", FlightDataEvent.Electrical_External_Power_Button_Annunciator_Upper),

                ("I_OH_FUEL_CENTER_1_L", FlightDataEvent.Fuel_Center_Tank_Pump_1_Button_Annunciator_Lower),
                ("I_OH_FUEL_CENTER_1_U", FlightDataEvent.Fuel_Center_Tank_Pump_1_Button_Annunciator_Upper),
                ("I_OH_FUEL_CENTER_2_L", FlightDataEvent.Fuel_Center_Tank_Pump_2_Button_Annunciator_Lower),
                ("I_OH_FUEL_CENTER_2_U", FlightDataEvent.Fuel_Center_Tank_Pump_2_Button_Annunciator_Upper),
                ("I_OH_FUEL_XFEED_L", FlightDataEvent.Fuel_Crossfeed_Button_Annunciator_Lower),
                ("I_OH_FUEL_XFEED_U", FlightDataEvent.Fuel_Crossfeed_Button_Annunciator_Upper),
            ]);

            // Variables that we are interestd in, coming from the event bus
            Bus2SourceMap.Add([
                ("S_ECAM_ENGINE",        FlightDataEvent.ECAM_ENG_Button),
                ("S_ECAM_APU",           FlightDataEvent.ECAM_APU_Button),
                ("S_ECAM_CLR_LEFT",      FlightDataEvent.ECAM_CLR_Left_Button),
                ("S_ECAM_TO",            FlightDataEvent.ECAM_TO_CONFIG_Button),
                ("S_ECAM_BLEED",         FlightDataEvent.ECAM_BLEED_Button),
                ("S_ECAM_COND",          FlightDataEvent.ECAM_COND_Button),
                ("S_ECAM_CAB_PRESS",     FlightDataEvent.ECAM_PRESS_Button),
                ("S_ECAM_DOOR",          FlightDataEvent.ECAM_DOOR_Button),
                ("S_ECAM_STATUS",        FlightDataEvent.ECAM_STS_Button),
                ("S_ECAM_ELEC",          FlightDataEvent.ECAM_ELEC_Button),
                ("S_ECAM_WHEEL",         FlightDataEvent.ECAM_WHEEL_Button),
                ("S_ECAM_RCL",           FlightDataEvent.ECAM_RCL_Button),
                ("S_ECAM_EMER_CANCEL",   FlightDataEvent.ECAM_EMER_CANC_Button),
                ("S_ECAM_HYD",           FlightDataEvent.ECAM_HYD_Button),
                ("S_ECAM_FCTL",          FlightDataEvent.ECAM_FCTL_Button),
                ("S_ECAM_FUEL",          FlightDataEvent.ECAM_FUEL_Button),
                ("S_ECAM_ALL",           FlightDataEvent.ECAM_ALL_Button),
                ("S_ECAM_CLR_RIGHT",     FlightDataEvent.ECAM_CLR_Right_Button),
                ("S_DISPLAY_ATT_HDG",    FlightDataEvent.Switching_ATT_HDG_Knob),
                ("S_DISPLAY_AIR_DATA",   FlightDataEvent.Switching_AIR_DATA_Knob),
                ("S_DISPLAY_EIS_DMC",    FlightDataEvent.Switching_EIS_DMC_Knob),
                ("S_DISPLAY_ECAM_ND_XFR",FlightDataEvent.Switching_ECAM_ND_XFR_Knob),

                ("S_OH_EXT_LT_STROBE",      FlightDataEvent.ExteriorLight_Strobe_Switch),
                ("S_OH_EXT_LT_BEACON",      FlightDataEvent.ExteriorLight_Beacon_Switch),
                ("S_OH_EXT_LT_WING",        FlightDataEvent.ExteriorLight_Wing_Switch),
                ("S_OH_EXT_LT_NAV_LOGO",    FlightDataEvent.ExteriorLight_Navigation_Switch),
                ("S_OH_EXT_LT_RWY_TURNOFF", FlightDataEvent.ExteriorLight_Runway_Turnoff_Switch),
                ("S_OH_EXT_LT_LANDING_L",   FlightDataEvent.ExteriorLight_Landing_L_Switch),
                ("S_OH_EXT_LT_LANDING_R",   FlightDataEvent.ExteriorLight_Landing_R_Switch),
                ("S_OH_EXT_LT_NOSE",        FlightDataEvent.ExteriorLight_Nose_Switch),

                ("S_OH_SIGNS",          FlightDataEvent.Sign_Seat_Belts_Switch),
                ("S_OH_SIGNS_SMOKING",  FlightDataEvent.Sign_No_Smoking_Switch),
                ("S_OH_INT_LT_EMER",    FlightDataEvent.Sign_Emergency_Exit_Switch),
            ]);
        }
    }
}
