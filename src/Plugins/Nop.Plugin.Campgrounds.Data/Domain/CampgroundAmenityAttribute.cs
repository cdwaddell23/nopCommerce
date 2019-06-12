using Nop.Core;

namespace Nop.Plugin.Campgrounds.Data.Domain
{
    /// <summary>
    /// Represents a Campground specification attribute
    /// </summary>
    public partial class CampgroundAmenityAttribute : BaseEntity
    {
        /// <summary>
        /// Gets or sets the Campground identifier
        /// </summary>
        public int CampgroundId { get; set; }

        /// <summary>
        /// Gets or sets the attribute type ID
        /// </summary>
        public int AttributeTypeId { get; set; }

        /// <summary>
        /// Gets or sets the specification attribute identifier
        /// </summary>
        public int AmenityAttributeOptionId { get; set; }

        /// <summary>
        /// Gets or sets the custom value
        /// </summary>
        public string CustomValue { get; set; }

        /// <summary>
        /// Gets or sets whether the attribute can be filtered by
        /// </summary>
        public bool AllowFiltering { get; set; }

        /// <summary>
        /// Gets or sets whether the attribute will be shown on the Campground page
        /// </summary>
        public bool ShowOnCampgroundPage { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }
        
        /// <summary>
        /// Gets or sets the Campground
        /// </summary>
        public virtual Campground Campground { get; set; }

        /// <summary>
        /// Gets or sets the specification attribute option
        /// </summary>
        public virtual Nop.Core.Domain.Catalog.SpecificationAttributeOption AmenityAttributeOption { get; set; }

        /// <summary>
        /// Gets the attribute control type
        /// </summary>
        public Nop.Core.Domain.Catalog.SpecificationAttributeType AttributeType
        {
            get
            {
                return (Nop.Core.Domain.Catalog.SpecificationAttributeType)this.AttributeTypeId;
            }
            set
            {
                this.AttributeTypeId = (int)value;
            }
        }
    }
}
