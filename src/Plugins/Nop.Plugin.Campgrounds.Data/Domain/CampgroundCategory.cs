using Nop.Core;
using Nop.Core.Domain.Catalog;

namespace Nop.Plugin.Campgrounds.Data.Domain
{
    /// <summary>
    /// Represents a Campground category mapping
    /// </summary>
    public partial class CampgroundCategory : BaseEntity
    {
        /// <summary>
        /// Gets or sets the Campground identifier
        /// </summary>
        public int CampgroundId { get; set; }

        /// <summary>
        /// Gets or sets the category identifier
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Campground is featured
        /// </summary>
        public bool IsFeaturedCampground { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }
        
        /// <summary>
        /// Gets the category
        /// </summary>
        public virtual Category Category { get; set; }

        /// <summary>
        /// Gets the Campground
        /// </summary>
        public virtual Campground Campground { get; set; }

    }

}
