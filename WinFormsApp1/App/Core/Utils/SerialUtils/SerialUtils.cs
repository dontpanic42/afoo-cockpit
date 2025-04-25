using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.DataSource;
using FSUIPC;
using NLog;

namespace AFooCockpit.App.Core.Utils.SerialUtils
{
    internal class SerialUtils
    {

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public struct SerialPortConfig
        {
            public string Port;
            public int BaudRate;
            public int ReadTimeout = 2000;

            public SerialPortConfig(string port, int baudRate) 
            {
                Port = port;
                BaudRate = baudRate;
            }
        }

        public static List<string> GetPortList()
        {
            return SerialPort.GetPortNames().ToList();
        }

        public static SerialPort Connect(SerialPortConfig config)
        {
            logger.Debug($"Configuring arduino data source on {config.Port}");

            var serial = new SerialPort(config.Port);

            serial.BaudRate = config.BaudRate;
            serial.ReadTimeout = config.ReadTimeout;

            serial.Open();
            logger.Debug($"Opened port {serial.PortName}, baud {serial.BaudRate}");

            Thread.Sleep(100);
            return serial;
        }

        public static void DiscardData(SerialPort port)
        {
            while(port.BytesToRead > 0)
            {
                port.ReadByte();
            }
        }

        /// <summary>
        /// Sends a command to the generic arduino firmware. In case the command fails due to a timeout, it's trying to re-issue the command up to "retries" times
        /// </summary>
        /// <param name="port">Serial port on which the arduino is connected</param>
        /// <param name="command">Command as byte array</param>
        /// <param name="expectedResponseSize">Size of the expected response - this should always be constant</param>
        /// <param name="timeout">The send command function waits for a result of the given size to be returned. This parameter describes the time in MS before it's giving up</param>
        /// <param name="retries">The number of times the command will be resent before giving up</param>
        /// <returns></returns>
        /// <exception cref="Exception">The command wasn't successfull, even after the given amount of retries</exception>
        public static byte[] SendCommandWithRetry(SerialPort port, byte[] command, int expectedResponseSize = 6, int timeout=2000, int retries=5)
        {
            int numRetries = 0;
            while(numRetries < retries) { 
                try
                {
                    numRetries++;
                    return SendCommand(port, command, expectedResponseSize, timeout);
                }
                catch(TimeoutException)
                {
                    logger.Warn($"Sending Command to Arduino failed. Retry {numRetries}/{retries}");
                    Thread.Sleep(1000);
                }
            }

            logger.Error($"Exceeded number of retries ({retries}), giving up");
            throw new Exception("Cannot send command to Arduino - no response");
        }

        /// <summary>
        /// Sends a command to the generic arduino firmware
        /// </summary>
        /// <param name="port">Serial port on which the arduino is connected</param>
        /// <param name="command">Command as byte array</param>
        /// <param name="expectedResponseSize">Size of the expected response - this should always be constant</param>
        /// <param name="timeout">The send command function waits for a result of the given size to be returned. This parameter describes the time in MS before it's giving up</param>
        /// <returns></returns>
        /// <exception cref="TimeoutException">Thrown when waiting for a response exceeds the value given in "timeout"</exception>
        public static byte[] SendCommand(SerialPort port, byte[] command, int expectedResponseSize = 6, int timeout=2000)
        {
            lock(port)
            {
                var startTime = DateTime.Now;
                // Get rid of any data that is already available
                DiscardData(port);
                // Write our command
                port.Write(command, 0, command.Length);
                port.BaseStream.Flush();

                logger.Trace($"Wrote command {GetByteArrayString(command)}");

                // Buffer to write the response into
                byte[] readBuffer = new byte[expectedResponseSize];
                int bytesRead = 0;

                // Waiting for bytes to be written
                while (bytesRead < expectedResponseSize)
                {
                    if (port.BytesToRead < expectedResponseSize)
                    {
                        if ((DateTime.Now - startTime).TotalMilliseconds > timeout)
                        {
                            throw new TimeoutException("Timout while waiting for response from Arduino");
                        }
                        Thread.Sleep(100);
                        continue;
                    }

                    logger.Trace($"reading... available: {port.BytesToRead}");
                    int read = port.Read(readBuffer, bytesRead, expectedResponseSize - bytesRead);
                    if (read == 0)
                    {
                        logger.Debug("Read timeout while waiting for command");
                    }
                    bytesRead += read;
                }

                return readBuffer;
            }
        }

        public static void Disconnect(SerialPort port)
        {
            if(port.IsOpen)
            {
                port.Close();
            }
        }

        /// <summary>
        /// Takes a byte array and transforms it in a human readable string
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetByteArrayString(byte[] data)
        {
            if (data == null || data.Length == 0)
                return "Array is empty or null.";

            var builder = new StringBuilder();

            foreach (byte b in data)
            {
                builder.AppendFormat("{0:X2} ", b); // Hex format with two digits
            }

            return builder.ToString().TrimEnd(); // Remove trailing space
        }
    }
}
