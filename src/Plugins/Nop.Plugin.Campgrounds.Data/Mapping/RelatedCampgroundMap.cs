using Nop.Data.Mapping;
using Nop.Plugin.Campgrounds.Data.Domain;

namespace Nop.Plugin.Campgrounds.Data.Mapping
{
    public partial class RelatedCampgroundMap : NopEntityTypeConfiguration<RelatedCampground>
    {
        public RelatedCampgroundMap()
        {
            this.ToTable("RelatedCampground");
            this.HasKey(c => c.Id);
        }
    }
}