using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.DataSource.DataSources.Serial;
using AFooCockpit.App.Core.DataSource;
using AFooCockpit.App.Core.Device.DeviceFeatures;

namespace AFooCockpit.App.Implementations.Java.DeviceFeatures
{
    internal class JavaDeviceFeatureSelectorSwitch : DeviceFeatureSelectorSwitch<JavaDeviceFeatureConfig, SerialDataSource>
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private string SerialEvent;

        private int[] Map3Pos = [0, 1, 2];

        public JavaDeviceFeatureSelectorSwitch(JavaDeviceFeatureConfig deviceFeatureConfig) : base(deviceFeatureConfig)
        {
            SerialEvent = deviceFeatureConfig.SerialEvent;
        }

        protected override void DataSourceConnected(SerialDataSource dataSource)
        {
            dataSource.OnDataReceive += DataSource_OnDataReceive;
        }

        private void DataSource_OnDataReceive(DataSource<SerialDataSourceConfig, SerialDataSourceData> sender, DataSourceReciveEventArgs<SerialDataSourceData> e)
        {
            // Line should be something like "AB01,1"
            var split = e.Data.Line.Split(',');
            if (split.Length == 2 && split[0].Equals(SerialEvent))
            {
                int value = 0;
                if (int.TryParse(split[1], out value))
                {
                    // If this is a 3 position switch, we need to remap the values
                    if (Config.Is3PosSwitch && value >= 0 && value <= 3)
                    {
                        value = Map3Pos[value];
                    }

                    SendSelect(Convert.ToDouble(value));
                }
                else
                {
                    logger.Warn($"Selector switch {Name} - got event, but could not parse event value {split[1]}");
                }
            }
        }

        protected override void DataSourceDisconnected(SerialDataSource dataSource)
        {
            dataSource.OnDataReceive -= DataSource_OnDataReceive;
        }
    }
}
