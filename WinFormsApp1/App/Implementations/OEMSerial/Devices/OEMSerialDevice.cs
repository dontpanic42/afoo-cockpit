using AFooCockpit.App.Core.DataSource.DataSources.Arduino;
using AFooCockpit.App.Core.Device;
using AFooCockpit.App.Core.FlightData;
using AFooCockpit.App.Implementations.OEMSerial.DeviceFeatures;

namespace AFooCockpit.App.Implementations.OEMSerial.Devices
{
    internal class OEMSerialDevice : Device<OEMSerialDeviceFeatureConfig, ArduinoSerialDataSource>
    {
        public OEMSerialDevice(string deviceName, FlightDataEventBus flightDataEventBus) : base(deviceName, flightDataEventBus)
        {
        }
    }
}
