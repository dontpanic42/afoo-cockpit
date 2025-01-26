using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.FlightData;

namespace AFooCockpit.App.Core.Aircraft
{
    delegate void AddVariableMappingHandler(AircraftVariableMap map, AircraftVariableMapEventArgs args);

    internal class AircraftVariableMap
    {
        private Dictionary<string, FlightDataEvent> Forward = new Dictionary<string, FlightDataEvent>();
        private Dictionary<FlightDataEvent, string> Reverse = new Dictionary<FlightDataEvent, string>();

        public event AddVariableMappingHandler? OnAddVariableMapping;

        public AircraftVariableMap() 
        { 
        }

        public void Add((string, FlightDataEvent)[] mapping)
        {
            foreach (var mappingItem in mapping)
            {
                Add(mappingItem);
            }
        }

        public void Add((string, FlightDataEvent) mapping)
        {
            Add(mapping.Item1, mapping.Item2);
        }

        public void Add(string variable, FlightDataEvent flightDataEvent)
        {
            Forward.Add(variable, flightDataEvent);
            Reverse.Add(flightDataEvent, variable);
            OnAddVariableMapping?.Invoke(this, new AircraftVariableMapEventArgs { FlightDataEvent = flightDataEvent, VariableName = variable });
        }

        public bool ContainsVariable(string variable)
        {
            return Forward.ContainsKey(variable);
        }

        public bool ContainsFlightDataEvent(FlightDataEvent flightDataEvent)
        {
            return Reverse.ContainsKey(flightDataEvent);
        }

        /// <summary>
        /// Returns a FlightDataEvent for a given variable
        /// </summary>
        /// <param name="variable"></param>
        /// <returns></returns>
        public FlightDataEvent GetFlightEvent(string variable)
        {
            return Forward[variable];
        }

        /// <summary>
        /// Returns a list of flight data events that we have mappings for
        /// </summary>
        /// <returns></returns>
        public List<FlightDataEvent> GetFlightDataEvents()
        {
            return Forward.Values.ToList();
        }

        /// <summary>
        /// Returns a variable for the given FlightDataEvent
        /// </summary>
        /// <param name="flightDataEvent"></param>
        /// <returns></returns>
        public string GetVariable(FlightDataEvent flightDataEvent)
        {
            return Reverse[flightDataEvent];
        }

        /// <summary>
        /// Returns a list of all variables
        /// </summary>
        /// <returns></returns>
        public string[] GetVariables()
        {
            return Forward.Keys.ToArray();
        }
    }
}
