using Nop.Data.Mapping;
using Nop.Plugin.Campgrounds.Data.Domain;

namespace Nop.Plugin.Campgrounds.Data.Mapping
{
    public partial class CampgroundAttributeTypeCombinationMap : NopEntityTypeConfiguration<CampgroundAttributeCombination>
    {
        public CampgroundAttributeTypeCombinationMap()
        {
            this.ToTable("CampgroundAttributeTypeCombination");
            this.HasKey(pac => pac.Id);

            this.Property(pac => pac.OverriddenPrice).HasPrecision(18, 4);

            this.HasRequired(pac => pac.Campground)
                .WithMany(p => p.CampgroundAttributeTypeCombinations)
                .HasForeignKey(pac => pac.CampgroundId);
        }
    }
}