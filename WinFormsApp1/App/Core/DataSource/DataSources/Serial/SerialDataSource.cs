using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.DataSource;
using Microsoft.VisualBasic.Logging;

namespace AFooCockpit.App.Core.DataSource.DataSources.Serial
{
    /// <summary>
    /// Data source that abstracts a serial port connection
    /// </summary>
    /// <param name="eventSourceConfig"></param>
    internal class SerialDataSource(SerialDataSourceConfig eventSourceConfig) : DataSource<SerialDataSourceConfig, SerialDataSourceData>(eventSourceConfig)
    {
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
                logger.Debug($"Configuring serial data source on {Config.Port}");

                MySerialPort = new SerialPort(Config.Port);

                MySerialPort.BaudRate = Config.BaudRate;
                MySerialPort.ReadTimeout = Config.ReadTimeout;
                MySerialPort.NewLine = Config.NewLine;
                MySerialPort.DataBits = Config.DataBits;
                MySerialPort.Handshake = Config.Handshake;
                MySerialPort.Parity = Config.Parity;
                MySerialPort.DtrEnable = Config.DtrEnable;
                MySerialPort.RtsEnable = Config.RtsEnable;

                MySerialPort.DataReceived += MySerialPort_DataReceived;

                MySerialPort.Open();
                logger.Info($"Serial data source opened on port {Config.Port} with Baud Rate {Config.BaudRate}");
            }
            catch
            {
                throw new FatalSourceConnectException($"Unable to open Serial port {Config.Port}");
            }
        }

        /// <summary>
        /// Receive data from serial port
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MySerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            lock(MySerialPort) { 
                try
                {
                    while (MySerialPort!.BytesToRead > 0)
                    {
                        logger.Debug($"Reading serial data on {Config.Port}");

                        var line = MySerialPort.ReadLine();
                        var data = new SerialDataSourceData { Line = line };
                        TriggerDataReceiveEvent(data);
                    }
                }
                catch (TimeoutException)
                {
                    logger.Error("Timeout while reading serial data");
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                }
            }
        }

        protected override void DisconnectSource()
        {
            logger.Debug($"Serial data source opened CLOSED on port {Config.Port}");
            if (MySerialPort != null)
            {
                MySerialPort!.DataReceived -= MySerialPort_DataReceived;
                MySerialPort!.Close();
                MySerialPort!.Dispose();
            }
        }

        /// <summary>
        /// Send data to serial port
        /// </summary>
        /// <param name="data"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void Send(SerialDataSourceData data)
        {
            if (State == SourceState.Connected)
            {
                lock(MySerialPort!)
                { 
                    logger.Debug($"Writing serial command {data.Line} on port {Config.Port}");
                    MySerialPort?.WriteLine(data.Line);
                }
            }
        }
    }
}
