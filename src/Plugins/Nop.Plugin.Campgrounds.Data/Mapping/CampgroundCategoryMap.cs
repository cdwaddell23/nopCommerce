using Nop.Data.Mapping;
using Nop.Plugin.Campgrounds.Data.Domain;

namespace Nop.Plugin.Campgrounds.Data.Mapping
{
    public partial class CampgroundCategoryMap : NopEntityTypeConfiguration<CampgroundCategory>
    {
        public CampgroundCategoryMap()
        {
            this.ToTable("Campground_Category_Mapping");
            this.HasKey(cc => cc.Id);
            
            this.HasRequired(cc => cc.Category)
                .WithMany()
                .HasForeignKey(cc => cc.CategoryId);


            this.HasRequired(cc => cc.Campground)
                .WithMany(c => c.CampgroundCategories)
                .HasForeignKey(cc => cc.CampgroundId);
        }
    }
}