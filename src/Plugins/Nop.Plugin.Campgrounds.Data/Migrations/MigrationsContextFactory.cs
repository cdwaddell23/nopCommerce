using System.Data.Entity.Infrastructure;
using Nop.Core.Data;
using Nop.Plugin.Campgrounds.Data;


namespace Nop.Plugin.Campgrounds.Data.Migrations
{
        public class MigrationsContextFactory : IDbContextFactory<CampgroundsObjectContext>
        {
            public CampgroundsObjectContext Create()
            {
                var dataSettingsManager = new DataSettingsManager();
                var dataSettings = dataSettingsManager.LoadSettings();
                return new CampgroundsObjectContext(dataSettings.DataConnectionString);
            }
        }
}
