using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static AFooCockpit.App.Core.DataSource.DataSources.Arduino.ArduinoSerialDataSource;

namespace AFooCockpit.App.Core.DataSource.DataSources.ArduinoSerial
{

    delegate ArduinoSerialDataSourceData ResultParser(string resultString);
    delegate void ResultVerifyer(string resultString);

    public enum Pin
    {
        Pin0,
        Pin1,
        Pin2,
        Pin3,
        Pin4,
        Pin5,
        Pin6,
        Pin7,
        Pin8,
        Pin9,
        Pin10,
        Pin11,
        Pin12,
        Pin13,

        PinAnalog1,
        PinAnalog2,
        PinAnalog3,
        PinAnalog4
    }

    internal class ArduinoSerialDataSourcePinConfiguration
    {
        public required Pin Pin;
        public required DataType Type;
        public required DataDirection Direction;
        public bool PullupEnabled = true;

        private static readonly Dictionary<Pin, string> pin2string = new Dictionary<Pin, string>
            {
                { Pin.Pin0, "P0" },
                { Pin.Pin1, "P1" },
                { Pin.Pin2, "P2" },
                { Pin.Pin3, "P3" },
                { Pin.Pin4, "P4" }

                //{ Pin.PinAnalog1, "P1" },
                //{ Pin.PinAnalog1, "P2" },
                //{ Pin.PinAnalog1, "P3" },
                //{ Pin.PinAnalog1, "P4" },
            };

        /// <summary>
        /// Returns true if this pin needs a configuration command to be 
        /// send to the arduino firmware
        /// </summary>
        public bool RequiresConfiguration => Type != DataType.Analog;

        /// <summary>
        /// For digital/pwm inputs/outputs, we need to set a configuration on start.
        /// Digital can be input, output. If output, PullupEnabled is ignored
        /// PWM can only be output, if input, exception
        /// Analog cannot be configured, always exception
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public (string Command, ResultVerifyer Verify) GetConfigurationCommand()
        {
            string param = "";

            switch(Type)
            {
                case DataType.Digital:
                    {
                        if (Direction == DataDirection.Input)   param = PullupEnabled ? "I1" : "I0";
                        else                                    param = "OD";
                        break;
                    }
                case DataType.PWM:
                    {
                        if (Direction == DataDirection.Output) param = "OP";
                        else throw new Exception("Cannot configure PWM pin as input");
                        break;
                    }
                default: throw new Exception("Illegal pin configuration or pin configuration doesn't need configuration command");
            }

            return (
                Command: BuildCommand("CS", param),
                Verify: x => VerifyResult(x, BuildExpectedResult(param))
            );
        }

        /// <summary>
        /// Gets the write command for the supplied data and the current config
        /// </summary>
        /// <param name="data">The data that should be written using this command</param>
        /// <returns>Tupel that contains the command as string, and a verify function that throws if the result is not the expceted one</returns>
        /// <exception cref="Exception"></exception>
        public (string Command, ResultVerifyer Verify) GetWriteCommand(ArduinoSerialDataSourceData data)
        {
            if (data.Type != Type)
            {
                throw new Exception("Cannot write data - data is of wrong type for this pin.");
            }

            switch (Type)
            {
                case DataType.Digital: return GetDigitalWriteCommand(data);
                case DataType.PWM: return GetPWMWriteCommand(data);
                default: throw new Exception("Cannot write data - unsupporeted data type");
            }
        }

        private (string Command, ResultVerifyer Verify) GetDigitalWriteCommand(ArduinoSerialDataSourceData data)
        {
            if (data.DigitalValue == null)
            {
                throw new Exception("Cannot write data - data is of right type but does not contain value!");
            }

            return (
                Command: BuildCommand("DS", (bool)data.DigitalValue),
                Verify: x => VerifyResult(x, BuildExpectedResult((bool)data.DigitalValue))
            );
        }

        private (string Command, ResultVerifyer Verify) GetPWMWriteCommand(ArduinoSerialDataSourceData data)
        {
            if (data.PWMValue == null)
            {
                throw new Exception("Cannot write data - data is of right type but does not contain value!");
            }


            return (
                Command: BuildCommand("PS", (int)data.PWMValue),
                Verify: x => VerifyResult(x, BuildExpectedResult((int)data.PWMValue))
            );
        }

        public (string Command, ResultParser Parse) GetReadCommand()
        {
            switch(Type)
            {
                case DataType.Digital: return GetDigitalReadCommand();
                case DataType.PWM: return GetPWMReadCommand();
                case DataType.Analog: return GetAnalogReadCommand();
                default: throw new Exception("Cannot read data - unsupporeted data type");
            }
        }

        private (string Command, ResultParser Parse) GetDigitalReadCommand()
        {
            return (
                Command: BuildCommand("DG"),
                Parse: x => ArduinoSerialDataSourceValueParser.Parse(Pin, DataType.Digital, x)
            );
        }

        private (string Command, ResultParser Parse) GetAnalogReadCommand()
        {
            return (
                Command: BuildCommand("AG"),
                Parse: x => ArduinoSerialDataSourceValueParser.Parse(Pin, DataType.Analog, x)
            );
        }

        private (string Command, ResultParser Parse) GetPWMReadCommand()
        {
            return (
                Command: BuildCommand("PG"),
                Parse: x => ArduinoSerialDataSourceValueParser.Parse(Pin, DataType.PWM, x)
            );
        }

        private string BuildCommand(string command)
        {
            return $"{command} {pin2string[Pin]}";
        }

        private string BuildCommand<T>(string command, T value)
        {
            return $"{command} {pin2string[Pin]} {GetValueString(value)}";
        }

        private string GetValueString<T> (T value)
        {
            if (typeof(T) == typeof(int))
            {
                int intVal = (int)Convert.ChangeType(value, typeof(int))!;
                return $"V{intVal.ToString()}";
            }

            if (typeof(T) == typeof(bool))
            {
                bool boolVal = (bool)Convert.ChangeType(value, typeof(bool))!;
                return boolVal == true ? "V1" : "V0";
            }

            // Required for configuration commands
            if (typeof(T) == typeof(string))
            {
                return (string)Convert.ChangeType(value, typeof(string))!;
            }

            throw new Exception($"Cannot convert type {typeof(T).Name} to string val");
        }

        private string BuildExpectedResult<T>(T result)
        {
            return $"OK {GetValueString(result)}";
        }

        private void VerifyResult(string result, string expectedResult)
        {
            if(!result.Equals(expectedResult))
            {
                throw new Exception($"Result verification failed - expected result '{expectedResult}' doesn not match actual result '{result}'");
            }
        }
    }
}
