using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.DataSource.DataSources.Arinc429TranscieverDataSource;
using AFooCockpit.App.Core.DataSource.DataSources.GenericArduino;
using AFooCockpit.App.Core.Device;
using AFooCockpit.App.Core.FlightData;
using AFooCockpit.App.Implementations.GenericArduinoDevice.DeviceFeatures;

namespace AFooCockpit.App.Implementations.GenericArduinoDevice.Devices
{
    internal abstract class GenericArduinoDevice : Device<GenericArduinoDeviceFeatureConfig, GenericArduinoDataSource>
    {
        public GenericArduinoDevice(string deviceName, FlightDataEventBus flightDataEventBus) : base(deviceName, flightDataEventBus)
        {
        }
    }
}
