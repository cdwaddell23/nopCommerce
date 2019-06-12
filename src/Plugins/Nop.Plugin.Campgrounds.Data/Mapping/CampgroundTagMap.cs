using Nop.Data.Mapping;
using Nop.Plugin.Campgrounds.Data.Domain;

namespace Nop.Plugin.Campgrounds.Data.Mapping
{
    public partial class CampgroundTagMap : NopEntityTypeConfiguration<CampgroundTag>
    {
        public CampgroundTagMap()
        {
            this.ToTable("CampgroundTag");
            this.HasKey(pt => pt.Id);
            this.Property(pt => pt.Name).IsRequired().HasMaxLength(400);
        }
    }
}