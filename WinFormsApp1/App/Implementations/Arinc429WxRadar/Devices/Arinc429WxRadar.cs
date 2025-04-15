using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.Device.DeviceFeatures;
using AFooCockpit.App.Core.FlightData;
using AFooCockpit.App.Implementations.Arinc429TranscieverDevice.DeviceFeatures;
using AFooCockpit.App.Implementations.Arinc429TranscieverDevice.Devices;
using AFooCockpit.App.Implementations.Arinc429WxRadar.DeviceFeatures;

namespace AFooCockpit.App.Implementations.Arinc429WxRadar.Devices
{
    internal class Arinc429WxRadar : Arinc429TranscieverDevice.Devices.Arinc429TranscieverDevice
    {
        public Arinc429WxRadar(FlightDataEventBus flightDataEventBus) : base(nameof(Arinc429WxRadar), flightDataEventBus)
        {
            AddMdfSelector(flightDataEventBus);
            AddTiltSelector(flightDataEventBus);
            AddGainSelector(flightDataEventBus);
        }

        private void AddGainSelector(FlightDataEventBus flightDataEventBus)
        {
            AddDeviceFeature<Arinc429WxRadarGainSelector>(new Arinc429TranscieverDeviceFeatureConfig
            {
                DeviceFeatureName = nameof(Arinc429WxRadarGainSelector),
                FlightDataEventBus = flightDataEventBus,
                FlightDataEvent = FlightDataEvent.TestEvent_DoNotUse
            });
        }

        private void AddTiltSelector(FlightDataEventBus flightDataEventBus)
        {
            AddDeviceFeature<Arinc429WxRadarTiltSelector>(new Arinc429TranscieverDeviceFeatureConfig
            {
                DeviceFeatureName = nameof(Arinc429WxRadarTiltSelector),
                FlightDataEventBus = flightDataEventBus,
                FlightDataEvent = FlightDataEvent.TestEvent_DoNotUse
            });
        }

        private void AddMdfSelector(FlightDataEventBus flightDataEventBus)
        {
            AddDeviceFeature<Arinc429WxRadarMfdSelector>(new Arinc429TranscieverDeviceFeatureConfig
            {
                DeviceFeatureName = nameof(Arinc429WxRadarMfdSelector),
                FlightDataEventBus = flightDataEventBus,
                FlightDataEvent = FlightDataEvent.TestEvent_DoNotUse
            });
        }
    }
}
