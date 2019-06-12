using System;
using Nop.Core;
using System.Collections.Generic;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Stores;

namespace Nop.Plugin.Campgrounds.Data.Domain
{
    /// <summary>
    /// Represents a Campground review
    /// </summary>
    public partial class CampgroundReview : BaseEntity
    {
        private ICollection<CampgroundReviewHelpfulness> _campgroundReviewHelpfulnessEntries;
        private ICollection<CampgroundPicture> _campgroundPictures;

        /// <summary>
        /// Gets or sets the customer identifier
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the Campground identifier
        /// </summary>
        public int CampgroundId { get; set; }

        /// <summary>
        /// Gets or sets the store identifier
        /// </summary>
        public int StoreId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the content is approved
        /// </summary>
        public bool IsApproved { get; set; }

        /// <summary>
        /// Gets or sets the title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the review text
        /// </summary>
        public string ReviewText { get; set; }

        /// <summary>
        /// Gets or sets the reply text
        /// </summary>
        public string ReplyText { get; set; }

        /// <summary>
        /// Review rating
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// Review helpful votes total
        /// </summary>
        public int HelpfulYesTotal { get; set; }

        /// <summary>
        /// Review not helpful votes total
        /// </summary>
        public int HelpfulNoTotal { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity has been deleted
        /// </summary>
        public bool Deleted { get; set; }
        /// <summary>
        /// Gets or sets the date and time of instance creation
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the customer
        /// </summary>
        public virtual Customer Customer { get; set; }

        /// <summary>
        /// Gets the Campground
        /// </summary>
        public virtual Campground Campground { get; set; }

        /// <summary>
        /// Gets or sets the store
        /// </summary>
        public virtual Store Store { get; set; }

        /// <summary>
        /// Gets the Campground Pictures
        /// </summary>
        public virtual ICollection<CampgroundPicture> CampgroundPictures
        {
            get { return _campgroundPictures ?? (_campgroundPictures = new List<CampgroundPicture>()); }
            protected set { _campgroundPictures = value; }
        }
        /// <summary>
        /// Gets the entries of Campground review helpfulness
        /// </summary>
        public virtual ICollection<CampgroundReviewHelpfulness> CampgroundReviewHelpfulnessEntries
        {
            get { return _campgroundReviewHelpfulnessEntries ?? (_campgroundReviewHelpfulnessEntries = new List<CampgroundReviewHelpfulness>()); }
            protected set { _campgroundReviewHelpfulnessEntries = value; }
        }
    }
}
