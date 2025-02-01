using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.DataSource;
using AFooCockpit.App.Core.Device.DeviceFeatures;
using AFooCockpit.App.Core.FlightData;

namespace AFooCockpit.App.Core.Device
{
    public class DeviceManager
    {
        private static readonly string CONNECT_DATASOURCE_METHOD_NAME = "ConnectDataSource";

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private static Dictionary<string, Type> DeviceTypes = new Dictionary<string, Type>();

        private FlightDataEventBus EventBus;

        private Dictionary<string, IDevice> DeviceInstances = new Dictionary<string, IDevice>();

        private HashSet<IDataSource> DataSourceInstances = new HashSet<IDataSource>();

        /// <summary>
        /// Returns a list of device instances managed by this class
        /// </summary>
        public List<IDevice> Devices => DeviceInstances.Values.ToList();

        /// <summary>
        /// Returns a list of data sources connected to devices managed by this class
        /// </summary>
        public List<IDataSource> DataSources => DataSourceInstances.ToList();

        public DeviceManager(FlightDataEventBus flightDataEventBus)
        {
            EventBus = flightDataEventBus;
        }

        /// <summary>
        /// Registers a new device type with a given name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <exception cref="ArgumentException"></exception>
        public static void RegisterDeviceType(string name, Type type)
        {
            if (DeviceTypes.ContainsKey(name))
            {
                throw new ArgumentException($"Device with name {name} already exists!");
            }

            DeviceTypes.Add(name, type);
        }

        /// <summary>
        /// Returns the string keys of the types that can use a datasource of type T
        /// </summary>
        /// <typeparam name="T">The type of datasource that we are looking for</typeparam>
        /// <returns></returns>
        public static List<string> GetDeviceTypesByDataSourceType<T>() where T : IDataSource
        {
            return DeviceTypes
                .Where(t => GetSourceType(t.Value).IsAssignableFrom(typeof(T)))
                .Select(t => t.Key)
                .ToList();
        }

        /// <summary>
        /// Method that finds the type of DataSource that the given Device Type 
        /// supports.
        /// </summary>
        /// <param name="device">The Device Type we want to get the DataSource Type for</param>
        /// <returns>The type of the DataSource</returns>
        /// <exception cref="Exception"></exception>
        private static Type GetSourceType(Type device)
        {
            var methodInfo = device.GetMethod(CONNECT_DATASOURCE_METHOD_NAME);

            if (methodInfo == null)
            {
                logger.Error($"GetSourceType - Device {device.Name} doesn't have method 'ConnectDataSource'");
                throw new Exception($"Device {device.Name} doesn't have method 'ConnectDataSource'");
            }

            var parameterInfo = methodInfo.GetParameters();

            if (parameterInfo.Length != 1)
            {
                logger.Error($"GetSourceType - Invalid parameter signature for Device type {device.Name} - expecting exactly 1 parameter, got {parameterInfo.Length}");
                throw new Exception($"Invalid parameter signature for Device type {device.Name}");
            }

            var parameter = parameterInfo[0];
            var paramType = parameter.ParameterType;

            return paramType;
        }

        /// <summary>
        /// Creates a new device
        /// </summary>
        /// <param name="deviceType">The string type name of the device, for example "Java Overhaead Panel"</param>
        /// <param name="deviceInstanceName">A key that marks the specific instance of the device, for example "Radio Pilot Side"</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception"></exception>
        public void CreateDeviceInstance(string deviceType, string deviceInstanceName)
        {
            if (!DeviceTypes.ContainsKey(deviceType))
            {
                throw new ArgumentException($"Device type {deviceType} does not exist - did your register it?");
            }

            Type type = DeviceTypes[deviceType];
            IDevice? device = (IDevice?)Activator.CreateInstance(type, EventBus);

            if (device == null)
            {
                throw new Exception($"Could not instanciate device ${deviceType} - check logs for errors");
            }

            DeviceInstances[deviceInstanceName] = device;
        }

        /// <summary>
        /// Connects a datasource to a device
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deviceInstanceName"></param>
        /// <param name="dataSource"></param>
        /// <exception cref="Exception"></exception>
        public void ConnectDataSource<T>(string deviceInstanceName, T dataSource) where T : IDataSource
        {
            if (!DeviceInstances.ContainsKey(deviceInstanceName))
            {
                throw new Exception($"Don't have a device called {deviceInstanceName}");
            }

            var device = DeviceInstances[deviceInstanceName];
            var dataSourceType = GetSourceType(device.GetType());

            if (!dataSourceType.IsAssignableFrom(typeof(T)))
            {
                throw new Exception($"Cannot connect data source to device - expecting type {dataSourceType.Name} but got {typeof(T).Name}");
            }

            var connectDSMethod = device.GetType().GetMethod(CONNECT_DATASOURCE_METHOD_NAME);
            if (connectDSMethod == null)
            {
                throw new Exception($"Cannot call {CONNECT_DATASOURCE_METHOD_NAME} on object - method does not exist");
            }

            logger.Debug($"Connecting {dataSource.GetType().Name} to {deviceInstanceName}");
            connectDSMethod.Invoke(device, [dataSource]);

            if (!DataSourceInstances.Contains(dataSource))
            {
                DataSourceInstances.Add(dataSource);
            }
        }
    }
}
