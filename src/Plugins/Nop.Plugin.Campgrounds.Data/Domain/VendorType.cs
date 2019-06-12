namespace Nop.Plugin.Campgrounds.Data.Domain
{
    /// <summary>
    /// Represents a product type
    /// </summary>
    public enum CampgroundVendorType
    {
        /// <summary>
        /// Campground
        /// </summary>
        CampgroundHost = 0,
        /// <summary>
        /// Simple
        /// </summary>
        CampgroundManager = 5,
        /// <summary>
        /// Grouped (product with variants)
        /// </summary>
        Other = 10,
    }
}
