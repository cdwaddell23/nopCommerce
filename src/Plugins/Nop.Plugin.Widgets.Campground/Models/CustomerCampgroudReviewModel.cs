using System.Collections.Generic;
using Nop.Web.Framework.Mvc.Models;
using Nop.Web.Models.Common;

namespace Nop.Plugin.Widgets.Campgrounds.Models
{

    public class CustomerCampgroundReviewModel : BaseNopModel
    {
        public int CampgroundId { get; set; }
        public string CampgroundName { get; set; }
        public string CampgroundSeName { get; set; }
        public string Title { get; set; }
        public string ReviewText { get; set; }
        public string ReplyText { get; set; }
        public int Rating { get; set; }
        public string WrittenOnStr { get; set; }
        public string ApprovalStatus { get; set; }
    }

    public class CustomerCampgroundReviewsModel : BaseNopModel
    {
        public CustomerCampgroundReviewsModel()
        {
            CampgroundReviews = new List<CustomerCampgroundReviewModel>();
        }

        public IList<CustomerCampgroundReviewModel> CampgroundReviews { get; set; }
        public PagerModel PagerModel { get; set; }

        #region Nested class

        /// <summary>
        /// Class that has only page for route value. Used for (My Account) My Campground Reviews pagination
        /// </summary>
        public partial class CustomerCampgroundReviewsRouteValues : IRouteValues
        {
            public int pageNumber { get; set; }
        }

        #endregion
    }
}