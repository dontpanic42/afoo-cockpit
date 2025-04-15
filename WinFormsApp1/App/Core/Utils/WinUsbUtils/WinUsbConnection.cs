using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.DataSource;
using FSUIPC;
using LibUsbDotNet;
using LibUsbDotNet.Main;
using static System.Windows.Forms.AxHost;

namespace AFooCockpit.App.Core.Utils.WinUsbUtils
{
    /// <summary>
    /// Base class for all Win Usb exceptions
    /// </summary>
    internal class WinUsbConnectionException : Exception
    {
        public WinUsbConnectionException(string message) : base(message) { }

    }

    /// <summary>
    /// Thrown when a given device cannot be found
    /// </summary>
    internal class WinUsbConnectionDeviceNotFoundException : WinUsbConnectionException
    {
        public WinUsbConnectionDeviceNotFoundException(string message) : base(message) { }

    }

    /// <summary>
    /// Thrown when a read or write to usb device failed
    /// </summary>
    internal class WinUsbConnectionIOException : WinUsbConnectionException
    {
        private ErrorCode _errorCode;

        public ErrorCode ErrorCode { get => _errorCode; }

        public WinUsbConnectionIOException(string message, ErrorCode errorCode = ErrorCode.None) : base(message)
        {
            _errorCode = errorCode;
        }
    }

    internal class WinUsbConnection
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Handle for the current USB device
        /// </summary>
        private UsbDevice? MyUsbDevice;

        /// <summary>
        /// Helper to find a usb device with given vid/pid
        /// </summary>
        private UsbDeviceFinder? MyUsbDeviceFinder;

        /// <summary>
        /// Used to read data from a usb interface
        /// </summary>
        private UsbEndpointReader? MyUsbEndpointReader;

        /// <summary>
        /// Used to write data to a usb interface
        /// </summary>
        private UsbEndpointWriter? MyUsbEndpointWriter;

        /// <summary>
        /// Timeout used for writes
        /// </summary>
        private readonly int WriteTimeout = 2000;

        /// <summary>
        /// Timeout used for reads
        /// </summary>
        private readonly int ReadTimeout = 100;

        /// <summary>
        /// Mutex that can be used for exclusive read/write operations
        /// </summary>
        private Mutex ReadWriteMutex = new Mutex();

        /// <summary>
        /// Returns wether the device is currently connected or not
        /// </summary>
        public bool IsConnected => MyUsbDevice != null && MyUsbDevice.IsOpen;


        /// <summary>
        /// Connects to the usb device with the given VID/PID
        /// </summary>
        /// <exception cref="UsbDeviceNotFoundException">Thrown when the device could not be found</exception>
        /// <exception cref="UsbDeviceException">Thrown when an unknown error/unexpected error occured</exception>
        public void Connect(int vid, int pid)
        {
            MyUsbDeviceFinder = new UsbDeviceFinder(vid, pid);

            try
            {
                MyUsbDevice = UsbDevice.OpenUsbDevice(MyUsbDeviceFinder);

                if (MyUsbDevice == null)
                {
                    logger.Debug("Usb device not found");
                    throw new WinUsbConnectionDeviceNotFoundException($"Could not find USB Device with VID {FormatUsbId(MyUsbDeviceFinder.Vid)} and PID {FormatUsbId(MyUsbDeviceFinder.Pid)}");
                }

                // If this is a "whole" usb device (libusb-win32, linux libusb)
                // it will have an IUsbDevice interface. If not (WinUSB) the
                // variable will be null indicating this is an interface of a
                // device.
                IUsbDevice wholeUsbDevice = (MyUsbDevice as IUsbDevice)!;
                if (!ReferenceEquals(wholeUsbDevice, null))
                {
                    // This is a "whole" USB device. Before it can be used,
                    // the desired configuration and interface must be selected.

                    // Select config
                    wholeUsbDevice.SetConfiguration(1);

                    // Claim interface
                    wholeUsbDevice.ClaimInterface(1);
                }

                MyUsbEndpointReader = MyUsbDevice.OpenEndpointReader(ReadEndpointID.Ep01);
                MyUsbEndpointWriter = MyUsbDevice.OpenEndpointWriter(WriteEndpointID.Ep01);
            }
            catch (Exception e) when (!e.GetType().IsAssignableFrom(typeof(WinUsbConnectionDeviceNotFoundException)))
            {
                logger.Error(e);

                Disconnect(false);

                throw new WinUsbConnectionException($"Couldn't open USB device due to an unexpected failure: {e.Message}");
            }
        }

        /// <summary>
        /// Locks the resource
        /// </summary>
        public void AquireLock()
        {
            ReadWriteMutex.WaitOne();
        }

        /// <summary>
        /// Releases the resource
        /// </summary>
        public void ReleaseLock()
        {
            ReadWriteMutex.ReleaseMutex();
        }

        /// <summary>
        /// Disconnects from the usb device and cleans up
        /// </summary>
        /// <param name="exitProgram">Releases the usb driver when set</param>
        public void Disconnect(bool exitProgram = false)
        {
            // In case of error, close gracefully
            if (IsConnected)
            {
                MyUsbDevice?.Close();
            }

            MyUsbDevice = null;

            if (exitProgram)
            {
                UsbDevice.Exit();
            }
        }

        /// <summary>
        /// Read a given amount of bytes from the usb device.
        /// </summary>
        /// <param name="lenght">Number of bytes to read from the device</param>
        /// <returns></returns>
        /// <exception cref="UsbDeviceException"></exception>
        /// <exception cref="UsbDeviceIOException"></exception>
        protected byte[] Read(int lenght)
        {
            if (!IsConnected) throw new WinUsbConnectionException("Device not connected");

            byte[] buffer = new byte[lenght];
            int bytesRead = 0;

            var errorCode = MyUsbEndpointReader?.Read(buffer, ReadTimeout, out bytesRead);

            if (errorCode != null && errorCode != ErrorCode.None && errorCode != ErrorCode.Success)
            {
                throw new WinUsbConnectionIOException(UsbDevice.LastErrorString, (ErrorCode)errorCode!);
            }

            return buffer;
        }

        /// <summary>
        /// Writes the given bytes to the usb device
        /// </summary>
        /// <param name="data"></param>
        /// <exception cref="UsbDeviceException"></exception>
        /// <exception cref="UsbDeviceIOException"></exception>
        protected void Write(byte[] data)
        {
            if (!IsConnected) throw new WinUsbConnectionException("Device not connected");

            int bytesWritten = 0;
            var errorCode = MyUsbEndpointWriter?.Write(data, WriteTimeout, out bytesWritten);

            if (errorCode != null && errorCode != ErrorCode.None && errorCode != ErrorCode.Success)
            {
                throw new WinUsbConnectionIOException(UsbDevice.LastErrorString, (ErrorCode)errorCode!);
            }

            logger.Debug($"USB Device - wrote {bytesWritten} bytes");
        }

        /// <summary>
        /// Thread-safe helper method that sends a command to the usb device and then reads the result
        /// </summary>
        /// <param name="command"></param>
        /// <param name="resultLength"></param>
        /// <param name="result"></param>
        public byte[] SendCommand(byte[] command, int resultLength)
        {
            AquireLock();
            try
            {
                Write(command);
                return Read(resultLength);

            } 
            finally
            {
                ReleaseLock();
            }
        }

        /// <summary>
        /// Helper method that formats a given number into a four character hex string
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected string FormatUsbId(int number)
        {
            return "0x" + string.Format("{0:X}", number).PadLeft(4, '0');
        }
    }
}
