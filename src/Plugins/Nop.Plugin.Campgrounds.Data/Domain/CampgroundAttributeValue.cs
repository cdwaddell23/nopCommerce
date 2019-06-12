using Nop.Core;
using Nop.Core.Domain.Localization;

namespace Nop.Plugin.Campgrounds.Data.Domain
{
    /// <summary>
    /// Represents a Campground attribute value
    /// </summary>
    public partial class CampgroundAttributeValue : BaseEntity, ILocalizedEntity
    {
        /// <summary>
        /// Gets or sets the Campground attribute mapping identifier
        /// </summary>
        public int CampgroundAttributeMappingId { get; set; }

        /// <summary>
        /// Gets or sets the attribute value type identifier
        /// </summary>
        public int CampgroundAttributeValueTypeId { get; set; }

        /// <summary>
        /// Gets or sets the associated Campground identifier (used only with AttributeValueType.AssociatedToCampground)
        /// </summary>
        public int AssociatedCampgroundId { get; set; }

        /// <summary>
        /// Gets or sets the Campground attribute name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the color RGB value (used with "Color squares" attribute type)
        /// </summary>
        public string ColorSquaresRgb { get; set; }

        /// <summary>
        /// Gets or sets the picture ID for image square (used with "Image squares" attribute type)
        /// </summary>
        public int ImageSquaresPictureId { get; set; }

        /// <summary>
        /// Gets or sets the price adjustment (used only with AttributeValueType.Simple)
        /// </summary>
        public decimal PriceAdjustment { get; set; }

        /// <summary>
        /// Gets or sets the attibute value cost (used only with AttributeValueType.Simple)
        /// </summary>
        public decimal Cost { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customer can enter the quantity of associated Campground (used only with AttributeValueType.AssociatedToCampground)
        /// </summary>
        public bool CustomerEntersQty { get; set; }

        /// <summary>
        /// Gets or sets the quantity of associated Campground (used only with AttributeValueType.AssociatedToCampground)
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the value is pre-selected
        /// </summary>
        public bool IsPreSelected { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the picture (identifier) associated with this value. This picture should replace a Campground main picture once clicked (selected).
        /// </summary>
        public int PictureId { get; set; }

        /// <summary>
        /// Gets the Campground attribute mapping
        /// </summary>
        public virtual CampgroundAttributeMapping CampgroundAttributeMapping { get; set; }

        /// <summary>
        /// Gets or sets the attribute value type
        /// </summary>
        public CampgroundAttributeValueType CampgroundAttributeValueType
        {
            get
            {
                return (CampgroundAttributeValueType)this.CampgroundAttributeValueTypeId;
            }
            set
            {
                this.CampgroundAttributeValueTypeId = (int)value;
            }
        }
    }
}
