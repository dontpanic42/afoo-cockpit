using AFooCockpit.App.Core.DataSource;
using AFooCockpit.App.Core.FlightData;
using AFooCockpit.App.Gui;
using AFooCockpit.App.Implementations.Java.Devices;
using AFooCockpit.App.Implementations.FlightSim.FlightSimulator2024.FlightSimConnection;
using AFooCockpit.App.Implementations.FlightSim.FlightSimulator2024.FlightSimVariables;
using AFooCockpit.App.Core.Aircraft;
using AFooCockpit.App.Implementations.Aircraft;
using AFooCockpit.App.Core.Device;
using AFooCockpit.App.Core.Settings;
using AFooCockpit.App.Core.DataSource.DataSources.Arduino;
using AFooCockpit.App.Implementations.ArduinoSerialDevice.Devices;

namespace WinFormsApp1
{
    public partial class MainForm : Form
    {
        private FlightDataEventBus? FlightDataEventBus;
        private DataSourceLifecycleManager? LM;
        private DebugView? DebugView;

        public MainForm()
        {
            DeviceManager.RegisterDeviceType("Java ECAM Panel", typeof(JavaDeviceEcam));
            DeviceManager.RegisterDeviceType("Java Switching Panel", typeof(JavaDeviceSwitching));
            DeviceManager.RegisterDeviceType("Java Overhead Panel", typeof(JavaDeviceOverhead));

            DeviceManager.RegisterDeviceType("OEM Panel Backlight", typeof(ArduinoSerialDevicePanelLighting));

            InitializeComponent();
            InitializeSettingsHandling();
        }

        /// <summary>
        /// Disconnect a currently active connection
        /// </summary>
        private void Disconnect()
        {
            if (LM != null)
            {
                try
                {
                    LM.Disconnect();
                }
                catch (Exception ex)
                {
                    ShowError(ex);
                    EnableForm();
                }
            }
        }

        /// <summary>
        /// Disables the form to prevent changes when already connected
        /// </summary>
        private void DisableForm()
        {
            BeginInvoke(() =>
            {
                tsbConnect.Enabled = false;
                tsbEventView.Enabled = true;
                tsbDisconnect.Enabled = true;
                dgvSerialDevices.Enabled = false;
            });
        }

        /// <summary>
        /// Enables the form after connection is disconnected
        /// </summary>
        private void EnableForm()
        {
            BeginInvoke(() =>
            {
                tsbConnect.Enabled = true;
                tsbEventView.Enabled = false;
                tsbDisconnect.Enabled = false;
                dgvSerialDevices.Enabled = true;
            });
        }

        /// <summary>
        /// Updates the status labels
        /// </summary>
        /// <param name="status"></param>
        /// <param name="aircraft"></param>
        private void UpdateStatus(string status, string aircraft)
        {
            BeginInvoke(() =>
            {
                tssStatus.Text = status;
                tssAircraft.Text = $"({aircraft})";
            });
        }

        /// <summary>
        /// Show an error message to the user
        /// </summary>
        /// <param name="message"></param>
        private void ShowError(string message)
        {
            BeginInvoke(() =>
            {
                MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            });
        }

        /// <summary>
        /// Show an exception to the user
        /// </summary>
        /// <param name="ex"></param>
        private void ShowError(Exception ex)
        {
            ShowError(ex.Message);
        }

        private void Connect()
        {
            try
            {
                // Instanciate new Flight Data Event Bus
                FlightDataEventBus = new FlightDataEventBus();
                // Instanciate application state machine
                LM = new DataSourceLifecycleManager();

                // Create an instance of the event log viewer (but do not show it by default)
                CreateEventLog(FlightDataEventBus);

                // Create device manager
                DeviceManager deviceManager = new DeviceManager(FlightDataEventBus);
                // Add serial devices from the serial device view
                dgvSerialDevices.CreateDevices(deviceManager);
                dgvArduinoDevices.CreateDevices(deviceManager);

                // Hardcoded data sources - Flight Simulator 2024
                var flightSimConnection = new FS2024ConnectionDataSource();
                var flightSimVariables = new FS2024VariableDataSource();

                // Add data sources to the app lifecycle,
                // Start with serial hardware
                LM.Add(DataSourceLifecycleState.HardwareConnect, deviceManager.DataSources);
                // Add the hardcoded flight simulator data sources
                LM.Add(DataSourceLifecycleState.SimConnect, flightSimConnection);
                LM.Add(DataSourceLifecycleState.FlightConnect, flightSimVariables);

                Aircraft aircraft;

                LM.OnStateChange += (_, eventArgs) =>
                {
                    UpdateStatus(LM.StateName, "No aircraft");

                    switch (eventArgs.State)
                    {
                        case DataSourceLifecycleState.Connect:
                            DisableForm();
                            break;

                        case DataSourceLifecycleState.FlightConnected:
                            // Sim is ready and in flight - we can create the aircraft instance
                            aircraft = new FenixA3XX(FlightDataEventBus, flightSimVariables);

                            // Sync current state of the simulator to hardware
                            aircraft.ForceSync();

                            // Sync devices
                            deviceManager.Devices.ForEach(d => d.ForceSync());

                            // Update labels
                            UpdateStatus(LM.StateName, aircraft.Name);
                            break;

                        // The connection is closed (success)
                        case DataSourceLifecycleState.Finished:
                            // Update labels
                            UpdateStatus("Ready to connect", "No aircraft");
                            DisposeEventLog();
                            EnableForm();
                            break;
                        // The connection is closed (error)
                        case DataSourceLifecycleState.Failed:
                            ShowError($"An error occured in stage {eventArgs.FailedStateName}: {eventArgs.FailedException?.Message}");
                            EnableForm();
                            break;
                    }
                };

                LM.Connect();

                ShowEventLog();
            }
            catch (Exception ex)
            {
                ShowError(ex);
                EnableForm();
            }
        }

        /// <summary>
        /// Create a new event log (when starting a new connection)
        /// </summary>
        /// <param name="eventBus"></param>
        private void CreateEventLog(FlightDataEventBus eventBus)
        {
            BeginInvoke(() =>
            {
                DebugView = new DebugView();
                DebugView.ConnectBus(eventBus);

                tsbEventView.Enabled = true;
            });
        }

        /// <summary>
        /// Show the event log dialog if it's hidden
        /// </summary>
        private void ShowEventLog()
        {
            BeginInvoke(() =>
            {
                if (DebugView != null && !DebugView.IsDisposed)
                {
                    if (!DebugView.Visible)
                    {
                        DebugView.Show(this);
                    }

                    // Bring the form the the foreground if it is minimizedf
                    if (DebugView.WindowState == FormWindowState.Minimized)
                    {
                        DebugView.WindowState = FormWindowState.Normal;
                    }
                }
            });
        }

        /// <summary>
        /// Delete the current event log (when connection is disconnected)
        /// </summary>
        private void DisposeEventLog()
        {
            BeginInvoke(() =>
            {
                if (DebugView != null)
                {
                    if (!DebugView.IsDisposed)
                    {
                        DebugView.Dispose();
                    }

                    DebugView = null;
                }

                tsbEventView.Enabled = false;
            });
        }

        private void tsbConnect_Click(object sender, EventArgs e)
        {
            Connect();
        }

        private void tspEventView_Click(object sender, EventArgs e)
        {
            ShowEventLog();
        }

        private void tsbDisconnect_Click(object sender, EventArgs e)
        {
            // Prevent disconnect from bein called multiple times
            tsbDisconnect.Enabled = false;
            Disconnect();
        }

        /// <summary>
        /// Initializes the logic for the settings "save" button.
        /// </summary>
        private void InitializeSettingsHandling()
        {
            // Subscribe to "unsaved changes" event
            Settings.App.OnHasUnsavedChanges += App_OnHasUnsavedChanges;
            // Set the button enabled/disabled based on current value
            tsbSaveSettings.Enabled = Settings.App.HasUnsavedChanges;
        }

        private void App_OnHasUnsavedChanges(Settings.SettingsRoot sender, Settings.SettingsRoot.HasUnsavedChangesEventArgs args)
        {
            // This might be called from another thread...
            BeginInvoke(() =>
            {
                // Enable the save button when there's unsaved changes
                tsbSaveSettings.Enabled = args.HasUnsavedChanges;
            });
        }

        private void tsbSaveSettings_Click(object sender, EventArgs e)
        {
            // Save current settings. This should automatically diable the button
            // via the OnHasUnsavedChanges event
            Settings.App.Save();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Check if we have unsaved changes
            if (Settings.App.HasUnsavedChanges)
            {
                // Show message box
                var result = MessageBox.Show("You have unsaved changes. Do you want to save them now?", "Unsaved Changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                switch(result)
                {
                    // Save settings
                    case DialogResult.Yes:
                        {
                            Settings.App.Save();
                            break;
                        }
                    // Cancel and stop app from closing
                    case DialogResult.Cancel:
                        {
                            e.Cancel = true;
                            break;
                        }
                    // Close the app
                    default:
                        {
                            break;
                        }
                }
            }
        }
    }
}
