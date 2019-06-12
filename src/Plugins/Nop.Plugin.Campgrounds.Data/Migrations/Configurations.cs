using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Reflection;
using Nop.Core.Data;
using Nop.Plugin.Campgrounds.Data;

namespace Nop.Plugin.Campgrounds.Data.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<CampgroundsObjectContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;

            AutomaticMigrationDataLossAllowed = true;

            MigrationsAssembly = Assembly.GetExecutingAssembly();
            MigrationsNamespace = "Nop.Plugin.Campgrounds.Data.Migrations";

            //specify database so that it doesn't throw error during migration. Otherwise, for some reasons it defaults to sqlce and gives error 
            var dataSettingsManager = new DataSettingsManager();
            var dataSettings = dataSettingsManager.LoadSettings();
            TargetDatabase = new DbConnectionInfo(dataSettings.DataConnectionString, "System.Data.SqlClient");

        }
    }
}