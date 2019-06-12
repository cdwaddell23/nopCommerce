using Nop.Data.Mapping;
using Nop.Plugin.Campgrounds.Data.Domain;

namespace Nop.Plugin.Campgrounds.Data.Mapping
{
    public partial class CampgroundAttributeValueMap : NopEntityTypeConfiguration<CampgroundAttributeValue>
    {
        public CampgroundAttributeValueMap()
        {
            this.ToTable("CampgroundAttributeValue");
            this.HasKey(cav => cav.Id);
            this.Property(cav => cav.Name).IsRequired().HasMaxLength(400);
            this.Property(cav => cav.ColorSquaresRgb).HasMaxLength(100);

            this.Property(cav => cav.PriceAdjustment).HasPrecision(18, 4);
            this.Property(cav => cav.Cost).HasPrecision(18, 4);

            this.Ignore(cav => cav.CampgroundAttributeValueType);

            this.HasRequired(cav => cav.CampgroundAttributeMapping)
                .WithMany(cam => cam.CampgroundAttributeValues)
                .HasForeignKey(cav => cav.CampgroundAttributeMappingId);
        }
    }
}