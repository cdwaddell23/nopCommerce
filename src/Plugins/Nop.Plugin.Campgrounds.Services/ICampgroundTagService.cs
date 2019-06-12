using Nop.Plugin.Campgrounds.Data.Domain;
using System.Collections.Generic;


namespace Nop.Plugin.Campgrounds.Services
{
    /// <summary>
    /// Campground tag service interface
    /// </summary>
    public partial interface ICampgroundTagService
    {
        /// <summary>
        /// Delete a campground tag
        /// </summary>
        /// <param name="campgroundTag">Campground tag</param>
        void DeleteCampgroundTag(CampgroundTag campgroundTag);

        /// <summary>
        /// Gets all campground tags
        /// </summary>
        /// <returns>Campground tags</returns>
        IList<CampgroundTag> GetAllCampgroundTags();

        /// <summary>
        /// Gets campground tag
        /// </summary>
        /// <param name="campgroundTagId">Campground tag identifier</param>
        /// <returns>Campground tag</returns>
        CampgroundTag GetCampgroundTagById(int campgroundTagId);
        
        /// <summary>
        /// Gets campground tag by name
        /// </summary>
        /// <param name="name">Campground tag name</param>
        /// <returns>Campground tag</returns>
        CampgroundTag GetCampgroundTagByName(string name);

        /// <summary>
        /// Inserts a campground tag
        /// </summary>
        /// <param name="campgroundTag">Campground tag</param>
        void InsertCampgroundTag(CampgroundTag campgroundTag);

        /// <summary>
        /// Updates the campground tag
        /// </summary>
        /// <param name="campgroundTag">Campground tag</param>
        void UpdateCampgroundTag(CampgroundTag campgroundTag);

        /// <summary>
        /// Get number of campgrounds
        /// </summary>
        /// <param name="campgroundTagId">Campground tag identifier</param>
        /// <param name="storeId">Store identifier</param>
        /// <returns>Number of campgrounds</returns>
        int GetCampgroundCount(int campgroundTagId, int storeId);

        /// <summary>
        /// Update campground tags
        /// </summary>
        /// <param name="campground">Campground for update</param>
        /// <param name="campgroundTags">Campground tags</param>
        void UpdateCampgroundTags(Campground campground, string[] campgroundTags);
    }
}
