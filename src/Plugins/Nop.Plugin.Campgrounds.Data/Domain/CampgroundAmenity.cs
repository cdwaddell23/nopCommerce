using Nop.Core;
using Nop.Core.Domain.Localization;

namespace Nop.Plugin.Campgrounds.Data.Domain
{
    /// <summary>
    /// Represents a Campground Amenity
    /// </summary>
    public partial class CampgroundAmenity : BaseEntity, ILocalizedEntity
    {
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        public string Description { get; set; }
    }
}