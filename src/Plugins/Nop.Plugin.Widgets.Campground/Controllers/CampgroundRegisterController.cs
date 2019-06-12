using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Customers;
using Nop.Plugin.Campgrounds.Data.Domain;
using Nop.Plugin.Campgrounds.Services;
using Nop.Plugin.Widgets.Campgrounds.Factories;
using Nop.Plugin.Widgets.Campgrounds.Models;
using Nop.Services.Catalog;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Seo;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Framework.Security;
using Nop.Web.Framework.Security.Captcha;
using Microsoft.AspNetCore.Http;
using System;
using Nop.Plugin.Campgrounds.Services.Messaging;
using Nop.Services.Helpers;
using Nop.Services.Directory;
using Nop.Core;
using Nop.Plugin.Widgets.Campgrounds.Extensions;

namespace Nop.Plugin.Widgets.Campgrounds.Controllers
{
    public partial class CampgroundRegisterController : BasePluginController
    {
        public const string CAMPGROUND_HOST_APPLY = "~/Plugins/Campgrounds/Views/Campground/CampgroundRegister.cshtml";
        public const string EDIT_VIEW = "~/Plugins/Campgrounds/Views/Admin/Campground/Edit.cshtml";
        public const string CREATE_VIEW = "~/Plugins/Campgrounds/Views/Admin/Campground/Create.cshtml";

        #region Fields

        private readonly ICampgroundModelFactory _campgroundModelFactory;
        private readonly ICategoryService _categoryService;
        private readonly ICampgroundService _campgroundService;
        private readonly ICampgroundHostService _campgroundHostService;
        private readonly IWorkContext _workContext;
        private readonly ILocalizationService _localizationService;
        private readonly ICustomerService _customerService;
        private readonly ICampgroundWorkflowMessageService _campgroundWorkflowMessageService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IPictureService _pictureService;
        private readonly LocalizationSettings _localizationSettings;
        private readonly CampgroundSettings _campgroundSettings;
        private readonly CaptchaSettings _captchaSettings;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly ILogger _logger;

        #endregion

        #region Ctor

        public CampgroundRegisterController(
        ICampgroundModelFactory campgroundModelFactory,
        ICategoryService categoryService,
        ICampgroundService campgroundService,
        ICampgroundHostService campgroundHostService,
        IWorkContext workContext,
        ILocalizationService localizationService,
        ICustomerService customerService,
        ICampgroundWorkflowMessageService campgroundWorkflowMessageService,
        IUrlRecordService urlRecordService,
        IPictureService pictureService,
        LocalizationSettings localizationSettings,
        CampgroundSettings campgroundSettings,
        CaptchaSettings captchaSettings,
        IDateTimeHelper dateTimeHelper,
        IStateProvinceService stateProvinceService,
        ILogger logger)
        {
            this._campgroundModelFactory = campgroundModelFactory;
            this._categoryService = categoryService;
            this._campgroundService = campgroundService;
            this._campgroundHostService = campgroundHostService;
            this._workContext = workContext;
            this._localizationService = localizationService;
            this._customerService = customerService;
            this._campgroundWorkflowMessageService = campgroundWorkflowMessageService;
            this._urlRecordService = urlRecordService;
            this._pictureService = pictureService;
            this._localizationSettings = localizationSettings;
            this._campgroundSettings = campgroundSettings;
            this._captchaSettings = captchaSettings;
            this._dateTimeHelper = dateTimeHelper;
            this._stateProvinceService = stateProvinceService;
            this._logger = logger;
        }

        #endregion

        #region Utility
        protected virtual IActionResult InvokeHttp404()
        {
            Response.StatusCode = 404;
            return new EmptyResult();
        }
        #endregion

        #region Campground Hosts

        public class Data
        {
            public string Name { get; set; }
            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Zip { get; set; }
            public decimal? Longitude { get; set; }
            public decimal? Latitude { get; set; }
        }

        [HttpPut]
        public IActionResult VerifyAddress([FromBody]Data data)
        {
            var results = new Location();
            var returnSuccess = false;

            if (data != null)
            {
                var address = new CampgroundAddress
                {
                    Company = data.Name,
                    Address1 = data.Address1,
                    Address2 = data.Address2,
                    City = data.City,
                    ZipPostalCode = data.Zip,
                    Latitude = data.Latitude,
                    Longitude = data.Longitude
                };


                results = _campgroundService.GetCampgroundLatLong(address);
            }

            return Json(new
            {
                success = returnSuccess,
                response = results
            });
        }

        [HttpsRequirement(SslRequirement.Yes)]
        public IActionResult CampgroundRegister(string widgetZone)
        {
            var model = new CampgroundRegisterModel();

            if (!_campgroundSettings.AllowCustomersToApplyForCampgroundAccount)
            {
                model.Result = _localizationService.GetResource("Campgrounds.ApplyAccount.NoLongerOpenToAdd");
                return View(CAMPGROUND_HOST_APPLY, model);
            }

            //if (!_workContext.CurrentCustomer.IsRegistered())
            //    return Challenge();

            model = _campgroundModelFactory.PrepareCampgroundRegisterModel(model, false, false);
            model.DisableFormInput = TempData["DisableFormInput"] != null ? (bool)TempData["DisableFormInput"] : false;
            model.Result = TempData["Result"] != null ? TempData["Result"].ToString() : string.Empty;
            return View(CAMPGROUND_HOST_APPLY, model);
        }

        [HttpPost, ActionName("CampgroundRegister")]
        [PublicAntiForgery]
        [ValidateCaptcha]
        public virtual IActionResult CampgroundRegisterSubmit(CampgroundRegisterModel model, bool captchaValid, IFormFile uploadedFile)
        {
            if (!_campgroundSettings.AllowCustomersToApplyForCampgroundAccount)
            {
                model.Result = _localizationService.GetResource("Campgrounds.ApplyAccount.NoLongerOpenToAdd");
                return View(CAMPGROUND_HOST_APPLY, model);
            }

            //if (!_workContext.CurrentCustomer.IsRegistered())
            //    return Challenge();

            //validate CAPTCHA
            if (_captchaSettings.Enabled && _captchaSettings.ShowOnApplyCampgroundPage && !captchaValid)
            {
                ModelState.AddModelError("", _captchaSettings.GetWrongCaptchaMessage(_localizationService));
            }

            if (_campgroundSettings.VerifyExistingCampgroundsEnabled)
            {
                var campgrounds = _campgroundService.FindByName(name: model.Campground.Name.Replace("Campground", string.Empty));
                if (campgrounds.Count > 0)
                {
                    foreach (var campground in campgrounds)
                    {
                        var result = new CampgroundDetailModel
                        {
                            Name = campground.Name,
                            Website = campground.Website,
                            CampgroundAddress = campground.CampgroundAddress,
                        };

                        model.Campgrounds.Add(result);
                    }
                    model.Result = _localizationService.GetResource("Campgrounds.ApplyAccount.CampgroundExists");
                    //DISPLAY CAMPGROUNDS IN GRID AND ALLOW PERSON TO CLAIM.
                    return View(CAMPGROUND_HOST_APPLY, model);
                }
            }
            var pictureId = 0;

            if (uploadedFile != null && !string.IsNullOrEmpty(uploadedFile.FileName))
            {
                try
                {
                    var contentType = uploadedFile.ContentType;
                    var campgroundPictureBinary = uploadedFile.GetPictureBits();
                    var picture = _pictureService.InsertPicture(campgroundPictureBinary, contentType, null);

                    if (picture != null)
                    {
                        pictureId = picture.Id;
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", _localizationService.GetResource("Campgrounds.ApplyAccount.Picture.ErrorMessage"));
                }
            }

            if (ModelState.IsValid)
            {
                var description = Core.Html.HtmlHelper.FormatText(model.Campground.ShortDescription, false, false, true, false, false, false);
                var CampgroundHost = _campgroundHostService.GetCampgroundHostByEmail(_workContext.CurrentCustomer.Email);

                var campground = model.Campground.ToEntity();
                campground.CampgroundAddress = model.Campground.CampgroundAddress.ToEntity();
                //var campground = new Campground
                //{
                //    Name = model.Campground.Name,
                //    AvailableCampsites = model.Campground.AvailableCampsites,
                //    Website = model.Campground.Website,
                //    MarkAsNew = true,
                //    MarkAsNewStartDateTimeUtc = _dateTimeHelper.ConvertToUserTime(DateTime.Now, DateTimeKind.Utc),
                //    MarkAsNewEndDateTimeUtc = _dateTimeHelper.ConvertToUserTime(DateTime.Now.AddDays(30), DateTimeKind.Utc),
                //    Published = false,
                //    VisibleIndividually = true,
                //    ShortDescription = model.Description,
                //    FullDescription = model.Description,
                //    AllowCampgroundReviews = true,
                //    CampgroundAddress = new CampgroundAddress
                //    {
                //        Company = model.Campground.Name,
                //        Address1 = model.Campground.CampgroundAddress.Address1,
                //        Address2 = model.Campground.CampgroundAddress.Address2,
                //        City = model.Campground.CampgroundAddress.City,
                //        StateProvinceId = model.Campground.CampgroundAddress.StateProvinceId,
                //        ZipPostalCode = model.Campground.CampgroundAddress.ZipPostalCode,
                //        Latitude = model.Campground.CampgroundAddress.Latitude != 0 ? model.Campground.CampgroundAddress.Latitude : null,
                //        Longitude = model.Campground.CampgroundAddress.Longitude != 0 ? model.Campground.CampgroundAddress.Longitude : null,
                //        GooglePlaceId = !string.IsNullOrEmpty(model.Campground.CampgroundAddress.GooglePlaceId) ? model.Campground.CampgroundAddress.GooglePlaceId : null,
                //        CountryId = 1,
                //        CreatedOnUtc = _dateTimeHelper.ConvertToUserTime(DateTime.Now, DateTimeKind.Utc)
                //    },
                //};

                campground.CampgroundAddress.CreatedOnUtc = _dateTimeHelper.ConvertToUserTime(DateTime.Now, DateTimeKind.Utc);
                campground.BillingAddress = campground.CampgroundAddress;
                campground.Addresses.Add(campground.CampgroundAddress);
                campground.Addresses.Add(campground.BillingAddress);

                if (pictureId > 0)
                {
                    var campgroundPicture = new CampgroundPicture
                    {
                        PictureId = pictureId,
                        CampgroundId = campground.Id
                    };

                    campground.CampgroundPictures.Add(campgroundPicture);
                }

                campground.FormatCampgroundURL();

                if (CampgroundHost == null)
                {
                    CampgroundHost = new CampgroundHost
                    {
                        AcceptedTerms = model.TermsOfServiceEnabled,
                        //some default settings
                        PageSize = 10,
                        AllowCustomersToSelectPageSize = true,
                        PageSizeOptions = _campgroundSettings.DefaultCampgroundPageSizeOptions,
                        Description = description,
                        CreatedOnUtc = _dateTimeHelper.ConvertToUserTime(DateTime.Now, DateTimeKind.Utc),
                        CustomerId = _workContext.CurrentCustomer.Id,
                    };
                }
                campground.CampgroundHost.Add(CampgroundHost);
                _campgroundService.InsertCampground(campground);
                //_campgroundService.UpdateCampground(campground);

                //notify store owner
                if (_campgroundSettings.NotifyCampgroundHostAboutNewCampground)
                    _campgroundWorkflowMessageService.SendNewCampgroundAccountApplyStoreOwnerNotification(_workContext.CurrentCustomer, CampgroundHost, _localizationSettings.DefaultAdminLanguageId);

                //notify store owner here (email)

                TempData["DisableFormInput"] = true;
                TempData["Result"] = _localizationService.GetResource("Campgrounds.ApplyAccount.Submitted");
                return RedirectToAction("CampgroundRegister");
            }

            //If we got this far, something failed, redisplay form
            model.Campground.CampgroundAddress.AvailableStates = _campgroundModelFactory.PrepareAvailableStates(Id: model.Campground.CampgroundAddress.StateProvinceId.GetValueOrDefault());
            return View(CAMPGROUND_HOST_APPLY, model);
        }

        #endregion
    }
}