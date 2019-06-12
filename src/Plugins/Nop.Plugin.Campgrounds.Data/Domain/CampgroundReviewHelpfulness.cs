
using Nop.Core;

namespace Nop.Plugin.Campgrounds.Data.Domain
{
    /// <summary>
    /// Represents a Campground review helpfulness
    /// </summary>
    public partial class CampgroundReviewHelpfulness : BaseEntity
    {
        /// <summary>
        /// Gets or sets the Campground review identifier
        /// </summary>
        public int CampgroundReviewId { get; set; }

        /// <summary>
        /// A value indicating whether a review a helpful
        /// </summary>
        public bool WasHelpful { get; set; }

        /// <summary>
        /// Gets or sets the customer identifier
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets the Campground
        /// </summary>
        public virtual CampgroundReview CampgroundReview { get; set; }
    }
}
