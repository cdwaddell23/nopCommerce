using Nop.Data.Mapping;
using Nop.Plugin.Campgrounds.Data.Domain;

namespace Nop.Plugin.Campgrounds.Data.Mapping
{
    public partial class CampgroundMigrationsMap : NopEntityTypeConfiguration<CampgroundMigrations>
    {
        public CampgroundMigrationsMap()
        {
            this.ToTable("CampgroundMigrations");
            this.HasKey(c => c.Id);
        }
    }
}