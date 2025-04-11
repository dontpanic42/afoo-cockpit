using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.DataSource;
using AFooCockpit.App.Core.FlightData;

namespace AFooCockpit.App.Core.Device.DeviceFeatures
{
    internal abstract class DeviceFeatureDummy<C, T> : DeviceFeature<C, T>
        where C : DeviceFeatureConfig
        where T : IDataSource
    {

        public DeviceFeatureDummy(C deviceFeatureConfig) : base(deviceFeatureConfig)
        {
        }

        /// <summary>
        /// Send a predefined ("None") data event when triggered
        /// </summary>
        protected void SendEvent()
        {
            SendEvent(Config.FlightDataEvent, new FlightDataEventArgs
            {
                SenderName = Name,
                Data = FlightDataEventValue.None,
                Event = Config.FlightDataEvent
            });
        }
    }
}
