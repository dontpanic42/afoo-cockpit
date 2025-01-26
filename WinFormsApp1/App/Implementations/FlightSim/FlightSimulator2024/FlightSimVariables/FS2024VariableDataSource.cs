using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.DataSource;
using AFooCockpit.App.Core.DataSource.DataSources.Sim.VariableSource;
using AFooCockpit.App.Core.FlightData;
using FSUIPC;
using NLog;
using VS = FSUIPC.MSFSVariableServices;

namespace AFooCockpit.App.Implementations.FlightSim.FlightSimulator2024.FlightSimVariables
{
    internal class FS2024VariableDataSource : FlightSimVariableDataSource
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private Dictionary<string, FsLVar> MonitoredLVars = new Dictionary<string, FsLVar>();   

        public FS2024VariableDataSource() : base()
        {
        }

        public override SourceState State => (VS.IsRunning && VS.LVars.Count > 0) ? SourceState.Connected : SourceState.Disconnected;

        protected override void ConnectSource()
        {
            try
            {
                if (!VS.IsRunning)
                { 
                    VS.Start();
                }

                if (VS.LVars.Count == 0)
                {
                    throw new RetryableSourceConnectException("Variable service not ready - no LVars found");
                }
            }
            catch (Exception ex)
            {
                logger.Debug(ex);
                throw new RetryableSourceConnectException("Variable service not ready");
            }
        }

        protected override void DisconnectSource()
        {
            if (VS.IsRunning)
            {
                VS.Stop();
            }
        }

        protected override void RegisterFlightVariable(string variable)
        {
            if (VS.LVars.Exists(variable))
            {
                var lvar = VS.LVars[variable];
                MonitoredLVars.Add(variable, lvar);
                lvar.OnValueChanged += Lvar_OnValueChanged;
            }
            else
            {
                logger.Warn($"Requsted sim variable {variable}, but variable doesn't exist in sim");
            }
        }

        /// <summary>
        /// Event handler that gets called when a variable was updated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Lvar_OnValueChanged(object? sender, LVarEvent e)
        {
            var lvar = e.LVar;
            if (lvar != null)
            {
                var data = new FlightSimVariableDataSourceData { 
                    VariableName = lvar.Name, 
                    VariableValue = lvar.Value 
                };

                TriggerDataReceiveEvent(data);
            }
        }

        /// <summary>
        /// Remove monitoring of a variable
        /// </summary>
        /// <param name="variable"></param>
        protected override void UnregisterFlightVariable(string variable)
        {
            if (MonitoredLVars.ContainsKey(variable))
            {
                MonitoredLVars[variable].OnValueChanged -= Lvar_OnValueChanged;
                MonitoredLVars.Remove(variable);
            }
        }

        /// <summary>
        /// Set a flight variable
        /// </summary>
        /// <param name="data"></param>
        public override void Send(FlightSimVariableDataSourceData data)
        {
            if (VS.LVars.Exists(data.VariableName))
            {
                VS.LVars[data.VariableName].SetValue(data.VariableValue);
            }
            else
            {
                logger.Warn($"Attemtp to write to variable {data.VariableName}, but variable doesn't exist");
            }
        }
    }
}
