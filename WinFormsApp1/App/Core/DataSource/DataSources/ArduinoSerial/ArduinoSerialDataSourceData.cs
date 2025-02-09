using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static AFooCockpit.App.Core.DataSource.DataSources.Arduino.ArduinoSerialDataSource;

namespace AFooCockpit.App.Core.DataSource.DataSources.ArduinoSerial
{
    public enum DataType
    {
        Analog,
        Digital,
        PWM
    }

    public enum DataDirection
    {
        Input,
        Output
    }

    internal class ArduinoSerialDataSourceData : DataSourceData
    {
        public static readonly int AnalogMaxValue = 1023;
        public static readonly int AnalogMinValue = 0;

        public static readonly int PWMMaxValue = 1023;
        public static readonly int PWMMinValue = 0;

        public required Pin Pin;
        public required DataType Type;

        private int? _analogValue;
        private bool? _digitalValue;
        private int? _pwmValue;

        public int? AnalogValue { 
            get => _analogValue; set
            {
                if(value < AnalogMinValue || value > AnalogMaxValue)
                {
                    throw new ArgumentOutOfRangeException("value is out of range for an analog value");
                }

                _analogValue = value;
            }
        }

        public bool? DigitalValue
        {
            get => _digitalValue;
            set => _digitalValue = value;
        }

        public int? PWMValue
        {
            get => _pwmValue;
            set
            {
                if (value < PWMMinValue || value > PWMMaxValue)
                {
                    throw new ArgumentOutOfRangeException("value is out of range for an analog value");
                }

                _pwmValue = value;
            }
        }

        public override bool Equals(object? obj)
        {
            if (obj?.GetType() != GetType())
            {
                return false;
            }

            var otherData = (ArduinoSerialDataSourceData) Convert.ChangeType(obj, GetType());

            //if(otherData.Pin != Pin || otherData.Type != Type)
            //{
            //    return false;
            //}

            switch (Type)
            {
                case DataType.Analog: return AnalogValue.Equals(otherData.AnalogValue);
                case DataType.PWM: return PWMValue.Equals(otherData.PWMValue);
                case DataType.Digital: return DigitalValue.Equals(otherData.DigitalValue);
                default: return false;
            }
        }

        /// <summary>
        /// Convert the given value to a string value following our arduino protocol
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public override string ToString()
        {
            if (Type == DataType.Digital)
            {
                if(DigitalValue == null)
                {
                    throw new Exception("Cannot convert digital value to string - digital value is null");
                }

                return DigitalValue == true ? "V1" : "V0";
            }

            if (Type == DataType.Analog)
            {
                if (AnalogValue == null)
                {
                    throw new Exception("Cannot convert analog value to string - analog value is null");
                }

                return ((int)AnalogValue).ToString();
            }

            if (Type == DataType.PWM)
            {
                if (PWMValue == null)
                {
                    throw new Exception("Cannot convert pwm value to string - pwm value is null");
                }

                return ((int)PWMValue).ToString();    
            }

            throw new Exception("Invalid data type - cannot convert to string");
        }
    }
}
