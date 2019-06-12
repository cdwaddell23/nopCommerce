using Nop.Data.Mapping;
using Nop.Plugin.Campgrounds.Data.Domain;

namespace Nop.Plugin.Campgrounds.Data.Mapping
{
    public partial class CampgroundMap : NopEntityTypeConfiguration<Campground>
    {
        public CampgroundMap()
        {
            this.ToTable("Campground");
            this.HasKey(c => c.Id);
            this.Property(c => c.Name).IsRequired().HasMaxLength(400);
            this.Property(c => c.MetaKeywords).HasMaxLength(400);
            this.Property(c => c.MetaTitle).HasMaxLength(400);
            this.Property(c => c.FullUrl).HasMaxLength(255);
            this.Property(c => c.Distance).HasPrecision(6, 2);
            this.Property(c => c.Price).HasPrecision(18, 4);
            this.Property(c => c.OldPrice).HasPrecision(18, 4);
            this.Property(c => c.BasepriceAmount).HasPrecision(18, 4);
            this.Property(c => c.BasepriceBaseAmount).HasPrecision(18, 4);

            this.Ignore(c => c.CampgroundClass);
            this.Ignore(c => c.AppliedDiscounts);
            //this.Ignore(c => c.Distance);

            this.HasMany(ct => ct.CampgroundTags)
                .WithMany(c => c.Campgrounds)
                .Map(m => m.ToTable("Campground_CampgroundTag_Mapping"));

            this.HasMany(ct => ct.CampgroundType)
                .WithMany(c => c.Campgrounds)
                .Map(m => m.ToTable("Campground_CampgroundType_Mapping"));

            this.HasMany(ch => ch.CampgroundHost)
                .WithMany(c => c.Campgrounds)
                .Map(m => m.ToTable("Campground_CampgroundHost_Mapping"));

            this.HasMany(c => c.Addresses)
                .WithMany()
                .Map(m => m.ToTable("Campground_CampgroundAddresses"));

        }
    }
}