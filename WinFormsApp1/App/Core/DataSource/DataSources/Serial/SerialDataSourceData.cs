using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.DataSource;

namespace AFooCockpit.App.Core.DataSource.DataSources.Serial
{
    internal class SerialDataSourceData : DataSourceData
    {
        /// <summary>
        /// Simple data definition - a serial event (for us) is always a full line written
        /// </summary>
        public required string Line;
    }
}
