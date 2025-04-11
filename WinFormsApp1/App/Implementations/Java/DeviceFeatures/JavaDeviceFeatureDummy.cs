using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.DataSource;
using AFooCockpit.App.Core.DataSource.DataSources.Serial;
using AFooCockpit.App.Core.Device.DeviceFeatures;
using AFooCockpit.App.Core.FlightData;

namespace AFooCockpit.App.Implementations.Java.DeviceFeatures
{
    internal class JavaDeviceFeatureDummy : DeviceFeatureDummy<JavaDeviceFeatureConfig, SerialDataSource>
    {

        public JavaDeviceFeatureDummy(JavaDeviceFeatureConfig deviceFeatureConfig) : base(deviceFeatureConfig)
        {
        }

        protected override void DataSourceConnected(SerialDataSource dataSource)
        {
            dataSource.OnDataReceive += DataSource_OnDataReceive;
        }

        private void DataSource_OnDataReceive(DataSource<SerialDataSourceConfig, SerialDataSourceData> sender, DataSourceReciveEventArgs<SerialDataSourceData> e)
        {
            var eventName = $"{Config.SerialEvent},";
            if (e.Data.Line.Equals(eventName))
            {
                SendEvent();
            }
        }

        protected override void DataSourceDisconnected(SerialDataSource dataSource)
        {
            dataSource.OnDataReceive -= DataSource_OnDataReceive;
        }
    }
}
