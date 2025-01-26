using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFooCockpit.App.Core.DataSource.DataSources.Sim.VariableSource
{
    internal class FlightSimVariableDataSourceData : DataSourceData
    {
        public required string VariableName;
        public required double VariableValue;
    }
}
