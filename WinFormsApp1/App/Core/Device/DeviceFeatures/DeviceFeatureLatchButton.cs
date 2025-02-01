using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.DataSource;
using AFooCockpit.App.Core.FlightData;
using NLog;

namespace AFooCockpit.App.Core.Device.DeviceFeatures
{
    internal abstract class DeviceFeatureLatchButton<C, T> : DeviceFeature<C, T>
        where C : DeviceFeatureConfig
        where T : IDataSource
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Latch buttons are stateful, so we need to track the state here
        /// </summary>
        protected bool IsLatched { get; private set; } = false;

        /// <summary>
        /// The lifecycle of this button is follows:
        /// 
        /// 1. Press -> Latch       Latch = true, Release = false
        /// 1. Release -> Ignore    Latch = true, Release = true
        /// 2. Press -> Ignore      Latch = true, Release = true
        /// 2. Release -> Unlatch   Latch = false,Release = false
        /// </summary>
        protected bool IsReleased { get; private set; } = false;

        public DeviceFeatureLatchButton(C deviceFeatureConfig) : base(deviceFeatureConfig)
        {
        }

        /// <summary>
        /// Send a "Press" event to the flight data bus
        /// </summary>
        protected void OnPress()
        {
            // If the button is unlatched, we're lathing it on press
            if (!IsLatched) { 
                SendEvent(Config.FlightDataEvent, new FlightDataEventArgs
                {
                    SenderName = Name,
                    Data = FlightDataEventValue.ButtonLatch,
                    Event = Config.FlightDataEvent
                });

                IsLatched = true;
                IsReleased = false;
            }
        }

        /// <summary>
        /// Send a "Release" event to the flight data bus
        /// </summary>
        protected void OnRelease()
        {
            // If the button is latched, we're unlatch is on release
            if (IsLatched && IsReleased) { 
                SendEvent(Config.FlightDataEvent, new FlightDataEventArgs
                {
                    SenderName = Name,
                    Data = FlightDataEventValue.ButtonUnlatch,
                    Event = Config.FlightDataEvent
                });

                IsLatched = false;
                IsReleased = false;
            } 
            // If IsReleased is not true, this is the first press (latch Press)
            else if (IsLatched)
            {
                IsReleased = true;
            }
        }
    }
}
