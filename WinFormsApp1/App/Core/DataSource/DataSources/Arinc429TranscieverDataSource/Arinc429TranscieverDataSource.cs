using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using AFooCockpit.App.Core.Utils.WinUsbUtils;
using Newtonsoft.Json.Linq;
using static AFooCockpit.App.Core.Utils.Arinc429Utils.Arinc429Utils;

namespace AFooCockpit.App.Core.DataSource.DataSources.Arinc429TranscieverDataSource
{
    internal class Arinc429TranscieverDataSource : DataSource<Arinc429TranscieverDataSourceConfig, Arinc429TranscieverDataSourceData>
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private WinUsbConnection Connection = new WinUsbConnection();

        private CancellationTokenSource? PollCancelationToken = null;

        private static readonly int PollIntervalMs = 100;

        private static readonly int DataPackageSize = 64;

        public Arinc429TranscieverDataSource(Arinc429TranscieverDataSourceConfig eventSourceConfig) : base(eventSourceConfig)
        {
        }

        public override SourceState State => Connection.IsConnected ? SourceState.Connected : SourceState.Disconnected;

        public override void Send(Arinc429TranscieverDataSourceData data)
        {
            // For now, we're not supporting sending data (even if it's possible...)
            throw new NotImplementedException();
        }

        protected override void ConnectSource()
        {
            try { 
                Connection.Connect(Config.Vid, Config.Pid);

                _ = StartPollData();
            } 
            catch (WinUsbConnectionDeviceNotFoundException) 
            {
                throw new RetryableSourceConnectException("Waiting for USB device");
            }
            catch (Exception ex)
            {
                throw new FatalSourceConnectException(ex.Message);
            }
        }

        protected override void DisconnectSource()
        {
            if (PollCancelationToken != null && !PollCancelationToken.IsCancellationRequested)
            {
                PollCancelationToken.Cancel();
                PollCancelationToken = null;
            }

            Connection.Disconnect();
        }

        /// <summary>
        /// The LS transciever seems to ever only accept combined write/read requests. Since we currently
        /// don't have anything to write, we're just sending:
        /// - 1st byte = length of the data packsge, for us always 64
        /// - 63 bytes of zeros.
        /// </summary>
        /// <returns></returns>
        private byte[] GenerateDummyReadCommand()
        {
            byte[] dataWrite = Enumerable.Repeat<byte>(0, DataPackageSize).ToArray();
            dataWrite[0] = (byte) DataPackageSize;
            return dataWrite;
        }

        /// <summary>
        /// Reads data from the Arinc429 Transciever
        /// </summary>
        private void ReadData()
        {
            logger.Debug("Reading ARINC429 data");

            var command = GenerateDummyReadCommand();
            var result = Connection.SendCommand(command, DataPackageSize);

            logger.Debug($"Received data from ARINC429 source: {ByteArrayToString(result)}");

            // Messages are 4 bytes long, each 4 bytes is a new message
            for (int i = 0; i < result.Length - 4; i += 4)
            {
                // Messages come in 
                var message = new Arinc429Message(result.Skip(i).Take(4).ToArray());
                // Ignoring messages with label 0
                if (message.Label != 0)
                {
                    var data = new Arinc429TranscieverDataSourceData { Message = message };

                    logger.Debug($"Triggering ARINC429 data receive envent with label {IntToOctString(message.Label)}");
                    TriggerDataReceiveEvent(data);
                }
            }
        }

        /// <summary>
        /// Method that regularly polls the Arinc429 Transciever for new data
        /// </summary>
        /// <returns></returns>
        private async Task StartPollData()
        {
            if (PollCancelationToken != null)
            {
                PollCancelationToken.Cancel();
            }

            PollCancelationToken = new CancellationTokenSource();

            while (!PollCancelationToken.IsCancellationRequested && Connection.IsConnected)
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

        /// <summary>
        /// Helper method that converts an integer into a string in octal notation
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private string IntToOctString(int number)
        {
            return Convert.ToString(number, 8);
        }

        /// <summary>
        /// Helper method that turns a byte array into a printable string
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private string ByteArrayToString(byte[] bytes)
        {
            var sb = new StringBuilder("new byte[] { ");
            foreach (var b in bytes)
            {
                sb.Append(b + ", ");
            }
            sb.Append("}");
            return sb.ToString();
        }
    }
}
