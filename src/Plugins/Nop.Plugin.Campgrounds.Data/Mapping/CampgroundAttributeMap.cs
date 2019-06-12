using Nop.Data.Mapping;
using Nop.Plugin.Campgrounds.Data.Domain;

namespace Nop.Plugin.Campgrounds.Data.Mapping
{
    public partial class CampgroundAttributeTypeMap : NopEntityTypeConfiguration<CampgroundAttributeType>
    {
        public CampgroundAttributeTypeMap()
        {
            this.ToTable("CampgroundAttributeType");
            this.HasKey(ca => ca.Id);
            this.Property(ca => ca.Name).IsRequired();
        }
    }
}