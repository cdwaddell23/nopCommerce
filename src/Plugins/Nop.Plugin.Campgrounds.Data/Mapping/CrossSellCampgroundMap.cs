using Nop.Data.Mapping;
using Nop.Plugin.Campgrounds.Data.Domain;

namespace Nop.Plugin.Campgrounds.Data.Mapping
{
    public partial class CrossSellCampgroundMap : NopEntityTypeConfiguration<CrossSellCampground>
    {
        public CrossSellCampgroundMap()
        {
            this.ToTable("CrossSellCampground");
            this.HasKey(csc => csc.Id);
        }
    }
}