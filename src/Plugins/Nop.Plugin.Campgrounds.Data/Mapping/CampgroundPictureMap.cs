using Nop.Data.Mapping;
using Nop.Plugin.Campgrounds.Data.Domain;

namespace Nop.Plugin.Campgrounds.Data.Mapping
{
    public partial class CampgroundPictureMap : NopEntityTypeConfiguration<CampgroundPicture>
    {
        public CampgroundPictureMap()
        {
            this.ToTable("Campground_Picture_Mapping");
            this.HasKey(cp => cp.Id);
            
            this.HasRequired(cp => cp.Picture)
                .WithMany()
                .HasForeignKey(cp => cp.PictureId);


            this.HasRequired(cp => cp.Campground)
                .WithMany(c => c.CampgroundPictures)
                .HasForeignKey(cp => cp.CampgroundId);

            this.Ignore(cp => cp.Review);
        }
    }
}