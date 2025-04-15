using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.DataSource.DataSources.Arinc429TranscieverDataSource;
using AFooCockpit.App.Core.Device.DeviceFeatures;
using AFooCockpit.App.Implementations.Arinc429TranscieverDevice.DeviceFeatures;
using static AFooCockpit.App.Core.Utils.Arinc429Utils.Arinc429Utils;

namespace AFooCockpit.App.Implementations.Arinc429WxRadar.DeviceFeatures
{
    internal class Arinc429WxRadarGainSelector : DeviceFeatureSelectorSwitch<Arinc429TranscieverDeviceFeatureConfig, Arinc429TranscieverDataSource>
    {
        private double LastGainValue = 0;

        public Arinc429WxRadarGainSelector(Arinc429TranscieverDeviceFeatureConfig deviceFeatureConfig) : base(deviceFeatureConfig)
        {
        }

        protected override void DataSourceConnected(Arinc429TranscieverDataSource dataSource)
        {
            dataSource.OnDataReceive += DataSource_OnDataReceive;
        }

        private void DataSource_OnDataReceive(Core.DataSource.DataSource<Arinc429TranscieverDataSourceConfig, Arinc429TranscieverDataSourceData> sender, Core.DataSource.DataSourceReciveEventArgs<Arinc429TranscieverDataSourceData> e)
        {
            var message = e.Data.Message;
            double gainValue = DecodeWxPanelMessage(message);

            if (gainValue != LastGainValue)
            {
                LastGainValue = gainValue;
                SendSelect(gainValue);
            }
        }

        protected override void DataSourceDisconnected(Arinc429TranscieverDataSource dataSource)
        {
            dataSource.OnDataReceive -= DataSource_OnDataReceive;
        }

        private double DecodeWxPanelMessage(Arinc429Message message)
        {
            int dataInt = Convert.ToInt32(message.Data & 0x3f);
            byte dataByte = Convert.ToByte(dataInt);

            // Ensure data word is 6 bits long and reverse it
            string bits = Convert.ToString(dataByte, 2).PadLeft(6, '0');
            string reversed = new string(bits.Reverse().ToArray());

            return Convert.ToInt32(reversed, 2);
        }
    }
}
