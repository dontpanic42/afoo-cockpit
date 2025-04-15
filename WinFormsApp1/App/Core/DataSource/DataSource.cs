using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Gui.DataSourceConfigForms;

namespace AFooCockpit.App.Core.DataSource
{
    /// <summary>
    /// A data source, like a serial port, a usb port, an aircraft, ...
    /// </summary>
    /// <typeparam name="C">Configuration type of the data source</typeparam>
    /// <typeparam name="T">Data type of the data source</typeparam>
    internal abstract class DataSource<C, T> : IDataSource
        where T : DataSourceData
        where C : DataSourceConfig
    {

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();


        /// <summary>
        /// Event handler for data source data receive events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void DataReceiveEventHandler(DataSource<C, T> sender, DataSourceReciveEventArgs<T> e);

        /// <summary>
        /// Event that gets triggered when new data has been recieved from the source
        /// </summary>
        public event DataReceiveEventHandler? OnDataReceive;

        /// <summary>
        /// Event that gets triggered when the state of the event source changes (connect/disconnect)
        /// </summary>
        public event StateEventHandler? OnStateEvent;

        public abstract SourceState State { get; }

        public C Config { get; private set; }

        public DataSource(C eventSourceConfig)
        {
            Config = eventSourceConfig;
        }

        /// <summary>
        /// Connect data source
        /// </summary>
        public void Connect()
        {
            ConnectSource();
            TriggerConnectEvent();
        }

        /// <summary>
        /// Disconnect data source
        /// </summary>
        public void Disconnect()
        {
            DisconnectSource();
            TriggerDisconnectEvent();
        }

        /// <summary>
        /// Triggers a connect event
        /// </summary>
        private void TriggerConnectEvent()
        {
            OnStateEvent?.Invoke(this, new StateEventArgs { State = SourceState.Connected });
        }

        /// <summary>
        /// Triggers a disconnect event. Should only be used in an unexpected disconnect event,
        /// i.e. not when 'Disconnect()' is called
        /// </summary>
        protected void TriggerDisconnectEvent()
        {
            OnStateEvent?.Invoke(this, new StateEventArgs { State = SourceState.Disconnected });
        }

        /// <summary>
        /// To be used by the implementing data source to trigger a DataReceived event
        /// </summary>
        /// <param name="data">The data that was received</param>
        protected void TriggerDataReceiveEvent(T data)
        {
            var eventArgs = new DataSourceReciveEventArgs<T> { Data = data };
            OnDataReceive?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// Connect to a data source
        /// </summary>
        protected abstract void ConnectSource();

        /// <summary>
        /// Disconnect from a data source
        /// </summary>
        protected abstract void DisconnectSource();

        /// <summary>
        /// Send data to the data source
        /// </summary>
        /// <param name="data"></param>
        public abstract void Send(T data);
    }
}
