using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Catalog;

namespace Nop.Plugin.Campgrounds.Data.Domain
{
    /// <summary>
    /// Represents a Campground attribute mapping
    /// </summary>
    public partial class CampgroundAttributeMapping : BaseEntity, ILocalizedEntity
    {
        private ICollection<CampgroundAttributeValue> _campgroundAttributeValues;

        /// <summary>
        /// Gets or sets the Campground identifier
        /// </summary>
        public int CampgroundId { get; set; }

        /// <summary>
        /// Gets or sets the Campground attribute identifier
        /// </summary>
        public int CampgroundAttributeTypeId { get; set; }

        /// <summary>
        /// Gets or sets a value a text prompt
        /// </summary>
        public string TextPrompt { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is required
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Gets or sets the attribute control type identifier
        /// </summary>
        public int AttributeControlTypeId { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }

        //validation fields

        /// <summary>
        /// Gets or sets the validation rule for minimum length (for textbox and multiline textbox)
        /// </summary>
        public int? ValidationMinLength { get; set; }

        /// <summary>
        /// Gets or sets the validation rule for maximum length (for textbox and multiline textbox)
        /// </summary>
        public int? ValidationMaxLength { get; set; }

        /// <summary>
        /// Gets or sets the default value (for textbox and multiline textbox)
        /// </summary>
        public string DefaultValue { get; set; }



        /// <summary>
        /// Gets or sets a condition (depending on other attribute) when this attribute should be enabled (visible).
        /// Leave empty (or null) to enable this attribute.
        /// Conditional attributes that only appear if a previous attribute is selected, such as having an option 
        /// for personalizing clothing with a name and only providing the text input box if the "Personalize" radio button is checked.
        /// </summary>
        public string ConditionAttributeXml { get; set; }



        /// <summary>
        /// Gets the attribute control type
        /// </summary>
        public AttributeControlType AttributeControlType
        {
            get
            {
                return (AttributeControlType)this.AttributeControlTypeId;
            }
            set
            {
                this.AttributeControlTypeId = (int)value; 
            }
        }

        /// <summary>
        /// Gets the Campground attribute
        /// </summary>
        public virtual CampgroundAttributeType CampgroundAttributeType { get; set; }

        /// <summary>
        /// Gets the Campground
        /// </summary>
        public virtual Campground Campground { get; set; }
        
        /// <summary>
        /// Gets the Campground attribute values
        /// </summary>
        public virtual ICollection<CampgroundAttributeValue> CampgroundAttributeValues
        {
            get { return _campgroundAttributeValues ?? (_campgroundAttributeValues = new List<CampgroundAttributeValue>()); }
            protected set { _campgroundAttributeValues = value; }
        }

    }

}
