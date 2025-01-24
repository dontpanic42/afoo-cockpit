using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.DataSource;
using AFooCockpit.App.Core.FlightData;

namespace AFooCockpit.App.Core.Device
{
    internal class Device<C, T> : IDevice
        where C : DeviceFeatureConfig
        where T : IDataSource
    {

        /// <summary>
        /// List containing all features of the device
        /// </summary>
        private readonly List<DeviceFeature<C, T>> Features = new List<DeviceFeature<C, T>>();

        /// <summary>
        /// Datasource (if any) connected to this device
        /// </summary>
        private T? DataSource;

        /// <summary>
        /// Getter for the Flight Data Bus this device is listening & publishing to
        /// </summary>
        private readonly FlightDataEventBus MyFlightDataEventBus;
        public FlightDataEventBus FlightDataEventBus => MyFlightDataEventBus;

        /// <summary>
        /// Returns a friendly name for this device
        /// </summary>
        private readonly string MyDeviceName;
        public string DeviceName => MyDeviceName;

        public Device(string deviceName, FlightDataEventBus flightDataEventBus)
        {
            MyFlightDataEventBus = flightDataEventBus;
            MyDeviceName = deviceName;
        }

        /// <summary>
        /// Add Device Features
        /// </summary>
        /// <param name="feature"></param>
        /// <exception cref="Exception"></exception>
        public void AddDeviceFeature(DeviceFeature<C, T> feature) 
        {
            Features.Add(feature);
            if (DataSource != null)
            {
                feature.ConnectDataSource(DataSource);
            }
        }

        /// <summary>
        /// Connect a datasource to this device.
        /// </summary>
        /// <param name="dataSource"></param>
        public void ConnectDataSource(T dataSource)
        {
            DataSource = dataSource;
            Features.ForEach(feature => feature.ConnectDataSource(dataSource));
        }
    }
}
