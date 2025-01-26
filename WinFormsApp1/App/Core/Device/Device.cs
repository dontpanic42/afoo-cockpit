using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.DataSource;
using AFooCockpit.App.Core.Device.DeviceFeatures;
using AFooCockpit.App.Core.FlightData;
using NLog;

namespace AFooCockpit.App.Core.Device
{
    internal class Device<C, T> : IDevice
        where C : DeviceFeatureConfig
        where T : IDataSource
    {

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

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
        /// Connect a datasource to this device.
        /// </summary>
        /// <param name="dataSource"></param>
        public void ConnectDataSource(T dataSource)
        {
            DataSource = dataSource;
            Features.ForEach(feature => feature.ConnectDataSource(dataSource));
        }

        /// <summary>
        /// Generic method to instanciate and create a device feature
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <param name="deviceFeatureConfig"></param>
        protected void AddDeviceFeature<K>(C deviceFeatureConfig) where K : DeviceFeature<C, T>
        {
            var feature = Activator.CreateInstance(typeof(K), deviceFeatureConfig) as K;
            if (feature != null)
            {
                AddDeviceFeature(feature!);
            }
            else
            {
                logger.Error($"Cannot add device feature of type {typeof(K)}: Constructor not taking one arg?");
            }
            AddDeviceFeature(feature!);
        }

        /// <summary>
        /// Add Device Features
        /// </summary>
        /// <param name="feature"></param>
        /// <exception cref="Exception"></exception>
        protected void AddDeviceFeature<K>(K feature) where K : DeviceFeature<C, T>
        {
            Features.Add(feature);
            if (DataSource != null)
            {
                feature.ConnectDataSource(DataSource);
            }
        }
    }
}
