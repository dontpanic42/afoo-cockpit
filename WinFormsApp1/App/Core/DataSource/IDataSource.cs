using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFooCockpit.App.Core.DataSource
{

    public abstract class SourceConnectException : Exception
    {
        public SourceConnectException(string message) : base(message)
        {
        }
    }

    /// <summary>
    /// Exception that is thrown when a connection was not possible, but can be re-tried
    /// </summary>
    public class RetryableSourceConnectException : SourceConnectException
    {
        public RetryableSourceConnectException(string message) : base(message)
        {
        }
    }

    /// <summary>
    /// Exception that is triggered when a connection was not possible, and should not be re-tried
    /// </summary>
    public class FatalSourceConnectException : SourceConnectException
    {
        public FatalSourceConnectException(string message) : base(message)
        {
        }
    }

    /// <summary>
    /// Enum that holds the possible DataSource states
    /// </summary>
    public enum SourceState
    {
        Disconnected,
        Connected
    }

    /// <summary>
    /// Event args definition for data source state events
    /// </summary>
    public class StateEventArgs : EventArgs
    {
        public SourceState State;
    }

    /// <summary>
    /// Event handler for data source state events
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="eventArgs"></param>
    delegate void StateEventHandler(IDataSource sender, StateEventArgs eventArgs);

    internal interface IDataSource
    {
        /// <summary>
        /// Event that gets triggered when the state of the event source changes (connect/disconnect)
        /// </summary>
        public event StateEventHandler? OnStateEvent;

        public SourceState State { get; }

        public void Connect();

        public void Disconnect();
    }
}
