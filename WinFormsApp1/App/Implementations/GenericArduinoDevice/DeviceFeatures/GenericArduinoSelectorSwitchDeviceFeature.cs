﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.DataSource.DataSources.GenericArduino;
using AFooCockpit.App.Core.Device.DeviceFeatures;
using static AFooCockpit.App.Core.Utils.ArduinoGenericFirmwareUtils.ArduinoGenericFirmwareUtils;

namespace AFooCockpit.App.Implementations.GenericArduinoDevice.DeviceFeatures
{
    internal class GenericArduinoSelectorSwitchDeviceFeature : DeviceFeatureSelectorSwitch<GenericArduinoDeviceFeatureConfig, GenericArduinoDataSource>
    {
        public GenericArduinoSelectorSwitchDeviceFeature(GenericArduinoDeviceFeatureConfig deviceFeatureConfig) : base(deviceFeatureConfig)
        {
        }

        protected override void DataSourceConnected(GenericArduinoDataSource dataSource)
        {
            dataSource.OnDataReceive += DataSource_OnDataReceive;
        }

        private void DataSource_OnDataReceive(Core.DataSource.DataSource<GenericArduinoDataSourceConfig, GenericArduinoDataSourceData> sender, Core.DataSource.DataSourceReciveEventArgs<GenericArduinoDataSourceData> e)
        {
            if (e.Data.PinId == Config.PinId)
            {
                SendSelect((double)e.Data.Value);
            }
        }

        protected override void DataSourceDisconnected(GenericArduinoDataSource dataSource)
        {
            dataSource.OnDataReceive -= DataSource_OnDataReceive;
        }
    }
}
