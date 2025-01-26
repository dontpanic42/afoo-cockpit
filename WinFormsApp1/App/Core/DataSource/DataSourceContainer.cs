using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFooCockpit.App.Core.DataSource
{
    internal class DataSourceContainer
    {
        private static readonly int RETRY_INTERVAL_MS = 2000;

        private List<IDataSource> DataSources = new List<IDataSource>();

        public void AddDataSource(IDataSource myDataSource)
        {
            DataSources.Add(myDataSource);
        }

        /// <summary>
        /// Connect all data sources (with retry)
        /// </summary>
        /// <returns></returns>
        public async Task ConnectAll()
        {
            await Task.WhenAll(DataSources.Select(ConnectDataSource));
        }

        /// <summary>
        /// Disconnects all data sources
        /// </summary>
        public async Task DisconnectAll()
        {
            await Task.WhenAll(DataSources.Select(DisconnectDataSource));
        }

        /// <summary>
        /// Connects a single data source with retry
        /// </summary>
        /// <param name="dataSource">The datasource to connect</param>
        /// <returns>Task that is completed when the source is connected</returns>
        private Task ConnectDataSource(IDataSource dataSource)
        {
            return Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        dataSource.Connect();
                        break;
                    }
                    catch (RetryableSourceConnectException)
                    {
                        Thread.Sleep(RETRY_INTERVAL_MS);
                    }
                }
            });
        }

        private Task DisconnectDataSource(IDataSource dataSource)
        {
            return Task.Run(() =>
            {
                dataSource.Disconnect();
            });
        }
    }
}
