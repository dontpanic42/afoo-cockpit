using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.DataSource.DataSources.Sim.VariableSource;
using AFooCockpit.App.Core.FlightData;
using NLog;

namespace AFooCockpit.App.Core.Aircraft
{
    internal abstract class Aircraft
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Friendly name of this Aircraft
        /// </summary>
        public string Name { get; private set; }

        protected FlightDataEventBus EventBus { get; private set; }

        protected FlightSimVariableDataSource VariableDataSource { get; private set; }

        /// <summary>
        /// Holds mappsing in the form of flight sim variable -> FlightDataEvent
        /// </summary>
        protected readonly AircraftVariableMap Source2BusMap = new AircraftVariableMap();

        /// <summary>
        /// Holds mappings in the form of FlightDataEvent -> flight sim variable
        /// </summary>
        protected readonly AircraftVariableMap Bus2SourceMap = new AircraftVariableMap();

        public Aircraft(string aircraftName, FlightDataEventBus eventBus, FlightSimVariableDataSource variableDataSource)
        {
            Name = aircraftName;
            VariableDataSource = variableDataSource;

            // Setup event bus
            EventBus = eventBus;
            // When a mapping is added, sign up for bus notification for the given mapping
            Bus2SourceMap.OnAddVariableMapping += Bus2SourceMap_OnAddVariableMapping;
            // Register bus events for each existing mapping
            Bus2SourceMap.GetFlightDataEvents().ForEach(e => EventBus.FlightEvent(e).OnDataReceived += EventBus_OnDataReceived);

            // When a new mapping is added, register it with the data source
            Source2BusMap.OnAddVariableMapping += Source2BusMap_OnAddVariableMapping;
            // Register all existing variables
            VariableDataSource.RequestFlightVariables(Source2BusMap.GetVariables());
            // Register for data source received data event
            VariableDataSource.OnDataReceive += VariableDataSource_OnDataReceive;
        }

        /// <summary>
        /// Executed when a new bus mapping is added, signs up for the variable on the bus
        /// </summary>
        /// <param name="map"></param>
        /// <param name="args"></param>
        private void Bus2SourceMap_OnAddVariableMapping(AircraftVariableMap map, AircraftVariableMapEventArgs args)
        {
            EventBus.FlightEvent(args.FlightDataEvent).OnDataReceived += EventBus_OnDataReceived;
        }


        /// <summary>
        /// When we receive data from the bus, we transform it into our local format and send it to the source
        /// </summary>
        /// <param name="bus"></param>
        /// <param name="eventArgs"></param>
        private void EventBus_OnDataReceived(FlightDataEventBus bus, FlightDataEventArgs eventArgs)
        {
            var variableName = Bus2SourceMap.GetVariable(eventArgs.Event);
            var variableValue = eventArgs.Data;

            // Were ignoring bus events that come in before the data source was connected
            // TODO: Cache/Queue bus events?
            if(VariableDataSource.State == DataSource.SourceState.Connected)
            {
                var data = new FlightSimVariableDataSourceData { VariableName = variableName, VariableValue = variableValue };
                VariableDataSource.Send(data);
            }
        }

        /// <summary>
        /// Receives a data event from the data source and sends it to the event bus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VariableDataSource_OnDataReceive(DataSource.DataSource<DataSource.DataSourceConfig, FlightSimVariableDataSourceData> sender, DataSource.DataSourceReciveEventArgs<FlightSimVariableDataSourceData> e)
        {
            // Check if this is a known variable
            if(Source2BusMap.ContainsVariable(e.Data.VariableName))
            {
                // Get the flight event for the variable name
                var flightEvent = Source2BusMap.GetFlightEvent(e.Data.VariableName);

                // Construct flight data event args
                var flightDataEventArgs = new FlightDataEventArgs
                {
                    SenderName = Name,
                    Event = flightEvent,
                    Data = e.Data.VariableValue
                };

                // Put the event on the bus
                EventBus.TriggerDataEvent(flightEvent, flightDataEventArgs);
            }
            else
            {
                logger.Warn($"Received event from aircraft data source, but variable is unknown: {e.Data.VariableName}");
            }
        }

        /// <summary>
        /// When a new mapping is added, register it with the datasource
        /// </summary>
        /// <param name="map"></param>
        /// <param name="args"></param>
        private void Source2BusMap_OnAddVariableMapping(AircraftVariableMap map, AircraftVariableMapEventArgs args)
        {
            VariableDataSource.RequestFlightVariable(args.VariableName);
        }

        /// <summary>
        /// Method that triggers an event for each registered variable - used to sync e.g. state lights when
        /// starting the application after the simulation has started (and we're therefore missing events)
        /// </summary>
        public void ForceSync()
        {
            VariableDataSource.ForceSync();
        }
    }
}
