using System.Collections.Generic;
using Nop.Plugin.Campgrounds.Data.Domain;

namespace Nop.Plugin.Campgrounds.Services
{
    /// <summary>
    /// Campground template interface
    /// </summary>
    public partial interface ICampgroundTemplateService
    {
        /// <summary>
        /// Delete Campground template
        /// </summary>
        /// <param name="CampgroundTemplate">Campground template</param>
        void DeleteCampgroundTemplate(CampgroundTemplate CampgroundTemplate);

        /// <summary>
        /// Gets all Campground templates
        /// </summary>
        /// <returns>Campground templates</returns>
        IList<CampgroundTemplate> GetAllCampgroundTemplates();

        /// <summary>
        /// Gets a Campground template
        /// </summary>
        /// <param name="CampgroundTemplateId">Campground template identifier</param>
        /// <returns>Campground template</returns>
        CampgroundTemplate GetCampgroundTemplateById(int CampgroundTemplateId);

        /// <summary>
        /// Inserts Campground template
        /// </summary>
        /// <param name="CampgroundTemplate">Campground template</param>
        void InsertCampgroundTemplate(CampgroundTemplate CampgroundTemplate);

        /// <summary>
        /// Updates the Campground template
        /// </summary>
        /// <param name="CampgroundTemplate">Campground template</param>
        void UpdateCampgroundTemplate(CampgroundTemplate CampgroundTemplate);
    }
}
