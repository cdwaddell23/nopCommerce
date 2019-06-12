using Nop.Plugin.Campgrounds.Data.Domain;
using Nop.Web.Framework.Mvc.Models;
using System.Collections.Generic;

namespace Nop.Plugin.Widgets.Campgrounds.Models
{
    public class CampgroundCategoryModel : BaseNopModel
    {
        public CampgroundCategoryModel()
        {
            SubCategories = new List<CampgroundCategoryModel>();
            //Campgrounds = new List<Campground>();
        }

        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public string SeName { get; set; }
        public string Name { get; set; }
        public int? NumberOfCampgrounds { get; set; }
        public bool IncludeInTopMenu { get; set; }
        public bool SuppressNearby { get; set; }
        public int Level { get; set; }
        public IList<CampgroundCategoryModel> SubCategories { get; set; }
        //public IList<Campground> Campgrounds { get; set; }
    }
}
