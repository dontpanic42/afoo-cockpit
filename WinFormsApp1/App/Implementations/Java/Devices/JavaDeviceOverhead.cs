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

        private static readonly (FlightDataEvent flightDataEvent, string serialEvent)[] IndicatorLights = [
            // ADIRS
            (FlightDataEvent.ADIRS_IR_1_Annunciator_Lower, "K_L10"),
            (FlightDataEvent.ADIRS_IR_1_Annunciator_Upper, "K_U10"),
            (FlightDataEvent.ADIRS_IR_2_Annunciator_Lower, "K_L11"),
            (FlightDataEvent.ADIRS_IR_2_Annunciator_Upper, "K_U11"),
            (FlightDataEvent.ADIRS_IR_3_Annunciator_Lower, "K_L12"),
            (FlightDataEvent.ADIRS_IR_3_Annunciator_Upper, "K_U12"),

            // APU
            (FlightDataEvent.APU_Master_Button_Annunciator_Lower, "K_L2"),
            (FlightDataEvent.APU_Master_Button_Annunciator_Upper, "K_U2"),
            (FlightDataEvent.APU_Start_Button_Annunciator_Lower, "K_L1"),
            (FlightDataEvent.APU_Start_Button_Annunciator_Upper, "K_U1"),

            // ANTI ICE
            (FlightDataEvent.Icing_Engine_1_Button_Annunciator_Lower, "K_L4"),
            (FlightDataEvent.Icing_Engine_1_Button_Annunciator_Upper, "K_U4"),
            (FlightDataEvent.Icing_Engine_2_Button_Annunciator_Lower, "K_L5"),
            (FlightDataEvent.Icing_Engine_2_Button_Annunciator_Upper, "K_U5"),
            (FlightDataEvent.Icing_Wing_Button_Annunciator_Lower, "K_L3"),
            (FlightDataEvent.Icing_Wing_Button_Annunciator_Upper, "K_U3"),

            // Oxygen
            // TBD: Upper
            (FlightDataEvent.Oxygen_Crew_Button_Annunciator, "K_L6"),

            // Air Cond
            (FlightDataEvent.Pneumatic_Pack_1_Button_Annunciator_Lower, "K_L7"),
            (FlightDataEvent.Pneumatic_Pack_1_Button_Annunciator_Upper, "K_U7"),
            (FlightDataEvent.Pneumatic_Pack_2_Button_Annunciator_Lower, "K_L9"),
            (FlightDataEvent.Pneumatic_Pack_2_Button_Annunciator_Upper, "K_U9"),
            (FlightDataEvent.Pneumatic_APU_Bleed_Button_Annunciator_Lower, "K_L8"),
            (FlightDataEvent.Pneumatic_APU_Bleed_Button_Annunciator_Upper, "K_U8"),

            // GPWS
            // TBD: Upper
            (FlightDataEvent.GPWS_Landing_Flap_3_Button_Annunciator, "K_L8"),

            // ELEC
            (FlightDataEvent.Electrical_Battery_1_Button_Annunciator_Lower, "K_L14"),
            (FlightDataEvent.Electrical_Battery_1_Button_Annunciator_Upper, "K_U14"),
            (FlightDataEvent.Electrical_Battery_2_Button_Annunciator_Lower, "K_L15"),
            (FlightDataEvent.Electrical_Battery_2_Button_Annunciator_Upper, "K_U15"),
            (FlightDataEvent.Electrical_External_Power_Button_Annunciator_Lower, "K_L16"),
            (FlightDataEvent.Electrical_External_Power_Button_Annunciator_Upper, "K_U16"),

            // FUEL
            // TBD: L TK PUMP
            // TBD: R TK PUMP
            (FlightDataEvent.Fuel_Center_Tank_Pump_1_Button_Annunciator_Lower, "K_L19"),
            (FlightDataEvent.Fuel_Center_Tank_Pump_1_Button_Annunciator_Upper, "K_U19"),
            (FlightDataEvent.Fuel_Center_Tank_Pump_2_Button_Annunciator_Lower, "K_L21"),
            (FlightDataEvent.Fuel_Center_Tank_Pump_2_Button_Annunciator_Upper, "K_U21"),
            (FlightDataEvent.Fuel_Crossfeed_Button_Annunciator_Lower, "K_L20"),
            (FlightDataEvent.Fuel_Crossfeed_Button_Annunciator_Upper, "K_U20"),
        ];

        public JavaDeviceOverhead(FlightDataEventBus flightDataEventBus) : base("Java Overhead Panel", flightDataEventBus)
        {
            AddDeviceFeatures<JavaDeviceFeatureSelectorSwitch>(SelectorSwitches);

            AddDeviceFeatures<JavaDeviceFeatureIndicatorLight>(IndicatorLights);
        }
    }
}
