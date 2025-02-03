using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AFooCockpit.App.Core.FlightData;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AFooCockpit.App.Gui
{
    public partial class DebugView : Form
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private FlightDataEventBus? FlightDataEventBus;

        public DebugView()
        {
            InitializeComponent();

            InitializeEventSelector();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            SendFlightDataEvent();
        }

        /// <summary>
        /// Initialize the select box with all possible event names
        /// </summary>
        private void InitializeEventSelector()
        {
            string[] items = Enum.GetNames(typeof(FlightDataEvent));
            cobEventName.Items.AddRange(items);
            cobEventName.SelectedIndex = 0;
        }

        /// <summary>
        /// Connect an event bus to this view
        /// </summary>
        /// <param name="flightDataEventBus"></param>
        public void ConnectBus(FlightDataEventBus flightDataEventBus)
        {
            // Remove old event hanlder
            if (FlightDataEventBus != null)
            {
                FlightDataEventBus!.OnLogMessageReceived -= FlightDataEventBus_OnLogMessageReceived;
            }

            lvLog.Items.Clear();

            // Add new event handler
            FlightDataEventBus = flightDataEventBus;
            FlightDataEventBus.OnLogMessageReceived += FlightDataEventBus_OnLogMessageReceived;
        }

        /// <summary>
        /// Called when a new event message is received
        /// </summary>
        /// <param name="eventBus"></param>
        /// <param name="logMessage"></param>
        private void FlightDataEventBus_OnLogMessageReceived(FlightDataEventBus eventBus, string logMessage)
        {
            BeginInvoke(() =>
            {
                // Format log message with time
                var message = $"{DateTime.Now.ToLongTimeString()}: ${logMessage}";
                // Add new log message
                lvLog.Items.Add(message);
                // Scroll down
                lvLog.SelectedIndex = lvLog.Items.Count - 1;
                lvLog.SelectedIndex = -1;
            });
        }

        /// <summary>
        /// Sends a flight data event to the event bus given the infomration currently in the form
        /// </summary>
        private void SendFlightDataEvent()
        {
            var eventName = cobEventName.SelectedItem as string;
            if (!string.IsNullOrWhiteSpace(eventName) && FlightDataEventBus != null)
            {
                var value = Decimal.ToDouble(txtEventData.Value);
                var fevent = (FlightDataEvent)Enum.Parse(typeof(FlightDataEvent), eventName);

                logger.Debug("Sending GUI event");

                FlightDataEventBus.TriggerDataEvent(fevent, new FlightDataEventArgs
                {
                    SenderName = "Debug GUI",
                    Event = fevent,
                    Data = value
                });
            }
        }

        private void DebugView_FormClosing(object sender, FormClosingEventArgs e)
        {
            // This form is controlled by the main form - we don't want it really closed
            // since then, we'd lose all the event log. Instead we're just hiding it
            if (e.CloseReason == CloseReason.UserClosing)
            {
                // Cancel the close operation
                e.Cancel = true;

                // Hide the form instead
                this.Hide();
            }
        }
    }
}
