using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.DataSource.DataSources.Serial;
using AFooCockpit.App.Core.Device;
using AFooCockpit.App.Core.FlightData;
using AFooCockpit.App.Implementations.Java.DeviceFeatures;

namespace AFooCockpit.App.Implementations.Java.Devices
{
    internal class JavaDevice : Device<JavaDeviceFeatureConfig, SerialDataSource>
    {
        public JavaDevice(string deviceName, FlightDataEventBus flightDataEventBus) : base(deviceName, flightDataEventBus) 
        { 
        }

        private JavaDeviceFeatureConfig GenerateFeatureConfig(FlightDataEvent flightEvent, string serialEvent, string friendlyName)
        {
            return new JavaDeviceFeatureConfig
            {
                DeviceFeatureName = friendlyName,
                FlightDataEventBus = FlightDataEventBus,
                FlightDataEvent = flightEvent,
                SerialEvent = serialEvent
            };
        }

        /// <summary>
        /// Adds a simple push button
        /// </summary>
        /// <param name="flightDataEvent"></param>
        /// <param name="serialEvent"></param>
        protected void AddPushButton(FlightDataEvent flightDataEvent, string serialEvent)
        {
            var friendlyName = Enum.GetName<FlightDataEvent>(flightDataEvent);
            var config = GenerateFeatureConfig(flightDataEvent, serialEvent, friendlyName!);
            var button = new JavaDeviceFeaturePushButton(config);
            AddDeviceFeature(button);
        }

        /// <summary>
        /// Adds a 3 position selector switch - these are strange in Java Simulator device since they select 0 - 2 - 1 instead
        /// of 0 - 1 - 2, so we need to conver them
        /// </summary>
        /// <param name="flightDataEvent"></param>
        /// <param name="serialEvent"></param>
        protected void AddSelectorSwitch3Pos(FlightDataEvent flightDataEvent, string serialEvent)
        {
            var friendlyName = Enum.GetName<FlightDataEvent>(flightDataEvent);
            var config = GenerateFeatureConfig(flightDataEvent, serialEvent, friendlyName!);
            config.Is3PosSwitch = true;
            var button = new JavaDeviceFeatureSelectorSwitch(config);
            AddDeviceFeature(button);
        }

        /// <summary>
        /// Adds a selector switch with more or less than 3 positions
        /// </summary>
        /// <param name="flightDataEvent"></param>
        /// <param name="serialEvent"></param>
        protected void AddSelectorSwitch(FlightDataEvent flightDataEvent, string serialEvent)
        {
            var friendlyName = Enum.GetName<FlightDataEvent>(flightDataEvent);
            var config = GenerateFeatureConfig(flightDataEvent, serialEvent, friendlyName!);
            var button = new JavaDeviceFeatureSelectorSwitch(config);
            AddDeviceFeature(button);
        }
    }
}
