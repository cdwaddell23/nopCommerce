namespace Nop.Plugin.Campgrounds.Data.Domain
{
    /// <summary>
    /// Campground review approved event
    /// </summary>
    public class CampgroundReviewApprovedEvent
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="campgroundReview">Campground review</param>
        public CampgroundReviewApprovedEvent(CampgroundReview campgroundReview)
        {
            this.CampgroundReview = campgroundReview;
        }

        /// <summary>
        /// Campground review
        /// </summary>
        public CampgroundReview CampgroundReview { get; }
    }
}