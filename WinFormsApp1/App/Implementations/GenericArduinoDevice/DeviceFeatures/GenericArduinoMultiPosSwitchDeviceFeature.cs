using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.DataSource.DataSources.GenericArduino;
using AFooCockpit.App.Core.Device.DeviceFeatures;

namespace AFooCockpit.App.Implementations.GenericArduinoDevice.DeviceFeatures
{
    internal class GenericArduinoMultiPosSwitchDeviceFeature : DeviceFeatureSelectorSwitch<GenericArduinoDeviceFeatureConfig, GenericArduinoDataSource>
    {
        public GenericArduinoMultiPosSwitchDeviceFeature(GenericArduinoDeviceFeatureConfig deviceFeatureConfig) : base(deviceFeatureConfig)
        {
        }

        protected override void DataSourceConnected(GenericArduinoDataSource dataSource)
        {
            dataSource.OnDataReceive += DataSource_OnDataReceive;
        }

        private void DataSource_OnDataReceive(Core.DataSource.DataSource<GenericArduinoDataSourceConfig, GenericArduinoDataSourceData> sender, Core.DataSource.DataSourceReciveEventArgs<GenericArduinoDataSourceData> e)
        {
            // Multi selector switches don't care if the value is set back to 0,
            // we are just interested in the newly selected pin
            if(e.Data.Value == 0 || !Config.PinIds.ContainsKey(e.Data.PinId))
            {
                return;
            }

            SendSelect(Config.PinIds[e.Data.PinId]);
        }

        protected override void DataSourceDisconnected(GenericArduinoDataSource dataSource)
        {
            dataSource.OnDataReceive -= DataSource_OnDataReceive;
        }
    }
}
