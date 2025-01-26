﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.DataSource;
using AFooCockpit.App.Core.FlightData;

namespace AFooCockpit.App.Core.Device.DeviceFeatures
{
    internal abstract class DeviceFeaturePushButton<C, T> : DeviceFeature<C, T>
        where C : DeviceFeatureConfig
        where T : IDataSource
    {

        public DeviceFeaturePushButton(C deviceFeatureConfig) : base(deviceFeatureConfig)
        {
        }

        /// <summary>
        /// Send a "Press" event to the flight data bus
        /// </summary>
        protected void SendPress()
        {
            Config.FlightDataEventBus.TriggerDataEvent(Config.FlightDataEvent, new FlightDataEventArgs
            {
                SenderName = Name,
                Data = FlightDataEventValue.ButtonPress,
                Event = Config.FlightDataEvent
            });
        }

        /// <summary>
        /// Send a "Release" event to the flight data bus
        /// </summary>
        protected void SendRelease()
        {
            Config.FlightDataEventBus.TriggerDataEvent(Config.FlightDataEvent, new FlightDataEventArgs 
            {
                SenderName = Name,
                Data = FlightDataEventValue.ButtonRelease,
                Event = Config.FlightDataEvent
            });
        }
    }
}
