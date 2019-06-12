using Nop.Data.Mapping;
using Nop.Plugin.Campgrounds.Data.Domain;

namespace Nop.Plugin.Campgrounds.Data.Mapping
{
    public partial class PredefinedCampgroundAttributeValueMap : NopEntityTypeConfiguration<PredefinedCampgroundAttributeValue>
    {
        public PredefinedCampgroundAttributeValueMap()
        {
            this.ToTable("PredefinedCampgroundAttributeValue");
            this.HasKey(pav => pav.Id);
            this.Property(pav => pav.Name).IsRequired().HasMaxLength(400);

            this.Property(pav => pav.PriceAdjustment).HasPrecision(18, 4);
            this.Property(pav => pav.Cost).HasPrecision(18, 4);

            this.HasRequired(pav => pav.CampgroundAttributeType)
                .WithMany()
                .HasForeignKey(pav => pav.CampgroundAttributeTypeId);
        }
    }
}