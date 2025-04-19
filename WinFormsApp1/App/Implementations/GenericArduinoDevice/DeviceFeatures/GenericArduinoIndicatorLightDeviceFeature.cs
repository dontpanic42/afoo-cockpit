using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.DataSource.DataSources.GenericArduino;
using AFooCockpit.App.Core.Device.DeviceFeatures;

namespace AFooCockpit.App.Implementations.GenericArduinoDevice.DeviceFeatures
{
    internal class GenericArduinoIndicatorLightDeviceFeature : DeviceFeatureIndicatorLight<GenericArduinoDeviceFeatureConfig, GenericArduinoDataSource>
    {
        public GenericArduinoIndicatorLightDeviceFeature(GenericArduinoDeviceFeatureConfig config) : base(config)
        {
        }

        protected override void TurnOff()
        {
            DataSource?.Send(new GenericArduinoDataSourceData
            {
                PinId = Config.PinId,
                Value = 0
            });
        }

        protected override void TurnOn()
        {
            DataSource?.Send(new GenericArduinoDataSourceData
            {
                PinId = Config.PinId,
                Value = 1
            });
        }
    }
}
