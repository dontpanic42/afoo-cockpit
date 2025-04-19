using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.FlightData;
using AFooCockpit.App.Implementations.GenericArduinoDevice.DeviceFeatures;
using AFooCockpit.App.Implementations.GenericArduinoDevice.Devices;

namespace AFooCockpit.App.Implementations.GenericArduinoWxRadar.Devices
{
    internal class GenericArduinoWxRadarDevice : GenericArduinoDevice.Devices.GenericArduinoDevice
    {
        public GenericArduinoWxRadarDevice(FlightDataEventBus flightDataEventBus) : base(nameof(GenericArduinoWxRadarDevice), flightDataEventBus)
        {
            AddDeviceFeature<GenericArduinoIndicatorLightDeviceFeature>(new GenericArduinoDeviceFeatureConfig
            {
                DeviceFeatureName = "Panel Backlight",
                FlightDataEvent = FlightDataEvent.Elect_Bus_Power_AC_1,
                FlightDataEventBus = flightDataEventBus,
                PinId = "PANEL_BACKLIGHT_RELAY"
            });
        }
    }
}
