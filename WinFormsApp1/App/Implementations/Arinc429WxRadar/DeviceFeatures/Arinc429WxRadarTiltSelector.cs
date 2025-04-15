using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.DataSource.DataSources.Arinc429TranscieverDataSource;
using AFooCockpit.App.Core.Device.DeviceFeatures;
using AFooCockpit.App.Implementations.Arinc429TranscieverDevice.DeviceFeatures;
using static AFooCockpit.App.Core.Utils.Arinc429Utils.Arinc429Utils;

namespace AFooCockpit.App.Implementations.Arinc429WxRadar.DeviceFeatures
{
    internal class Arinc429WxRadarTiltSelector : DeviceFeatureSelectorSwitch<Arinc429TranscieverDeviceFeatureConfig, Arinc429TranscieverDataSource>
    {
        private double LastTiltValue = 0;

        public Arinc429WxRadarTiltSelector(Arinc429TranscieverDeviceFeatureConfig deviceFeatureConfig) : base(deviceFeatureConfig)
        {
        }

        protected override void DataSourceConnected(Arinc429TranscieverDataSource dataSource)
        {
            dataSource.OnDataReceive += DataSource_OnDataReceive;
        }

        private void DataSource_OnDataReceive(Core.DataSource.DataSource<Arinc429TranscieverDataSourceConfig, Arinc429TranscieverDataSourceData> sender, Core.DataSource.DataSourceReciveEventArgs<Arinc429TranscieverDataSourceData> e)
        {
            var message = e.Data.Message;
            double tiltValue = DecodeWxPanelMessage(message);

            if (tiltValue != LastTiltValue)
            {
                LastTiltValue = tiltValue;
                SendSelect(tiltValue);
            }
        }

        protected override void DataSourceDisconnected(Arinc429TranscieverDataSource dataSource)
        {
            dataSource.OnDataReceive -= DataSource_OnDataReceive;
        }

        private double DecodeWxPanelMessage(Arinc429Message message)
        {
            var data = message.Data;

            int dataInt = Convert.ToInt32((message.Data >> 6) & 0xFF);
            byte dataByte = Convert.ToByte(dataInt);

            // Ensure data byte length and reverse bit order
            string bits = Convert.ToString(dataByte, 2).PadLeft(8, '0');
            string reversed = new string(bits.Reverse().ToArray());
            byte reversedByte = Convert.ToByte(reversed, 2);

            return ConvertTwosComplementByteToInteger(reversedByte);
        }

        private double ConvertTwosComplementByteToInteger(byte rawValue)
        {
            // If a positive value, return it
            if ((rawValue & 0x80) == 0)
            {
                return rawValue;
            }

            // Otherwise perform the 2's complement math on the value
            return (byte)(~(rawValue - 0x01)) * -1;
        }
    }
}
