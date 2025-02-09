using AFooCockpit.App.Core.DataSource.DataSources.ArduinoSerial;
using AFooCockpit.App.Core.Device.DeviceFeatures;

namespace AFooCockpit.App.Implementations.ArduinoSerialDevice.DeviceFeatures
{
    internal class ArduinoSerialDeviceFeatureConfig : DeviceFeatureConfig
    {
        public required Pin Pin;
        public required DataType PinType;
        public required DataDirection PinDirection;
        bool PinPullupEnabled = true;

        /// <summary>
        /// Helper method to create a ArduinoSerialDataSourceData matching this config
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ArduinoSerialDataSourceData CreateDigitalDataObject(bool data)
        {
            return new ArduinoSerialDataSourceData { 
                Pin = Pin, 
                Type = PinType, 
                DigitalValue = data 
            };
        }

        /// <summary>
        /// Helper method to create a ArduinoSerialDataSourceData matching this config
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ArduinoSerialDataSourceData CreatePWMArduinoSerialDataAnalog(int data)
        {
            return new ArduinoSerialDataSourceData
            {
                Pin = Pin,
                Type = PinType,
                PWMValue = data
            };
        }

        /// <summary>
        /// Helper method to cerate a ArduinoSerialDataSourcePinConfiguration from this 
        /// feature configuration
        /// </summary>
        /// <returns></returns>
        public ArduinoSerialDataSourcePinConfiguration CreatePinConfiguration()
        {
            return new ArduinoSerialDataSourcePinConfiguration
            {
                Pin = Pin,
                Direction = PinDirection,
                Type = PinType,
                PullupEnabled =
                PinPullupEnabled
            };
        }
    }
}
