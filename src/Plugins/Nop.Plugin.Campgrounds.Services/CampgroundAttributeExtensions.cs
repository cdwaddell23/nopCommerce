using Nop.Core.Domain.Catalog;
using Nop.Plugin.Campgrounds.Data.Domain;

namespace Nop.Plugin.Campgrounds.Services
{
    /// <summary>
    /// Extensions
    /// </summary>
    public static class CampgroundAttributeTypeExtensions
    {
        /// <summary>
        /// A value indicating whether this campground attribute should have values
        /// </summary>
        /// <param name="campgroundAttributeTypeMapping">Campground attribute mapping</param>
        /// <returns>Result</returns>
        public static bool ShouldHaveValues(this CampgroundAttributeMapping campgroundAttributeTypeMapping)
        {
            if (campgroundAttributeTypeMapping == null)
                return false;

            if (campgroundAttributeTypeMapping.AttributeControlType == AttributeControlType.TextBox ||
                campgroundAttributeTypeMapping.AttributeControlType == AttributeControlType.MultilineTextbox ||
                campgroundAttributeTypeMapping.AttributeControlType == AttributeControlType.Datepicker ||
                campgroundAttributeTypeMapping.AttributeControlType == AttributeControlType.FileUpload)
                return false;

            //other attribute controle types support values
            return true;
        }

        /// <summary>
        /// A value indicating whether this campground attribute can be used as condition for some other attribute
        /// </summary>
        /// <param name="campgroundAttributeTypeMapping">Campground attribute mapping</param>
        /// <returns>Result</returns>
        public static bool CanBeUsedAsCondition(this CampgroundAttributeMapping campgroundAttributeTypeMapping)
        {
            if (campgroundAttributeTypeMapping == null)
                return false;

            if (campgroundAttributeTypeMapping.AttributeControlType == AttributeControlType.ReadonlyCheckboxes || 
                campgroundAttributeTypeMapping.AttributeControlType == AttributeControlType.TextBox ||
                campgroundAttributeTypeMapping.AttributeControlType == AttributeControlType.MultilineTextbox ||
                campgroundAttributeTypeMapping.AttributeControlType == AttributeControlType.Datepicker ||
                campgroundAttributeTypeMapping.AttributeControlType == AttributeControlType.FileUpload)
                return false;

            //other attribute controle types support it
            return true;
        }

        /// <summary>
        /// A value indicating whether this campground attribute should can have some validation rules
        /// </summary>
        /// <param name="campgroundAttributeTypeMapping">Campground attribute mapping</param>
        /// <returns>Result</returns>
        public static bool ValidationRulesAllowed(this CampgroundAttributeMapping campgroundAttributeTypeMapping)
        {
            if (campgroundAttributeTypeMapping == null)
                return false;

            if (campgroundAttributeTypeMapping.AttributeControlType == AttributeControlType.TextBox ||
                campgroundAttributeTypeMapping.AttributeControlType == AttributeControlType.MultilineTextbox ||
                campgroundAttributeTypeMapping.AttributeControlType == AttributeControlType.FileUpload)
                return true;

            //other attribute controle types does not have validation
            return false;
        }

        /// <summary>
        /// A value indicating whether this campground attribute is non-combinable
        /// </summary>
        /// <param name="campgroundAttributeTypeMapping">Campground attribute mapping</param>
        /// <returns>Result</returns>
        public static bool IsNonCombinable(this CampgroundAttributeMapping campgroundAttributeTypeMapping)
        {
            //When you have a campground with several attributes it may well be that some are combinable,
            //whose combination may form a new SKU with its own inventory,
            //and some non-combinable are more used to add accesories

            if (campgroundAttributeTypeMapping == null)
                return false;

            //we can add a new property to "CampgroundAttributeMapping" entity indicating whether it's combinable/non-combinable
            //but we assume that attributes
            //which cannot have values (any value can be entered by a customer)
            //are non-combinable
            var result = !ShouldHaveValues(campgroundAttributeTypeMapping);
            return result;
        }
    }
}
