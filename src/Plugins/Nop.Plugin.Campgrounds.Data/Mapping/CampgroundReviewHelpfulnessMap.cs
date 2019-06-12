using Nop.Data.Mapping;
using Nop.Plugin.Campgrounds.Data.Domain;

namespace Nop.Plugin.Campgrounds.Data.Mapping
{
    public partial class CampgroundReviewHelpfulnessMap : NopEntityTypeConfiguration<CampgroundReviewHelpfulness>
    {
        public CampgroundReviewHelpfulnessMap()
        {
            this.ToTable("CampgroundReviewHelpfulness");
            this.HasKey(cr => cr.Id);

            this.HasRequired(crh => crh.CampgroundReview)
                .WithMany(cr => cr.CampgroundReviewHelpfulnessEntries)
                .HasForeignKey(crh => crh.CampgroundReviewId).WillCascadeOnDelete(true);
        }
    }
}