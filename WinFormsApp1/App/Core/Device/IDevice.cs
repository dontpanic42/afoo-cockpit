using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.DataSource;
using AFooCockpit.App.Core.FlightData;

namespace AFooCockpit.App.Core.Device
{
    public interface IDevice
    {
        /// <summary>
        /// Returns the event bus instance that this device is listening & publishing to
        /// </summary>
        public FlightDataEventBus FlightDataEventBus { get; }

        /// <summary>
        /// Returns a friendly name for this device
        /// </summary>
        public string DeviceName {  get; }

        /// <summary>
        /// Forces a synchronization with the datasource and the bus
        /// </summary>
        public void ForceSync();
    }
}
