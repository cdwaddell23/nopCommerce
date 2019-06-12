using Nop.Data.Mapping;
using Nop.Plugin.Campgrounds.Data.Domain;

namespace Nop.Plugin.Campgrounds.Data.Mapping
{
    public partial class CampgroundAmenityMap : NopEntityTypeConfiguration<CampgroundAmenity>
    {
        public CampgroundAmenityMap()
        {
            this.ToTable("CampgroundAmenity");
            this.HasKey(c => c.Id);
            this.Property(c => c.Code).IsRequired().HasMaxLength(10);
            this.Property(c => c.Description).IsRequired().HasMaxLength(500);
        }
    }
}