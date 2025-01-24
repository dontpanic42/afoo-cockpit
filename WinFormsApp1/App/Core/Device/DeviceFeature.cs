using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.DataSource;
using AFooCockpit.App.Core.FlightData;

namespace AFooCockpit.App.Core.Device
{
    internal abstract class DeviceFeature<C, T> : IDeviceFeature
        where C : DeviceFeatureConfig
        where T : IDataSource
    {

        /// <summary>
        /// Flag that is true when ConnectDataSource was called
        /// Used to prevent multiple data sources to be conncted to the same 
        /// Device Feature
        /// </summary>
        private bool HasConnectedDataSource = false;

        private readonly string DeviceFeatureName;
        public string Name => DeviceFeatureName;

        /// <summary>
        /// The FlightDataEvent that this feature is listening for or sending data for
        /// </summary>
        private readonly FlightDataEvent FeatureFlightDataEvent;
        public FlightDataEvent FlightDataEvent => FeatureFlightDataEvent;

        /// <summary>
        /// The Fligh Data Bus that this feature is listening on or sending events to
        /// </summary>
        private readonly FlightDataEventBus FeatureFlightDataEventBus;
        public FlightDataEventBus FlightDataEventBus => FeatureFlightDataEventBus;

        /// <summary>
        /// Holds the feature device specific configuration
        /// </summary>
        public C Config { get; private set; }

        public DeviceFeature(C config)
        {
            DeviceFeatureName = config.DeviceFeatureName;
            FeatureFlightDataEvent = config.FlightDataEvent;
            FeatureFlightDataEventBus = config.FlightDataEventBus;
            Config = config;
        }

        /// <summary>
        /// Connects a new data source to this Device Feature
        /// </summary>
        /// <param name="dataSource"></param>
        public void ConnectDataSource(T dataSource)
        {
            if (HasConnectedDataSource)
            {
                throw new Exception("Cannot onnect data source - there is already a datasource connected to this device feature");
            }

            dataSource.OnStateEvent += DataSource_OnStateEvent;
            // If the data source is already connected, we will not receive a
            // 'connected' event, so we need to fake it
            if (dataSource.State == SourceState.Connected)
            {
                DataSourceConnected(dataSource);
            }

            HasConnectedDataSource = true;
        }

        /// <summary>
        /// Event handler that calls the internal DataSourceConnected/DataSourceDisconnected 
        /// methods
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void DataSource_OnStateEvent(IDataSource sender, StateEventArgs eventArgs)
        {
            switch (eventArgs.State) {
                case SourceState.Connected:
                    DataSourceConnected((T) sender);
                    break;
                case SourceState.Disconnected:
                    DataSourceDisconnected((T) sender);
                    break;
                }
        }

        protected abstract void DataSourceConnected(T dataSource); 
        protected abstract void DataSourceDisconnected(T dataSource);
    }
}
