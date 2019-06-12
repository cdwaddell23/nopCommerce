using Nop.Data.Mapping;
using Nop.Plugin.Campgrounds.Data.Domain;

namespace Nop.Plugin.Campgrounds.Data.Mapping
{
    public partial class SeasonalPriceMap : NopEntityTypeConfiguration<SeasonalPrice>
    {
        public SeasonalPriceMap()
        {
            this.ToTable("SeasonalPrice");
            this.HasKey(tp => tp.Id);
            this.Property(tp => tp.Price).HasPrecision(18, 4);

            this.HasRequired(tp => tp.Campground)
                .WithMany(p => p.SeasonalPrices)
                .HasForeignKey(tp => tp.CampgroundId);

            this.HasOptional(tp => tp.CustomerRole)
                .WithMany()
                .HasForeignKey(tp => tp.CustomerRoleId)
                .WillCascadeOnDelete(true);
        }
    }
}