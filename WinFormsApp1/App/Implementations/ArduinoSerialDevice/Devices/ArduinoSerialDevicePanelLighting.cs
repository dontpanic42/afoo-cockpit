using AFooCockpit.App.Core.FlightData;
using AFooCockpit.App.Core.DataSource.DataSources.ArduinoSerial;
using AFooCockpit.App.Implementations.ArduinoSerialDevice.DeviceFeatures;

namespace AFooCockpit.App.Implementations.ArduinoSerialDevice.Devices
{
    internal class ArduinoSerialDevicePanelLighting : ArduinoSerialDevice
    {
        public ArduinoSerialDevicePanelLighting(FlightDataEventBus flightDataEventBus) : base("Panel Backlight Device", flightDataEventBus)
        {
            var feature = new ArduinoSerialDeviceIndicatorLight(new ArduinoSerialDeviceFeatureConfig
            {
                Pin = Pin.Pin4,
                PinType = DataType.Digital,
                PinDirection = DataDirection.Output,
                FlightDataEventBus = flightDataEventBus,
                DeviceFeatureName = "Panel Backlight 5v",
                FlightDataEvent = FlightDataEvent.Elect_Bus_Power_AC_1
            });

            AddDeviceFeature(feature);
        }
    }
}
