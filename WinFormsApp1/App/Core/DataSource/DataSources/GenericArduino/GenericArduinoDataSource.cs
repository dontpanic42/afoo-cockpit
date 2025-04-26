using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.Utils.ArduinoGenericFirmwareUtils;
using AFooCockpit.App.Core.Utils.SerialUtils;
using static AFooCockpit.App.Core.DataSource.DataSources.GenericArduino.GenericArduinoDataSourceConfig;
using static AFooCockpit.App.Core.Utils.ArduinoGenericFirmwareUtils.ArduinoGenericFirmwareUtils;

namespace AFooCockpit.App.Core.DataSource.DataSources.GenericArduino
{
    internal class GenericArduinoDataSource : DataSource<GenericArduinoDataSourceConfig, GenericArduinoDataSourceData>
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private static readonly int PollIntervalMs = 100;

        private SerialPort? SerialPort;

        private CancellationTokenSource? PollCancelationToken = null;

        private Dictionary<PinConfig, PinState> LastPinState = new Dictionary<PinConfig, PinState>();

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
                    var result = SerialUtils.SendCommandWithRetry(SerialPort!, cmd);

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
            Config.PinConfigs
                .Where(p => p.Enabled)                          // We only read active pins
                .ToList()
                .ForEach(pin =>
            {

                // Clear pin states so we get a new event when we read pin data
                LastPinState.Clear();

                logger.Debug($"Configuring pin {pin.PinName} (Id {pin.PinId})");

                var cmd = GetCommandSetUpDigitalPin(
                    pin.PinNumber, 
                    pin.Direction, 
                    pin.PullUp
                );

                var result = SerialUtils.SendCommandWithRetry(SerialPort!, cmd );

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

                _ = StartPollData();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw new FatalSourceConnectException($"Unable to open Serial port {Config.Port}");
            }

        }

        /// <summary>
        /// Read all enabled input pins. Trigger data event if the data changed
        /// </summary>
        private void ReadData()
        {
            Config.PinConfigs
                .Where(p => p.Enabled)                          // We only read active pins
                .Where(p => p.Direction == PinDirection.In)     // We only read inputs
                .ToList()
                .ForEach(pin =>
            {
                var cmd = GetCommandGetDigitalPin(pin.PinNumber);
                var result = SerialUtils.SendCommandWithRetry(SerialPort!, cmd );

                if (!IsResultSuccess(result))
                {
                    logger.Error($"Failed to read Pin {pin.PinName} (Id {pin.PinId})");
                    logger.Error($"Error message: {GetErrorMessage(result)}");
                    return;
                }

                var pinState = GetPinState(result);

                // If Pull Up resistor is enabled for this pin, we need to invert the value
                if (pin.PullUp == PullUp.Enable)
                {
                    pinState = InvertPinState(pinState);
                }

                if (LastPinState.ContainsKey(pin) && LastPinState[pin] != pinState)
                {
                    TriggerDataReceiveEvent(new GenericArduinoDataSourceData { 
                        PinId = pin.PinId, 
                        Value = pinState,
                        PullUp = pin.PullUp
                    });
                }

                LastPinState[pin] = pinState;
            });
        }

        private PinState InvertPinState(PinState pinState)
        {
            return pinState == PinState.On ? PinState.Off : PinState.On;
        }

        /// <summary>
        /// Start regularly polling the arduino for new dta
        /// </summary>
        /// <returns></returns>
        private async Task StartPollData()
        {
            if (PollCancelationToken != null)
            {
                PollCancelationToken.Cancel();
            }

            PollCancelationToken = new CancellationTokenSource();

            while (!PollCancelationToken.IsCancellationRequested &&  State == SourceState.Connected)
            {
                try
                {
                    ReadData();

                    await Task.Delay(TimeSpan.FromMilliseconds(PollIntervalMs), PollCancelationToken.Token);
                }
                catch (TaskCanceledException)
                {
                    // Task was cancelled - exit gracefully
                    break;
                }
                catch (Exception ex)
                {
                    // Handle unexpected exceptions
                    logger.Error(ex);
                }
            }
        }

        protected override void DisconnectSource()
        {
            if (PollCancelationToken != null && !PollCancelationToken.IsCancellationRequested)
            {
                PollCancelationToken.Cancel();
                PollCancelationToken = null;
            }

            if (SerialPort != null)
            {
                SerialUtils.Disconnect(SerialPort);
                SerialPort = null;
            }
        }
    }
}
