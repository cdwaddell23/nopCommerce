using Nop.Core;
using Nop.Plugin.Campgrounds.Data.Domain;

namespace Nop.Plugin.Campgrounds.Services
{
    /// <summary>
    /// CampgroundType service interface
    /// </summary>
    public partial interface ICampgroundTypeService
    {
        /// <summary>
        /// Gets a campgroundType by campgroundType identifier
        /// </summary>
        /// <param name="campgroundTypeId">CampgroundType identifier</param>
        /// <returns>CampgroundType</returns>
        CampgroundType GetCampgroundTypeById(int campgroundTypeId);

        /// <summary>
        /// Delete a campgroundType
        /// </summary>
        /// <param name="campgroundType">CampgroundType</param>
        void DeleteCampgroundType(CampgroundType campgroundType);

        /// <summary>
        /// Gets all campgroundTypes
        /// </summary>
        /// <param name="name">CampgroundType name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>CampgroundTypes</returns>
        IPagedList<CampgroundType> GetAllCampgroundTypes(string name = "", 
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

        /// <summary>
        /// Inserts a campgroundType
        /// </summary>
        /// <param name="campgroundType">CampgroundType</param>
        void InsertCampgroundType(CampgroundType campgroundType);

        /// <summary>
        /// Updates the campgroundType
        /// </summary>
        /// <param name="campgroundType">CampgroundType</param>
        void UpdateCampgroundType(CampgroundType campgroundType);
    }
}