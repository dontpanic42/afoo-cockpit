using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.DataSource.DataSources.Serial;
using AFooCockpit.App.Core.DataSource;
using FSUIPC;
using AFooCockpit.App.Core.Device.DeviceFeatures;
using NLog;

namespace AFooCockpit.App.Implementations.Java.DeviceFeatures
{
    internal class JavaDeviceFeatureLatchButton : DeviceFeatureLatchButton<JavaDeviceFeatureConfig, SerialDataSource>
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public JavaDeviceFeatureLatchButton(JavaDeviceFeatureConfig deviceFeatureConfig) : base(deviceFeatureConfig)
        {
        }

        protected override void DataSourceConnected(SerialDataSource dataSource)
        {
            dataSource.OnDataReceive += DataSource_OnDataReceive;
        }

        private void DataSource_OnDataReceive(DataSource<SerialDataSourceConfig, SerialDataSourceData> sender, DataSourceReciveEventArgs<SerialDataSourceData> e)
        {
            // Line should be something like "AB01,1"
            // If the number after the comma is a 1, it's pressed
            // If the number after the comma is a 0, it's released
            var split = e.Data.Line.Split(',');
            if (split.Length == 2 && split[0].Equals(Config.SerialEvent))
            {
                if (split[1].Equals("1"))
                {
                    OnPress();
                }
                else
                {
                    OnRelease();
                }
            }
        }

        protected override void DataSourceDisconnected(SerialDataSource dataSource)
        {
            dataSource.OnDataReceive -= DataSource_OnDataReceive;
        }
    }
}
