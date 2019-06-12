using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Mvc.Models;

namespace Nop.Plugin.Widgets.Campgrounds.Models
{
    public partial class CampgroundListModel : BaseNopModel
    {
        public CampgroundListModel()
        {
            AvailableCategories = new List<SelectListItem>();
            AvailableCampgroundHosts = new List<SelectListItem>();
            AvailableCampgroundTypes = new List<SelectListItem>();
            AvailablePublishedOptions = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Admin.Campgrounds.List.SearchCampgroundName")]
        public string SearchCampgroundName { get; set; }
        [NopResourceDisplayName("Admin.Campgrounds.List.SearchCategory")]
        public int SearchCategoryId { get; set; }
        [NopResourceDisplayName("Admin.Campgrounds.List.SearchIncludeSubCategories")]
        public bool SearchIncludeSubCategories { get; set; }
        [NopResourceDisplayName("Admin.Campgrounds.List.SearchCampgroundHost")]
        public int SearchCampgroundHostId { get; set; }
        [NopResourceDisplayName("Admin.Campgrounds.List.SearchCampgroundType")]
        public int SearchCampgroundTypeId { get; set; }
        [NopResourceDisplayName("Admin.Campgrounds.List.SearchPublished")]
        public int SearchPublishedId { get; set; }

        public bool IsLoggedInAsCampgroundHost { get; set; }

        public bool AllowCampgroundHostsToImportCampgrounds { get; set; }

        public IList<SelectListItem> AvailableCategories { get; set; }
        public IList<SelectListItem> AvailableCampgroundHosts { get; set; }
        public IList<SelectListItem> AvailableCampgroundTypes { get; set; }
        public IList<SelectListItem> AvailablePublishedOptions { get; set; }
    }
}