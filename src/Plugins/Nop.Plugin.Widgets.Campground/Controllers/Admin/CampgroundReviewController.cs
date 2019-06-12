using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Services.Events;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Security;
using Nop.Services.Stores;
using Nop.Web.Framework.Kendoui;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Plugin.Campgrounds.Services;
using Nop.Web.Areas.Admin.Controllers;
using Nop.Plugin.Widgets.Campgrounds.Models;
using Nop.Plugin.Campgrounds.Data.Domain;

namespace Nop.Plugin.Widgets.Campgrounds.Controllers.Admin
{
    public partial class CampgroundReviewController : BaseAdminController
    {
        #region CONST
        public const string LIST_VIEW = "~/Plugins/Campgrounds/Views/Admin/CampgroundReview/List.cshtml";
        public const string EDIT_VIEW = "~/Plugins/Campgrounds/Views/Admin/CampgroundReview/Edit.cshtml";
        #endregion

        #region Fields

        private readonly ICampgroundService _campgroundService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly IEventPublisher _eventPublisher;
        private readonly IStoreService _storeService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IWorkContext _workContext;

        #endregion Fields

        #region Ctor

        public CampgroundReviewController(ICampgroundService campgroundService, 
            IDateTimeHelper dateTimeHelper,
            ILocalizationService localizationService, 
            IPermissionService permissionService,
            IEventPublisher eventPublisher,
            IStoreService storeService,
            ICustomerActivityService customerActivityService,
            IWorkContext workContext)
        {
            this._campgroundService = campgroundService;
            this._dateTimeHelper = dateTimeHelper;
            this._localizationService = localizationService;
            this._permissionService = permissionService;
            this._eventPublisher = eventPublisher;
            this._storeService = storeService;
            this._customerActivityService = customerActivityService;
            this._workContext = workContext;
        }

        #endregion

        #region Utilities

        protected virtual void PrepareCampgroundReviewModel(CampgroundReviewAdminModel model,
            CampgroundReview campgroundReview, bool excludeProperties, bool formatReviewAndReplyText)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (campgroundReview == null)
                throw new ArgumentNullException(nameof(campgroundReview));

            model.Id = campgroundReview.Id;
            model.CampgroundId = campgroundReview.CampgroundId;
            model.CampgroundName = campgroundReview.Campground.Name;
            model.CustomerId = campgroundReview.CustomerId;
            var customer = campgroundReview.Customer;
            //model.CustomerInfo = customer.IsRegistered() ? customer.Email : _localizationService.GetResource("Admin.Customers.Guest");
            model.CustomerInfo = !string.IsNullOrEmpty(customer.Email) ? customer.Email : _localizationService.GetResource("Admin.Customers.Guest");
            model.Rating = campgroundReview.Rating;
            model.CreatedOn = _dateTimeHelper.ConvertToUserTime(campgroundReview.CreatedOnUtc, DateTimeKind.Utc);
            if (!excludeProperties)
            {
                model.Title = campgroundReview.Title;
                if (formatReviewAndReplyText)
                {
                    model.ReviewText = Core.Html.HtmlHelper.FormatText(campgroundReview.ReviewText, false, true, false, false, false, false);
                    model.ReplyText = Core.Html.HtmlHelper.FormatText(campgroundReview.ReplyText, false, true, false, false, false, false);
                }
                else
                {
                    model.ReviewText = campgroundReview.ReviewText;
                    model.ReplyText = campgroundReview.ReplyText;
                }
                model.IsApproved = campgroundReview.IsApproved;
            }

            //a host should have access only to their campgrounds
            model.IsLoggedInAsHost = false; // _workContext.CurrentHost != null;
        }

        #endregion

        #region Methods

        //list
        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedView();

            var model = new CampgroundReviewAdminListModel();
            //{
            //    //a host should have access only to his campgrounds
            //    IsLoggedInAsHost = _workContext.CurrentHost != null
            //};


            //"approved" property
            //0 - all
            //1 - approved only
            //2 - disapproved only
            model.AvailableApprovedOptions.Add(new SelectListItem { Text = _localizationService.GetResource("Admin.CampgroundReviews.List.SearchApproved.All"), Value = "0" });
            model.AvailableApprovedOptions.Add(new SelectListItem { Text = _localizationService.GetResource("Admin.CampgroundReviews.List.SearchApproved.ApprovedOnly"), Value = "1" });
            model.AvailableApprovedOptions.Add(new SelectListItem { Text = _localizationService.GetResource("Admin.CampgroundReviews.List.SearchApproved.DisapprovedOnly"), Value = "2" });

            return View(LIST_VIEW, model);
        }

        [HttpPost]
        public virtual IActionResult List(DataSourceRequest command, CampgroundReviewAdminListModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedKendoGridJson();

            //a host should have access only to his campgrounds
            //var hostId = 0;
            //if (_workContext.CurrentHost != null)
            //{
            //    hostId = _workContext.CurrentHost.Id;
            //}

            var createdOnFromValue = (model.CreatedOnFrom == null) ? null
                            : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.CreatedOnFrom.Value, _dateTimeHelper.CurrentTimeZone);

            var createdToFromValue = (model.CreatedOnTo == null) ? null
                            : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.CreatedOnTo.Value, _dateTimeHelper.CurrentTimeZone).AddDays(1);

            bool? approved = null;
            if (model.SearchApprovedId > 0)
                approved = model.SearchApprovedId == 1;

            var campgroundReviews = _campgroundService.GetAllCampgroundReviews(customerId: 0, approved: approved, 
                fromUtc: createdOnFromValue, toUtc: createdToFromValue, message: model.SearchText, 
                pageIndex: command.Page - 1, pageSize: command.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = campgroundReviews.Select(x =>
                {
                    var m = new CampgroundReviewAdminModel();
                    PrepareCampgroundReviewModel(m, x, false, true);
                    return m;
                }),
                Total = campgroundReviews.TotalCount
            };

            return Json(gridModel);
        }

        //edit
        public virtual IActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedView();

            var campgroundReview = _campgroundService.GetCampgroundReviewById(id);
            if (campgroundReview == null)
                //No campground review found with the specified id
                return RedirectToAction("List");

            //a host should have access only to his campgrounds
            //if (_workContext.CurrentHost != null && campgroundReview.Campground.HostId != _workContext.CurrentHost.Id)
            //    return RedirectToAction("List");

            var model = new CampgroundReviewAdminModel();
            PrepareCampgroundReviewModel(model, campgroundReview, false, false);
            return View(EDIT_VIEW, model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Edit(CampgroundReviewAdminModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedView();

            var campgroundReview = _campgroundService.GetCampgroundReviewById(model.Id);
            if (campgroundReview == null)
                //No campground review found with the specified id
                return RedirectToAction("List");

            //a host should have access only to his campgrounds
            //if (_workContext.CurrentHost != null && campgroundReview.Campground.HostId != _workContext.CurrentHost.Id)
            //    return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var isLoggedInAsHost = false; // _workContext.CurrentHost != null;

                var previousIsApproved = campgroundReview.IsApproved;
                //host can edit "Reply text" only
                if (!isLoggedInAsHost)
                {
                    campgroundReview.Title = model.Title;
                    campgroundReview.ReviewText = model.ReviewText;
                    campgroundReview.IsApproved = model.IsApproved;
                }

                campgroundReview.ReplyText = model.ReplyText;
                _campgroundService.UpdateCampground(campgroundReview.Campground);

                //activity log
                _customerActivityService.InsertActivity("EditCampgroundReview", _localizationService.GetResource("ActivityLog.EditCampgroundReview"), campgroundReview.Id);

                //host can edit "Reply text" only
                if (!isLoggedInAsHost)
                {
                    //update campground totals
                    _campgroundService.UpdateCampgroundReviewTotals(campgroundReview.Campground);

                    //raise event (only if it wasn't approved before and is approved now)
                    if (!previousIsApproved && campgroundReview.IsApproved)
                        _eventPublisher.Publish(new CampgroundReviewApprovedEvent(campgroundReview));

                }

                SuccessNotification(_localizationService.GetResource("Admin.CampgroundReviews.Updated"));

                return continueEditing ? RedirectToAction("Edit", new { id = campgroundReview.Id }) : RedirectToAction("List");
            }


            //If we got this far, something failed, redisplay form
            PrepareCampgroundReviewModel(model, campgroundReview, true, false);
            return View(EDIT_VIEW, model);
        }

        //delete
        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedView();

            var campgroundReview = _campgroundService.GetCampgroundReviewById(id);
            if (campgroundReview == null)
                //No campground review found with the specified id
                return RedirectToAction("List");

            //a host does not have access to this functionality
            //if (_workContext.CurrentHost != null)
            //    return RedirectToAction("List");

            var campground = campgroundReview.Campground;
            _campgroundService.DeleteCampgroundReview(campgroundReview);

            //activity log
            _customerActivityService.InsertActivity("DeleteCampgroundReview", _localizationService.GetResource("ActivityLog.DeleteCampgroundReview"), campgroundReview.Id);

            //update campground totals
            _campgroundService.UpdateCampgroundReviewTotals(campground);

            SuccessNotification(_localizationService.GetResource("Admin.CampgroundReviews.Deleted"));
            return RedirectToAction("List");
        }

        [HttpPost]
        public virtual IActionResult ApproveSelected(ICollection<int> selectedIds)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedView();

            //a host does not have access to this functionality
            //if (_workContext.CurrentHost != null)
            //    return RedirectToAction("List");

            if (selectedIds != null)
            {
                //filter not approved reviews
                var campgroundReviews = _campgroundService.GetCampgroundReviewsByIds(selectedIds.ToArray()).Where(review => !review.IsApproved);
                foreach (var campgroundReview in campgroundReviews)
                {
                    campgroundReview.IsApproved = true;
                    _campgroundService.UpdateCampground(campgroundReview.Campground);
                    
                    //update campground totals
                    _campgroundService.UpdateCampgroundReviewTotals(campgroundReview.Campground);

                    //raise event 
                    _eventPublisher.Publish(new CampgroundReviewApprovedEvent(campgroundReview));
                }
            }

            return Json(new { Result = true });
        }

        [HttpPost]
        public virtual IActionResult DisapproveSelected(ICollection<int> selectedIds)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedView();

            //a host does not have access to this functionality
            //if (_workContext.CurrentHost != null)
            //    return RedirectToAction("List");

            if (selectedIds != null)
            {
                //filter approved reviews
                var campgroundReviews = _campgroundService.GetCampgroundReviewsByIds(selectedIds.ToArray()).Where(review => review.IsApproved);
                foreach (var campgroundReview in campgroundReviews)
                {
                    campgroundReview.IsApproved = false;
                    _campgroundService.UpdateCampground(campgroundReview.Campground);

                    //update campground totals
                    _campgroundService.UpdateCampgroundReviewTotals(campgroundReview.Campground);
                }
            }

            return Json(new { Result = true });
        }

        [HttpPost]
        public virtual IActionResult DeleteSelected(ICollection<int> selectedIds)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedView();

            //a host does not have access to this functionality
            //if (_workContext.CurrentHost != null)
            //    return RedirectToAction("List");

            if (selectedIds != null)
            {
                var campgroundReviews = _campgroundService.GetCampgroundReviewsByIds(selectedIds.ToArray());
                var campgrounds = _campgroundService.GetCampgroundsByIds(campgroundReviews.Select(p => p.CampgroundId).Distinct().ToArray());

                _campgroundService.DeleteCampgroundReviews(campgroundReviews);

                //update campground totals
                foreach (var campground in campgrounds)
                {
                    _campgroundService.UpdateCampgroundReviewTotals(campground);
                }
            }

            return Json(new { Result = true });
        }

        public virtual IActionResult CampgroundSearchAutoComplete(string term)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return Content("");

            const int searchTermMinimumLength = 3;
            if (string.IsNullOrWhiteSpace(term) || term.Length < searchTermMinimumLength)
                return Content("");

            //a host should have access only to his campgrounds
            //var hostId = 0;
            //if (_workContext.CurrentHost != null)
            //{
            //    hostId = _workContext.CurrentHost.Id;
            //}

            //campgrounds
            const int campgroundNumber = 15;
            var campgrounds = _campgroundService.SearchCampgrounds(
                keywords: term,
                pageSize: campgroundNumber,
                showHidden: true);

            var result = (from p in campgrounds
                select new
                {
                    label = p.Name,
                    campgroundid = p.Id
                })
                .ToList();
            return Json(result);
        }

        #endregion
    }
}