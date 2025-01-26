using AFooCockpit.App.Core;
using AFooCockpit.App.Core.DataSource;
using AFooCockpit.App.Core.DataSource.DataSources.Serial;
using AFooCockpit.App.Core.FlightData;
using AFooCockpit.App.Gui;
using AFooCockpit.App.Implementations.Java.Devices;
using NLog.Config;
using NLog.Targets;
using NLog;
using AFooCockpit.App.Implementations.FlightSim.FlightSimulator2024.FlightSimConnection;
using AFooCockpit.App.Implementations.FlightSim.FlightSimulator2024.FlightSimVariables;
using AFooCockpit.App.Core.Aircraft;
using AFooCockpit.App.Implementations.Aircraft;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private FlightDataEventBus? FlightDataEventBus;

        public Form1()
        {
            InitializeComponent();
        }


        private void btnConnect_Click(object sender, EventArgs e)
        {

        }

        private async Task Connect()
        {
            try
            {
                FlightDataEventBus = new FlightDataEventBus();

                var com7 = new SerialDataSource(new SerialDataSourceConfig { Port = "COM7" });
                var com12 = new SerialDataSource(new SerialDataSourceConfig { Port = "COM12" });

                var ecam = new JavaDeviceEcam(FlightDataEventBus);
                ecam.ConnectDataSource(com7);

                var switching = new JavaDeviceSwitching(FlightDataEventBus);
                switching.ConnectDataSource(com7);

                var overhead = new JavaDeviceOverhead(FlightDataEventBus);
                overhead.ConnectDataSource(com12);

                var flightSimConnection = new FS2024ConnectionDataSource();
                var flightSimVariables = new FS2024VariableDataSource();

                DataSourceLifecycleManager LM = new DataSourceLifecycleManager();

                LM.Add(DataSourceLifecycleState.HardwareConnect, com7);
                LM.Add(DataSourceLifecycleState.HardwareConnect, com12);

                LM.Add(DataSourceLifecycleState.SimConnect, flightSimConnection);
                LM.Add(DataSourceLifecycleState.FlightConnect, flightSimVariables);

                Aircraft aircraft;

                LM.OnStateChange += (_, eventArgs) =>
                {
                    BeginInvoke(() =>
                    {
                        tssStatus.Text = LM.StateName;

                        switch (eventArgs.State)
                        {
                            case DataSourceLifecycleState.Failed:
                                MessageBox.Show($"An error occured in stage {eventArgs.FailedStateName}: {eventArgs.FailedException?.Message}");
                                break;

                            case DataSourceLifecycleState.FlightConnected:
                                aircraft = new FenixA3XX(FlightDataEventBus, flightSimVariables);
                                tssAircraft.Text = $"({aircraft.Name})";
                                break;
                        }
                    });
                };

                LM.Connect();

                ShowEventLog();
            }
            catch (Exception)
            {
                BeginInvoke(() =>
                {
                    tsbConnect.Enabled = false;
                    tsbEventView.Enabled = true;
                    dgvDevices.Enabled = true;
                });
            }

        }

        private void btnDebug_Click(object sender, EventArgs e)
        {


        }

        private void btnEventLog_Click(object sender, EventArgs e)
        {
        }

        private void ShowEventLog()
        {
            if (FlightDataEventBus != null)
            {
                var debugView = new DebugView();
                debugView.Show(this);
                debugView.ConnectBus(FlightDataEventBus);
            }
        }

        private void tsbConnect_Click(object sender, EventArgs e)
        {
            tsbConnect.Enabled = false;
            tsbEventView.Enabled = true;
            dgvDevices.Enabled = false;

            _ = Connect();
        }

        private void tspEventView_Click(object sender, EventArgs e)
        {
            ShowEventLog();
        }

        private void dgvDevices_Load(object sender, EventArgs e)
        {

        }
    }
}
