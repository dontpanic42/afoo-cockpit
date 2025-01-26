using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.DataSource;
using AFooCockpit.App.Core.FlightData;

namespace AFooCockpit.App.Core.Device.DeviceFeatures
{
    /// <summary>
    /// In contrast to device indicators, device features have a datasource hat inputs events
    /// </summary>
    /// <typeparam name="C"></typeparam>
    /// <typeparam name="T"></typeparam>
    internal abstract class DeviceFeature<C, T> : IDeviceFeature
        where C : DeviceFeatureConfig
        where T : IDataSource
    {


        /// <summary>
        /// Returns a friendly name for the device feature
        /// </summary>
        public string Name => Config.DeviceFeatureName ;

        /// <summary>
        /// Stores the configuration object for the feature
        /// </summary>
        protected C Config { get; private set; }

        /// <summary>
        /// Accessor for current data source. Can be null when not yet initialized
        /// with a datasource
        /// </summary>
        protected T? DataSource { get; private set; } = default(T);

        public DeviceFeature(C config)
        {
            Config = config;
        }

        /// <summary>
        /// Connects a new data source to this Device Feature
        /// </summary>
        /// <param name="dataSource"></param>
        public void ConnectDataSource(T dataSource)
        {
            if (DataSource != null)
            {
                // Safeguard against multiple data source connections
                return;
            }

            DataSource = dataSource;

            DataSource.OnStateEvent += DataSource_OnStateEvent;
            // If the data source is already connected, we will not receive a
            // 'connected' event, so we need to fake it
            if (dataSource.State == SourceState.Connected)
            {
                DataSourceConnected(DataSource);
            }
        }

        /// <summary>
        /// Event handler that calls the internal DataSourceConnected/DataSourceDisconnected 
        /// methods
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void DataSource_OnStateEvent(IDataSource sender, StateEventArgs eventArgs)
        {
            switch (eventArgs.State)
            {
                case SourceState.Connected:
                    DataSourceConnected((T)sender);
                    break;
                case SourceState.Disconnected:
                    DataSourceDisconnected((T)sender);
                    break;
            }
        }

        protected abstract void DataSourceConnected(T dataSource);
        protected abstract void DataSourceDisconnected(T dataSource);
    }
}
