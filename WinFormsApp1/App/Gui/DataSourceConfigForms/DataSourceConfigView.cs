using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.DataSource;

namespace AFooCockpit.App.Gui.DataSourceConfigForms
{
    public class DataSourceConfigView : Form
    {

        public C GetDataSourceConfig<C>()
        {
            var defaultValue = default(C);
            if(defaultValue == null)
            {
                throw new Exception("Cannot create data source config - trying to use default config, but default config is null");
            }

            return defaultValue!;
        }
    }
}
