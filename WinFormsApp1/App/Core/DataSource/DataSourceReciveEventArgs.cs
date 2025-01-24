using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFooCockpit.App.Core.DataSource
{
    internal class DataSourceReciveEventArgs<T> : EventArgs
        where T : DataSourceData
    {
        public required T Data;
    }
}
