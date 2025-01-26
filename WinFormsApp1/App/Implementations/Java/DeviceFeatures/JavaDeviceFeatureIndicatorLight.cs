using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.DataSource.DataSources.Serial;
using AFooCockpit.App.Core.Device.DeviceFeatures;

namespace AFooCockpit.App.Implementations.Java.DeviceFeatures
{
    internal class JavaDeviceFeatureIndicatorLight : DeviceFeatureIndicatorLight<JavaDeviceFeatureConfig, SerialDataSource>
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private static readonly string LIGHT_ON = "1";
        private static readonly string LIGHT_OFF = "0";

        public JavaDeviceFeatureIndicatorLight(JavaDeviceFeatureConfig config) : base(config)
        {
        }

        protected override void TurnOff()
        {
            if (DataSource != null)
            {
                var cmd = $"{Config.SerialEvent},{LIGHT_OFF};";
                logger.Debug($"Turning off light");
                DataSource.Send(new SerialDataSourceData { Line = cmd });
            }
        }

        protected override void TurnOn()
        {
            if (DataSource != null)
            {
                logger.Debug($"Turning on light");
                var cmd = $"{Config.SerialEvent},{LIGHT_ON};";
                DataSource.Send(new SerialDataSourceData { Line = cmd });
            }
        }
    }
}
