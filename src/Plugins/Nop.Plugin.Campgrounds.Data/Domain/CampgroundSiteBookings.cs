using System;
using Nop.Core;
using System.Collections.Generic;
using Nop.Core.Domain.Customers;

namespace Nop.Plugin.Campgrounds.Data.Domain
{
    public partial class CampgroundSiteBookings : BaseEntity
    {
        private ICollection<CampgroundSite> _campgroundSites;


        /// <summary>
        /// Gets or sets the site identifier
        /// </summary>
        public int SiteId { get; set; }

        /// <summary>
        /// Gets or sets the available start date and time
        /// </summary>
        public DateTime? BookedStartDateTimeUtc { get; set; }
        /// <summary>
        /// Gets or sets the available end date and time
        /// </summary>
        public DateTime? BookedEndDateTimeUtc { get; set; }

        public virtual Customer Customer { get; set; }

        /// <summary>
        /// Gets or sets the collection of CampgroundCategory
        /// </summary>
        public virtual ICollection<CampgroundSite> CampgroundSites
        {
            get { return _campgroundSites ?? (_campgroundSites = new List<CampgroundSite>()); }
            protected set { _campgroundSites = value; }
        }
    }
}
