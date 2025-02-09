using AFooCockpit.App.Core.Device.DeviceFeatures;
using AFooCockpit.App.Core.DataSource.DataSources.Arduino;

namespace AFooCockpit.App.Implementations.ArduinoSerialDevice.DeviceFeatures
{

    internal class ArduinoSerialDeviceIndicatorLight : DeviceFeatureIndicatorLight<ArduinoSerialDeviceFeatureConfig, ArduinoSerialDataSource>
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private static readonly bool LIGHT_ON = true;
        private static readonly bool LIGHT_OFF = false;

        public ArduinoSerialDeviceIndicatorLight(ArduinoSerialDeviceFeatureConfig config) : base(config)
        {
        }

        protected override void TurnOff()
        {
            logger.Debug($"Turning off light");
            DataSource?.Send(Config.CreateDigitalDataObject(LIGHT_OFF));
        }

        protected override void TurnOn()
        {
            logger.Debug($"Turning on light");
            DataSource?.Send(Config.CreateDigitalDataObject(LIGHT_ON));
        }
        protected override void DataSourceConnected(ArduinoSerialDataSource dataSource)
        {
            // Call base function
            base.DataSourceConnected(dataSource);
            // Configure arduino pin
            dataSource.ConfigurePin(Config.CreatePinConfiguration());
        }
    }
}
