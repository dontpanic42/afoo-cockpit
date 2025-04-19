using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.Utils.ArduinoGenericFirmwareUtils;
using AFooCockpit.App.Core.Utils.SerialUtils;
using static AFooCockpit.App.Core.Utils.ArduinoGenericFirmwareUtils.ArduinoGenericFirmwareUtils;

namespace AFooCockpit.App.Core.DataSource.DataSources.GenericArduino
{
    internal class GenericArduinoDataSource : DataSource<GenericArduinoDataSourceConfig, GenericArduinoDataSourceData>
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private SerialPort? SerialPort;

        public GenericArduinoDataSource(GenericArduinoDataSourceConfig eventSourceConfig) : base(eventSourceConfig)
        {
        }

        public override SourceState State {
            get 
            {
                if (SerialPort != null && SerialPort.IsOpen)
                {
                    return SourceState.Connected;
                }

                return SourceState.Disconnected;
            }
        }

        public override void Send(GenericArduinoDataSourceData data)
        {
            if (State != SourceState.Connected)
            {
                logger.Warn("Writing command before source is connected, command will be ignored.");
                return;
            }

            // Check if the pin exists/is configured
            if (Config.HasActivePinsWithId(data.PinId))
            {
                // Get the matching hardware pin
                Config.GetActivePinsWithId(data.PinId).ForEach(pin =>
                {
                    if (pin.Direction == PinDirection.In)
                    {
                        logger.Error("Configuration error - trying to send data to input '{data.PinId}', hw pin '{pin.PinName}' - pyhsical '{pin.PinNumber}'. Pin needs to be configured as output.");
                        return;
                    }

                    // For now, we only support digital pins
                    PinState pinState = (data.Value > 0) ? PinState.On : PinState.Off;
                    byte[] cmd = GetCommandSetDigitalPin(pin.PinNumber, pinState);
                    var result = SerialUtils.SendCommand(SerialPort!, cmd);

                    if (!IsResultSuccess(result))
                    {
                        logger.Warn($"Could not send event to pin '{data.PinId}', hw pin '{pin.PinName}' - pyhsical '{pin.PinNumber}': Result is an error");
                        logger.Warn($"Error message: {GetErrorMessage(result)}");
                    }
                    else
                    {
                        logger.Debug($"Successfully sent event to pin '{data.PinId}', hw pin '{pin.PinName}' - pyhsical '{pin.PinNumber}': Result is success");
                    }
                });
            } 
            else
            {
                logger.Debug($"Attempt to write to pin '{data.PinId}' that either doesn't exist or is not enabled.");
            }
        }

        /// <summary>
        /// After data source connection, we need to set up all the arduino pins
        /// </summary>
        private void ConfigurePins()
        {
            Config.PinConfigs.ForEach(pin =>
            {
                if(!pin.Enabled)
                {
                    logger.Debug($"Ignoring disabled pin {pin.PinName}");
                    return;
                }

                var cmd = GetCommandSetUpDigitalPin(
                    pin.PinNumber, 
                    pin.Direction, 
                    pin.Pullup
                );

                var result = SerialUtils.SendCommand(SerialPort!, cmd );

                if (!IsResultSuccess(result))
                {
                    logger.Error($"Failed to initialize Pin {pin.PinName} (Id {pin.PinId})");
                    logger.Error($"Error message: {GetErrorMessage(result)}");
                }
            });
        }

        protected override void ConnectSource()
        {
            try
            {
                SerialPort = SerialUtils.Connect(new SerialUtils.SerialPortConfig(Config.Port, Config.BaudRate));

                ConfigurePins();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw new FatalSourceConnectException($"Unable to open Serial port {Config.Port}");
            }

        }

        protected override void DisconnectSource()
        {
            if (SerialPort != null)
            {
                SerialUtils.Disconnect(SerialPort);
                SerialPort = null;
            }
        }
    }
}
