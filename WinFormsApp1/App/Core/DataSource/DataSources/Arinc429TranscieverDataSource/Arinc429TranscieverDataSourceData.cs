using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.Utils.Arinc429Utils;
using static AFooCockpit.App.Core.Utils.Arinc429Utils.Arinc429Utils;

namespace AFooCockpit.App.Core.DataSource.DataSources.Arinc429TranscieverDataSource
{
    internal class Arinc429TranscieverDataSourceData : DataSourceData
    {
        public required Arinc429Message Message;
    }
}
