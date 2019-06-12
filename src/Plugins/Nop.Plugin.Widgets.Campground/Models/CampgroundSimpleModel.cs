using System.Collections.Generic;
using Nop.Web.Framework.Mvc.Models;

namespace Nop.Plugin.Widgets.Campgrounds.Models
{
    public class CampgroundSimpleModel : BaseNopEntityModel
    {
        public CampgroundSimpleModel()
        {
            SubCategories = new List<CampgroundSimpleModel>();
        }

        public string Name { get; set; }

        public string SeName { get; set; }

        public int? NumberOfSites { get; set; }

        public bool IncludeInTopMenu { get; set; }

        public List<CampgroundSimpleModel> SubCategories { get; set; }
    }
}