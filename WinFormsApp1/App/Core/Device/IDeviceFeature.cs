using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.FlightData;

namespace AFooCockpit.App.Core.Device
{
    internal interface IDeviceFeature
    {
        /// <summary>
        /// Name of the feature, for logging purposes
        /// </summary>
        public string Name { get; }
    }
}
