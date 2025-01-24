using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFooCockpit.App.Core.FlightData
{
    public delegate void FlightDataEventHandler(FlightDataEventBus bus, FlightDataEventArgs eventArgs);

    public delegate void FlightDataLogEventHandler(FlightDataEventBus eventBus, string logMessage);

    public class FlightDataEventBus
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Simple container that wraps an "event" object
        /// </summary>
        public class FlightDataEventContainer
        {
            public required string EventFriendlyName;

            public event FlightDataEventHandler? OnDataReceived;

            public void Invoke(FlightDataEventBus bus, FlightDataEventArgs eventArgs)
            {
                OnDataReceived?.Invoke(bus, eventArgs);
            }
        }

        /// <summary>
        /// Contains all event that we have subscriptions for
        /// </summary>
        private Dictionary<FlightDataEvent, FlightDataEventContainer> EventContainers = new Dictionary<FlightDataEvent, FlightDataEventContainer>();

        /// <summary>
        /// Event that gets triggered when log messages are written
        /// </summary>
        public event FlightDataLogEventHandler? OnLogMessageReceived;

        /// <summary>
        /// Returns the flight data event wrapper for the given flight event so we can subscribe
        /// </summary>
        /// <param name="flightDataEvent"></param>
        /// <returns></returns>
        public FlightDataEventContainer FlightEvent(FlightDataEvent flightDataEvent)
        {
            if (!EventContainers.ContainsKey(flightDataEvent))
            {
                var eventName = Enum.GetName<FlightDataEvent>(flightDataEvent)!;
                EventContainers.Add(flightDataEvent, new FlightDataEventContainer 
                { 
                    EventFriendlyName = eventName 
                });
            }

            return EventContainers[flightDataEvent];
        }

        /// <summary>
        /// Triggers the given flight data event
        /// </summary>
        /// <param name="flightDataEvent"></param>
        /// <param name="eventArgs"></param>
        public void TriggerDataEvent(FlightDataEvent flightDataEvent, FlightDataEventArgs eventArgs)
        {
            if (!EventContainers.ContainsKey(flightDataEvent))
            {
                var friendlyName = Enum.GetName<FlightDataEvent>(flightDataEvent);
                EventContainers.Add(flightDataEvent, new FlightDataEventContainer 
                { 
                    EventFriendlyName = friendlyName! 
                });
            }

            var container = EventContainers[flightDataEvent];
            container.Invoke(this, eventArgs);

            WriteEventLog($"Event: {container.EventFriendlyName}, Value: {eventArgs.Data}, Sender: {eventArgs.SenderName}");
        }

        /// <summary>
        /// Write a message to the log
        /// </summary>
        /// <param name="log"></param>
        private void WriteEventLog(string log)
        {
            logger.Debug(log);
            OnLogMessageReceived?.Invoke(this, log);
        }
    }
}
