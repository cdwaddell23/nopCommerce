
using Nop.Core;
using Nop.Core.Domain.Media;

namespace Nop.Plugin.Campgrounds.Data.Domain
{
    /// <summary>
    /// Represents a Campground picture mapping
    /// </summary>
    public partial class CampgroundPicture : BaseEntity
    {
        /// <summary>
        /// Gets or sets the Campground identifier
        /// </summary>
        public int CampgroundId { get; set; }

        /// <summary>
        /// Gets or sets the picture identifier
        /// </summary>
        public int PictureId { get; set; }

        /// <summary>
        /// Gets or sets the review identifier
        /// </summary>
        public int ReviewId { get; set; }

        /// <summary>
        /// Gets or sets flag for default image
        /// </summary>
        public bool IsDefault { get; set; }
        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }
        
        /// <summary>
        /// Gets the picture
        /// </summary>
        public virtual Picture Picture { get; set; }

        /// <summary>
        /// Gets the Campground
        /// </summary>
        public virtual Campground Campground { get; set; }

        /// <summary>
        /// Gets the Campground
        /// </summary>
        public virtual CampgroundReview Review { get; set; }

    }

}
