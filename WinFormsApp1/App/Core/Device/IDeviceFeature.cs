using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.FlightData;

namespace AFooCockpit.App.Core.Device
{
    internal interface IDeviceFeature
    {
        /// <summary>
        /// Name of the feature, for logging purposes
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Each device feature can only belong to one flight data event
        /// The data event can either be reading (e.g. indicators)
        /// or sending (e.g. buttons) or both
        /// </summary>
        public FlightDataEvent FlightDataEvent { get; }

        /// <summary>
        /// Returns the flight data event bus that this device is attached
        /// to
        /// </summary>
        public FlightDataEventBus FlightDataEventBus { get; }
    }
}
