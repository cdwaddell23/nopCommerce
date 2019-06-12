using System;
using System.Collections.Generic;
using Nop.Plugin.Campgrounds.Data.Domain;
using Nop.Web.Models.Media;
using Nop.Web.Framework.Mvc.Models;

namespace Nop.Plugin.Widgets.Campgrounds.Models
{
    public partial class CampgroundOverviewModel : BaseNopEntityModel
    {
        public CampgroundOverviewModel()
        {
            Campgrounds = new List<CampgroundDetailModel>();
            CampgroundPrice = new CampgroundPriceModel();
            CampgroundCategory = new CampgroundCategoryModel();
            DefaultPictureModel = new PictureModel();
            PagingFilteringContext = new CampgroundPagingFilteringModel();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public string SeName { get; set; }

        public bool MarkAsNew { get; set; }
        public int ParentCategoryId { get; set; }

        public int NumberOfCampgrounds { get; set; }
        
        //campground class
        public CampgroundClass CampgroundClass { get; set; }
        //campgrounds
        public IList<CampgroundDetailModel> Campgrounds { get; set; }
        //price
        public CampgroundPriceModel CampgroundPrice { get; set; }
        //state campgrounds
        public CampgroundCategoryModel CampgroundCategory { get; set; }
        //picture
        public PictureModel DefaultPictureModel { get; set; }

        public CampgroundPagingFilteringModel PagingFilteringContext { get; set; }

        #region Nested Classes

        public partial class CampgroundPriceModel : BaseNopModel
        {
            public string OldPrice { get; set; }
            public string Price { get; set; }
            public decimal PriceValue { get; set; }
            public bool DisableBuyButton { get; set; }
            public bool DisableWishlistButton { get; set; }
            public bool DisableAddToCompareListButton { get; set; }

            public bool AvailableForPreOrder { get; set; }
            public DateTime? PreOrderAvailabilityStartDateTimeUtc { get; set; }

            public bool IsRental { get; set; }

            public bool ForceRedirectionAfterAddingToCart { get; set; }

            /// <summary>
            /// A value indicating whether we should display tax/shipping info (used in Germany)
            /// </summary>
            public bool DisplayTaxShippingInfo { get; set; }
        }

        #endregion
    }
}