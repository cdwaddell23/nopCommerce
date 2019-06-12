using Autofac;
using Microsoft.AspNetCore.Hosting;
using Nop.Core.Configuration;
using Nop.Core.Data;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Core.Plugins;
using Nop.Data;
using Nop.Plugin.Campgrounds.Data.Migrations;
using System;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;

namespace Nop.Plugin.Campgrounds.Data.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        private const string ContextName = "nop_object_context_campgroundplugin_efmigrations";

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
        {
            //Load custom data settings
            var dataSettingsManager = new DataSettingsManager();
            var dataSettings = dataSettingsManager.LoadSettings();

            //Register custom object context
            builder.Register<IDbContext>(c => RegisterIDbContext(c, dataSettings)).Named<IDbContext>(ContextName).InstancePerRequest();
            builder.Register(c => RegisterIDbContext(c, dataSettings)).InstancePerRequest();

            var pluginFinderTypes = typeFinder.FindClassesOfType<IPluginFinder>();

            var isInstalled = false;

            foreach (var pluginFinderType in pluginFinderTypes)
            {
                var pluginFinder = Activator.CreateInstance(pluginFinderType) as IPluginFinder;
                var pluginDescriptor = pluginFinder.GetPluginDescriptorBySystemName("Widgets.Campgrounds");

                if (pluginDescriptor != null && pluginDescriptor.Installed)
                {
                    isInstalled = true;
                    break;
                }
            }

            if (isInstalled && config.UseEFCodeFirstMigrations)
            {
                //db migrations, lets update if needed
                var dbConfig = new Configuration();
                dbConfig.TargetDatabase = new System.Data.Entity.Infrastructure.DbConnectionInfo(dataSettings.DataConnectionString, "CampTaleDB");
                var migrator = new DbMigrator(new Configuration());

                var scriptor = new MigratorScriptingDecorator(migrator);
                string script = scriptor.ScriptUpdate(sourceMigration: null, targetMigration: null).ToString();

                migrator.Update();
            }

        }

        /// <summary>
        /// Registers the I db context.
        /// </summary>
        /// <param name="componentContext">The component context.</param>
        /// <param name="dataSettings">The data settings.</param>
        /// <returns></returns>
        private CampgroundsObjectContext RegisterIDbContext(IComponentContext componentContext, DataSettings dataSettings)
        {
            string dataConnectionStrings;

            if (dataSettings != null && dataSettings.IsValid())
                dataConnectionStrings = dataSettings.DataConnectionString;
            else
                dataConnectionStrings = componentContext.Resolve<DataSettings>().DataConnectionString;

            return new CampgroundsObjectContext(dataConnectionStrings);

        }

        public int Order { get; }
    }
}
