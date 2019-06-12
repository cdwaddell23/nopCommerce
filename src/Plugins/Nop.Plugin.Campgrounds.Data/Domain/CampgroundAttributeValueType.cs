namespace Nop.Plugin.Campgrounds.Data.Domain
{
    /// <summary>
    /// Represents an attribute value type
    /// </summary>
    public enum CampgroundAttributeValueType
    {
        /// <summary>
        /// Simple attribute value
        /// </summary>
        Campground = 0,
        /// <summary>
        /// Federal attribute value
        /// </summary>
        FederalLand = 5,
        /// <summary>
        /// State attribute value
        /// </summary>
        StateLand = 10,
        /// <summary>
        /// Associated to a campground (used when configuring bundled products)
        /// </summary>
        AssociatedToCampground = 15,
    }
}
