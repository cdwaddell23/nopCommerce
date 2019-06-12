using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Plugin.Campgrounds.Data.Domain;
using Nop.Plugin.Widgets.Campgrounds.Validators;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Mvc.Models;
using Nop.Web.Models.Media;

namespace Nop.Plugin.Widgets.Campgrounds.Models
{
    public partial class CampgroundReviewOverviewModel : BaseNopModel
    {
        public int CampgroundId { get; set; }

        public int RatingSum { get; set; }

        public int TotalReviews { get; set; }

        public bool AllowCampgroundReviews { get; set; }
    }

    [Validator(typeof(CampgroundReviewsValidator))]
    public partial class CampgroundReviewsModel : BaseNopModel
    {
        public CampgroundReviewsModel()
        {
            Items = new List<CampgroundReviewModel>();
            AddCampgroundReview = new AddCampgroundReviewModel();
        }
        public int CampgroundId { get; set; }

        public string CampgroundName { get; set; }

        public string Description { get; set; }

        public string CampgroundSeName { get; set; }

        public string SeName { get; set; }

        public bool IsLoggedInAsHost { get; set; }

        public IList<CampgroundReviewModel> Items { get; set; }
        public AddCampgroundReviewModel AddCampgroundReview { get; set; }
    }

    public partial class CampgroundReviewModel : BaseNopEntityModel
    {
        public int CustomerId { get; set; }

        public string CustomerName { get; set; }

        public bool AllowViewingProfiles { get; set; }
        
        public string Title { get; set; }

        public string ReviewText { get; set; }

        public string ReplyText { get; set; }

        public int Rating { get; set; }

        public IList<PictureModel> Pictures { get; set; }

        public CampgroundReviewHelpfulnessModel Helpfulness { get; set; }

        public string WrittenOnStr { get; set; }
    }

    public partial class CampgroundReviewHelpfulnessModel : BaseNopModel
    {
        public int CampgroundReviewId { get; set; }

        public int HelpfulYesTotal { get; set; }

        public int HelpfulNoTotal { get; set; }
    }

    public partial class AddCampgroundReviewModel : BaseNopModel
    {
        public AddCampgroundReviewModel()
        {
            Pictures = new List<PictureModel>();
        }

        [NopResourceDisplayName("Reviews.Fields.Title")]
        public string Title { get; set; }
        
        [NopResourceDisplayName("Reviews.Fields.ReviewText")]
        public string ReviewText { get; set; }

        [NopResourceDisplayName("Reviews.Fields.Rating")]
        public int Rating { get; set; }
        public IList<PictureModel> Pictures { get; set; }
        public bool DisplayCaptcha { get; set; }
        public bool CanCurrentCustomerLeaveReview { get; set; }
        public bool SuccessfullyAdded { get; set; }
        public string Result { get; set; }
    }

    public partial class CampgroundReviewRatingModel: BaseNopModel
    {
        public CampgroundReviewRatingModel()
        {
            Pictures = new List<PictureModel>();
        }
        public int CustomerId { get; set; }
        public int CampgroundId { get; set; }
        public int Rating { get; set; }
        public string Title { get; set; }
        public string ReviewText { get; set; }
        public IList<PictureModel> Pictures { get; set; }
    }

    [Validator(typeof(CampgroundReviewAdminModel))]
    public partial class CampgroundReviewAdminModel : BaseNopEntityModel
    {
        [NopResourceDisplayName("Admin.CampgroundReviews.Fields.Campground")]
        public int CampgroundId { get; set; }
        [NopResourceDisplayName("Admin.CampgroundReviews.Fields.Campground")]
        public string CampgroundName { get; set; }

        [NopResourceDisplayName("Admin.CampgroundReviews.Fields.Customer")]
        public int CustomerId { get; set; }
        [NopResourceDisplayName("Admin.CampgroundReviews.Fields.Customer")]
        public string CustomerInfo { get; set; }

        [NopResourceDisplayName("Admin.CampgroundReviews.Fields.Title")]
        public string Title { get; set; }

        [NopResourceDisplayName("Admin.CampgroundReviews.Fields.ReviewText")]
        public string ReviewText { get; set; }

        [NopResourceDisplayName("Admin.CampgroundReviews.Fields.ReplyText")]
        public string ReplyText { get; set; }

        [NopResourceDisplayName("Admin.CampgroundReviews.Fields.Rating")]
        public int Rating { get; set; }

        [NopResourceDisplayName("Admin.CampgroundReviews.Fields.IsApproved")]
        public bool IsApproved { get; set; }

        [NopResourceDisplayName("Admin.CampgroundReviews.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        //host
        public bool IsLoggedInAsHost { get; set; }
    }

    public partial class CampgroundReviewAdminListModel : BaseNopModel
    {
        public CampgroundReviewAdminListModel()
        {
            AvailableStores = new List<SelectListItem>();
            AvailableApprovedOptions = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Admin.CampgroundReviews.List.CreatedOnFrom")]
        [UIHint("DateNullable")]
        public DateTime? CreatedOnFrom { get; set; }

        [NopResourceDisplayName("Admin.CampgroundReviews.List.CreatedOnTo")]
        [UIHint("DateNullable")]
        public DateTime? CreatedOnTo { get; set; }

        [NopResourceDisplayName("Admin.CampgroundReviews.List.SearchText")]
        public string SearchText { get; set; }

        [NopResourceDisplayName("Admin.CampgroundReviews.List.SearchCampground")]
        public int SearchCampgroundId { get; set; }

        [NopResourceDisplayName("Admin.CampgroundReviews.List.SearchApproved")]
        public int SearchApprovedId { get; set; }

        //host
        public bool IsLoggedInAsHost { get; set; }

        public IList<SelectListItem> AvailableStores { get; set; }
        public IList<SelectListItem> AvailableApprovedOptions { get; set; }
    }
}