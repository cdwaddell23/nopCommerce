using Nop.Data.Mapping;
using Nop.Plugin.Campgrounds.Data.Domain;

namespace Nop.Plugin.Campgrounds.Data.Mapping
{
    public partial class CampgroundAddressMap : NopEntityTypeConfiguration<CampgroundAddress>
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public CampgroundAddressMap()
        {
            this.ToTable("CampgroundAddress");
            this.HasKey(a => a.Id);

            this.Property(a => a.Latitude).HasPrecision(18, 10);
            this.Property(a => a.Longitude).HasPrecision(18, 10);

            //this.Ignore(c => c.Country);
            //this.Ignore(c => c.StateProvince);

            this.HasOptional(a => a.Country)
                .WithMany()
                .HasForeignKey(a => a.CountryId).WillCascadeOnDelete(false);

            this.HasOptional(a => a.StateProvince)
                .WithMany()
                .HasForeignKey(a => a.StateProvinceId).WillCascadeOnDelete(false);
        }
    }
}
