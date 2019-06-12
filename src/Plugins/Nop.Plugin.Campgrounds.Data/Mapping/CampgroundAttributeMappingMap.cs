using Nop.Data.Mapping;
using Nop.Plugin.Campgrounds.Data.Domain;

namespace Nop.Plugin.Campgrounds.Data.Mapping
{
    public partial class CampgroundAttributeMappingMap : NopEntityTypeConfiguration<CampgroundAttributeMapping>
    {
        public CampgroundAttributeMappingMap()
        {
            this.ToTable("Campground_CampgroundAttributeType_Mapping");
            this.HasKey(cam => cam.Id);

            this.Ignore(cam => cam.AttributeControlType);

            this.HasRequired(cam => cam.Campground)
                .WithMany(c => c.CampgroundAttributeMappings)
                .HasForeignKey(cam => cam.CampgroundId);

            this.HasRequired(cam => cam.CampgroundAttributeType)
                .WithMany()
                .HasForeignKey(cam => cam.CampgroundAttributeTypeId);
        }
    }
}