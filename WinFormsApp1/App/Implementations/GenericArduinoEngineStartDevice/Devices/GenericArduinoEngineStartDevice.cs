using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.FlightData;
using AFooCockpit.App.Implementations.GenericArduinoDevice.DeviceFeatures;
using AFooCockpit.App.Implementations.GenericArduinoWxRadar.Devices;

namespace AFooCockpit.App.Implementations.GenericArduinoEngineStartDevice.Devices
{
    internal class GenericArduinoEngineStartDevice : GenericArduinoDevice.Devices.GenericArduinoDevice
    {
        public GenericArduinoEngineStartDevice(FlightDataEventBus flightDataEventBus) : base(nameof(GenericArduinoEngineStartDevice), flightDataEventBus)
        {
            AddDeviceFeature<GenericArduinoIndicatorLightDeviceFeature>(new GenericArduinoDeviceFeatureConfig
            {
                DeviceFeatureName = "Engine1FireIndicator",
                FlightDataEvent = FlightDataEvent.Elect_Bus_Power_AC_1,
                FlightDataEventBus = flightDataEventBus,
                PinId = "ENGINE_1_FIRE_INDICATOR"
            });

            AddDeviceFeature<GenericArduinoIndicatorLightDeviceFeature>(new GenericArduinoDeviceFeatureConfig
            {
                DeviceFeatureName = "Engine2FireIndicator",
                FlightDataEvent = FlightDataEvent.Elect_Bus_Power_AC_1,
                FlightDataEventBus = flightDataEventBus,
                PinId = "ENGINE_2_FIRE_INDICATOR"
            });

            AddDeviceFeature<GenericArduinoIndicatorLightDeviceFeature>(new GenericArduinoDeviceFeatureConfig
            {
                DeviceFeatureName = "Engine1FailIndicator",
                FlightDataEvent = FlightDataEvent.Elect_Bus_Power_AC_1,
                FlightDataEventBus = flightDataEventBus,
                PinId = "ENGINE_1_FAULT_INDICATOR"
            });

            AddDeviceFeature<GenericArduinoIndicatorLightDeviceFeature>(new GenericArduinoDeviceFeatureConfig
            {
                DeviceFeatureName = "Engine2FailIndicator",
                FlightDataEvent = FlightDataEvent.Elect_Bus_Power_AC_1,
                FlightDataEventBus = flightDataEventBus,
                PinId = "ENGINE_2_FAULT_INDICATOR"
            });

            AddDeviceFeature<GenericArduinoSelectorSwitchDeviceFeature>(new GenericArduinoDeviceFeatureConfig 
            { 
                DeviceFeatureName = "Engine 1 Master Switch",
                FlightDataEvent = FlightDataEvent.TestEvent_DoNotUse,
                FlightDataEventBus = flightDataEventBus,
                PinId = "EINGE_1_MASTER_SWITCH"
            });

            AddDeviceFeature<GenericArduinoSelectorSwitchDeviceFeature>(new GenericArduinoDeviceFeatureConfig
            {
                DeviceFeatureName = "Engine 2 Master Switch",
                FlightDataEvent = FlightDataEvent.TestEvent_DoNotUse,
                FlightDataEventBus = flightDataEventBus,
                PinId = "EINGE_2_MASTER_SWITCH"
            });
        }
    }
}
