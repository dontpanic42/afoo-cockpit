using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.DataSource.DataSources.GenericArduino;
using AFooCockpit.App.Core.Device.DeviceFeatures;
using static AFooCockpit.App.Core.Utils.ArduinoGenericFirmwareUtils.ArduinoGenericFirmwareUtils;

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
                Value = PinState.Off
            });
        }

        protected override void TurnOn()
        {
            DataSource?.Send(new GenericArduinoDataSourceData
            {
                PinId = Config.PinId,
                Value = PinState.On
            });
        }
    }
}
