using Nop.Data.Mapping;
using Nop.Plugin.Campgrounds.Data.Domain;

namespace Nop.Plugin.Campgrounds.Data.Mapping
{
    public partial class CampgroundReviewMap : NopEntityTypeConfiguration<CampgroundReview>
    {
        public CampgroundReviewMap()
        {
            this.ToTable("CampgroundReview");
            this.HasKey(cr => cr.Id);

            this.HasRequired(cr => cr.Campground)
                .WithMany(c => c.CampgroundReviews)
                .HasForeignKey(cr => cr.CampgroundId);

            this.HasRequired(cr => cr.Customer)
                .WithMany()
                .HasForeignKey(cr => cr.CustomerId);

            this.HasRequired(cr => cr.Store)
                .WithMany()
                .HasForeignKey(cr => cr.StoreId);
        }
    }
}