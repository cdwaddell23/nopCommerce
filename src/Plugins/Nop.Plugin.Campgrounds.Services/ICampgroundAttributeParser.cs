using Nop.Plugin.Campgrounds.Data.Domain;
using System.Collections.Generic;

namespace Nop.Plugin.Campgrounds.Services
{
    /// <summary>
    /// Campground attribute parser interface
    /// </summary>
    public partial interface ICampgroundAttributeTypeParser
    {
        #region Campground attributes

        /// <summary>
        /// Gets selected campground attribute mappings
        /// </summary>
        /// <param name="attributesXml">Attributes in XML format</param>
        /// <returns>Selected campground attribute mappings</returns>
        IList<CampgroundAttributeMapping> ParseCampgroundAttributeMappings(string attributesXml);

        /// <summary>
        /// Get campground attribute values
        /// </summary>
        /// <param name="attributesXml">Attributes in XML format</param>
        /// <param name="campgroundAttributeTypeMappingId">Campground attribute mapping identifier; pass 0 to load all values</param>
        /// <returns>Campground attribute values</returns>
        IList<CampgroundAttributeValue> ParseCampgroundAttributeValues(string attributesXml, int campgroundAttributeTypeMappingId = 0);

        /// <summary>
        /// Gets selected campground attribute values
        /// </summary>
        /// <param name="attributesXml">Attributes in XML format</param>
        /// <param name="campgroundAttributeTypeMappingId">Campground attribute mapping identifier</param>
        /// <returns>Campground attribute values</returns>
        IList<string> ParseValues(string attributesXml, int campgroundAttributeTypeMappingId);

        /// <summary>
        /// Adds an attribute
        /// </summary>
        /// <param name="attributesXml">Attributes in XML format</param>
        /// <param name="campgroundAttributeTypeMapping">Campground attribute mapping</param>
        /// <param name="value">Value</param>
        /// <param name="quantity">Quantity (used with AttributeValueType.AssociatedToCampground to specify the quantity entered by the customer)</param>
        /// <returns>Updated result (XML format)</returns>
        string AddCampgroundAttributeType(string attributesXml, CampgroundAttributeMapping campgroundAttributeTypeMapping, string value, int? quantity = null);

        /// <summary>
        /// Remove an attribute
        /// </summary>
        /// <param name="attributesXml">Attributes in XML format</param>
        /// <param name="campgroundAttributeTypeMapping">Campground attribute mapping</param>
        /// <returns>Updated result (XML format)</returns>
        string RemoveCampgroundAttributeType(string attributesXml, CampgroundAttributeMapping campgroundAttributeTypeMapping);

        /// <summary>
        /// Are attributes equal
        /// </summary>
        /// <param name="attributesXml1">The attributes of the first campground</param>
        /// <param name="attributesXml2">The attributes of the second campground</param>
        /// <param name="ignoreNonCombinableAttributes">A value indicating whether we should ignore non-combinable attributes</param>
        /// <param name="ignoreQuantity">A value indicating whether we should ignore the quantity of attribute value entered by the customer</param>
        /// <returns>Result</returns>
        bool AreCampgroundAttributeTypesEqual(string attributesXml1, string attributesXml2, bool ignoreNonCombinableAttributes, bool ignoreQuantity = true);

        /// <summary>
        /// Check whether condition of some attribute is met (if specified). Return "null" if not condition is specified
        /// </summary>
        /// <param name="pam">Campground attribute</param>
        /// <param name="selectedAttributesXml">Selected attributes (XML format)</param>
        /// <returns>Result</returns>
        bool? IsConditionMet(CampgroundAttributeMapping pam, string selectedAttributesXml);

        /// <summary>
        /// Finds a campground attribute combination by attributes stored in XML 
        /// </summary>
        /// <param name="campground">Campground</param>
        /// <param name="attributesXml">Attributes in XML format</param>
        /// <param name="ignoreNonCombinableAttributes">A value indicating whether we should ignore non-combinable attributes</param>
        /// <returns>Found campground attribute combination</returns>
        CampgroundAttributeCombination FindCampgroundAttributeTypeCombination(Campground campground,
            string attributesXml, bool ignoreNonCombinableAttributes = true);

        /// <summary>
        /// Generate all combinations
        /// </summary>
        /// <param name="campground">Campground</param>
        /// <param name="ignoreNonCombinableAttributes">A value indicating whether we should ignore non-combinable attributes</param>
        /// <returns>Attribute combinations in XML format</returns>
        IList<string> GenerateAllCombinations(Campground campground, bool ignoreNonCombinableAttributes = false);

        #endregion

        #region Gift card attributes

        /// <summary>
        /// Add gift card attributes
        /// </summary>
        /// <param name="attributesXml">Attributes in XML format</param>
        /// <param name="recipientName">Recipient name</param>
        /// <param name="recipientEmail">Recipient email</param>
        /// <param name="senderName">Sender name</param>
        /// <param name="senderEmail">Sender email</param>
        /// <param name="giftCardMessage">Message</param>
        /// <returns>Attributes</returns>
        string AddGiftCardAttribute(string attributesXml, string recipientName,
            string recipientEmail, string senderName, string senderEmail, string giftCardMessage);

        /// <summary>
        /// Get gift card attributes
        /// </summary>
        /// <param name="attributesXml">Attributes in XML format</param>
        /// <param name="recipientName">Recipient name</param>
        /// <param name="recipientEmail">Recipient email</param>
        /// <param name="senderName">Sender name</param>
        /// <param name="senderEmail">Sender email</param>
        /// <param name="giftCardMessage">Message</param>
        void GetGiftCardAttribute(string attributesXml, out string recipientName,
            out string recipientEmail, out string senderName,
            out string senderEmail, out string giftCardMessage);

        #endregion
    }
}
