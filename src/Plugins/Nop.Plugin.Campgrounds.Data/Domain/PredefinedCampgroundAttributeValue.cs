using Nop.Core;
using Nop.Core.Domain.Localization;

namespace Nop.Plugin.Campgrounds.Data.Domain
{
    /// <summary>
    /// Represents a predefined (default) Campground attribute value
    /// </summary>
    public partial class PredefinedCampgroundAttributeValue : BaseEntity, ILocalizedEntity
    {
        /// <summary>
        /// Gets or sets the Campground attribute identifier
        /// </summary>
        public int CampgroundAttributeTypeId { get; set; }

        /// <summary>
        /// Gets or sets the Campground attribute name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the price adjustment
        /// </summary>
        public decimal PriceAdjustment { get; set; }

        /// <summary>
        /// Gets or sets the attibute value cost
        /// </summary>
        public decimal Cost { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the value is pre-selected
        /// </summary>
        public bool IsPreSelected { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets the Campground attribute
        /// </summary>
        public virtual CampgroundAttributeType CampgroundAttributeType { get; set; }
    }
}
