using AFooCockpit.App.Core.DataSource.DataSources.Arinc429TranscieverDataSource;
using AFooCockpit.App.Core.Device;
using AFooCockpit.App.Core.FlightData;
using AFooCockpit.App.Implementations.Arinc429TranscieverDevice.DeviceFeatures;

namespace AFooCockpit.App.Implementations.Arinc429TranscieverDevice.Devices
{
    internal abstract class Arinc429TranscieverDevice : Device<Arinc429TranscieverDeviceFeatureConfig, Arinc429TranscieverDataSource>
    {
        public Arinc429TranscieverDevice(string deviceName, FlightDataEventBus flightDataEventBus) : base(deviceName, flightDataEventBus)
        {
        }
    }
}
