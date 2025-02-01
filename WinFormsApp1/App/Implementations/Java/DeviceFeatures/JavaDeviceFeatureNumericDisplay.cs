using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.DataSource.DataSources.Serial;
using AFooCockpit.App.Core.Device.DeviceFeatures;
using NLog;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AFooCockpit.App.Implementations.Java.DeviceFeatures
{
    internal class JavaDeviceFeatureNumericDisplay : DeviceFeatureNumericDisplay<JavaDeviceFeatureConfig, SerialDataSource>
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private static readonly double MULTIPLIER = 10.0;

        public JavaDeviceFeatureNumericDisplay(JavaDeviceFeatureConfig config) : base(config)
        {
        }

        protected override void ShowNumber(double number)
        {
            if (DataSource != null)
            {
                double data = number * MULTIPLIER;
                string cmd = $"{Config.SerialEvent},{FormatValue(data)};";
                logger.Debug($"Display command {cmd}");
                DataSource.Send(new SerialDataSourceData { Line = cmd });
            }
        }

        private string FormatValue(double value)
        {
            return value.ToString("#.");
        }
    }
}
