using System.Collections.Generic;
using Nop.Core;
using Nop.Plugin.Campgrounds.Data.Domain;

namespace Nop.Plugin.Campgrounds.Services
{
    /// <summary>
    /// Campground attribute service interface
    /// </summary>
    public partial interface ICampgroundAttributeTypeService
    {
        #region Campground attributes

        /// <summary>
        /// Deletes a campground attribute
        /// </summary>
        /// <param name="campgroundAttributeType">Campground attribute</param>
        void DeleteCampgroundAttributeType(CampgroundAttributeType campgroundAttributeType);

        /// <summary>
        /// Gets all campground attributes
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Campground attributes</returns>
        IPagedList<CampgroundAttributeType> GetAllCampgroundAttributeTypes(int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Gets a campground attribute 
        /// </summary>
        /// <param name="campgroundAttributeTypeId">Campground attribute identifier</param>
        /// <returns>Campground attribute </returns>
        CampgroundAttributeType GetCampgroundAttributeTypeById(int campgroundAttributeTypeId);

        /// <summary>
        /// Inserts a campground attribute
        /// </summary>
        /// <param name="campgroundAttributeType">Campground attribute</param>
        void InsertCampgroundAttributeType(CampgroundAttributeType campgroundAttributeType);

        /// <summary>
        /// Updates the campground attribute
        /// </summary>
        /// <param name="campgroundAttributeType">Campground attribute</param>
        void UpdateCampgroundAttributeType(CampgroundAttributeType campgroundAttributeType);

        /// <summary>
        /// Returns a list of IDs of not existing attributes
        /// </summary>
        /// <param name="attributeId">The IDs of the attributes to check</param>
        /// <returns>List of IDs not existing attributes</returns>
        int[] GetNotExistingAttributes(int[] attributeId);

        #endregion

        #region Campground attributes mappings

        /// <summary>
        /// Deletes a campground attribute mapping
        /// </summary>
        /// <param name="campgroundAttributeTypeMapping">Campground attribute mapping</param>
        void DeleteCampgroundAttributeMapping(CampgroundAttributeMapping campgroundAttributeTypeMapping);

        /// <summary>
        /// Gets campground attribute mappings by campground identifier
        /// </summary>
        /// <param name="campgroundId">The campground identifier</param>
        /// <returns>Campground attribute mapping collection</returns>
        IList<CampgroundAttributeMapping> GetCampgroundAttributeMappingsByCampgroundId(int campgroundId);

        /// <summary>
        /// Gets a campground attribute mapping
        /// </summary>
        /// <param name="campgroundAttributeTypeMappingId">Campground attribute mapping identifier</param>
        /// <returns>Campground attribute mapping</returns>
        CampgroundAttributeMapping GetCampgroundAttributeMappingById(int campgroundAttributeTypeMappingId);

        /// <summary>
        /// Inserts a campground attribute mapping
        /// </summary>
        /// <param name="campgroundAttributeTypeMapping">The campground attribute mapping</param>
        void InsertCampgroundAttributeMapping(CampgroundAttributeMapping campgroundAttributeTypeMapping);

        /// <summary>
        /// Updates the campground attribute mapping
        /// </summary>
        /// <param name="campgroundAttributeTypeMapping">The campground attribute mapping</param>
        void UpdateCampgroundAttributeMapping(CampgroundAttributeMapping campgroundAttributeTypeMapping);

        #endregion

        #region Campground attribute values

        /// <summary>
        /// Deletes a campground attribute value
        /// </summary>
        /// <param name="campgroundAttributeTypeValue">Campground attribute value</param>
        void DeleteCampgroundAttributeValue(CampgroundAttributeValue campgroundAttributeTypeValue);

        /// <summary>
        /// Gets campground attribute values by campground attribute mapping identifier
        /// </summary>
        /// <param name="campgroundAttributeTypeMappingId">The campground attribute mapping identifier</param>
        /// <returns>Campground attribute values</returns>
        IList<CampgroundAttributeValue> GetCampgroundAttributeValues(int campgroundAttributeTypeMappingId);

        /// <summary>
        /// Gets a campground attribute value
        /// </summary>
        /// <param name="campgroundAttributeTypeValueId">Campground attribute value identifier</param>
        /// <returns>Campground attribute value</returns>
        CampgroundAttributeValue GetCampgroundAttributeValueById(int campgroundAttributeTypeValueId);

        /// <summary>
        /// Inserts a campground attribute value
        /// </summary>
        /// <param name="campgroundAttributeTypeValue">The campground attribute value</param>
        void InsertCampgroundAttributeValue(CampgroundAttributeValue campgroundAttributeTypeValue);

        /// <summary>
        /// Updates the campground attribute value
        /// </summary>
        /// <param name="campgroundAttributeTypeValue">The campground attribute value</param>
        void UpdateCampgroundAttributeValue(CampgroundAttributeValue campgroundAttributeTypeValue);

        #endregion

        #region Predefined campground attribute values

        /// <summary>
        /// Deletes a predefined campground attribute value
        /// </summary>
        /// <param name="ppav">Predefined campground attribute value</param>
        void DeletePredefinedCampgroundAttributeValue(PredefinedCampgroundAttributeValue ppav);

        /// <summary>
        /// Gets predefined campground attribute values by campground attribute identifier
        /// </summary>
        /// <param name="campgroundAttributeTypeId">The campground attribute identifier</param>
        /// <returns>Campground attribute mapping collection</returns>
        IList<PredefinedCampgroundAttributeValue> GetPredefinedCampgroundAttributeValues(int campgroundAttributeTypeId);

        /// <summary>
        /// Gets a predefined campground attribute value
        /// </summary>
        /// <param name="id">Predefined campground attribute value identifier</param>
        /// <returns>Predefined campground attribute value</returns>
        PredefinedCampgroundAttributeValue GetPredefinedCampgroundAttributeValueById(int id);

        /// <summary>
        /// Inserts a predefined campground attribute value
        /// </summary>
        /// <param name="ppav">The predefined campground attribute value</param>
        void InsertPredefinedCampgroundAttributeValue(PredefinedCampgroundAttributeValue ppav);

        /// <summary>
        /// Updates the predefined campground attribute value
        /// </summary>
        /// <param name="ppav">The predefined campground attribute value</param>
        void UpdatePredefinedCampgroundAttributeValue(PredefinedCampgroundAttributeValue ppav);

        #endregion

        #region Campground attribute combinations

        /// <summary>
        /// Deletes a campground attribute combination
        /// </summary>
        /// <param name="combination">Campground attribute combination</param>
        void DeleteCampgroundAttributeTypeCombination(CampgroundAttributeCombination combination);

        /// <summary>
        /// Gets all campground attribute combinations
        /// </summary>
        /// <param name="campgroundId">Campground identifier</param>
        /// <returns>Campground attribute combinations</returns>
        IList<CampgroundAttributeCombination> GetAllCampgroundAttributeTypeCombinations(int campgroundId);

        /// <summary>
        /// Gets a campground attribute combination
        /// </summary>
        /// <param name="campgroundAttributeTypeCombinationId">Campground attribute combination identifier</param>
        /// <returns>Campground attribute combination</returns>
        CampgroundAttributeCombination GetCampgroundAttributeTypeCombinationById(int campgroundAttributeTypeCombinationId);

        /// <summary>
        /// Inserts a campground attribute combination
        /// </summary>
        /// <param name="combination">Campground attribute combination</param>
        void InsertCampgroundAttributeTypeCombination(CampgroundAttributeCombination combination);

        /// <summary>
        /// Updates a campground attribute combination
        /// </summary>
        /// <param name="combination">Campground attribute combination</param>
        void UpdateCampgroundAttributeTypeCombination(CampgroundAttributeCombination combination);

        #endregion
       
    }
}
