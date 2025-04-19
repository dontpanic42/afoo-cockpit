using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace AFooCockpit.App.Core.Utils.ArduinoGenericFirmwareUtils
{
    public class ArduinoGenericFirmwareUtils
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private static UInt16 Magic = 0xDEAD;

        private static readonly int CommandLength = 4;

        private static readonly int ResultCodeFieldNumber = 3;

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct SetupDigitalPinCommand
        {
            public byte command;     
            public byte pin;
        }

        public enum Command
        {
            SetupDigitalPin = 0,
            SetDigitalPin = 1,
            GetDigitalPin = 2
        }

        public enum PinDirection
        {
            In = 0,
            Out = 1
        }

        public enum Pullup
        {
            Enable = 1,
            Disable = 0
        }

        public enum PinState
        {
            On = 1,
            Off = 0
        }

        private static Dictionary<byte, string> ArduinoErrorCodes = new Dictionary<byte, string>
        {
            {0, "RESULT_SUCCESS" },
            {1, "RESULT_ERROR" },
            {2, "RESULT_ERROR_INVALID_COMMAND" },
            {3, "RESULT_ERROR_INVALID_CONFIGURATION" },
            {4, "RESULT_ERROR_INVALID_PARAMETER" },
            {5, "RESULT_ERROR_INVALID_PIN" },
        };

        /// <summary>
        /// Returns the command array for setting up a digital pin
        /// </summary>
        /// <param name="pinNumber"></param>
        /// <param name="direction"></param>
        /// <param name="pullup"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static byte[] GetCommandSetUpDigitalPin(int pinNumber, PinDirection direction, Pullup pullup = Pullup.Disable)
        {
            if (direction == PinDirection.Out && pullup == Pullup.Enable)
            {
                logger.Debug($"Will not enable pullup for pin {pinNumber} since it's an output");
            }

            byte command = ((int)Command.SetupDigitalPin & 0x0F);
            command |= (byte)(((int)direction & 0x01) << 4);                 // 4th bit is the direction, 1 if output, 0 if input
            command |= (byte)(((int)pullup & 0x01) << 5);                    // 5th bit is set when pullup is enabled

            return PadArray([
                command,
                (byte)(pinNumber & 0xFF)
            ], CommandLength);
        }

        /// <summary>
        /// Creates the command array for setting a digital pin
        /// </summary>
        /// <param name="pinNumber"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] GetCommandSetDigitalPin(int pinNumber, PinState value)
        {
            return PadArray([
                (byte)Command.SetDigitalPin,
                (byte)(pinNumber & 0xFF),
                (byte)value
            ], CommandLength);
        }

        /// <summary>
        /// Returns the command array for setting a digital pin
        /// </summary>
        /// <param name="pinNumber"></param>
        /// <returns></returns>
        public static byte[] GetCommandGetDigitalPin(int pinNumber)
        {
            return PadArray([
                (byte)Command.SetDigitalPin,
                (byte)(pinNumber & 0xFF)
            ], CommandLength);
        }

        /// <summary>
        /// Returns true if the result of a command indicates the command
        /// was a success
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool IsResultSuccess(byte[] result)
        {
            if (result.Length != 6)
            {
                logger.Warn("Got invalid result - doesn't have 6 bytes");
                return false;
            }

            if (result[0] != (Magic & 0xFF) || result[1] != ((Magic >> 8) & 0xFF))
            {
                logger.Warn("Got invalid result - Magic header missing");
                return false;
            }

            logger.Debug($"Result: {SerialUtils.SerialUtils.GetByteArrayString(result)}");
            return (result[ResultCodeFieldNumber] == 0);
        }

        /// <summary>
        /// Returns the matching error message for a given result
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static string GetErrorMessage(byte[] result)
        {
            if (result.Length < ResultCodeFieldNumber)
            {
                return "Invalid result object - doesn't have result code.";
            }
            byte code = result[ResultCodeFieldNumber];
            if (ArduinoErrorCodes.ContainsKey(code))
            {
                return ArduinoErrorCodes[code];
            }

            return $"Unknown error (Code {code})";
        }

        /// <summary>
        /// Fills the given array with zero bytes until it has a length of "length"
        /// </summary>
        /// <param name="array"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown when the given array length is > length parameter</exception>
        private static byte[] PadArray(byte[] array, int length)
        {
            if(array.Length > length)
            {
                throw new ArgumentException($"Command is already longer than {length} bytes");
            }

            byte[] padded = new byte[length];  // all zeros by default
            Array.Copy(array, padded, array.Length);

            return padded;
        }
    }
}
