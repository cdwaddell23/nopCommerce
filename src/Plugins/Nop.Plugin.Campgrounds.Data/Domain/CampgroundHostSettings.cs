using Nop.Core.Configuration;
using Nop.Core.Domain.Vendors;

namespace Nop.Plugin.Campgrounds.Data.Domain
{
    /// <summary>
    /// Vendor settings
    /// </summary>
    public partial class CampgroundHostSettings : VendorSettings, ISettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether to display vendor name on the product details page
        /// </summary>
        public bool ShowCampgroundHostOnCampgroundDetailsPage { get; set; }
        /// <summary>
        /// Gets or sets a maximum number of campgrounds per vendor
        /// </summary>
        public int MaximumCampgroundNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether vendors are allowed to import products
        /// </summary>
        public bool AllowCampgroundHostToImportCampgrounds { get; set; }
    }
}
