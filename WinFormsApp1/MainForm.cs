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
using AFooCockpit.App.Core.Device;
using System.Diagnostics;

namespace WinFormsApp1
{
    public partial class MainForm : Form
    {
        private FlightDataEventBus? FlightDataEventBus;

        public MainForm()
        {
            DeviceManager.RegisterDeviceType("Java ECAM Panel", typeof(JavaDeviceEcam));
            DeviceManager.RegisterDeviceType("Java Switching Panel", typeof(JavaDeviceSwitching));
            DeviceManager.RegisterDeviceType("Java Overhead Panel", typeof(JavaDeviceOverhead));

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
                DeviceManager deviceManager = new DeviceManager(FlightDataEventBus);

                //var com7 = new SerialDataSource(new SerialDataSourceConfig { Port = "COM7" });
                //var com12 = new SerialDataSource(new SerialDataSourceConfig { Port = "COM12" });

                //var ecam = new JavaDeviceEcam(FlightDataEventBus);
                //ecam.ConnectDataSource(com7);

                //var switching = new JavaDeviceSwitching(FlightDataEventBus);
                //switching.ConnectDataSource(com7);

                //var overhead = new JavaDeviceOverhead(FlightDataEventBus);
                //overhead.ConnectDataSource(com12);

                dgvDevices.CreateDevices(deviceManager);

                var flightSimConnection = new FS2024ConnectionDataSource();
                var flightSimVariables = new FS2024VariableDataSource();

                DataSourceLifecycleManager LM = new DataSourceLifecycleManager();

                //LM.Add(DataSourceLifecycleState.HardwareConnect, com7);
                //LM.Add(DataSourceLifecycleState.HardwareConnect, com12);

                Debug.WriteLine($"-----------> having {deviceManager.DataSources.Count} hw data sources");
                LM.Add(DataSourceLifecycleState.HardwareConnect, deviceManager.DataSources);

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

                                // Sync current state of the simulator to hardware
                                aircraft.ForceSync();

                                // Sync devices
                                //switching.ForceSync();
                                //overhead.ForceSync();
                                //ecam.ForceSync();
                                deviceManager.Devices.ForEach(d => d.ForceSync());

                                // Update labels
                                tssAircraft.Text = $"({aircraft.Name})";
                                break;
                        }
                    });
                };

                LM.Connect();

                ShowEventLog();
            }
            catch (Exception ex)
            {
                BeginInvoke((Delegate)(() =>
                {
                    MessageBox.Show(ex.Message);
                    tsbConnect.Enabled = false;
                    tsbEventView.Enabled = true;
                    dgvDevices.Enabled = true;
                }));
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

        private void dgvDevices_Load_1(object sender, EventArgs e)
        {

        }
    }
}
