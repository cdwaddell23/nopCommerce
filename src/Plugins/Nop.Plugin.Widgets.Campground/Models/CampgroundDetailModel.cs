using System;
using System.Collections.Generic;
using Nop.Plugin.Campgrounds.Data.Domain;
using Nop.Web.Models.Media;
using Nop.Web.Framework.Mvc.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Nop.Plugin.Widgets.Campgrounds.Models
{
     public partial class CampgroundDetailModel : BaseNopEntityModel
    {
        public CampgroundDetailModel()
        {
            DefaultPictureModel = new PictureModel();
            FeaturedCampground = new List<CampgroundOverviewModel>();
            SubCampgrounds = new List<SubCampgroundDetailModel>();
            CampgroundBreadcrumb = new List<CampgroundDetailModel>();
            NearbyCampgrounds = new List<CampgroundDetailModel>();
            CampgroundReviewOverviewModel = new CampgroundReviewOverviewModel();
            CampgroundReviewHelpfulnessModel = new CampgroundReviewHelpfulnessModel();
            CampgroundAddress = new CampgroundAddress();
        }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.Name")]
        public string Name { get; set; }
        [NopResourceDisplayName("Admin.Campgrounds.Fields.ShortDescription")]
        public string Description { get; set; }
        [NopResourceDisplayName("Admin.Campgrounds.Fields.FullDescription")]
        public string FullDescription { get; set; }
        [NopResourceDisplayName("Admin.Campgrounds.Fields.Phone")]
        public string Phone { get; set; }
        [NopResourceDisplayName("Admin.Campgrounds.Fields.Website")]
        public string Website { get; set; }
        [NopResourceDisplayName("Admin.Campgrounds.Fields.AvailableCampsites")]
        public int AvailableCampsites { get; set; }
        [NopResourceDisplayName("Admin.Campgrounds.Fields.MetaKeywords")]
        public string MetaKeywords { get; set; }
        [NopResourceDisplayName("Admin.Campgrounds.Fields.MetaDescription")]
        public string MetaDescription { get; set; }
        [NopResourceDisplayName("Admin.Campgrounds.Fields.MetaTitle")]
        public string MetaTitle { get; set; }
        [NopResourceDisplayName("Admin.Campgrounds.Fields.SeName")]
        public string SeName { get; set; }
        [NopResourceDisplayName("Admin.Campgrounds.Fields.CampgroundSeName")]
        public string CampgroundSeName { get; set; }
        public int ParentCategoryId { get; set; }
        public DateTime? MarkAsNewStartDate { get; set; }
        public decimal Distance { get; set; }
        public bool RegistrationsEnabled { get; set; }
        public bool ShowReviewRatingFrame { get; set; }

        public PictureModel DefaultPictureModel { get; set; }
        public bool DisplayCategoryBreadcrumb { get; set; }
        public IList<CampgroundDetailModel> CampgroundBreadcrumb { get; set; }
        public IList<CampgroundDetailModel> NearbyCampgrounds { get; set; }
        public IList<SubCampgroundDetailModel> SubCampgrounds { get; set; }
        public IList<CampgroundOverviewModel> FeaturedCampground { get; set; }
        public CampgroundReviewOverviewModel CampgroundReviewOverviewModel { get; set; }
        public CampgroundReviewHelpfulnessModel CampgroundReviewHelpfulnessModel { get; set; }
        public CampgroundAddress CampgroundAddress { get; set; }
        public CampgroundPagingFilteringModel PagingFilteringContext { get; set; }

        #region Nested Classes

        public partial class SubCampgroundDetailModel : BaseNopEntityModel
        {
            public SubCampgroundDetailModel()
            {
                PictureModel = new PictureModel();
            }

            public string Name { get; set; }

            public string SeName { get; set; }

            public string Description { get; set; }

            public PictureModel PictureModel { get; set; }
        }

        #endregion
    }

}