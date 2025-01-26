using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.FlightData;
using NLog;
using NLog.Config;


namespace AFooCockpit.App.Core.DataSource
{
    delegate void DataSourceLifeCycleChangeHandler(DataSourceLifecycleManager lifeCycleManager, DataSourceLifecycleChangeEventArgs eventArgs);

    enum DataSourceLifecycleState
    {
        /// <summary>
        /// No event is triggered - default state before connect
        /// </summary>
        None,

        /// <summary>
        /// First event to be triggered, before any hardware/sim/flight connection
        /// </summary>
        Connect,

        /// <summary>
        /// Event triggered before hardware is connected
        /// </summary>
        HardwareConnect,
        /// <summary>
        /// Event triggered when hardware is fully connected
        /// </summary>
        HardwareConnected,

        /// <summary>
        /// Event triggered when hardware is connected, but sim not yet connected
        /// </summary>
        SimConnect,
        /// <summary>
        /// Event triggered when sim is connected
        /// </summary>
        SimConnected,

        /// <summary>
        /// Event triggered when sim is connected, but flight is not yet started
        /// </summary>
        FlightConnect,
        /// <summary>
        /// Event triggered when flight has been started (= Flight variables are available)
        /// </summary>
        FlightConnected,

        /// <summary>
        /// Event triggered before hardware is disconnected
        /// 
        /// Note: Disconnection can also happen without this event being triggered, see Failed
        /// </summary>
        Disconnect,
        /// <summary>
        /// Event triggered after hardware is disconnected
        /// </summary>
        Disconnected,

        /// <summary>
        /// The lifecycle finished
        /// </summary>
        Finished,
        /// <summary>
        /// There was an error in the lifecycle exection - check event args for details
        /// </summary>
        Failed
    }

    internal class DataSourceLifecycleManager
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static readonly Dictionary<DataSourceLifecycleState, DataSourceLifecycleState> StateGraph = new Dictionary<DataSourceLifecycleState, DataSourceLifecycleState>()
        {
            { DataSourceLifecycleState.None, DataSourceLifecycleState.Connect },
            { DataSourceLifecycleState.Connect, DataSourceLifecycleState.HardwareConnect },
            { DataSourceLifecycleState.HardwareConnect, DataSourceLifecycleState.HardwareConnected },
            { DataSourceLifecycleState.HardwareConnected, DataSourceLifecycleState.SimConnect },
            { DataSourceLifecycleState.SimConnect, DataSourceLifecycleState.SimConnected },
            { DataSourceLifecycleState.SimConnected, DataSourceLifecycleState.FlightConnect },
            { DataSourceLifecycleState.FlightConnect, DataSourceLifecycleState.FlightConnected },
            { DataSourceLifecycleState.Disconnect, DataSourceLifecycleState.Disconnected },
            { DataSourceLifecycleState.Disconnected, DataSourceLifecycleState.Finished }
        };

        /// <summary>
        /// Contains datasources per state
        /// </summary>
        private Dictionary<DataSourceLifecycleState, DataSourceContainer> StateDataSourceContainers = new Dictionary<DataSourceLifecycleState, DataSourceContainer>();

        /// <summary>
        /// We need to disconnect our data source containers in reverse order, so here we're keeping a stack
        /// that we can walk through reverse
        /// </summary>
        private Stack<DataSourceContainer> DataSourceConnectionStack = new Stack<DataSourceContainer>();

        /// <summary>
        /// The current state in the lifecycle
        /// </summary>
        public DataSourceLifecycleState State { get; private set; } = DataSourceLifecycleState.None;

        public string StateName { get => GetStateName(State); }

        /// <summary>
        /// Event that gets triggered for each lifecycle event change
        /// </summary>
        public event DataSourceLifeCycleChangeHandler? OnStateChange;

        public DataSourceLifecycleManager()
        {
            // Initialize StateDataSourceContainers by creating a DataSourceContainer for each possible state
            Enum
                .GetValues(typeof(DataSourceLifecycleState))
                .Cast<DataSourceLifecycleState>()
                .ToList()
                .ForEach(state => StateDataSourceContainers.Add(state, new DataSourceContainer()));

            OnStateChange += DataSourceLifecycleManager_OnStateChange;
        }

        private void DataSourceLifecycleManager_OnStateChange(DataSourceLifecycleManager lifeCycleManager, DataSourceLifecycleChangeEventArgs eventArgs)
        {
            logger.Debug($"Got event {GetStateName(eventArgs.State)}");

            switch(eventArgs.State)
            {
                // Life cycle events that don't need a specific action - we can just jump to the next state
                case DataSourceLifecycleState.Connect:
                case DataSourceLifecycleState.HardwareConnected:
                case DataSourceLifecycleState.SimConnected:
                case DataSourceLifecycleState.Disconnected:
                    Next(eventArgs.State);
                    break;

                // Connect type life cycle events - connect the containers for the specific state
                case DataSourceLifecycleState.HardwareConnect:
                case DataSourceLifecycleState.SimConnect:
                case DataSourceLifecycleState.FlightConnect:
                    _ = ConnectDataSourceContainer(eventArgs.State);
                    break;

                // Disconnect type life cycle events - disconnect all containers
                case DataSourceLifecycleState.Disconnect:
                    _ = DisconnectDataSourceContainers(eventArgs.State);
                    break;

                // End state
                case DataSourceLifecycleState.FlightConnected:
                case DataSourceLifecycleState.Finished:
                case DataSourceLifecycleState.Failed:
                    break;

                default:
                    logger.Warn("Uknown event - don't know what to do.");
                    break;
            }
        }

        /// <summary>
        /// Starts the container life cycle
        /// </summary>
        public void Connect()
        {
            SetCurrentState(DataSourceLifecycleState.Connect);
        }

        /// <summary>
        /// Disconnects all data sources. Can only be used when
        /// in FlightConnected state
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void Disconnect()
        {
            SetCurrentState(DataSourceLifecycleState.Disconnect);
        }

        /// <summary>
        /// Adds a new datasource to a given state
        /// </summary>
        /// <param name="state"></param>
        /// <param name="dataSource"></param>
        /// <exception cref="Exception"></exception>
        public void Add(DataSourceLifecycleState state, IDataSource dataSource)
        {
            if (State != DataSourceLifecycleState.None)
            {
                throw new Exception("Cannot add datasource to lifecycle - lifecycle already started");
            }

            if (!StateDataSourceContainers.ContainsKey(state))
            {
                throw new Exception("Cannot add data source to container for given state - state is unknown");
            }

            StateDataSourceContainers[state].AddDataSource(dataSource);
        }

        /// <summary>
        /// Set the current state, with a default lifecycle state event arg
        /// </summary>
        /// <param name="state"></param>
        private void SetCurrentState(DataSourceLifecycleState state)
        {
            var eventArgs = new DataSourceLifecycleChangeEventArgs { State = state };
            SetCurrentState(state, eventArgs);
        }

        /// <summary>
        /// Sets the current state, with a given lifecylce state event arg
        /// </summary>
        /// <param name="state"></param>
        /// <param name="eventArgs"></param>
        private void SetCurrentState(DataSourceLifecycleState state, DataSourceLifecycleChangeEventArgs eventArgs)
        {
            logger.Info($"Entering life cycle state {GetStateName(state)}");
            State = state;
            OnStateChange?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// Switch to the next state (if there is any)
        /// </summary>
        /// <param name="state"></param>
        private void Next(DataSourceLifecycleState state)
        {
            if (StateGraph.ContainsKey(state))
            {
                var nextState = StateGraph[state];

                if(nextState == state)
                {
                    throw new Exception($"Loop detected - next state for {GetStateName(state)} would be same state!");
                }

                SetCurrentState(nextState);
            } 
            else
            {
                logger.Warn($"Try entering next state, but state has no successor {GetStateName(state)}");
            }
        }

        /// <summary>
        /// Connect a datasource container and trigger the next state
        /// </summary>
        /// <param name="state">The current state</param>
        /// <returns></returns>
        private async Task ConnectDataSourceContainer(DataSourceLifecycleState state)
        {
            var container = StateDataSourceContainers.GetValueOrDefault(state, new DataSourceContainer());

            try
            {
                await container.ConnectAll();
                // After successfully connecting the container, we need to push it on the disconnect stack
                DataSourceConnectionStack.Push(container);
                Next(state);
            }
            catch (Exception ex) 
            {
                _ = HandleLifecycleException(state, ex);
            }
        }

        /// <summary>
        /// Disconnects data source containers in reverse order and triggers the next state
        /// </summary>
        /// <param name="state">The current state</param>
        /// <returns></returns>
        private async Task DisconnectDataSourceContainers(DataSourceLifecycleState state)
        {
            try
            {
                while (DataSourceConnectionStack.Count > 0)
                {
                    var container = DataSourceConnectionStack.Pop();
                    await DisconnectDataSourceContainer(state, container);
                }

                Next(state);
            }
            catch (Exception ex)
            {
                _ = HandleLifecycleException(state, ex);
            }
        }

        /// <summary>
        /// Disconnects a given datasource container
        /// </summary>
        /// <param name="state">Needed for exception handling</param>
        /// <param name="container">The container to disconnect</param>
        /// <returns></returns>
        private async Task DisconnectDataSourceContainer(DataSourceLifecycleState state, DataSourceContainer container)
        {
            try
            {
                await container.DisconnectAll();
            }
            catch (Exception ex)
            {
                _ = HandleLifecycleException(state, ex);
            }
        }

        /// <summary>
        /// Method that handles exceptions during the lifecycle
        /// 
        /// - It sets the lifecycle state to Failed
        /// - It disconnects all so far connected data source containers
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        private async Task HandleLifecycleException(DataSourceLifecycleState state, Exception ex)
        {
            if (state == DataSourceLifecycleState.Failed)
            {
                logger.Warn($"Got error during error handling {ex}, ignoring");
                return;
            }

            var eventArgs = new DataSourceLifecycleChangeEventArgs();
            eventArgs.State = DataSourceLifecycleState.Failed;
            eventArgs.FailedState = state;
            eventArgs.FailedStateName = GetStateName(state);
            eventArgs.FailedException = ex;

            SetCurrentState(state, eventArgs);

            await DisconnectDataSourceContainers(DataSourceLifecycleState.Failed);
        }

        /// <summary>
        /// Returns a friendly name for the given state
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private string GetStateName(DataSourceLifecycleState state)
        {
            return Enum.GetName(typeof(DataSourceLifecycleState), State)!;
        }
    }
}
