using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFooCockpit.App.Core.DataSource.DataSources.Arinc429TranscieverDataSource;
using AFooCockpit.App.Gui.DataSourceConfigForms;

namespace AFooCockpit.App.Core.DataSource
{
    public class DataSourceConfigCreationCanceledException : Exception
    {
        public DataSourceConfigCreationCanceledException(string message) : base(message) { }
    }

    public class DataSourceConfigCreationFailedException : Exception
    {
        public DataSourceConfigCreationFailedException(string message) : base(message) { }
    }

    internal class DataSourceConfigFactory
    {

        /// <summary>
        /// Contains the mapping between a datasource config type and a (matching!) datasource configuration view
        /// </summary>
        public static Dictionary<Type, Type> DataSourceRegistry = new Dictionary<Type, Type> {
            { typeof(Arinc429TranscieverDataSourceConfig), typeof(Arinc429TranscieverDataSourceConfigView) }
        };

        /// <summary>
        /// Creates a data source config of a given type by prompting the user to configure it.
        /// 
        /// Note that this method relies on the view to set the dialog result correctly. If not dialog result
        /// is set, it will be handled as "user canceled"
        /// </summary>
        /// <typeparam name="C">DataSourceConfig type</typeparam>
        /// <returns>A full data source config</returns>
        /// <exception cref="DataSourceConfigCreationFailedException"></exception>
        public static async Task<C> CreateDataSourceConfig<C>(C? baseConfig = default(C)) where C : DataSourceConfig, new()
        {
            var dataSourceConfigType = typeof(C);
            if (!DataSourceRegistry.ContainsKey(dataSourceConfigType))
            {
                throw new DataSourceConfigCreationFailedException($"Could not create config - config type {dataSourceConfigType.Name} is unknwon.");
            }

            var tcs = new TaskCompletionSource<C>();

            var formType = DataSourceRegistry[dataSourceConfigType];

            if (!typeof(DataSourceConfigView).IsAssignableFrom(formType))
            {
                throw new DataSourceConfigCreationFailedException($"Could not create config dialog - dialog type {formType.Name} is not a DataSourceConfigView.");
            }

            if(!typeof(IDataSourceConfigView<C>).IsAssignableFrom(formType))
            {
                throw new DataSourceConfigCreationFailedException($"Could not create config dialog - dialog type {formType.Name} is not a IDataSourceConfigView.");
            }

            var form = (Form?)Activator.CreateInstance(formType) ?? throw new DataSourceConfigCreationFailedException("Could not cast form to Form type?");
            var configSource = (IDataSourceConfigView<C>)form;

            form.FormClosed += (sender, args) =>
            {
                if (form.DialogResult == DialogResult.OK)
                {
                    var config = configSource.DataSourceConfig;

                    tcs.SetResult(config);
                } else {
                    tcs.SetException(new DataSourceConfigCreationCanceledException("User canceled"));    
                }
            };

            if (baseConfig != null)
            {
                // If we already have a source and want to edit it, we set the form to baseConfig
                configSource.DataSourceConfig = baseConfig;
            } 
            else 
            {
                // If this is a completely new config, we create a new object
                configSource.DataSourceConfig = new C();
            }

            form.ShowDialog();

            return await tcs.Task;
        }
    }
}
