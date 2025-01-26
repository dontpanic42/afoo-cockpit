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
                ("I_ECAM_CLR_RIGHT",    FlightDataEvent.ECAM_CLR_Right_Button_Annunciator)
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
                ("S_DISPLAY_ECAM_ND_XFR",FlightDataEvent.Switching_ECAM_ND_XFR_Knob)
            ]);
        }
    }
}
