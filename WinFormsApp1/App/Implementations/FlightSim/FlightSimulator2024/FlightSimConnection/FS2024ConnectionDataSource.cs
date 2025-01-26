using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.DataSource;
using FSUIPC;
using VS = FSUIPC.MSFSVariableServices;

namespace AFooCockpit.App.Implementations.FlightSim.FlightSimulator2024.FlightSimConnection
{
    internal class FS2024ConnectionDataSource : DataSource<DataSourceConfig, DataSourceData>
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public FS2024ConnectionDataSource() : base(new DataSourceConfig())
        {
            VS.Init();
            VS.LogLevel = LOGLEVEL.LOG_LEVEL_INFO;
        }

        public override SourceState State
        {
            get
            {
                return FSUIPCConnection.IsOpen ? SourceState.Connected : SourceState.Disconnected;
            }
        }

        protected override void ConnectSource()
        {
            try
            {
                FSUIPCConnection.Open();
            }
            catch (FSUIPCException e) 
            {
                if (e.FSUIPCErrorCode == FSUIPCError.FSUIPC_ERR_NOFS)
                {
                    throw new RetryableSourceConnectException("Error while opening FSUIPC Connection, FS not running");
                }

                throw new FatalSourceConnectException($"FSUIPC error: {e.Message}");
            }
            catch (Exception e)
            {
                throw new FatalSourceConnectException($"Error: {e.Message}");
            }
        }

        protected override void DisconnectSource()
        {
            if (FSUIPCConnection.IsOpen)
            {
                {
                    FSUIPCConnection.Close();
                }
            }
        }

        public override void Send(DataSourceData data)
        {
            throw new NotImplementedException();
        }
    }
}
