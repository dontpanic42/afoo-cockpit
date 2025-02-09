using AFooCockpit.App.Core.DataSource.DataSources.Arduino;
using AFooCockpit.App.Core.Device;
using AFooCockpit.App.Core.FlightData;
using AFooCockpit.App.Implementations.ArduinoSerialDevice.DeviceFeatures;

namespace AFooCockpit.App.Implementations.ArduinoSerialDevice.Devices
{
    internal class ArduinoSerialDevice : Device<ArduinoSerialDeviceFeatureConfig, ArduinoSerialDataSource>
    {
        public ArduinoSerialDevice(string deviceName, FlightDataEventBus flightDataEventBus) : base(deviceName, flightDataEventBus)
        {
        }
    }
}
