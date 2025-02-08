using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace AFooCockpit.App.Core.DataSource.DataSources.ArduinoSerial
{
    internal abstract class ArduinoSerialDataSourceValueParserImpl<T>
    {
        protected void CheckResultWord(string resultString)
        {
            var result = resultString.Trim().Split(' ');
            if (result.Length != 2)
            {
                throw new Exception($"Cannot parse result word - expected value to contain 2 words, but got {result.Length}");
            }

            if (!result[0].Equals("OK"))
            {
                throw new Exception($"Expected result word to be 'OK', but got {result[0]}");
            }
        }

        protected string GetValueString(string resultString)
        {
            var result = resultString.Trim().Split(' ');
            if (result.Length != 2)
            {
                throw new Exception($"Cannot parse result value - expected value to contain 2 words, but got {result.Length}");
            }

            var resultWord = result[1];
            if (!resultWord.StartsWith("V"))
            {
                throw new Exception($"Cannot parse result value - expected value word to start with 'V', but got {resultWord[0]} instead");
            }

            return resultWord.Substring(1).Trim();
        }

        public abstract T Parse(string resultString);
    }

    internal class ArduinoSerialDataSourceValueParserBool : ArduinoSerialDataSourceValueParserImpl<bool>
    {
        public override bool Parse(string resultString)
        {
            CheckResultWord(resultString);
            var stringValue = GetValueString(resultString);
            return stringValue.Equals("1");
        }
    }

    internal class ArduinoSerialDataSourceValueParserInt : ArduinoSerialDataSourceValueParserImpl<int>
    {
        public override int Parse(string resultString)
        {
            CheckResultWord(resultString);
            var stringValue = GetValueString(resultString);
            if(int.TryParse(stringValue, out int value))
            {
                return value;
            }

            throw new Exception($"Could not parse int value - expected integer value but got {stringValue}");
        }
    }

    /// <summary>
    /// Static class that helps parsing a pin's current value, based on the data type
    /// </summary>
    internal static class ArduinoSerialDataSourceValueParser
    {
        private static ArduinoSerialDataSourceValueParserInt IntParser = new ArduinoSerialDataSourceValueParserInt();
        private static ArduinoSerialDataSourceValueParserBool BoolParser = new ArduinoSerialDataSourceValueParserBool();

        public static ArduinoSerialDataSourceData Parse(Pin pin, DataType type, string resultString)
        {
            if(type == DataType.Digital)
            {
                return new ArduinoSerialDataSourceData { 
                    Pin = pin, 
                    Type = type, 
                    DigitalValue = BoolParser.Parse(resultString) };
            }

            if (type == DataType.PWM)
            {
                return new ArduinoSerialDataSourceData
                {
                    Pin = pin,
                    Type = type,
                    PWMValue = IntParser.Parse(resultString)
                };
            }

            if (type == DataType.Analog)
            {
                return new ArduinoSerialDataSourceData
                {
                    Pin = pin,
                    Type = type,
                    AnalogValue = IntParser.Parse(resultString)
                };
            }

            throw new Exception("Unknown data type - cannot parse result");
        }
    }
}
