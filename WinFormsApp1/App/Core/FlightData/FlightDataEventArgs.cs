using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFooCockpit.App.Core.FlightData
{
    public class FlightDataEventArgs : EventArgs
    {
        public required string SenderName;
        public required double Data;
        public required FlightDataEvent Event;
    }
}
