using Nop.Data.Mapping;
using Nop.Plugin.Campgrounds.Data.Domain;

namespace Nop.Plugin.Campgrounds.Data.Mapping
{
    public partial class CampgroundTypeMap : NopEntityTypeConfiguration<CampgroundType>
    {
        public CampgroundTypeMap()
        {
            this.ToTable("CampgroundType");
            this.HasKey(p => p.Id);
            this.Property(p => p.Code).IsRequired().HasMaxLength(10);
            this.Property(p => p.Description).IsRequired().HasMaxLength(500);
        }
    }
}