using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core;
using System.Xml.Linq;
using AFooCockpit.App.Core.DataSource;
using AFooCockpit.App.Core.FlightData;

namespace AFooCockpit.App.Core.Device.DeviceFeatures
{
    internal abstract class DeviceFeatureSelectorSwitch<C, T> : DeviceFeature<C, T>
        where C : DeviceFeatureConfig
        where T : IDataSource
    {
        protected DeviceFeatureSelectorSwitch(C deviceFeatureConfig) : base(deviceFeatureConfig)
        {
        }

        /// <summary>
        /// Send the selection to the flight data bus
        /// </summary>
        protected void SendSelect(double value)
        {
            FlightDataEventBus.TriggerDataEvent(FlightDataEvent, new FlightDataEventArgs
            {
                SenderName = Name,
                Data = value,
                Event = FlightDataEvent
            });
        }
    }
}
