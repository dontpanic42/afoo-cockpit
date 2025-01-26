using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.FlightData;

namespace AFooCockpit.App.Core.Aircraft
{
    internal class AircraftVariableMapEventArgs : EventArgs
    {
        public required string VariableName;
        public required FlightDataEvent FlightDataEvent;
    }
}
