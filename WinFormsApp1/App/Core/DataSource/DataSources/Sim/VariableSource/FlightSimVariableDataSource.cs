using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.FlightData;

namespace AFooCockpit.App.Core.DataSource.DataSources.Sim.VariableSource
{
    internal abstract class FlightSimVariableDataSource : DataSource<DataSourceConfig, FlightSimVariableDataSourceData>
    {
        /// <summary>
        /// List of variables that are monitored
        /// </summary>
        private List<string> RequestedVariables = new List<string>();

        public FlightSimVariableDataSource() : base(new DataSourceConfig())
        {
            OnStateEvent += FlightSimVariableDataSource_OnStateEvent;
        }

        private void FlightSimVariableDataSource_OnStateEvent(IDataSource sender, StateEventArgs eventArgs)
        {
            switch (eventArgs.State)
            {
                case SourceState.Connected: 
                    RequestedVariables.ForEach(RegisterFlightVariable); 
                    break;

                case SourceState.Disconnected: 
                    RequestedVariables.ForEach(UnregisterFlightVariable);
                    RequestedVariables.Clear();
                    break;
            }
        }

        public void ReqeustFlightVariables(string[] variables)
        {
            foreach (string variable in variables)
            {
                RequestFlightVariable(variable);
            }
        }

        /// <summary>
        /// Registers a new request for a flight data event
        /// 
        /// Since we cannot always update all sim variables, we need to know which ones we'll need to monitor.
        /// For that, we ask dependant components (=> Aircraft) to register their data requriements with us 
        /// </summary>
        /// <param name="variable"></param>
        public void RequestFlightVariable(string variable)
        {
            // If the event is already registered, no need to add it again
            if (RequestedVariables.Contains(variable))
            {
                return;
            }

            RequestedVariables.Add(variable);

            // If we're already connected, we can process the request immediately
            if (State == SourceState.Connected)
            {
                RegisterFlightVariable(variable);
            }
        }

        /// <summary>
        /// Registers a flight data event for monitoring in the data source.
        /// Is guaranteed to be called when the source is connected
        /// </summary>
        /// <param name="variable"></param>
        protected abstract void RegisterFlightVariable(string variable);

        /// <summary>
        /// Pendant to RegisterFlightVariable - unregisters a sim variable
        /// This method is usually called after disconnect
        /// </summary>
        /// <param name="variable"></param>
        protected abstract void UnregisterFlightVariable(string variable);
    }
}
