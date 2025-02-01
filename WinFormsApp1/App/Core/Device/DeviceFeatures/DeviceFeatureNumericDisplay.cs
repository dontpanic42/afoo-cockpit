using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.DataSource;

namespace AFooCockpit.App.Core.Device.DeviceFeatures
{
    internal abstract class DeviceFeatureNumericDisplay<C, T> : DeviceFeature<C, T>
        where C : DeviceFeatureConfig
        where T : IDataSource
    {
        public DeviceFeatureNumericDisplay(C config) : base(config)
        {
            Config.FlightDataEventBus.FlightEvent(Config.FlightDataEvent).OnDataReceived += DeviceFeatureNumericDisplay_OnDataReceived;
        }

        private void DeviceFeatureNumericDisplay_OnDataReceived(FlightData.FlightDataEventBus bus, FlightData.FlightDataEventArgs eventArgs)
        {
            ShowNumber(eventArgs.Data);
        }

        /// <summary>
        /// Shows a number on the given display
        /// </summary>
        /// <param name="number"></param>
        protected abstract void ShowNumber(double number);

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
