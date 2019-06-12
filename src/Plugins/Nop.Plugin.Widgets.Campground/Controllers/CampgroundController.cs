using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Media;
using Nop.Core.Infrastructure;
using Nop.Plugin.Campgrounds.Data.Domain;
using Nop.Plugin.Campgrounds.Services;
using Nop.Plugin.Widgets.Campgrounds.Factories;
using Nop.Plugin.Widgets.Campgrounds.Models;
using Nop.Plugin.Widgets.Campgrounds.Helpers;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Events;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.Stores;
using Nop.Web.Factories;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Framework.Security;
using Nop.Web.Framework.Security.Captcha;
using System;
using System.Linq;
using Nop.Core.Caching;
using System.Collections.Generic;
using Nop.Plugin.Widgets.Campgrounds.Infrastructure.Cache;
using Nop.Web.Framework;

namespace Nop.Plugin.Widgets.Campgrounds.Controllers
{
    public partial class CampgroundController : BasePluginController
    {
        const string VIEW_CAMPGROUND_DETAIL = "~/Plugins/Campgrounds/Views/Campground/CampgroundDetail.cshtml";
        const string VIEW_CAMPGROUND_NEARBY = "~/Plugins/Campgrounds/Views/Shared/Components/CampingNearby/Default.cshtml";
        const string VIEW_CAMPGROUNDS = "~/Plugins/Campgrounds/Views/Campground/Campgrounds.cshtml";
        const string VIEW_CAMPGROUND_OFFLINE = "~/Plugins/Campgrounds/Views/Campground/Offline.cshtml";
        const string VIEW_CAMPGROUND_SEARCH = "~/Plugins/Campgrounds/Views/Campground/CampgroundSummary.InGridOrLines.cshtml";
        const string VIEW_CAMPGROUND_REVIEW = "~/Plugins/Campgrounds/Views/Campground/CampgroundReviews.cshtml";

        #region Fields

        private readonly ICatalogModelFactory _catalogModelFactory;
        private readonly ICampgroundModelFactory _campgroundModelFactory;
        private readonly ICampgroundTypeService _campgroundTypeService;
        private readonly ICategoryService _categoryService;
        private readonly ICampgroundService _campgroundService;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly ILocalizationService _localizationService;
        private readonly IWebHelper _webHelper;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IAclService _aclService;
        private readonly IStoreMappingService _storeMappingService;
        private readonly IPermissionService _permissionService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly CampgroundMediaSettings _mediaSettings;
        private readonly CatalogSettings _catalogSettings;
        private readonly CampgroundSettings _campgroundSettings;
        private readonly CaptchaSettings _captchaSettings;
        private readonly IStaticCacheManager _cacheManager;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly IEventPublisher _eventPublisher;
        private readonly LocalizationSettings _localizationSettings;

        #endregion

        #region Ctor

        public CampgroundController(ICatalogModelFactory catalogModelFactory,
            ICampgroundModelFactory campgroundModelFactory,
            ICampgroundTypeService campgroundTypeService,
            ICategoryService categoryService,
            ICampgroundService campgroundService,
            IWorkContext workContext,
            IStoreContext storeContext,
            ILocalizationService localizationService,
            IWebHelper webHelper,
            IGenericAttributeService genericAttributeService,
            IAclService aclService,
            IStoreMappingService storeMappingService,
            IPermissionService permissionService,
            ICustomerActivityService customerActivityService,
            CampgroundMediaSettings mediaSettings,
            CatalogSettings catalogSettings,
            CampgroundSettings campgroundSettings,
            CaptchaSettings captchaSettings,
            IStaticCacheManager cacheManager,
            IWorkflowMessageService workflowMessageService,
            IEventPublisher eventPublisher,
            LocalizationSettings localizationSettings)
        {
            this._catalogModelFactory = catalogModelFactory;
            this._campgroundModelFactory = campgroundModelFactory;
            this._campgroundTypeService = campgroundTypeService;
            this._categoryService = categoryService;
            this._campgroundService = campgroundService;
            this._workContext = workContext;
            this._storeContext = storeContext;
            this._localizationService = localizationService;
            this._webHelper = webHelper;
            this._genericAttributeService = genericAttributeService;
            this._aclService = aclService;
            this._storeMappingService = storeMappingService;
            this._permissionService = permissionService;
            this._customerActivityService = customerActivityService;
            this._mediaSettings = mediaSettings;
            this._catalogSettings = catalogSettings;
            this._campgroundSettings = campgroundSettings;
            this._captchaSettings = captchaSettings;
            this._cacheManager = cacheManager;
            this._localizationSettings = localizationSettings;
            this._workflowMessageService = workflowMessageService;
            this._eventPublisher = eventPublisher;
        }

        #endregion

        #region Utility
        protected virtual IActionResult InvokeHttp404()
        {
            Response.StatusCode = 404;
            return new EmptyResult();
        }

        protected virtual IList<SelectListItem> PrepareAvailableCampgroundTypes(bool showHidden = false)
        {
            var AvailableCampgroundTypes = new List<SelectListItem>
            {
                new SelectListItem { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" }
            };
            var campgroundTypes = SelectListHelper.GetCampgroundTypeList(_campgroundTypeService, _cacheManager, showHidden);
            foreach (var ct in campgroundTypes)
                AvailableCampgroundTypes.Add(ct);

            return AvailableCampgroundTypes;
        }

        #endregion

        #region Campgrounds

        [HttpsRequirement(SslRequirement.No)]
        public virtual IActionResult Campgrounds()
        {
            var cacheKey = string.Format(ModelCacheEventConsumer.CAMPGROUND_MENU_MODEL_KEY, _storeContext.CurrentStore.Id);
            var model = _cacheManager.Get(cacheKey, () => _campgroundModelFactory.PrepareCampgroundCategoryModels(2));

            return View(VIEW_CAMPGROUNDS, model);
        }

        [HttpsRequirement(SslRequirement.No)]
        public virtual IActionResult Offline()
        {
            return View(VIEW_CAMPGROUND_OFFLINE);
        }

        [HttpsRequirement(SslRequirement.No)]
        public virtual IActionResult CampgroundDetail(string campgroundSeName, CampgroundPagingFilteringModel command)
        {
            //performance optimization, we load a cached verion here. It reduces number of SQL requests for each page load
            var urlRecordService = EngineContext.Current.Resolve<IUrlRecordService>();
            var urlRecord = urlRecordService.GetBySlugCached(campgroundSeName);

            var campground = urlRecord != null ? _campgroundService.GetCampgroundById(urlRecord.EntityId) : null;
            if (campground == null || campground.Deleted)
                return InvokeHttp404();

            //display "edit" (manage) link
            if (_permissionService.Authorize(StandardPermissionProvider.AccessAdminPanel) && _permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                DisplayEditLink(Url.Action("Edit", "Campground", new { id = campground.Id, area = AreaNames.Admin }));

            //activity log
            _customerActivityService.InsertActivity("PublicStore.ViewCampground", _localizationService.GetResource("ActivityLog.PublicStore.ViewCategory"), campground.Name);

            //model
            var model = _campgroundModelFactory.PrepareCampgroundDetailModel(campground, command);

            model.ParentCategoryId = campground.CampgroundCategories != null ? campground.CampgroundCategories.FirstOrDefault().CategoryId : 0;

            return View(VIEW_CAMPGROUND_DETAIL, model);
        }

        [HttpsRequirement(SslRequirement.No)]
        public virtual IActionResult CampgroundNearby(CampgroundPagingFilteringModel command)
        {
            var model = new CampgroundAddressModel();

            return View(VIEW_CAMPGROUND_NEARBY, model);
        }


        [HttpsRequirement(SslRequirement.No)]
        [HttpPost, ActionName("CampgroundNearby")]
        [PublicAntiForgery]
        public virtual IActionResult CampgroundNearbySubmit(CampgroundAddressModel campgroundAddressModel, CampgroundPagingFilteringModel command)
        {
            var detailModel = new CampgroundDetailModel
            {
                Id = 0,
                Name = "Camping near my location",
                Description = "Based on your current location we have found the following camping areas",
                MetaDescription = "Based on your current location we have found the following camping areas",
                MetaTitle = "Camping near my location",
                MetaKeywords = "rv camping nearby, car camping nearby, tent campsites nearby, boondocking nearby",
                CampgroundAddress = new CampgroundAddress
                {
                    Latitude = campgroundAddressModel.Latitude,
                    Longitude = campgroundAddressModel.Longitude
                }
            };

            //activity log
            _customerActivityService.InsertActivity("PublicStore.ViewNearbyCampgrounds", _localizationService.GetResource("ActivityLog.PublicStore.ViewCategory"));

            //model
            var model = _campgroundModelFactory.PrepareCampingNearby(campgroundAddressModel.Latitude.GetValueOrDefault(), campgroundAddressModel.Longitude.GetValueOrDefault(), command, campgroundDetail: detailModel);

            return View(VIEW_CAMPGROUND_DETAIL, model);
        }

        #endregion

        #region Campground reviews

        [HttpGet("camping/{SeName}/{campgroundSeName}/reviews/{campgroundId?}")]
        [HttpsRequirement(SslRequirement.No)]
        public virtual IActionResult CampgroundReviews(string SeName, string campgroundSeName, int campgroundId = 0)
        {
            if (campgroundId == 0)
            {
                //performance optimization, we load a cached verion here. It reduces number of SQL requests for each page load
                var urlRecordService = EngineContext.Current.Resolve<IUrlRecordService>();
                var urlRecord = urlRecordService.GetBySlugCached(campgroundSeName);

                return CampgroundReviews(urlRecord.EntityId);
            }
            else
                return CampgroundReviews(campgroundId);

        }

        [HttpsRequirement(SslRequirement.No)]
        public virtual IActionResult CampgroundReviews(int campgroundId)
        {
            var campground = _campgroundService.GetCampgroundById(campgroundId);
            if (campground == null || campground.Deleted || !campground.Published || !campground.AllowCampgroundReviews)
                return RedirectToRoute("HomePage");

            var model = new CampgroundReviewsModel();
            model = _campgroundModelFactory.PrepareCampgroundReviewsModel(model, campground);
            //only registered users can leave reviews
            if (_workContext.CurrentCustomer.IsGuest() && !_campgroundSettings.AllowAnonymousUsersToReviewCampground)
                ModelState.AddModelError("", _localizationService.GetResource("Reviews.OnlyRegisteredUsersCanWriteReviews"));

            //default value
            model.AddCampgroundReview.Rating = _campgroundSettings.DefaultCampgroundRatingValue;
            return View(VIEW_CAMPGROUND_REVIEW, model);
        }

        [HttpPost, ActionName("CampgroundReviews")]
        [PublicAntiForgery]
        [FormValueRequired("add-review")]
        [ValidateCaptcha]
        public virtual IActionResult CampgroundReviewsAdd(string SeName, string campgroundSeName, int campgroundId, CampgroundReviewsModel model, bool captchaValid)
        {
            if (campgroundId == 0)
            {
                var urlRecordService = EngineContext.Current.Resolve<IUrlRecordService>();
                var urlRecord = urlRecordService.GetBySlugCached(campgroundSeName);
                campgroundId = urlRecord.EntityId;
            }

            var campground = _campgroundService.GetCampgroundById(campgroundId);
            if (campground == null || campground.Deleted || !campground.Published || !campground.AllowCampgroundReviews)
                return RedirectToRoute("HomePage");

            //validate CAPTCHA
            if (_captchaSettings.Enabled && !captchaValid) // && _captchaSettings.ShowOnCampgroundReviewPage)
            {
                ModelState.AddModelError("", _captchaSettings.GetWrongCaptchaMessage(_localizationService));
            }

            if (_workContext.CurrentCustomer.IsGuest() && !_campgroundSettings.AllowAnonymousUsersToReviewCampground)
            {
                ModelState.AddModelError("", _localizationService.GetResource("Reviews.OnlyRegisteredUsersCanWriteReviews"));
            }

            if (ModelState.IsValid)
            {
                //save review
                var rating = model.AddCampgroundReview.Rating;
                if (rating < 1 || rating > 5)
                    rating = _campgroundSettings.DefaultCampgroundRatingValue;
                var isApproved = !_campgroundSettings.CampgroundReviewsMustBeApproved;

                var campgroundReview = new CampgroundReview
                {
                    CampgroundId = campground.Id,
                    CustomerId = _workContext.CurrentCustomer.Id,
                    Title = model.AddCampgroundReview.Title,
                    ReviewText = model.AddCampgroundReview.ReviewText,
                    Rating = rating,
                    HelpfulYesTotal = 0,
                    HelpfulNoTotal = 0,
                    IsApproved = isApproved,
                    CreatedOnUtc = DateTime.UtcNow,
                    StoreId = _storeContext.CurrentStore.Id,
                };
                campground.CampgroundReviews.Add(campgroundReview);
                _campgroundService.UpdateCampground(campground);

                //update campground totals
                _campgroundService.UpdateCampgroundReviewTotals(campground);

                //notify store owner
                //if (_campgroundSettings.NotifyStoreOwnerAboutNewCampgroundReviews)
                //    _workflowMessageService.SendCampgroundReviewNotificationMessage(campgroundReview, _localizationSettings.DefaultAdminLanguageId);

                //activity log
                _customerActivityService.InsertActivity("PublicStore.AddCampgroundReview", _localizationService.GetResource("ActivityLog.PublicStore.AddCampgroundReview"), campground.Name);

                //raise event
                if (campgroundReview.IsApproved)
                    _eventPublisher.Publish(new CampgroundReviewApprovedEvent(campgroundReview));

                model = _campgroundModelFactory.PrepareCampgroundReviewsModel(model, campground);
                model.AddCampgroundReview.Title = null;
                model.AddCampgroundReview.ReviewText = null;

                model.AddCampgroundReview.SuccessfullyAdded = true;
                if (!isApproved)
                    model.AddCampgroundReview.Result = _localizationService.GetResource("Reviews.Campground.SeeAfterApproving");
                else
                    model.AddCampgroundReview.Result = _localizationService.GetResource("Reviews.Campground.SuccessfullyAdded");

                return View(VIEW_CAMPGROUND_REVIEW, model);
            }

            //If we got this far, something failed, redisplay form
            model = _campgroundModelFactory.PrepareCampgroundReviewsModel(model, campground);
            return View(VIEW_CAMPGROUND_REVIEW, model);
        }

        [HttpPost]
        public virtual IActionResult SetCampgroundReviewHelpfulness(int campgroundReviewId, bool washelpful)
        {
            var campgroundReview = _campgroundService.GetCampgroundReviewById(campgroundReviewId);
            if (campgroundReview == null)
                throw new ArgumentException("No campground review found with the specified id");

            if (_workContext.CurrentCustomer.IsGuest() && !_campgroundSettings.AllowAnonymousUsersToReviewCampground)
            {
                return Json(new
                {
                    Result = _localizationService.GetResource("Reviews.Helpfulness.OnlyRegistered"),
                    TotalYes = campgroundReview.HelpfulYesTotal,
                    TotalNo = campgroundReview.HelpfulNoTotal
                });
            }

            //customers aren't allowed to vote for their own reviews
            if (campgroundReview.CustomerId == _workContext.CurrentCustomer.Id)
            {
                return Json(new
                {
                    Result = _localizationService.GetResource("Reviews.Helpfulness.YourOwnReview"),
                    TotalYes = campgroundReview.HelpfulYesTotal,
                    TotalNo = campgroundReview.HelpfulNoTotal
                });
            }

            //delete previous helpfulness
            var prh = campgroundReview.CampgroundReviewHelpfulnessEntries.FirstOrDefault(x => x.CustomerId == _workContext.CurrentCustomer.Id);
            if (prh != null)
            {
                //existing one
                prh.WasHelpful = washelpful;
            }
            else
            {
                //insert new helpfulness
                prh = new CampgroundReviewHelpfulness
                {
                    CampgroundReviewId = campgroundReview.Id,
                    CustomerId = _workContext.CurrentCustomer.Id,
                    WasHelpful = washelpful,
                };
                campgroundReview.CampgroundReviewHelpfulnessEntries.Add(prh);
            }
            _campgroundService.UpdateCampground(campgroundReview.Campground);

            //new totals
            campgroundReview.HelpfulYesTotal = campgroundReview.CampgroundReviewHelpfulnessEntries.Count(x => x.WasHelpful);
            campgroundReview.HelpfulNoTotal = campgroundReview.CampgroundReviewHelpfulnessEntries.Count(x => !x.WasHelpful);
            _campgroundService.UpdateCampground(campgroundReview.Campground);

            return Json(new
            {
                Result = _localizationService.GetResource("Reviews.Helpfulness.SuccessfullyVoted"),
                TotalYes = campgroundReview.HelpfulYesTotal,
                TotalNo = campgroundReview.HelpfulNoTotal
            });
        }

        public virtual IActionResult CustomerCampgroundReviews(int? pageNumber)
        {
            if (_workContext.CurrentCustomer.IsGuest())
                return Challenge();

            if (!_campgroundSettings.ShowCampgroundReviewsTabOnAccountPage)
            {
                return RedirectToRoute("CustomerInfo");
            }

            var model = _campgroundModelFactory.PrepareCustomerCampgroundReviewsModel(pageNumber);
            return View(VIEW_CAMPGROUND_REVIEW, model);
        }

        #endregion

        //#region Campground tags

        //[HttpsRequirement(SslRequirement.No)]
        //public virtual IActionResult CampgroundsByTag(int campgroundTagId, CampgroundPagingFilteringModel command)
        //{
        //    var campgroundTag = _campgroundTagService.GetCampgroundTagById(campgroundTagId);
        //    if (campgroundTag == null)
        //        return InvokeHttp404();

        //    var model = _campgroundModelFactory.PrepareCampgroundsByTagModel(campgroundTag, command);
        //    return View(model);
        //}

        //[HttpsRequirement(SslRequirement.No)]
        //public virtual IActionResult CampgroundTagsAll()
        //{
        //    var model = _campgroundModelFactory.PrepareCampgroundTagsAllModel();
        //    return View(model);
        //}

        //#endregion

        #region Searching)

        [HttpsRequirement(SslRequirement.No)]
        //[HttpPost, ActionName("SearchCampgrounds")]
        //[PublicAntiForgery]
        public virtual IActionResult SearchCampgrounds(string sTerms, int StateProvinceId = 0, string SeName = null)
        {
            Category category = null;
            if (!string.IsNullOrEmpty(SeName) && StateProvinceId == 0)
            {
                var urlRecordService = EngineContext.Current.Resolve<IUrlRecordService>();
                var urlRecord = urlRecordService.GetBySlugCached(SeName);
                category = _categoryService.GetCategoryById(_campgroundService.GetCampgroundCategoryIdFromStateId(urlRecord.EntityId));
            }
            else
            {
                category = StateProvinceId > 0 ? _categoryService.GetCategoryById(_campgroundService.GetCampgroundCategoryIdFromStateId(StateProvinceId)) : null;
            }

            var campgrounds = _campgroundService.SearchCampgrounds(
                keywords: sTerms,
                categoryIds: category != null && category.Id > 2 ? new List<int>(new int[] { category.Id }) : null,
                languageId: _workContext.WorkingLanguage.Id,
                searchCampgroundTags: true,
                searchDescriptions: true);

            var model = new CampgroundOverviewModel
            {
                Id = category != null ? category.Id : 0,
                Name = "Search for " + sTerms,
                Description = category != null ? category.GetLocalized(x => x.Description) : "Search for " + sTerms,
                MetaKeywords = category != null ? category.GetLocalized(x => x.MetaKeywords) : string.Empty,
                MetaDescription = category != null ? category.GetLocalized(x => x.MetaDescription) : "Campground Search - " + sTerms,
                MetaTitle = category != null ? category.GetLocalized(x => x.MetaTitle) : "Campground Search - " + sTerms,
                SeName = category != null ? SeoExtensions.GetSeName(category.Id, "Campgrounds", 0) : string.Empty,
                DefaultPictureModel = category != null && category.PictureId != 0 ? _campgroundModelFactory.PrepareCampgroundDefaultPictureModel(category) : null,
                ParentCategoryId = campgrounds.Count() == 0 && category != null ? category.ParentCategoryId : -1, //Set to negative number to enable title
                NumberOfCampgrounds = campgrounds.Count()
            };

            model.Campgrounds = _campgroundModelFactory.PrepareCampgroundDetailModels(campgrounds).ToList();

            sTerms = string.Empty;

            return View(VIEW_CAMPGROUND_SEARCH, model);
        }
        public virtual IActionResult SearchTermAutoComplete(string term)
        {
            if (string.IsNullOrWhiteSpace(term) || term.Length < _campgroundSettings.CampgroundSearchTermMinimumLength)
                return Content("");

            //campgrounds
            var campgroundNumber = _campgroundSettings.CampgroundSearchAutoCompleteNumberOfCampgrounds > 0 ?
                _campgroundSettings.CampgroundSearchAutoCompleteNumberOfCampgrounds : 10;

            var campgrounds = _campgroundService.SearchCampgrounds(
                keywords: term,
                languageId: _workContext.WorkingLanguage.Id,
                pageSize: campgroundNumber);

            var models = _campgroundModelFactory.PrepareCampgroundDetailModels(campgrounds).ToList();
            var result = (from c in models
                          select new
                          {
                              label = c.Name,
                              campgroundurl = Url.RouteUrl("Campground", new { c.SeName }),
                              campgroundpictureurl = c.DefaultPictureModel.ThumbImageUrl
                          })
                .ToList();
            return Json(result);
        }

        #endregion
    }
}