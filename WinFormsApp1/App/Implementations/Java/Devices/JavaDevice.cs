using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.DataSource.DataSources.Serial;
using AFooCockpit.App.Core.Device;
using AFooCockpit.App.Core.Device.DeviceFeatures;
using AFooCockpit.App.Core.FlightData;
using AFooCockpit.App.Implementations.Java.DeviceFeatures;

namespace AFooCockpit.App.Implementations.Java.Devices
{
    internal abstract class JavaDevice : Device<JavaDeviceFeatureConfig, SerialDataSource>
    {
        public JavaDevice(string deviceName, FlightDataEventBus flightDataEventBus) : base(deviceName, flightDataEventBus) 
        { 
        }

        protected JavaDeviceFeatureConfig GenerateFeatureConfig(FlightDataEvent flightDataEvent, string serialEvent, bool is3posSwitch)
        {
            var friendlyName = Enum.GetName<FlightDataEvent>(flightDataEvent)!;
            return new JavaDeviceFeatureConfig
            {
                DeviceFeatureName = friendlyName,
                FlightDataEventBus = FlightDataEventBus,
                FlightDataEvent = flightDataEvent,
                SerialEvent = serialEvent,
                Is3PosSwitch = is3posSwitch
            };
        }

        protected JavaDeviceFeatureConfig GenerateFeatureConfig(FlightDataEvent flightDataEvent, string serialEvent, int numericMultiplier)
        {
            var friendlyName = Enum.GetName<FlightDataEvent>(flightDataEvent)!;
            return new JavaDeviceFeatureConfig
            {
                DeviceFeatureName = friendlyName,
                FlightDataEventBus = FlightDataEventBus,
                FlightDataEvent = flightDataEvent,
                SerialEvent = serialEvent,
                Multiplier = numericMultiplier
            };
        }

        protected void AddDeviceFeatures<K>((FlightDataEvent flightDataEvent, string serialEvent)[] features) where K : DeviceFeature<JavaDeviceFeatureConfig, SerialDataSource>
        {
            Array.ForEach(features, f => AddDeviceFeature<K>(f.flightDataEvent, f.serialEvent));
        }

        protected void AddDeviceFeatures<K>((FlightDataEvent flightDataEvent, string serialEvent, bool is3posSwitch)[] features) where K : DeviceFeature<JavaDeviceFeatureConfig, SerialDataSource>
        {
            Array.ForEach(features, f => AddDeviceFeature<K>(f.flightDataEvent, f.serialEvent, f.is3posSwitch));
        }

        protected void AddDeviceFeature<K>(FlightDataEvent flightDataEvent, string serialEvent) where K : DeviceFeature<JavaDeviceFeatureConfig, SerialDataSource>
        {
            AddDeviceFeature<K>(flightDataEvent, serialEvent, false);
        }

        protected void AddDeviceFeature<K>(FlightDataEvent flightDataEvent, string serialEvent, bool is3posSwitch) where K : DeviceFeature<JavaDeviceFeatureConfig, SerialDataSource>
        {
            var config = GenerateFeatureConfig(flightDataEvent, serialEvent, is3posSwitch);
            AddDeviceFeature<K>(config);
        }

        protected void AddDeviceFeatures<K>((FlightDataEvent flightDataEvent, string serialEvent, int numericMultiplier)[] features) where K : DeviceFeature<JavaDeviceFeatureConfig, SerialDataSource>
        {
            Array.ForEach(features, f => AddDeviceFeature<K>(f.flightDataEvent, f.serialEvent, f.numericMultiplier));
        }

        protected void AddDeviceFeature<K>(FlightDataEvent flightDataEvent, string serialEvent, int numericMultiplier) where K : DeviceFeature<JavaDeviceFeatureConfig, SerialDataSource>
        {
            var config = GenerateFeatureConfig(flightDataEvent, serialEvent, numericMultiplier);
            AddDeviceFeature<K>(config);
        }
    }
}
