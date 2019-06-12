using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Mvc.Models;
using System.Collections.Generic;

namespace Nop.Plugin.Widgets.Campgrounds.Models
{
    public partial class CampgroundSearchBoxModel : BaseNopModel
    {
        public CampgroundSearchBoxModel()
        {
            AvailableStates = new List<SelectListItem>();
        }

        public bool AutoCompleteEnabled { get; set; }
        public bool ShowCampgroundImagesInSearchAutoComplete { get; set; }
        public int SearchTermMinimumLength { get; set; }
        public string sTerms { get; set; }
        [NopResourceDisplayName("Search.Fields.StateProvince")]
        public int? StateProvinceId { get; set; }
        public IList<SelectListItem> AvailableStates { get; set; }
    }
}