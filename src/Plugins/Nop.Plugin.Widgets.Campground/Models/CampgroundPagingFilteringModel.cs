using System.Collections.Generic;
using Nop.Web.Models.Catalog;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.UI.Paging;

namespace Nop.Plugin.Widgets.Campgrounds.Models
{
    /// <summary>
    /// Filtering and paging model for campground
    /// </summary>
    public partial class CampgroundPagingFilteringModel : BasePageableModel
    {
        #region Ctor

        /// <summary>
        /// Constructor
        /// </summary>
        public CampgroundPagingFilteringModel()
        {
            this.AvailableSortOptions = new List<SelectListItem>();
            this.AvailableViewModes = new List<SelectListItem>();
            this.PageSizeOptions = new List<SelectListItem>();
            this.FilterTypeModes = new List<SelectListItem>();
            this.FilterAmenitiesModes = new List<SelectListItem>();
            this.FilterActivitiesModes = new List<SelectListItem>();
            this.FilterServicesModes = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// A value indicating whether campground sorting is allowed
        /// </summary>
        public bool AllowSorting { get; set; }
        /// <summary>
        /// Available sort options
        /// </summary>
        public IList<SelectListItem> AvailableSortOptions { get; set; }

        /// <summary>
        /// A value indicating whether customers are allowed to change view mode
        /// </summary>
        public bool AllowViewModeChanging { get; set; }
        /// <summary>
        /// Available view mode options
        /// </summary>
        public IList<SelectListItem> AvailableViewModes { get; set; }

        /// <summary>
        /// A value indicating whether customers are allowed to select page size
        /// </summary>
        public bool AllowCustomersToSelectPageSize { get; set; }
        /// <summary>
        /// Available page size options
        /// </summary>
        public IList<SelectListItem> PageSizeOptions { get; set; }

        /// <summary>
        /// Order by
        /// </summary>
        public int? OrderBy { get; set; }

        /// <summary>
        /// Campground sorting
        /// </summary>
        public string ViewMode { get; set; }

        /// <summary>
        /// Available view mode options
        /// </summary>
        public IList<SelectListItem> FilterTypeModes { get; set; }
        public int[] CampgroundTypeSelectedIndexes { get; set; }
        /// <summary>
        /// Available view mode options
        /// </summary>
        public IList<SelectListItem> FilterAmenitiesModes { get; set; }
        public int[] AmenitiesSelectedIndexes { get; set; }
        /// <summary>
        /// Available view mode options
        /// </summary>
        public IList<SelectListItem> FilterActivitiesModes { get; set; }
        public int[] ActivitiesSelectedIndexes { get; set; }
        /// <summary>
        /// Available view mode options
        /// </summary>
        public IList<SelectListItem> FilterServicesModes { get; set; }
        public int[] ServicesSelectedIndexes { get; set; }

        #endregion


    }
}