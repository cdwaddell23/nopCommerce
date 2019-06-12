using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Mvc.Models;

namespace Nop.Plugin.Widgets.Campgrounds.Models
{
    public partial class CampgroundSearchModel : BaseNopModel
    {
        public CampgroundSearchModel()
        {
            CampgroundOverview = new List<CampgroundOverviewModel>();
            AvailableStateCategories = new List<SelectListItem>();
            AvailableCampgroundTypes = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Admin.Campgrounds.List.SearchCampgroundName")]
        public string SearchCampgroundName { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.List.SearchCampgroundCity")]
        public string SearchCampgroundCity { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.List.SearchCategory")]
        public int SearchStateId { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.List.SearchCampgroundType")]
        public int SearchCampgroundTypeId { get; set; }

        public bool IsLoggedInAsCampgroundHost { get; set; }

        public bool AllowCampgroundHostsToImportCampgrounds { get; set; }

        public IList<CampgroundOverviewModel> CampgroundOverview { get; set; }
        public IList<SelectListItem> AvailableStateCategories { get; set; }
        public IList<SelectListItem> AvailableCampgroundTypes { get; set; }
    }
}