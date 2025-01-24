using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.FlightData;

namespace AFooCockpit.App.Core.Device
{
    internal class DeviceFeatureConfig
    {
        /// <summary>
        /// Friendly name of the device feature
        /// </summary>
        public required string DeviceFeatureName;

        /// <summary>
        /// The event bus that the DeviceFeature is listening on or sending data to
        /// </summary>
        public required FlightDataEventBus FlightDataEventBus;

        /// <summary>
        /// The event that the DeviceFeature is listening or sending data to
        /// </summary>
        public required FlightDataEvent FlightDataEvent;
    }
}
