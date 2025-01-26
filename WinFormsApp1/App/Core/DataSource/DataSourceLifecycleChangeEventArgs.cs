using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFooCockpit.App.Core.DataSource
{
    internal class DataSourceLifecycleChangeEventArgs : EventArgs
    {
        public DataSourceLifecycleState State = DataSourceLifecycleState.None;

        /// <summary>
        /// If the app is in 'Failed' state, this field will contain the state that it failed in
        /// </summary>
        public DataSourceLifecycleState? FailedState;

        /// <summary>
        /// If the app is in 'Failed' state, this field will contain the name of the state that it failed in
        /// </summary>
        public string? FailedStateName;

        /// <summary>
        /// Field that is set if the State is Failed
        /// </summary>
        public Exception? FailedException;
    }
}
