using Nop.Core;
using Nop.Plugin.Campgrounds.Data.Domain;

namespace Nop.Plugin.Campgrounds.Services
{
    /// <summary>
    /// CampgroundHost service interface
    /// </summary>
    public partial interface ICampgroundHostService
    {
        /// <summary>
        /// Gets a campgroundHost by campgroundHost identifier
        /// </summary>
        /// <param name="campgroundHostId">CampgroundHost identifier</param>
        /// <returns>CampgroundHost</returns>
        CampgroundHost GetCampgroundHostById(int campgroundHostId);
        
        /// <summary>
        /// Gets campground host by customer id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        CampgroundHost GetCampgroundHostByCustomerId(int customerId);

        /// <summary>
        /// Get campground host by email address
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        CampgroundHost GetCampgroundHostByEmail(string email);
            
        /// <summary>
        /// Delete a campgroundHost
        /// </summary>
        /// <param name="campgroundHost">CampgroundHost</param>
        void DeleteCampgroundHost(CampgroundHost campgroundHost);

        /// <summary>
        /// Gets all campgroundHosts
        /// </summary>
        /// <param name="name">CampgroundHost name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>CampgroundHosts</returns>
        IPagedList<CampgroundHost> GetAllCampgroundHosts(string name = "", 
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

        /// <summary>
        /// Inserts a campgroundHost
        /// </summary>
        /// <param name="campgroundHost">CampgroundHost</param>
        void InsertCampgroundHost(CampgroundHost campgroundHost);

        /// <summary>
        /// Updates the campgroundHost
        /// </summary>
        /// <param name="campgroundHost">CampgroundHost</param>
        void UpdateCampgroundHost(CampgroundHost campgroundHost);
    }
}