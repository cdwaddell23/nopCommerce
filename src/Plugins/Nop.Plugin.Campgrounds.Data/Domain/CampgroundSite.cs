
using Nop.Core;
using System.Collections.Generic;

namespace Nop.Plugin.Campgrounds.Data.Domain
{
    /// <summary>
    /// Represents a record to manage product inventory per warehouse
    /// </summary>
    public partial class CampgroundSite : BaseEntity
    {
        private ICollection<CampgroundSiteAttributes> _siteAttributes;
        private ICollection<CampgroundSiteTypeAttributes> _siteTypeAttributes;

        /// <summary>
        /// Gets or sets the product identifier
        /// </summary>
        public int CampgroundId { get; set; }

        /// <summary>
        /// Gets the product
        /// </summary>
        public virtual Campground Campground { get; set; }

        public virtual ICollection<CampgroundSiteAttributes> SiteAttributes
        {
            get { return _siteAttributes ?? (_siteAttributes = new List<CampgroundSiteAttributes>()); }
            protected set { _siteAttributes = value; }
        }

        public virtual ICollection<CampgroundSiteTypeAttributes> SiteTypeAttributes
        {
            get { return _siteTypeAttributes ?? (_siteTypeAttributes = new List<CampgroundSiteTypeAttributes>()); }
            protected set { _siteTypeAttributes = value; }
        }

    }
}
