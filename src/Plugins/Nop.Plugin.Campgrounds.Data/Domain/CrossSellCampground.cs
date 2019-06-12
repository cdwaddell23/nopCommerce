using Nop.Core;

namespace Nop.Plugin.Campgrounds.Data.Domain
{ 
    /// <summary>
    /// Represents a cross-sell Campground
    /// </summary>
    public partial class CrossSellCampground : BaseEntity
    {
        /// <summary>
        /// Gets or sets the first Campground identifier
        /// </summary>
        public int CampgroundId1 { get; set; }

        /// <summary>
        /// Gets or sets the second Campground identifier
        /// </summary>
        public int CampgroundId2 { get; set; }
    }

}
