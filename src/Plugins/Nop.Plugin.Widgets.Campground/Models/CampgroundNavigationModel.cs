using System.Collections.Generic;
using Nop.Web.Framework.Mvc.Models;
using Nop.Web.Models.Catalog;

namespace Nop.Plugin.Widgets.Campgrounds.Models
{
    public partial class CampgroundNavigationModel : BaseNopModel
    {
        public CampgroundNavigationModel()
        {
            Categories = new List<CampgroundSimpleModel>();
        }

        public int CurrentCategoryId { get; set; }
        public List<CampgroundSimpleModel> Categories { get; set; }

        #region Nested classes

        public class CategoryLineModel : BaseNopModel
        {
            public int CurrentCategoryId { get; set; }
            public CampgroundSimpleModel Category { get; set; }
        }

        #endregion
    }
}