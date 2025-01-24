using AFooCockpit.App.Core;
using AFooCockpit.App.Core.DataSource;
using AFooCockpit.App.Core.DataSource.DataSources.Serial;
using AFooCockpit.App.Core.FlightData;
using AFooCockpit.App.Gui;
using AFooCockpit.App.Implementations.Java.Devices;
using NLog.Config;
using NLog.Targets;
using NLog;

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
            btnConnect.Enabled = false;
            btnEventLog.Enabled = true;

            _ = Connect();
        }

        private async Task Connect()
        {
            try
            {
                FlightDataEventBus = new FlightDataEventBus();

                var flightSimSource = new DataSourceContainer();
                var aircraftSource = new DataSourceContainer();
                var deviceSources = new DataSourceContainer();

                var com7 = new SerialDataSource(new SerialDataSourceConfig { Port = "COM7" });
                var com12 = new SerialDataSource(new SerialDataSourceConfig { Port = "COM12" });

                deviceSources.AddDataSourc(com7);
                deviceSources.AddDataSourc(com12);

                var ecam = new JavaEcamDevice(FlightDataEventBus);
                ecam.ConnectDataSource(com7);

                var switching= new JavaSwitchingDevice(FlightDataEventBus);
                switching.ConnectDataSource(com7);


                // First, connect to our devices
                await deviceSources.ConnectAll();
                // First - wait for flight sim to connect
                // await flightSimSource.ConnectAll();
                // Then - wait for aircraft to connect
                // await aircraftSource.ConnectAll();

                ShowEventLog();
            }
            catch (Exception)
            {
                BeginInvoke(() =>
                {
                    btnEventLog.Enabled = false;
                    btnConnect.Enabled = true;
                });
            }

        }

        private void btnDebug_Click(object sender, EventArgs e)
        {


        }

        private void btnEventLog_Click(object sender, EventArgs e)
        {
            ShowEventLog();
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
    }
}
