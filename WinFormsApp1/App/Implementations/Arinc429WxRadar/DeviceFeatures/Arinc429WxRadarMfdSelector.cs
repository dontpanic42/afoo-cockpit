using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.DataSource.DataSources.Arinc429TranscieverDataSource;
using AFooCockpit.App.Core.DataSource.DataSources.Serial;
using AFooCockpit.App.Core.Device.DeviceFeatures;
using AFooCockpit.App.Implementations.Arinc429TranscieverDevice.DeviceFeatures;
using AFooCockpit.App.Implementations.Java.DeviceFeatures;
using static AFooCockpit.App.Core.Utils.Arinc429Utils.Arinc429Utils;

namespace AFooCockpit.App.Implementations.Arinc429WxRadar.DeviceFeatures
{
    internal class Arinc429WxRadarMfdSelector : DeviceFeatureSelectorSwitch<Arinc429TranscieverDeviceFeatureConfig, Arinc429TranscieverDataSource>
    {
        enum MfdSelectorValue
        {
            Wx = 4,
            WxTurb = 3,
            Map = 2,

            Invalid = 0
        }

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private MfdSelectorValue LastSelectorValue = MfdSelectorValue.Invalid;

        public Arinc429WxRadarMfdSelector(Arinc429TranscieverDeviceFeatureConfig deviceFeatureConfig) : base(deviceFeatureConfig)
        {
        }

        /// <summary>
        /// Datasource was connected
        /// </summary>
        /// <param name="dataSource"></param>
        protected override void DataSourceConnected(Arinc429TranscieverDataSource dataSource)
        {
            dataSource.OnDataReceive += DataSource_OnDataReceive;
        }

        private void DataSource_OnDataReceive(Core.DataSource.DataSource<Arinc429TranscieverDataSourceConfig, Arinc429TranscieverDataSourceData> sender, Core.DataSource.DataSourceReciveEventArgs<Arinc429TranscieverDataSourceData> e)
        {
            var message = e.Data.Message;
            var selectorValue = DecodeWxPanelMessage(message);

            // The value we received was invalid
            if (selectorValue == MfdSelectorValue.Invalid)
            {
                logger.Error($"Received invalid WX MFD selector value {selectorValue}, ignoring");
                return;
            }

            // Check if we have an actual change in value, if not we're ignoring this input
            if (selectorValue == LastSelectorValue)
            {
                return;
            }

            // Save the current state
            LastSelectorValue = selectorValue;
            SendSelect((double)selectorValue);
        }

        /// <summary>
        /// Cleanup after disconnect
        /// </summary>
        /// <param name="dataSource"></param>
        protected override void DataSourceDisconnected(Arinc429TranscieverDataSource dataSource)
        {
            dataSource.OnDataReceive -= DataSource_OnDataReceive;
        }

        /// <summary>
        /// Decode the selector value from the Arinc429 data word
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private MfdSelectorValue DecodeWxPanelMessage(Arinc429Message message)
        {
            var data = message.Data;
            var selectorData = Convert.ToInt32(data >> 13) & 0x07;

            if(Enum.IsDefined(typeof(MfdSelectorValue), selectorData))
            {
                return (MfdSelectorValue)selectorData;
            }

            return MfdSelectorValue.Invalid;
        }
    }
}
