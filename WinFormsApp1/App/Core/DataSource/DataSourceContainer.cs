using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog.Config;

namespace AFooCockpit.App.Core.DataSource
{
    internal class DataSourceContainer
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private static readonly int RETRY_INTERVAL_MS = 2000;

        private List<IDataSource> DataSources = new List<IDataSource>();

        /// <summary>
        /// Flag that shows that disconnect was started.
        /// Explanation: We might be in a situation that the user asks us to disconnect
        /// Before the connect was finished. In that case, we need to set a flag to stop
        /// the connection attempts.
        /// </summary>
        private bool IsDisconnecting = false;
        /// <summary>
        /// Holds the current connect task. Used to wait for connect to finish 
        /// until we disconnect (=> Set IsDisconnecting flag, otherwise we'll wait
        /// forever for Retryable connection errors to be resolved)
        /// </summary>
        private Task? CurrentConnectTask = null;
        /// <summary>
        /// Same as with Connect, we save the disconnect task so we can wait for 
        /// the disconnect to finish before connecting
        /// </summary>
        private Task? CurrentDisconnectTask = null;

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
            logger.Debug($"Container - connecting {DataSources.Count} Data Sources");

            // If we're currently disconnecting, wait for the disconnect to be fully finished
            // before we resume with connecting
            if (CurrentDisconnectTask != null && !CurrentDisconnectTask.IsCompleted)
            {
                logger.Info("Disconnect task is still in progress, waiting for it to finish.");
                // Wait for the disconnect task to fully finish
                await CurrentDisconnectTask;
                // At this point, we're disconnected
                logger.Info("Disconnect task is still in progress, waiting for it to finish.");
            }

            // Clear the disconnecting flag so our connect retry works
            IsDisconnecting = false;

            // Connect...
            CurrentConnectTask = Task.WhenAll(DataSources.Select(ConnectDataSource));

            try 
            {
                await CurrentConnectTask;
            } 
            catch (SourceConnectionInterruptedException ex)
            {
                // We can safely ignore interruption exceptions
                logger.Debug(ex.Message);
            }
        }

        /// <summary>
        /// Disconnects all data sources
        /// </summary>
        public async Task DisconnectAll()
        {
            // If a disconnect task is already running, and it's not completed yet
            // return since there's nothing to do here
            if (CurrentDisconnectTask != null && !CurrentDisconnectTask.IsCompleted)
            {
                logger.Debug("Disconnect already in progress - ignoring");
                return;
            }

            // Set the flag that should abort the connection
            IsDisconnecting = true;

            // If we are currently still connecting
            if (CurrentConnectTask != null && !CurrentConnectTask.IsCompleted)
            {
                logger.Info("Connect task is still in progress, waiting for it to finish.");
                // Wait for the connection attempt to be fully finished
                try 
                { 
                    await CurrentConnectTask;
                } 
                catch (SourceConnectionInterruptedException)
                {
                    // It's expected that we get a source connection interrupted exception, we can safely ignore it
                    logger.Debug("Got expected source connection interrupted exception");
                }
                // At this point, the connect task has finished
                logger.Info("Connect task finished, proceeding with disconnect");
            }

            // Set the CurrentDisconnectTask to the task object, and wait for it to be completed
            //await (CurrentDisconnectTask = Task.WhenAll(DataSources.Select(DisconnectDataSource)));

            CurrentDisconnectTask = Task.WhenAll(DataSources.Select(DisconnectDataSource));
            await CurrentDisconnectTask;

            logger.Info("Disconnect finished.");

            IsDisconnecting = false;
            CurrentDisconnectTask = null;
        }

        /// <summary>
        /// Connects a single data source with retry
        /// </summary>
        /// <param name="dataSource">The datasource to connect</param>
        /// <returns>Task that is completed when the source is connected</returns>
        private async Task ConnectDataSource(IDataSource dataSource)
        {
            await Task.Run(() =>
            {
                // While loop for retrying the connection as long as we don't get a 
                // non-retryable exception, or the user is trying to disconnect
                while (true)
                {
                    try
                    {
                        logger.Debug($"Is Disconnecting? {IsDisconnecting}");
                        // Try to connect the data source
                        dataSource.Connect();
                        // If connection was sucessful (= didn't throw), we can break the loop
                        break;
                    }
                    catch (RetryableSourceConnectException)
                    {
                        if (IsDisconnecting)
                        {
                            throw new SourceConnectionInterruptedException($"{dataSource.GetType().Name} connection retry was interruped by disconnect");
                        }

                        // If we get a retryable exception, we're just waiting a bit 
                        // and let the loop go for another round
                        Thread.Sleep(RETRY_INTERVAL_MS);
                    }
                }
            });
        }

        private async Task DisconnectDataSource(IDataSource dataSource)
        {
            await Task.Run(dataSource.Disconnect);
        }
    }
}
