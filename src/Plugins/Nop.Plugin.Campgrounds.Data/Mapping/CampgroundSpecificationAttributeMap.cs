using Nop.Data.Mapping;
using Nop.Plugin.Campgrounds.Data.Domain;

namespace Nop.Plugin.Campgrounds.Data.Mapping
{
    public partial class CampgroundSpecificationAttributeMap : NopEntityTypeConfiguration<CampgroundSpecificationAttribute>
    {
        public CampgroundSpecificationAttributeMap()
        {
            this.ToTable("Campground_SpecificationAttribute_Mapping");
            this.HasKey(psa => psa.Id);

            this.Property(psa => psa.CustomValue).HasMaxLength(4000);

            this.Ignore(psa => psa.AttributeType);

            this.HasRequired(psa => psa.SpecificationAttributeOption)
                .WithMany()
                .HasForeignKey(psa => psa.SpecificationAttributeOptionId);


            this.HasRequired(psa => psa.Campground)
                .WithMany(p => p.CampgroundSpecificationAttributes)
                .HasForeignKey(psa => psa.CampgroundId);
        }
    }
}