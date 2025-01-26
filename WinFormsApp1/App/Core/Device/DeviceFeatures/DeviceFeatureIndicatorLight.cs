using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.DataSource;
using AFooCockpit.App.Core.DataSource.DataSources.Serial;
using AFooCockpit.App.Core.FlightData;

namespace AFooCockpit.App.Core.Device.DeviceFeatures
{
    internal abstract class DeviceFeatureIndicatorLight<C, T> : DeviceFeature<C, T>
        where C : DeviceFeatureConfig
        where T : IDataSource
    {

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public DeviceFeatureIndicatorLight(C config) : base(config)
        {
            Config.FlightDataEventBus.FlightEvent(Config.FlightDataEvent).OnDataReceived += DeviceFeatureIndicatorLight_OnDataReceived;
        }

        /// <summary>
        /// Called when we get a flight event from the event bus
        /// </summary>
        /// <param name="bus"></param>
        /// <param name="eventArgs"></param>
        private void DeviceFeatureIndicatorLight_OnDataReceived(FlightDataEventBus bus, FlightDataEventArgs eventArgs)
        {
            logger.Debug($"Indicator received flight data event from {eventArgs.SenderName}");
            switch (eventArgs.Data)
            {
                case FlightDataEventValue.Off: TurnOff(); break;
                case FlightDataEventValue.On: TurnOn(); break;
                default: logger.Warn($"Received flight data event, but event is out of the expected range."); break;
            }
        }

        /// <summary>
        /// Turns the light on
        /// </summary>
        protected abstract void TurnOn();

        /// <summary>
        /// Turns the light off
        /// </summary>
        protected abstract void TurnOff();

        /// <summary>
        /// Stub for data source connections - usually, we don't need to register listeners to the datasource,
        /// so we don't care about the connected/disconnected events
        /// </summary>
        /// <param name="dataSource"></param>
        protected override void DataSourceConnected(T dataSource)
        {
        }

        /// <summary>
        /// Stub for data source connections - usually, we don't need to register listeners to the datasource,
        /// so we don't care about the connected/disconnected events
        /// </summary>
        /// <param name="dataSource"></param>
        protected override void DataSourceDisconnected(T dataSource)
        {
        }
    }
}
