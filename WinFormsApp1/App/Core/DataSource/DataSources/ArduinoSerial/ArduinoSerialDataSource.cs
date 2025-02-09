using System.IO.Ports;
using AFooCockpit.App.Core.DataSource.DataSources.ArduinoSerial;
using Timer = System.Timers.Timer;

namespace AFooCockpit.App.Core.DataSource.DataSources.Arduino
{
    internal class ArduinoSerialDataSource : DataSource<ArduinoSerialDataSourceConfig,  ArduinoSerialDataSourceData>
    {
        private class Scheduler
        {
            public required Action Action;
            public required int IntervalMs;

            public bool IsEnabled { get; private set; } = false;

            private Timer? Timer;
            private Task? CurrentTask;

            /// <summary>
            /// Starts the regular execution of the scheduled action
            /// </summary>
            public void Start()
            {
                if (Timer != null)
                {
                    Timer.Enabled = false;
                    Timer.Elapsed -= Timer_Elapsed;
                    Timer = null;
                }

                Timer = new Timer(IntervalMs);
                Timer.Elapsed += Timer_Elapsed;
                Timer.AutoReset = false;
                Timer.Enabled = true;
            }

            private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
            {
                var timer = (Timer)sender!;
                timer.Stop();

                try
                {
                    CurrentTask = Task.Run(Action);
                    CurrentTask?.Wait();
                }
                finally
                {
                    if (IsEnabled)
                    {
                        timer.Start();
                    }
                }
            }

            /// <summary>
            /// Waits for the current task to be completed and then stops the scheduler
            /// </summary>
            public void Stop()
            {
                IsEnabled = false;
                if (CurrentTask != null && !CurrentTask.IsCompleted)
                {
                    logger.Warn("Task is currently running. Need to wait for it to complete before stopping");
                    CurrentTask.Wait();
                    CurrentTask = null;
                }
            }
        }

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();


        public override SourceState State
        {
            get
            {
                if (MySerialPort != null && MySerialPort.IsOpen)
                {
                    return SourceState.Connected;
                }

                return SourceState.Disconnected;
            }
        }

        private SerialPort? MySerialPort;

        private Dictionary<Pin, ArduinoSerialDataSourcePinConfiguration> PinConfigs = new Dictionary<Pin, ArduinoSerialDataSourcePinConfiguration>();
        private Dictionary<Pin, ArduinoSerialDataSourceData> LastPinData = new Dictionary<Pin, ArduinoSerialDataSourceData>();

        private Scheduler? PollScheduler;

        public ArduinoSerialDataSource(ArduinoSerialDataSourceConfig eventSourceConfig) : base(eventSourceConfig)
        {
            // Set up poll scheduler, but do not start it yet
            PollScheduler = new Scheduler { Action = Poll, IntervalMs = eventSourceConfig.PollIntervalMs };
        }

        /// <summary>
        /// Returns a list of all available ports on the computer
        /// </summary>
        /// <returns></returns>
        public static List<string> GetPortList()
        {
            return SerialPort.GetPortNames().ToList();
        }

        protected override void ConnectSource()
        {
            try
            {
                logger.Debug($"Configuring arduino data source on {Config.Port}");

                MySerialPort = new SerialPort(Config.Port);

                MySerialPort.BaudRate = Config.BaudRate;
                MySerialPort.ReadTimeout = Config.ReadTimeout;
                MySerialPort.NewLine = Config.NewLine;
                MySerialPort.DataBits = Config.DataBits;
                MySerialPort.Handshake = Config.Handshake;
                MySerialPort.Parity = Config.Parity;
                MySerialPort.DtrEnable = Config.DtrEnable;
                MySerialPort.RtsEnable = Config.RtsEnable;

                MySerialPort.Open();
                logger.Info($"Arduino serial data port opened on port {Config.Port} with Baud Rate {Config.BaudRate}");

                // Waiting after connect for FW to send some initial status values etc.
                Thread.Sleep(Config.AfterConnectTimeout);

                PollScheduler?.Start();
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new RetryableSourceConnectException("UnauthorizedAccessException, retrying");
            }
            catch (Exception ex) 
            {
                logger.Error(ex);
                throw new FatalSourceConnectException($"Unable to open Arduino serial data port {Config.Port}");
            }
        }

        protected override void DisconnectSource()
        {
            // Wait for any current tasks to finish
            PollScheduler?.Stop();

            logger.Debug($"Serial data source CLOSED on port {Config.Port}");
            if (MySerialPort != null)
            {
                MySerialPort!.Close();
                MySerialPort!.Dispose();
            }
        }

        /// <summary>
        /// Send data to serial port
        /// </summary>
        /// <param name="data"></param>
        /// <exception cref="ArgumentException">Data argument is malformed/out of range/...</exception>
        public override void Send(ArduinoSerialDataSourceData data)
        {
            if (PinConfigs.ContainsKey(data.Pin))
            {
                WriteData(PinConfigs[data.Pin], data);
                // Prevent a new data received event from being triggered...
                LastPinData[data.Pin] = data;
            }
            else
            {
                throw new Exception("Attempting to write data to pin, but pin is not configured for this data source");
            }
        }

        /// <summary>
        /// For all pin configs that are of type "Input", read the current value
        /// If the current value does not equal the last value, trigger a data receive event
        /// </summary>
        private void Poll()
        {
            PinConfigs.Values
                .Where(pinConf => pinConf.Direction == DataDirection.Input)
                .ToList()
                .ForEach(pinConf =>
                {
                    var data = ReadData(pinConf);
                    if (!LastPinData.ContainsKey(data.Pin) || !LastPinData[pinConf.Pin].Equals(data))
                    {
                        TriggerDataReceiveEvent(data);
                        LastPinData[pinConf.Pin] = data;
                    }
                });
        }

        /// <summary>
        /// Configures a pin with a given pin config
        /// </summary>
        /// <param name="pinConfig"></param>
        public void ConfigurePin(ArduinoSerialDataSourcePinConfiguration pinConfig)
        {
            if(pinConfig.RequiresConfiguration)
            {
                var cmd = pinConfig.GetConfigurationCommand();
                var result = ReadWriteSync(cmd.Command);
                cmd.Verify(result);
            }

            var currentData = ReadData(pinConfig);
            PinConfigs.Add(pinConfig.Pin, pinConfig);
            LastPinData.Add(pinConfig.Pin, currentData);
        }

        /// <summary>
        /// Writes data to a pin. Note that this doesn't update the last data dict
        /// </summary>
        /// <param name="pinConfig"></param>
        /// <param name="data"></param>
        private void WriteData(ArduinoSerialDataSourcePinConfiguration pinConfig, ArduinoSerialDataSourceData data)
        {
            var cmd = pinConfig.GetWriteCommand(data);
            var result = ReadWriteSync(cmd.Command);
            cmd.Verify(result);
        }

        /// <summary>
        /// Reads data from a pin. Note that this doesn't update the last data dict
        /// </summary>
        /// <param name="pinConfig"></param>
        /// <returns></returns>
        private ArduinoSerialDataSourceData ReadData(ArduinoSerialDataSourcePinConfiguration pinConfig)
        {
            var cmd = pinConfig.GetReadCommand();
            var result = ReadWriteSync(cmd.Command);
            return cmd.Parse(result);
        }

        /// <summary>
        /// Writes a line to the serial port and then immediately reads a line from the serial port
        /// </summary>
        /// <param name="write"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private string ReadWriteSync(string write)
        {
            if (MySerialPort == null)
            {
                throw new Exception("Read/Write before datasource connected");
            }

            lock(MySerialPort)
            {
                logger.Debug($"Sending command: {write}");
                 //Thread.Sleep(2000);

                while (MySerialPort.BytesToRead > 0 || MySerialPort.BytesToWrite > 0)
                {
                    MySerialPort.DiscardOutBuffer();
                    MySerialPort.DiscardInBuffer();
                }
                MySerialPort.WriteLine(write);
                var result = MySerialPort.ReadLine();

                logger.Debug($"Received {result} from command {write}");
                return result;
            }
        }
    }
}
