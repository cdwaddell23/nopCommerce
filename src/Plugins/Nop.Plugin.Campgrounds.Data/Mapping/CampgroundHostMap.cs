using Nop.Data.Mapping;
using Nop.Plugin.Campgrounds.Data.Domain;

namespace Nop.Plugin.Campgrounds.Data.Mapping
{
    public partial class CampgroundHostMap : NopEntityTypeConfiguration<CampgroundHost>
    {
        public CampgroundHostMap()
        {
            this.ToTable("CampgroundHost");
            this.HasKey(ch => ch.Id);

            this.Property(ch => ch.MetaKeywords).HasMaxLength(400);
            this.Property(ch => ch.MetaTitle).HasMaxLength(400);
            this.Property(ch => ch.PageSizeOptions).HasMaxLength(200);
        }
    }
}