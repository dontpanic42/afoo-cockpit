using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.DataSource;

namespace AFooCockpit.App.Gui.DataSourceConfigForms
{
    internal interface IDataSourceConfigView<C>
        where C : DataSourceConfig
    {
        public C DataSourceConfig { get; set; }
    }
}
