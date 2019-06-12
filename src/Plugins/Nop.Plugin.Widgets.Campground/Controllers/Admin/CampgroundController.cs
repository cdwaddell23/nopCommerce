using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Caching;
using Nop.Core.Domain.Localization;
using Nop.Plugin.Campgrounds.Data.Domain;
using Nop.Plugin.Campgrounds.Services;
using Nop.Plugin.Widgets.Campgrounds.Factories;
using Nop.Plugin.Widgets.Campgrounds.Models;
using Nop.Plugin.Widgets.Campgrounds.Extensions;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Web.Areas.Admin.Controllers;
using Nop.Web.Framework.Kendoui;
using Nop.Web.Framework.Security.Captcha;
using System.Collections.Generic;
using System.Linq;
using Nop.Web.Framework.Mvc.Filters;
using System;
using Nop.Services.Helpers;
using Nop.Services.Directory;
using Nop.Core.Domain.Directory;
using Nop.Services.Shipping.Date;
using System.Text;
using Nop.Services.Configuration;
using System.Net;
using Nop.Web.Framework.Mvc;
using Nop.Core.Domain.Catalog;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Extensions;
using Nop.Services.Common;
using Nop.Core.Domain.Common;
using Nop.Web.Extensions;
using Nop.Plugin.Widgets.Campgrounds.Helpers;

namespace Nop.Plugin.Widgets.Campgrounds.Controllers.Admin
{
    public partial class CampgroundController : BaseAdminController
    {
        #region CONST
        public const string LIST_VIEW = "~/Plugins/Campgrounds/Views/Admin/Campground/List.cshtml";
        public const string EDIT_VIEW = "~/Plugins/Campgrounds/Views/Admin/Campground/Edit.cshtml";
        public const string CREATE_VIEW = "~/Plugins/Campgrounds/Views/Admin/Campground/Create.cshtml";
        public const string RELATED_CAMPGROUND_VIEW = "~/Plugins/Campgrounds/Views/Admin/Campground/RelatedCampgroundAddPopup.cshtml";
        public const string REQUIRED_CAMPGROUND_VIEW = "~/Plugins/Campgrounds/Views/Admin/Campground/RequiredCampgroundAddPopup.cshtml";
        public const string EDIT_TAG_VIEW = "~/Plugins/Campgrounds/Views/Admin/Campground/EditCampgroundTag.cshtml";
        public const string CAMPGROUND_ATT_CREATE_VIEW = "~/Plugins/Campgrounds/Views/Admin/Campground/CampgroundAttributeMappingCreate.cshtml";
        public const string CAMPGROUND_ATT_EDIT_VIEW = "~/Plugins/Campgrounds/Views/Admin/Campground/CampgroundAttributeMappingEdit.cshtml";
        public const string CAMPGROUND_ATT_CREATE_POP_VIEW = "~/Plugins/Campgrounds/Views/Admin/Campground/CampgroundAttributeValueCreatePopup.cshtml";
        public const string CAMPGROUND_ATT_EDIT_POP_VIEW = "~/Plugins/Campgrounds/Views/Admin/Campground/CampgroundAttributeValueEditPopup.cshtml";
        public const string ASSOC_ATT_POP_VIEW = "~/Plugins/Campgrounds/Views/Admin/Campground/AssociateCampgroundToAttributeValuePopup.cshtml";
        public const string ASSOC_CAMPGROUND_ADD_POP_VIEW = "~/Plugins/Campgrounds/Views/Admin/Campground/AssociatedCampgroundAddPopup.cshtml";
        public const string CROSS_SELL_ADD_POP_VIEW = "~/Plugins/Campgrounds/Views/Admin/Campground/CrossSellProductAddPopup.cshtml";
        
        #endregion

        #region Fields

        private readonly ICampgroundModelFactory _campgroundModelFactory;
        private readonly ICategoryService _categoryService;
        private readonly ICampgroundService _campgroundService;
        private readonly ICampgroundAddressService _campgroundAddressService;
        private readonly ICampgroundTagService _campgroundTagService;
        private readonly ICampgroundRegisterModelFactory _campgroundRegisterModelFactory;
        private readonly ICampgroundAttributeTypeService _campgroundAttributeTypeService;
        private readonly ICampgroundWorkContext _workContext;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly ISpecificationAttributeService _specificationAttributeService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly ICampgroundHostService _campgroundHostService;
        private readonly ICampgroundTypeService _campgroundTypeService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IPictureService _pictureService;
        private readonly IPermissionService _permissionService;
        private readonly IAclService _aclService;
        private readonly AddressSettings _addressSettings;
        private readonly IAddressAttributeFormatter _addressAttributeFormatter;
        private readonly LocalizationSettings _localizationSettings;
        private readonly CampgroundHostSettings _campgroundHostSettings;
        private readonly CampgroundSettings _campgroundSettings;
        private readonly CaptchaSettings _captchaSettings;
        private readonly IStaticCacheManager _cacheManager;
        private readonly ICurrencyService _currencyService;
        private readonly CurrencySettings _currencySettings;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IDateRangeService _dateRangeService;
        private readonly ISettingService _settingService;
        private readonly IPdfService _pdfService;
        private readonly ILogger _logger;

        #endregion

        #region Ctor

        public CampgroundController(
        ICampgroundModelFactory campgroundModelFactory,
        ICategoryService categoryService,
        ICampgroundAddressService campgroundAddressService,
        ICampgroundService campgroundService,
        ICampgroundTagService campgroundTagService,
        ICampgroundRegisterModelFactory campgroundRegisterModelFactory,
        ICampgroundAttributeTypeService campgroundAttributeTypeService,
        ICampgroundWorkContext workContext,
        ILanguageService languageService,
        ILocalizationService localizationService,
        ILocalizedEntityService localizedEntityService,
        ISpecificationAttributeService specificationAttributeService,
        ICustomerActivityService customerActivityService,
        IWorkflowMessageService workflowMessageService,
        ICampgroundHostService campgroundHostService,
        ICampgroundTypeService campgroundTypeService,
        IUrlRecordService urlRecordService,
        IPictureService pictureService,
        IPermissionService permissionService,
        IAclService aclService,
        AddressSettings addressSettings,
        IAddressAttributeFormatter addressAttributeFormatter,
        LocalizationSettings localizationSettings,
        CampgroundHostSettings campgroundHostSettings,
        CampgroundSettings campgroundSettings,
        CaptchaSettings captchaSettings,
        ICurrencyService currencyService,
        CurrencySettings currencySettings,
        IDateTimeHelper dateTimeHelper,
        IDateRangeService dateRangeService,
        IStaticCacheManager cacheManager,
        ISettingService settingService,
        IPdfService pdfService, 
        ILogger logger)
        {
            this._campgroundModelFactory = campgroundModelFactory;
            this._categoryService = categoryService;
            this._campgroundService = campgroundService;
            this._campgroundAddressService = campgroundAddressService;
            this._campgroundTagService = campgroundTagService;
            this._campgroundRegisterModelFactory = campgroundRegisterModelFactory;
            this._campgroundAttributeTypeService = campgroundAttributeTypeService;
            this._workContext = workContext;
            this._languageService = languageService;
            this._localizationService = localizationService;
            this._localizedEntityService = localizedEntityService;
            this._specificationAttributeService = specificationAttributeService;
            this._customerActivityService = customerActivityService;
            this._workflowMessageService = workflowMessageService;
            this._campgroundHostService = campgroundHostService;
            this._campgroundTypeService = campgroundTypeService;
            this._urlRecordService = urlRecordService;
            this._pictureService = pictureService;
            this._permissionService = permissionService;
            this._aclService = aclService;
            this._addressSettings = addressSettings;
            this._addressAttributeFormatter = addressAttributeFormatter;
            this._localizationSettings = localizationSettings;
            this._campgroundHostSettings = campgroundHostSettings;
            this._campgroundSettings = campgroundSettings;
            this._captchaSettings = captchaSettings;
            this._currencyService = currencyService;
            this._currencySettings = currencySettings;
            this._dateTimeHelper = dateTimeHelper;
            this._dateRangeService = dateRangeService;
            this._settingService = settingService;
            this._cacheManager = cacheManager;
            this._pdfService = pdfService;
            this._logger = logger;
        }

        #endregion

        #region Utility
        protected virtual IActionResult InvokeHttp404()
        {
            Response.StatusCode = 404;
            return new EmptyResult();
        }

        protected virtual void UpdateLocales(Campground campground, CampgroundModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(campground,
                    x => x.Name,
                    localized.Name,
                    localized.LanguageId);
                _localizedEntityService.SaveLocalizedValue(campground,
                    x => x.ShortDescription,
                    localized.ShortDescription,
                    localized.LanguageId);
                _localizedEntityService.SaveLocalizedValue(campground,
                    x => x.FullDescription,
                    localized.FullDescription,
                    localized.LanguageId);
                _localizedEntityService.SaveLocalizedValue(campground,
                    x => x.MetaKeywords,
                    localized.MetaKeywords,
                    localized.LanguageId);
                _localizedEntityService.SaveLocalizedValue(campground,
                    x => x.MetaDescription,
                    localized.MetaDescription,
                    localized.LanguageId);
                _localizedEntityService.SaveLocalizedValue(campground,
                    x => x.MetaTitle,
                    localized.MetaTitle,
                    localized.LanguageId);

                //search engine name
                var seName = campground.ValidateSeName(localized.SeName, localized.Name, false);
                _urlRecordService.SaveSlug(campground, seName, localized.LanguageId);
            }
        }

        protected virtual void UpdateLocales(CampgroundTag campgroundTag, CampgroundTagModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(campgroundTag,
                    x => x.Name,
                    localized.Name,
                    localized.LanguageId);
            }
        }

        protected virtual void UpdateLocales(CampgroundAttributeMapping cam, CampgroundModel.CampgroundAttributeMappingModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(cam,
                    x => x.TextPrompt,
                    localized.TextPrompt,
                    localized.LanguageId);
            }
        }

        protected virtual void UpdateLocales(CampgroundAttributeValue cav, CampgroundModel.CampgroundAttributeValueModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(cav,
                    x => x.Name,
                    localized.Name,
                    localized.LanguageId);
            }
        }

        protected virtual void UpdatePictureSeoNames(Campground campground)
        {
            foreach (var cp in campground.CampgroundPictures)
                _pictureService.SetSeoFilename(cp.PictureId, _pictureService.GetPictureSeName(campground.Name));
        }

        //protected virtual void PrepareAclModel(CampgroundModel model, Campground campground, bool excludeProperties)
        //{
        //    if (model == null)
        //        throw new ArgumentNullException(nameof(model));

        //    if (!excludeProperties && campground != null)
        //        model.SelectedCustomerRoleIds = _aclService.GetCustomerRoleIdsWithAccess(campground).ToList();

        //    var allRoles = _customerService.GetAllCustomerRoles(true);
        //    foreach (var role in allRoles)
        //    {
        //        model.AvailableCustomerRoles.Add(new SelectListItem
        //        {
        //            Text = role.Name,
        //            Value = role.Id.ToString(),
        //            Selected = model.SelectedCustomerRoleIds.Contains(role.Id)
        //        });
        //    }
        //}

        //protected virtual void SaveCampgroundAcl(Campground campground, CampgroundModel model)
        //{
        //    campground.SubjectToAcl = model.SelectedCampgroundRoleIds.Any();

        //    var existingAclRecords = _aclService.GetAclRecords(campground);
        //    var allCampgroundRoles = _campgroundService.GetAllCampgroundRoles(true);
        //    foreach (var campgroundRole in allCampgroundRoles)
        //    {
        //        if (model.SelectedCampgroundRoleIds.Contains(campgroundRole.Id))
        //        {
        //            //new role
        //            if (existingAclRecords.Count(acl => acl.CampgroundRoleId == campgroundRole.Id) == 0)
        //                _aclService.InsertAclRecord(campground, campgroundRole.Id);
        //        }
        //        else
        //        {
        //            //remove role
        //            var aclRecordToDelete = existingAclRecords.FirstOrDefault(acl => acl.CampgroundRoleId == campgroundRole.Id);
        //            if (aclRecordToDelete != null)
        //                _aclService.DeleteAclRecord(aclRecordToDelete);
        //        }
        //    }
        //}

        protected virtual void PrepareCategoryMappingModel(CampgroundModel model, Campground campground, bool excludeProperties)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (!excludeProperties && campground != null)
                model.SelectedCategoryIds = _campgroundService.GetCampgroundCategoriesByCampgroundId(campground.Id, true).Select(c => c.CategoryId).ToList();

            var allCategories = SelectListHelper.GetStateCategoryList(_categoryService, _cacheManager, true);
            foreach (var c in allCategories)
            {
                c.Selected = model.SelectedCategoryIds.Contains(int.Parse(c.Value));
                model.AvailableCategories.Add(c);
            }
        }

        protected virtual void SaveCategoryMappings(Campground campground, CampgroundModel model)
        {
            var existingCampgroundCategories = _campgroundService.GetCampgroundCategoriesByCampgroundId(campground.Id, true);

            //delete categories
            foreach (var existingCampgroundCategory in existingCampgroundCategories)
                if (!model.SelectedCategoryIds.Contains(existingCampgroundCategory.CategoryId))
                    _campgroundService.DeleteCampgroundCategory(existingCampgroundCategory);

            //add categories
            foreach (var categoryId in model.SelectedCategoryIds)
                if (existingCampgroundCategories.FindCampgroundCategory(campground.Id, categoryId) == null)
                {
                    //find next display order
                    var displayOrder = 1;
                    var existingCategoryMapping = _campgroundService.GetCampgroundCategoriesByCategoryId(categoryId, showHidden: true);
                    if (existingCategoryMapping.Any())
                        displayOrder = existingCategoryMapping.Max(x => x.DisplayOrder) + 1;
                    _campgroundService.InsertCampgroundCategory(new CampgroundCategory
                    {
                        CampgroundId = campground.Id,
                        CategoryId = categoryId,
                        DisplayOrder = displayOrder
                    });
                }
        }

        protected virtual string[] ParseCampgroundTags(string campgroundTags)
        {
            var result = new List<string>();
            if (!string.IsNullOrWhiteSpace(campgroundTags))
            {
                var values = campgroundTags.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var val1 in values)
                    if (!string.IsNullOrEmpty(val1.Trim()))
                        result.Add(val1.Trim());
            }
            return result.ToArray();
        }


        protected virtual List<int> GetChildCategoryIds(int parentCategoryId)
        {
            var categoriesIds = new List<int>();
            var categories = _categoryService.GetAllCategoriesByParentCategoryId(parentCategoryId, true);
            foreach (var category in categories)
            {
                categoriesIds.Add(category.Id);
                categoriesIds.AddRange(GetChildCategoryIds(category.Id));
            }
            return categoriesIds;
        }

        protected virtual void PrepareCampgroundAttributeMappingModel(CampgroundModel.CampgroundAttributeMappingModel model,
            CampgroundAttributeMapping cam, Campground campground, bool excludeProperties)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (campground == null)
                throw new ArgumentNullException(nameof(campground));

            model.CampgroundId = campground.Id;

            foreach (var campgroundAttributeType in _campgroundAttributeTypeService.GetAllCampgroundAttributeTypes())
            {
                model.AvailableCampgroundAttributeTypes.Add(new SelectListItem
                {
                    Text = campgroundAttributeType.Name,
                    Value = campgroundAttributeType.Id.ToString()
                });
            }

            if (cam == null)
                return;

            model.Id = cam.Id;
            model.CampgroundAttributeType = _campgroundAttributeTypeService.GetCampgroundAttributeTypeById(cam.CampgroundAttributeTypeId).Name;
            model.AttributeControlType = cam.AttributeControlType.GetLocalizedEnum(_localizationService, _workContext);
            if (!excludeProperties)
            {
                model.CampgroundAttributeTypeId = cam.CampgroundAttributeTypeId;
                model.TextPrompt = cam.TextPrompt;
                model.IsRequired = cam.IsRequired;
                model.AttributeControlTypeId = cam.AttributeControlTypeId;
                model.DisplayOrder = cam.DisplayOrder;
                model.ValidationMinLength = cam.ValidationMinLength;
                model.ValidationMaxLength = cam.ValidationMaxLength;
                model.DefaultValue = cam.DefaultValue;
            }

            if (cam.ValidationRulesAllowed())
            {
                var validationRules = new StringBuilder(string.Empty);
                if (cam.ValidationMinLength != null)
                    validationRules.AppendFormat("{0}: {1}<br />",
                        _localizationService.GetResource("Admin.Campgrounds.CampgroundAttributes.Attributes.ValidationRules.MinLength"),
                        cam.ValidationMinLength);
                if (cam.ValidationMaxLength != null)
                    validationRules.AppendFormat("{0}: {1}<br />",
                        _localizationService.GetResource("Admin.Campgrounds.CampgroundAttributes.Attributes.ValidationRules.MaxLength"),
                        cam.ValidationMaxLength);
                if (!string.IsNullOrEmpty(cam.DefaultValue))
                    validationRules.AppendFormat("{0}: {1}<br />",
                        _localizationService.GetResource("Admin.Campgrounds.CampgroundAttributes.Attributes.ValidationRules.DefaultValue"),
                        WebUtility.HtmlEncode(cam.DefaultValue));
                model.ValidationRulesString = validationRules.ToString();
            }

            //currently any attribute can have condition. why not?
            //model.ConditionAllowed = true;
            //var conditionAttribute = _campgroundAttributeTypeParser.ParseCampgroundAttributeMappings(pam.ConditionAttributeXml).FirstOrDefault();
            //var conditionValue = _campgroundAttributeTypeParser.ParseCampgroundAttributeValues(pam.ConditionAttributeXml).FirstOrDefault();
            //if (conditionAttribute != null && conditionValue != null)
            //    model.ConditionString = $"{WebUtility.HtmlEncode(conditionAttribute.CampgroundAttributeType.Name)}: {WebUtility.HtmlEncode(conditionValue.Name)}";
            //else
            //    model.ConditionString = string.Empty;
        }

        protected virtual IList<SelectListItem> PrepareAvailableStateCategories(bool showHidden = true)
        {
            var AvailableStateCategories = new List<SelectListItem>
            {
                new SelectListItem { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" }
            };

            var categories = SelectListHelper.GetStateCategoryList(_categoryService, _cacheManager, true);
            foreach (var c in categories)
                AvailableStateCategories.Add(c);

            return AvailableStateCategories;

        }

        protected virtual IList<SelectListItem> PrepareAvailableCampgroundHosts(bool showHidden = true, int selectedId = 0)
        {
            var AvailableCampgroundHosts = new List<SelectListItem>
            {
                new SelectListItem { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" }
            };
            var campgroundHosts = SelectListHelper.GetCampgroundHostList(_campgroundHostService, _cacheManager, showHidden, selectedId);
            foreach (var ct in campgroundHosts)
                AvailableCampgroundHosts.Add(ct);

            return AvailableCampgroundHosts;
        }

        protected virtual IList<SelectListItem> PrepareAvailableCampgroundTypes(bool showHidden = true)
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

        //protected virtual void SaveConditionAttributes(CampgroundAttributeMapping campgroundAttributeTypeMapping, CampgroundAttributeTypeConditionModel model)
        //{
        //    string attributesXml = null;
        //    var form = model.Form;
        //    if (model.EnableCondition)
        //    {
        //        var attribute = _campgroundAttributeTypeService.GetCampgroundAttributeMappingById(model.SelectedCampgroundAttributeTypeId);
        //        if (attribute != null)
        //        {
        //            var controlId = $"campground_attribute_{attribute.Id}";
        //            switch (attribute.AttributeControlType)
        //            {
        //                case AttributeControlType.DropdownList:
        //                case AttributeControlType.RadioList:
        //                case AttributeControlType.ColorSquares:
        //                case AttributeControlType.ImageSquares:
        //                    {
        //                        var ctrlAttributes = form[controlId];
        //                        if (!StringValues.IsNullOrEmpty(ctrlAttributes))
        //                        {
        //                            var selectedAttributeId = int.Parse(ctrlAttributes);
        //                            if (selectedAttributeId > 0)
        //                            {
        //                                attributesXml = _campgroundAttributeTypeParser.AddCampgroundAttributeType(attributesXml,
        //                                    attribute, selectedAttributeId.ToString());
        //                            }
        //                            else
        //                            {
        //                                //for conditions we should empty values save even when nothing is selected
        //                                //otherwise "attributesXml" will be empty
        //                                //hence we won't be able to find a selected attribute
        //                                attributesXml = _campgroundAttributeTypeParser.AddCampgroundAttributeType(attributesXml,
        //                                    attribute, "");
        //                            }
        //                        }
        //                        else
        //                        {
        //                            //for conditions we should empty values save even when nothing is selected
        //                            //otherwise "attributesXml" will be empty
        //                            //hence we won't be able to find a selected attribute
        //                            attributesXml = _campgroundAttributeTypeParser.AddCampgroundAttributeType(attributesXml,
        //                                attribute, "");
        //                        }
        //                    }
        //                    break;
        //                case AttributeControlType.Checkboxes:
        //                    {
        //                        var cblAttributes = form[controlId];
        //                        if (!StringValues.IsNullOrEmpty(cblAttributes))
        //                        {
        //                            var anyValueSelected = false;
        //                            foreach (var item in cblAttributes.ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        //                            {
        //                                var selectedAttributeId = int.Parse(item);
        //                                if (selectedAttributeId > 0)
        //                                {
        //                                    attributesXml = _campgroundAttributeTypeParser.AddCampgroundAttributeType(attributesXml,
        //                                        attribute, selectedAttributeId.ToString());
        //                                    anyValueSelected = true;
        //                                }
        //                            }
        //                            if (!anyValueSelected)
        //                            {
        //                                //for conditions we should save empty values even when nothing is selected
        //                                //otherwise "attributesXml" will be empty
        //                                //hence we won't be able to find a selected attribute
        //                                attributesXml = _campgroundAttributeTypeParser.AddCampgroundAttributeType(attributesXml,
        //                                    attribute, "");
        //                            }
        //                        }
        //                        else
        //                        {
        //                            //for conditions we should save empty values even when nothing is selected
        //                            //otherwise "attributesXml" will be empty
        //                            //hence we won't be able to find a selected attribute
        //                            attributesXml = _campgroundAttributeTypeParser.AddCampgroundAttributeType(attributesXml,
        //                                attribute, "");
        //                        }
        //                    }
        //                    break;
        //                case AttributeControlType.ReadonlyCheckboxes:
        //                case AttributeControlType.TextBox:
        //                case AttributeControlType.MultilineTextbox:
        //                case AttributeControlType.Datepicker:
        //                case AttributeControlType.FileUpload:
        //                default:
        //                    //these attribute types are supported as conditions
        //                    break;
        //            }
        //        }
        //    }
        //    campgroundAttributeTypeMapping.ConditionAttributeXml = attributesXml;
        //    _campgroundAttributeTypeService.UpdateCampgroundAttributeMapping(campgroundAttributeTypeMapping);
        //}

        #endregion

        #region Campground
        public virtual IActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedView();

            var model = new CampgroundListModel()
            {
                //a campgroundHost should have access only to his campgrounds
                IsLoggedInAsCampgroundHost = _workContext.CurrentCampgroundHost != null,
                AllowCampgroundHostsToImportCampgrounds = _campgroundHostSettings.AllowCampgroundHostToImportCampgrounds
            };

            //state categories
            model.AvailableCategories = PrepareAvailableStateCategories();

            //campground hosts
            model.AvailableCampgroundHosts = PrepareAvailableCampgroundHosts();

            //campground types
            model.AvailableCampgroundTypes = PrepareAvailableCampgroundTypes();

            //"published" property
            //0 - all (according to "ShowHidden" parameter)
            //1 - published only
            //2 - unpublished only
            model.AvailablePublishedOptions.Add(new SelectListItem { Text = _localizationService.GetResource("Admin.Campgrounds.List.SearchPublished.All"), Value = "0" });
            model.AvailablePublishedOptions.Add(new SelectListItem { Text = _localizationService.GetResource("Admin.Campgrounds.List.SearchPublished.PublishedOnly"), Value = "1" });
            model.AvailablePublishedOptions.Add(new SelectListItem { Text = _localizationService.GetResource("Admin.Campgrounds.List.SearchPublished.UnpublishedOnly"), Value = "2" });

            return View(LIST_VIEW, model);
        }

        [HttpPost]
        public virtual IActionResult List(DataSourceRequest command, CampgroundListModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedKendoGridJson();

            var categoryIds = new List<int> { model.SearchCategoryId };
            //include subcategories
            if (model.SearchIncludeSubCategories && model.SearchCategoryId > 0)
                categoryIds.AddRange(GetChildCategoryIds(model.SearchCategoryId));

            //0 - all (according to "ShowHidden" parameter)
            //1 - published only
            //2 - unpublished only
            bool? overridePublished = null;
            if (model.SearchPublishedId == 1)
                overridePublished = true;
            else if (model.SearchPublishedId == 2)
                overridePublished = false;

            var campgrounds = _campgroundService.SearchCampgrounds(
                categoryIds: categoryIds,
                campgroundHostId: model.SearchCampgroundHostId > 0 ? model.SearchCampgroundHostId : 0,
                campgroundTypeId: model.SearchCampgroundTypeId > 0 ? model.SearchCampgroundTypeId : 0,
                keywords: model.SearchCampgroundName,
                pageIndex: command.Page - 1,
                pageSize: command.PageSize,
                showHidden: true,
                orderBy: CampgroundSortingEnum.CreatedOn,
                overridePublished: overridePublished
            );
            var gridModel = new DataSourceResult
            {
                Data = campgrounds.Select(x =>
                {
                    var campgroundModel = x.ToModel();
                    //little performance optimization: ensure that "FullDescription" is not returned
                    campgroundModel.FullDescription = "";

                    //picture
                    var defaultCampgroundPicture = _campgroundService.GetCampgroundPicturesByCampgroundId(x.Id).FirstOrDefault();
                    campgroundModel.PictureThumbnailUrl = defaultCampgroundPicture != null ? _pictureService.GetPictureUrl(defaultCampgroundPicture.Picture, 75, true) : null;
                    //campground type
                    campgroundModel.CampgroundTypeName = x.CampgroundType.FirstOrDefault()?.Description;
                    //friendly stock qantity
                    return campgroundModel;
                }),
                Total = campgrounds.TotalCount
            };

            return Json(gridModel);
        }

        //create campground
        public virtual IActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedView();

            ////validate maximum number of campgrounds per campgroundHost
            //if (_campgroundHostSettings.MaximumCampgroundNumber > 0 &&
            //    _workContext.CurrentCampgroundHost != null &&
            //    _campgroundService.GetNumberOfCampgroundsByCampgroundHostId(_workContext.CurrentCampgroundHost.Id) >= _campgroundHostSettings.MaximumCampgroundNumber)
            //{
            //    ErrorNotification(string.Format(_localizationService.GetResource("Admin.Campgrounds.ExceededMaximumNumber"), _campgroundHostSettings.MaximumCampgroundNumber));
            //    return RedirectToAction("List");
            //}

            var model = new CampgroundModel();

            _campgroundModelFactory.PrepareCampgroundModel(model, null, true, true);
            AddLocales(_languageService, model.Locales);
            //PrepareAclModel(model, null, false);
            PrepareCategoryMappingModel(model, null, false);

            return View(CREATE_VIEW, model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Create(CampgroundModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                //campground
                var campground = model.ToEntity();
                campground.CreatedOnUtc = DateTime.UtcNow;
                campground.UpdatedOnUtc = DateTime.UtcNow;
                _campgroundService.InsertCampground(campground);
                //search engine name
                model.SeName = campground.ValidateSeName(model.SeName, campground.Name, true);
                _urlRecordService.SaveSlug(campground, model.SeName, 0);
                //locales
                UpdateLocales(campground, model);
                //categories
                SaveCategoryMappings(campground, model);
                //ACL (campground roles)
                //SaveCampgroundAcl(campground, model);
                //tags
                _campgroundTagService.UpdateCampgroundTags(campground, ParseCampgroundTags(model.CampgroundTags));

                //activity log
                _customerActivityService.InsertActivity("AddNewCampground", _localizationService.GetResource("ActivityLog.AddNewCampground"), campground.Name);

                SuccessNotification(_localizationService.GetResource("Admin.Campgrounds.Added"));

                if (continueEditing)
                {
                    //selected tab
                    SaveSelectedTabName();

                    return RedirectToAction("Edit", new { id = campground.Id });
                }
                return RedirectToAction("List");
            }

            //If we got this far, something failed, redisplay form
            _campgroundModelFactory.PrepareCampgroundModel(model, null, false, true);

            //PrepareAclModel(model, null, true);
            PrepareCategoryMappingModel(model, null, true);

            return View(CREATE_VIEW, model);
        }

        //edit campground
        public virtual IActionResult Edit(int Id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedView();

            var campground = _campgroundService.GetCampgroundById(Id);
            if (campground == null || campground.Deleted)
                //No campground found with the specified id
                return RedirectToAction("List");

            ////a campgroundHost should have access only to his campgrounds
            //if (_workContext.CurrentCampgroundHost != null)
            //    return RedirectToAction("List");

            ////a campgroundHost should have access only to his campgrounds
            //if (_workContext.CurrentCampgroundHost != null)
            //{
            //    var campground = _campgroundService.GetCampgroundById(campgroundId);
            //    if (campground != null)
            //    {
            //        return Content("This is not your campground");
            //    }
            //}

            var model = campground.ToModel();
            //var addressModel = new CampgroundAddressModel();
            _campgroundModelFactory.PrepareCampgroundModel(model, campground, false, false);

            AddLocales(_languageService, model.Locales, (locale, languageId) =>
            {
                locale.Name = campground.GetLocalized(x => x.Name, languageId, false, false);
                locale.ShortDescription = campground.GetLocalized(x => x.ShortDescription, languageId, false, false);
                locale.FullDescription = campground.GetLocalized(x => x.FullDescription, languageId, false, false);
                locale.MetaKeywords = campground.GetLocalized(x => x.MetaKeywords, languageId, false, false);
                locale.MetaDescription = campground.GetLocalized(x => x.MetaDescription, languageId, false, false);
                locale.MetaTitle = campground.GetLocalized(x => x.MetaTitle, languageId, false, false);
                locale.SeName = campground.GetSeName(languageId, false, false);
            });
            //campground hosts
            model.AvailableCampgroundHosts = PrepareAvailableCampgroundHosts(selectedId: _workContext.CurrentCampgroundHost != null ? _workContext.CurrentCampgroundHost.Id : 0);

            //campground types
            model.AvailableCampgroundTypes = PrepareAvailableCampgroundTypes();

            //model.CampgroundAddress = addressModel;

            //PrepareAclModel(model, campground, false);
            PrepareCategoryMappingModel(model, campground, false);
            
            return View(EDIT_VIEW, model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Edit(CampgroundModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedView();

            var campground = _campgroundService.GetCampgroundById(model.Id);

            if (campground == null || campground.Deleted)
                //No campground found with the specified id
                return RedirectToAction("List");

            //check if the campground quantity has been changed while we were editing the campground
            //and if it has been changed then we show error notification
            //and redirect on the editing page without data saving
            if (ModelState.IsValid)
            {
                //campground
                campground = model.ToEntity(campground);
                //campground.CampgroundAddress = model.CampgroundAddress.Address;
                campground.CampgroundAddress.UpdatedOnUtc = campground.UpdatedOnUtc = DateTime.UtcNow;
                _campgroundService.UpdateCampground(campground);

                //search engine name
                model.SeName = campground.ValidateSeName(model.SeName, campground.Name, true);
                _urlRecordService.SaveSlug(campground, model.SeName, 0);
                
                //locales
                UpdateLocales(campground, model);
                
                //tags
                _campgroundTagService.UpdateCampgroundTags(campground, ParseCampgroundTags(model.CampgroundTags));
                
                //categories
                SaveCategoryMappings(campground, model);
                
                //ACL (campground roles)
                //SaveCampgroundAcl(campground, model);
                
                //picture seo names
                UpdatePictureSeoNames(campground);

                ////back in stock notifications
                // **** CAN USE THIS WHEN READY TO ADD CAMPSITES **** //
                //if (campground.ManageInventoryMethod == ManageInventoryMethod.ManageStock &&
                //    campground.BackorderMode == BackorderMode.NoBackorders &&
                //    campground.AllowBackInStockSubscriptions &&
                //    campground.GetTotalStockQuantity() > 0 &&
                //    prevTotalStockQuantity <= 0 &&
                //    campground.Published &&
                //    !campground.Deleted)
                //{
                //    _backInStockSubscriptionService.SendNotificationsToSubscribers(campground);
                //}

                //activity log
                _customerActivityService.InsertActivity("EditCampground", _localizationService.GetResource("ActivityLog.EditCampground"), campground.Name);

                SuccessNotification(_localizationService.GetResource("Admin.Campgrounds.Updated"));

                if (continueEditing)
                {
                    //selected tab
                    SaveSelectedTabName();

                    return RedirectToAction("Edit", new { id = campground.Id });
                }
                return RedirectToAction("List");
            }

            //If we got this far, something failed, redisplay form
            _campgroundModelFactory.PrepareCampgroundModel(model, campground, false, true);
            //PrepareAclModel(model, campground, true);
            PrepareCategoryMappingModel(model, campground, true);

            return View(EDIT_VIEW, model);
        }

        //delete campground
        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedView();

            var campground = _campgroundService.GetCampgroundById(id);
            if (campground == null)
                //No campground found with the specified id
                return RedirectToAction("List");

            _campgroundService.DeleteCampground(campground);

            //activity log
            _customerActivityService.InsertActivity("DeleteCampground", _localizationService.GetResource("ActivityLog.DeleteCampground"), campground.Name);

            SuccessNotification(_localizationService.GetResource("Admin.Campgrounds.Deleted"));
            return RedirectToAction("List");
        }

        [HttpPost]
        public virtual IActionResult DeleteSelected(ICollection<int> selectedIds)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedView();

            if (selectedIds != null)
            {
                _campgroundService.DeleteCampgrounds(_campgroundService.GetCampgroundsByIds(selectedIds.ToArray()).Where(c => _workContext.CurrentCampgroundHost == null).ToList());
            }

            return Json(new { Result = true });
        }

        //[HttpPost]
        //public virtual IActionResult CopyCampground(CampgroundModel model)
        //{
        //    if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
        //        return AccessDeniedView();

        //    var copyModel = model.CopyCampgroundModel;
        //    try
        //    {
        //        var originalCampground = _campgroundService.GetCampgroundById(copyModel.Id);

        //        var newCampground = _copyCampgroundService.CopyCampground(originalCampground,
        //            copyModel.Name, copyModel.Published, copyModel.CopyImages);
        //        SuccessNotification(_localizationService.GetResource("Admin.Campgrounds.Copied"));
        //        return RedirectToAction("Edit", new { id = newCampground.Id });
        //    }
        //    catch (Exception exc)
        //    {
        //        ErrorNotification(exc.Message);
        //        return RedirectToAction("Edit", new { id = copyModel.Id });
        //    }
        //}

        #endregion

        #region Associated campgrounds

        [HttpPost]
        public virtual IActionResult AssociatedCampgroundList(DataSourceRequest command, int campgroundId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedKendoGridJson();

            var associatedCampgrounds = _campgroundService.GetAssociatedCampgrounds(parentGroupedCampgroundId: campgroundId,
                showHidden: true);
            var associatedCampgroundsModel = associatedCampgrounds
                .Select(x => new CampgroundModel.AssociatedCampgroundModel
                {
                    Id = x.Id,
                    CampgroundName = x.Name,
                    DisplayOrder = x.DisplayOrder
                })
                .ToList();

            var gridModel = new DataSourceResult
            {
                Data = associatedCampgroundsModel,
                Total = associatedCampgroundsModel.Count
            };

            return Json(gridModel);
        }

        [HttpPost]
        public virtual IActionResult AssociatedCampgroundUpdate(CampgroundModel.AssociatedCampgroundModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedView();

            var associatedCampground = _campgroundService.GetCampgroundById(model.Id);
            if (associatedCampground == null)
                throw new ArgumentException("No associated campground found with the specified id");

            associatedCampground.DisplayOrder = model.DisplayOrder;
            _campgroundService.UpdateCampground(associatedCampground);

            return new NullJsonResult();
        }

        [HttpPost]
        public virtual IActionResult AssociatedCampgroundDelete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedView();

            var campground = _campgroundService.GetCampgroundById(id);
            if (campground == null)
                throw new ArgumentException("No associated campground found with the specified id");

            campground.ParentGroupedCampgroundId = 0;
            _campgroundService.UpdateCampground(campground);

            return new NullJsonResult();
        }

        public virtual IActionResult AssociatedCampgroundAddPopup(int campgroundId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedView();

            var model = new CampgroundModel.AddAssociatedCampgroundModel
            {
                //a campgroundHost should have access only to his campgrounds
                IsLoggedInAsCampgroundHost = _workContext.CurrentCampgroundHost != null
            };

            //state categories
            model.AvailableCategories = PrepareAvailableStateCategories();

            //campground hosts
            model.AvailableCampgroundHosts = PrepareAvailableCampgroundHosts();

            //campground types
            model.AvailableCampgroundTypes = PrepareAvailableCampgroundTypes();

            return View(ASSOC_CAMPGROUND_ADD_POP_VIEW, model);
        }

        [HttpPost]
        public virtual IActionResult AssociatedCampgroundAddPopupList(DataSourceRequest command, CampgroundModel.AddAssociatedCampgroundModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedKendoGridJson();

            var campgrounds = _campgroundService.SearchCampgrounds(
                categoryIds: new List<int> { model.SearchCategoryId },
                campgroundHostId: model.SearchCampgroundHostId,
                campgroundTypeId: model.SearchCampgroundTypeId,
                keywords: model.SearchCampgroundName,
                pageIndex: command.Page - 1,
                pageSize: command.PageSize,
                showHidden: true
                );
            var gridModel = new DataSourceResult
            {
                Data = campgrounds.Select(x =>
                {
                    var campgroundModel = x.ToModel();
                    //display already associated campgrounds
                    var parentGroupedCampground = _campgroundService.GetCampgroundById(x.ParentGroupedCampgroundId);
                    if (parentGroupedCampground != null)
                    {
                        campgroundModel.AssociatedToCampgroundId = x.ParentGroupedCampgroundId;
                        campgroundModel.AssociatedToCampgroundName = parentGroupedCampground.Name;
                    }
                    return campgroundModel;
                }),
                Total = campgrounds.TotalCount
            };

            return Json(gridModel);
        }

        [HttpPost]
        [FormValueRequired("save")]
        public virtual IActionResult AssociatedCampgroundAddPopup(CampgroundModel.AddAssociatedCampgroundModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedView();

            if (model.SelectedCampgroundIds != null)
            {
                foreach (var id in model.SelectedCampgroundIds)
                {
                    var campground = _campgroundService.GetCampgroundById(id);
                    if (campground != null)
                    {
                        campground.ParentGroupedCampgroundId = model.CampgroundId;
                        _campgroundService.UpdateCampground(campground);
                    }
                }
            }

            //a campgroundHost should have access only to his campgrounds
            model.IsLoggedInAsCampgroundHost = _workContext.CurrentCampgroundHost != null;
            ViewBag.RefreshPage = true;
            return View(ASSOC_CAMPGROUND_ADD_POP_VIEW, model);
        }

        #endregion

        #region Required campgrounds

        [HttpPost]
        public virtual IActionResult LoadCampgroundFriendlyNames(string campgroundIds)
        {
            var result = "";

            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return Json(new { Text = result });

            if (!string.IsNullOrWhiteSpace(campgroundIds))
            {
                var ids = new List<int>();
                var rangeArray = campgroundIds
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .ToList();

                foreach (var str1 in rangeArray)
                {
                    if (int.TryParse(str1, out int tmp1))
                        ids.Add(tmp1);
                }

                var campgrounds = _campgroundService.GetCampgroundsByIds(ids.ToArray());
                for (var i = 0; i <= campgrounds.Count - 1; i++)
                {
                    result += campgrounds[i].Name;
                    if (i != campgrounds.Count - 1)
                        result += ", ";
                }
            }

            return Json(new { Text = result });
        }

        public virtual IActionResult RequiredCampgroundAddPopup()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedView();

            var model = new CampgroundModel.AddRequiredCampgroundModel
            {
                //a campgroundHost should have access only to his campgrounds
                IsLoggedInAsCampgroundHost = _workContext.CurrentCampgroundHost != null
            };

            //state categories
            model.AvailableCategories = PrepareAvailableStateCategories();

            //campground hosts
            model.AvailableCampgroundHosts = PrepareAvailableCampgroundHosts();

            //campground types
            model.AvailableCampgroundTypes = PrepareAvailableCampgroundTypes();

            return View(REQUIRED_CAMPGROUND_VIEW, model);
        }

        [HttpPost]
        public virtual IActionResult RequiredCampgroundAddPopupList(DataSourceRequest command, CampgroundModel.AddRequiredCampgroundModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedKendoGridJson();

            var campgrounds = _campgroundService.SearchCampgrounds(
                categoryIds: new List<int> { model.SearchCategoryId },
                campgroundHostId: model.SearchCampgroundHostId,
                campgroundTypeId: model.SearchCampgroundTypeId,
                keywords: model.SearchCampgroundName,
                pageIndex: command.Page - 1,
                pageSize: command.PageSize,
                showHidden: true
                );
            var gridModel = new DataSourceResult
            {
                Data = campgrounds.Select(x => x.ToModel()),
                Total = campgrounds.TotalCount
            };

            return Json(gridModel);
        }

        #endregion

        #region Campground pictures

        public virtual IActionResult CampgroundPictureAdd(int pictureId, int displayOrder,
            string overrideAltAttribute, string overrideTitleAttribute,
            int campgroundId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedView();

            if (pictureId == 0)
                throw new ArgumentException();

            var campground = _campgroundService.GetCampgroundById(campgroundId);
            if (campground == null)
                throw new ArgumentException("No campground found with the specified id");

            var picture = _pictureService.GetPictureById(pictureId);
            if (picture == null)
                throw new ArgumentException("No picture found with the specified id");

            _pictureService.UpdatePicture(picture.Id,
                _pictureService.LoadPictureBinary(picture),
                picture.MimeType,
                picture.SeoFilename,
                overrideAltAttribute,
                overrideTitleAttribute);

            _pictureService.SetSeoFilename(pictureId, _pictureService.GetPictureSeName(campground.Name));

            _campgroundService.InsertCampgroundPicture(new CampgroundPicture
            {
                PictureId = pictureId,
                CampgroundId = campgroundId,
                DisplayOrder = displayOrder,
            });

            return Json(new { Result = true });
        }

        [HttpPost]
        public virtual IActionResult CampgroundPictureList(DataSourceRequest command, int campgroundId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedKendoGridJson();

            var campgroundPictures = _campgroundService.GetCampgroundPicturesByCampgroundId(campgroundId);
            var campgroundPicturesModel = campgroundPictures
                .Select(x =>
                {
                    var picture = _pictureService.GetPictureById(x.PictureId);
                    if (picture == null)
                        throw new Exception("Picture cannot be loaded");
                    var m = new CampgroundModel.CampgroundPictureModel
                    {
                        Id = x.Id,
                        CampgroundId = x.CampgroundId,
                        PictureId = x.PictureId,
                        PictureUrl = _pictureService.GetPictureUrl(picture),
                        OverrideAltAttribute = picture.AltAttribute,
                        OverrideTitleAttribute = picture.TitleAttribute,
                        DisplayOrder = x.DisplayOrder
                    };
                    return m;
                })
                .ToList();

            var gridModel = new DataSourceResult
            {
                Data = campgroundPicturesModel,
                Total = campgroundPicturesModel.Count
            };

            return Json(gridModel);
        }

        [HttpPost]
        public virtual IActionResult CampgroundPictureUpdate(CampgroundModel.CampgroundPictureModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedView();

            var campgroundPicture = _campgroundService.GetCampgroundPictureById(model.Id);
            if (campgroundPicture == null)
                throw new ArgumentException("No campground picture found with the specified id");

            var picture = _pictureService.GetPictureById(campgroundPicture.PictureId);
            if (picture == null)
                throw new ArgumentException("No picture found with the specified id");

            _pictureService.UpdatePicture(picture.Id,
                _pictureService.LoadPictureBinary(picture),
                picture.MimeType,
                picture.SeoFilename,
                model.OverrideAltAttribute,
                model.OverrideTitleAttribute);

            campgroundPicture.DisplayOrder = model.DisplayOrder;
            _campgroundService.UpdateCampgroundPicture(campgroundPicture);

            return new NullJsonResult();
        }

        [HttpPost]
        public virtual IActionResult CampgroundPictureDelete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedView();

            var campgroundPicture = _campgroundService.GetCampgroundPictureById(id);
            if (campgroundPicture == null)
                throw new ArgumentException("No campground picture found with the specified id");

            var campgroundId = campgroundPicture.CampgroundId;

            var pictureId = campgroundPicture.PictureId;
            _campgroundService.DeleteCampgroundPicture(campgroundPicture);

            var picture = _pictureService.GetPictureById(pictureId);
            if (picture == null)
                throw new ArgumentException("No picture found with the specified id");
            _pictureService.DeletePicture(picture);

            return new NullJsonResult();
        }

        #endregion

        #region Campground tags

        public virtual IActionResult CampgroundTags()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgroundTags))
                return AccessDeniedView();

            return View();
        }

        [HttpPost]
        public virtual IActionResult CampgroundTags(DataSourceRequest command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgroundTags))
                return AccessDeniedKendoGridJson();

            var tags = _campgroundTagService.GetAllCampgroundTags()
                //order by campground count
                .OrderByDescending(x => _campgroundTagService.GetCampgroundCount(x.Id, 0))
                .Select(x => new CampgroundTagModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    CampgroundCount = _campgroundTagService.GetCampgroundCount(x.Id, 0)
                })
                .ToList();

            var gridModel = new DataSourceResult
            {
                Data = tags.PagedForCommand(command),
                Total = tags.Count
            };

            return Json(gridModel);
        }

        [HttpPost]
        public virtual IActionResult CampgroundTagDelete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgroundTags))
                return AccessDeniedView();

            var tag = _campgroundTagService.GetCampgroundTagById(id);
            if (tag == null)
                throw new ArgumentException("No campground tag found with the specified id");
            _campgroundTagService.DeleteCampgroundTag(tag);

            return new NullJsonResult();
        }

        //edit
        public virtual IActionResult EditCampgroundTag(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgroundTags))
                return AccessDeniedView();

            var campgroundTag = _campgroundTagService.GetCampgroundTagById(id);
            if (campgroundTag == null)
                //No campground tag found with the specified id
                return RedirectToAction("List");

            var model = new CampgroundTagModel
            {
                Id = campgroundTag.Id,
                Name = campgroundTag.Name,
                CampgroundCount = _campgroundTagService.GetCampgroundCount(campgroundTag.Id, 0)
            };
            //locales
            AddLocales(_languageService, model.Locales, (locale, languageId) =>
            {
                locale.Name = campgroundTag.GetLocalized(x => x.Name, languageId, false, false);
            });

            return View(EDIT_TAG_VIEW, model);
        }

        [HttpPost]
        public virtual IActionResult EditCampgroundTag(CampgroundTagModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgroundTags))
                return AccessDeniedView();

            var campgroundTag = _campgroundTagService.GetCampgroundTagById(model.Id);
            if (campgroundTag == null)
                //No campground tag found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                campgroundTag.Name = model.Name;
                _campgroundTagService.UpdateCampgroundTag(campgroundTag);
                //locales
                UpdateLocales(campgroundTag, model);

                ViewBag.RefreshPage = true;
                return View(EDIT_TAG_VIEW, model);
            }

            //If we got this far, something failed, redisplay form
            return View(EDIT_TAG_VIEW, model);
        }

        #endregion

        #region Related campgrounds

        [HttpPost]
        public virtual IActionResult RelatedCampgroundList(DataSourceRequest command, int campgroundId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedKendoGridJson();

            var relatedCampgrounds = _campgroundService.GetRelatedCampgroundsByCampgroundId1(campgroundId, true);
            var relatedCampgroundsModel = relatedCampgrounds
                .Select(x => new CampgroundModel.RelatedCampgroundModel
                {
                    Id = x.Id,
                    //CampgroundId1 = x.CampgroundId1,
                    CampgroundId2 = x.CampgroundId2,
                    Campground2Name = _campgroundService.GetCampgroundById(x.CampgroundId2).Name,
                    DisplayOrder = x.DisplayOrder
                })
                .ToList();

            var gridModel = new DataSourceResult
            {
                Data = relatedCampgroundsModel,
                Total = relatedCampgroundsModel.Count
            };

            return Json(gridModel);
        }

        [HttpPost]
        public virtual IActionResult RelatedCampgroundUpdate(CampgroundModel.RelatedCampgroundModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedView();

            var relatedCampground = _campgroundService.GetRelatedCampgroundById(model.Id);
            if (relatedCampground == null)
                throw new ArgumentException("No related campground found with the specified id");

            relatedCampground.DisplayOrder = model.DisplayOrder;
            _campgroundService.UpdateRelatedCampground(relatedCampground);

            return new NullJsonResult();
        }

        [HttpPost]
        public virtual IActionResult RelatedCampgroundDelete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedView();

            var relatedCampground = _campgroundService.GetRelatedCampgroundById(id);
            if (relatedCampground == null)
                throw new ArgumentException("No related campground found with the specified id");

            var campgroundId = relatedCampground.CampgroundId1;

            _campgroundService.DeleteRelatedCampground(relatedCampground);

            return new NullJsonResult();
        }

        public virtual IActionResult RelatedCampgroundAddPopup(int campgroundId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedView();

            var model = new CampgroundModel.AddRelatedCampgroundModel
            {
                //a campgroundHost should have access only to his campgrounds
                IsLoggedInAsCampgroundHost = _workContext.CurrentCampgroundHost != null
            };

            //state categories
            model.AvailableCategories = PrepareAvailableStateCategories();

            //campground hosts
            model.AvailableCampgroundHosts = PrepareAvailableCampgroundHosts();

            //campground types
            model.AvailableCampgroundTypes = PrepareAvailableCampgroundTypes();

            return View(RELATED_CAMPGROUND_VIEW, model);
        }

        [HttpPost]
        public virtual IActionResult RelatedCampgroundAddPopupList(DataSourceRequest command, CampgroundModel.AddRelatedCampgroundModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedKendoGridJson();

            var campgrounds = _campgroundService.SearchCampgrounds(
                categoryIds: new List<int> { model.SearchCategoryId },
                campgroundHostId: model.SearchCampgroundHostId,
                campgroundTypeId: model.SearchCampgroundTypeId,
                keywords: model.SearchCampgroundName,
                pageIndex: command.Page - 1,
                pageSize: command.PageSize,
                showHidden: true
                );
            var gridModel = new DataSourceResult
            {
                Data = campgrounds.Select(x => x.ToModel()),
                Total = campgrounds.TotalCount
            };

            return Json(gridModel);
        }

        [HttpPost]
        [FormValueRequired("save")]
        public virtual IActionResult RelatedCampgroundAddPopup(CampgroundModel.AddRelatedCampgroundModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedView();

            if (model.SelectedCampgroundIds != null)
            {
                foreach (var id in model.SelectedCampgroundIds)
                {
                    var campground = _campgroundService.GetCampgroundById(id);
                    if (campground != null)
                    {
                        var existingRelatedCampgrounds = _campgroundService.GetRelatedCampgroundsByCampgroundId1(model.CampgroundId);
                        if (existingRelatedCampgrounds.FindRelatedCampground(model.CampgroundId, id) == null)
                        {
                            _campgroundService.InsertRelatedCampground(
                                new RelatedCampground
                                {
                                    CampgroundId1 = model.CampgroundId,
                                    CampgroundId2 = id,
                                    DisplayOrder = 1
                                });
                        }
                    }
                }
            }

            //a campgroundHost should have access only to his campgrounds
            model.IsLoggedInAsCampgroundHost = _workContext.CurrentCampgroundHost != null;
            ViewBag.RefreshPage = true;
            return View(RELATED_CAMPGROUND_VIEW, model);
        }

        #endregion

        #region Cross-sell campgrounds
        //Campground/CrossSellCampgroundList?campgroundId=101
        [HttpPost]
        public virtual IActionResult CrossSellCampgroundList(DataSourceRequest command, int campgroundId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedKendoGridJson();

            var crossSellCampgrounds = _campgroundService.GetCrossSellCampgroundsByCampgroundId1(campgroundId, true);
            var crossSellCampgroundsModel = crossSellCampgrounds
                .Select(x => new CampgroundModel.CrossSellCampgroundModel
                {
                    Id = x.Id,
                    //CampgroundId1 = x.CampgroundId1,
                    CampgroundId2 = x.CampgroundId2,
                    Campground2Name = _campgroundService.GetCampgroundById(x.CampgroundId2).Name,
                })
                .ToList();

            var gridModel = new DataSourceResult
            {
                Data = crossSellCampgroundsModel,
                Total = crossSellCampgroundsModel.Count
            };

            return Json(gridModel);
        }

        [HttpPost]
        public virtual IActionResult CrossSellCampgroundDelete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedView();

            var crossSellCampground = _campgroundService.GetCrossSellCampgroundById(id);
            if (crossSellCampground == null)
                throw new ArgumentException("No cross-sell campground found with the specified id");

            var campgroundId = crossSellCampground.CampgroundId1;

            _campgroundService.DeleteCrossSellCampground(crossSellCampground);

            return new NullJsonResult();
        }

        public virtual IActionResult CrossSellCampgroundAddPopup(int campgroundId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedView();

            var model = new CampgroundModel.AddCrossSellCampgroundModel
            {
                //a campgroundHost should have access only to his campgrounds
                IsLoggedInAsCampgroundHost = _workContext.CurrentCampgroundHost != null
            };

            //state categories
            model.AvailableCategories = PrepareAvailableStateCategories();

            //campground hosts
            model.AvailableCampgroundHosts = PrepareAvailableCampgroundHosts();

            //campground types
            model.AvailableCampgroundTypes = PrepareAvailableCampgroundTypes();

            return View(CROSS_SELL_ADD_POP_VIEW, model);
        }

        [HttpPost]
        public virtual IActionResult CrossSellCampgroundAddPopupList(DataSourceRequest command, CampgroundModel.AddCrossSellCampgroundModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedKendoGridJson();

            var campgrounds = _campgroundService.SearchCampgrounds(
                categoryIds: new List<int> { model.SearchCategoryId },
                campgroundHostId: model.SearchCampgroundHostId,
                campgroundTypeId: model.SearchCampgroundTypeId,
                keywords: model.SearchCampgroundName,
                pageIndex: command.Page - 1,
                pageSize: command.PageSize,
                showHidden: true
                );
            var gridModel = new DataSourceResult
            {
                Data = campgrounds.Select(x => x.ToModel()),
                Total = campgrounds.TotalCount
            };

            return Json(gridModel);
        }

        [HttpPost]
        [FormValueRequired("save")]
        public virtual IActionResult CrossSellCampgroundAddPopup(CampgroundModel.AddCrossSellCampgroundModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedView();

            if (model.SelectedCampgroundIds != null)
            {
                foreach (var id in model.SelectedCampgroundIds)
                {
                    var campground = _campgroundService.GetCampgroundById(id);
                    if (campground != null)
                    {
                        var existingCrossSellCampgrounds = _campgroundService.GetCrossSellCampgroundsByCampgroundId1(model.CampgroundId);
                        if (existingCrossSellCampgrounds.FindCrossSellCampground(model.CampgroundId, id) == null)
                        {
                            _campgroundService.InsertCrossSellCampground(
                                new CrossSellCampground
                                {
                                    CampgroundId1 = model.CampgroundId,
                                    CampgroundId2 = id,
                                });
                        }
                    }
                }
            }

            //a campgroundHost should have access only to his campgrounds
            model.IsLoggedInAsCampgroundHost = _workContext.CurrentCampgroundHost != null;
            ViewBag.RefreshPage = true;
            return View(CROSS_SELL_ADD_POP_VIEW, model);
        }

        #endregion

        #region Campground Addresses

        ////[HttpPost]
        //public virtual IActionResult CampgroundAddressesSelect(int campgroundId, DataSourceRequest command)
        //{
        //    if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
        //        return AccessDeniedKendoGridJson();

        //    var campground = _campgroundService.GetCampgroundById(campgroundId);
        //    if (campground == null)
        //        throw new ArgumentException("No campground address found with the specified id", "campgroundId");

        //    var addresses = campground.Addresses.OrderByDescending(a => a.CreatedOnUtc).ThenByDescending(a => a.Id).ToList();
        //    var gridModel = new DataSourceResult
        //    {
        //        Data = addresses.Select(x =>
        //        {
        //            var model = new CampgroundAddressModel();
        //            model.Address = new CampgroundAddress
        //            {
        //                Id = x.Id,
        //                FirstName = x.FirstName,
        //                LastName = x.LastName,
        //                Email = x.Email,
        //                Company = x.Company,
        //                CountryId = x.CountryId,
        //                StateProvinceId = x.StateProvinceId,
        //                City = x.City,
        //                Address1 = x.Address1,
        //                Address2 = x.Address2,
        //                ZipPostalCode = x.ZipPostalCode,
        //                PhoneNumber = x.PhoneNumber,
        //                FaxNumber = x.FaxNumber,
        //                Latitude = x.Latitude,
        //                Longitude = x.Longitude,
        //                GooglePlaceId = x.GooglePlaceId,
        //                GoogleGeocodeURL = x.GoogleGeocodeURL
        //            };
        //            var addressHtmlSb = new StringBuilder("<div>");
        //            if (_addressSettings.CompanyEnabled && !string.IsNullOrEmpty(x.Company))
        //                addressHtmlSb.AppendFormat("{0}<br />", WebUtility.HtmlEncode(x.Company));
        //            if (_addressSettings.StreetAddressEnabled && !string.IsNullOrEmpty(x.Address1))
        //                addressHtmlSb.AppendFormat("{0}<br />", WebUtility.HtmlEncode(x.Address1));
        //            if (_addressSettings.StreetAddress2Enabled && !string.IsNullOrEmpty(x.Address2))
        //                addressHtmlSb.AppendFormat("{0}<br />", WebUtility.HtmlEncode(x.Address2));
        //            if (_addressSettings.CityEnabled && !string.IsNullOrEmpty(x.City))
        //                addressHtmlSb.AppendFormat("{0},", WebUtility.HtmlEncode(x.City));
        //            if (_addressSettings.StateProvinceEnabled && !string.IsNullOrEmpty(x.StateProvince.Name))
        //                addressHtmlSb.AppendFormat("{0},", WebUtility.HtmlEncode(x.StateProvince.Name));
        //            if (_addressSettings.ZipPostalCodeEnabled && !string.IsNullOrEmpty(x.ZipPostalCode))
        //                addressHtmlSb.AppendFormat("{0}<br />", WebUtility.HtmlEncode(x.ZipPostalCode));
        //            if (_addressSettings.CountryEnabled && !string.IsNullOrEmpty(x.StateProvince.Country.Name))
        //                addressHtmlSb.AppendFormat("{0}", WebUtility.HtmlEncode(x.StateProvince.Country.Name));
        //            var customAttributesFormatted = _addressAttributeFormatter.FormatAttributes(x.CustomAttributes);
        //            if (!string.IsNullOrEmpty(customAttributesFormatted))
        //            {
        //                //already encoded
        //                addressHtmlSb.AppendFormat("<br />{0}", customAttributesFormatted);
        //            }
        //            addressHtmlSb.Append("</div>");

        //            model.AddressHtml = addressHtmlSb.ToString();
        //            model.CampgroundId = campground.Id;
        //            return model;
        //        }),
        //        Total = addresses.Count
        //    };

        //    return Json(gridModel);
        //}

        //[HttpPost]
        //public virtual IActionResult CampgroundAddressDelete(int id, int campgroundId)
        //{
        //    if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
        //        return AccessDeniedView();

        //    var campground = _campgroundService.GetCampgroundById(campgroundId);
        //    if (campground == null)
        //        throw new ArgumentException("No campground found with the specified id", "campgroundId");

        //    var campgroundAddress = campground.CampgroundAddress;
        //    if (campgroundAddress == null)
        //        //No campground found with the specified id
        //        return Content("No campground found with the specified id");
        //    campground.RemoveAddress(campgroundAddress);
        //    _campgroundService.UpdateCampground(campground);
        //    //now delete the campgroundAddress record
        //    _campgroundAddressService.DeleteCampgroundAddress(campgroundAddress);

        //    return new NullJsonResult();
        //}

        //public virtual IActionResult CampgroundAddressCreate(int campgroundId)
        //{
        //    var model = new CampgroundAddressModel();
        //    if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
        //        return AccessDeniedView();

        //    var campground = _campgroundService.GetCampgroundById(campgroundId);
        //    if (campground == null)
        //        //No campground found with the specified id
        //        return RedirectToAction("List");

        //    _campgroundModelFactory.PrepareCampgroundAddressModel(model, campground);

        //    return View(model);
        //}

        ////[HttpPost]
        ////public virtual IActionResult CampgroundAddressCreate(CampgroundAddressModel model)
        ////{
        ////    if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
        ////        return AccessDeniedView();

        ////    var campground = _campgroundService.GetCampgroundById(model.CampgroundId);
        ////    if (campground == null)
        ////        //No campground found with the specified id
        ////        return RedirectToAction("List");

        ////    //custom campgroundAddress attributes
        ////    var customAttributes = model.Form.ParseCustomAddressAttributes(_addressAttributeParser, _addressAttributeService);
        ////    var customAttributeWarnings = _addressAttributeParser.GetAttributeWarnings(customAttributes);
        ////    foreach (var error in customAttributeWarnings)
        ////    {
        ////        ModelState.AddModelError("", error);
        ////    }

        ////    if (ModelState.IsValid)
        ////    {
        ////        var campgroundAddress = model.ToEntity();
        ////        campgroundAddress.CustomAttributes = customAttributes;
        ////        campgroundAddress.CreatedOnUtc = DateTime.UtcNow;
        ////        //some validation
        ////        if (campgroundAddress.CountryId == 0)
        ////            campgroundAddress.CountryId = null;
        ////        if (campgroundAddress.StateProvinceId == 0)
        ////            campgroundAddress.StateProvinceId = null;
        ////        campground.CampgroundAddress.Add(campgroundAddress);
        ////        _campgroundService.UpdateCampground(campground);

        ////        SuccessNotification(_localizationService.GetResource("Admin.Campgrounds.Campgrounds.CampgroundAddress.Added"));
        ////        return RedirectToAction("AddressEdit", new { campgroundAddressId = campgroundAddress.Id, campgroundId = model.CampgroundId });
        ////    }

        ////    //If we got this far, something failed, redisplay form
        ////    model = _campgroundModelFactory.PrepareCampgroundAddressModel(null, campground);
        ////    return View(model);
        ////}

        //public virtual IActionResult CampgroundAddressEdit(int campgroundAddressId, int campgroundId)
        //{
        //    var model = new CampgroundAddressModel();
        //    if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
        //        return AccessDeniedView();

        //    var campground = _campgroundService.GetCampgroundById(campgroundId);
        //    if (campground == null)
        //        //No campground found with the specified id
        //        return RedirectToAction("List");

        //    //var campgroundAddress = _campgroundAddressService.GetCampgroundAddressById(campgroundAddressId);
        //    //if (campgroundAddress == null)
        //    //    //No campgroundAddress found with the specified id
        //    //    return RedirectToAction("Edit", new { id = campground.Id });

        //    _campgroundModelFactory.PrepareCampgroundAddressModel(model, campground);
        //    return View(model);
        //}

        //[HttpPost]
        //public virtual IActionResult CampgroundAddressEdit(CampgroundAddressModel model)
        //{
        //    if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
        //        return AccessDeniedView();

        //    var campground = _campgroundService.GetCampgroundById(model.CampgroundId);
        //    if (campground == null)
        //        //No campground found with the specified id
        //        return RedirectToAction("List");

        //    var campgroundAddress = _campgroundAddressService.GetCampgroundAddressById(model.CampgroundAddress.Id);
        //    if (campgroundAddress == null)
        //        //No campgroundAddress found with the specified id
        //        return RedirectToAction("Edit", new { id = campground.Id });

        //    //custom campgroundAddress attributes
        //    var customAttributes = model.Form.ParseCustomAddressAttributes(_campgroundAddressAttributeParser, _campgroundAddressAttributeService);
        //    var customAttributeWarnings = _campgroundAddressAttributeParser.GetAttributeWarnings(customAttributes);
        //    foreach (var error in customAttributeWarnings)
        //    {
        //        ModelState.AddModelError("", error);
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        campgroundAddress = model.Address.ToEntity(campgroundAddress);
        //        campgroundAddress.CustomAttributes = customAttributes;
        //        _campgroundAddressService.UpdateAddress(campgroundAddress);

        //        SuccessNotification(_localizationService.GetResource("Admin.Campgrounds.Campgrounds.CampgroundAddress.Updated"));
        //        return RedirectToAction("AddressEdit", new { campgroundAddressId = model.Address.Id, campgroundId = model.CampgroundId });
        //    }

        //    //If we got this far, something failed, redisplay form
        //    model = _campgroundModelFactory.PrepareCampgroundAddressModel(campgroundAddress, campground);

        //    return View(model);
        //}

        #endregion

        #region Export / Import

        //[HttpPost, ActionName("List")]
        //[FormValueRequired("download-catalog-pdf")]
        //public virtual IActionResult DownloadCampgroundsAsPdf(CampgroundListModel model)
        //{
        //    if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
        //        return AccessDeniedView();

        //    //a campgroundHost should have access only to his campgrounds
        //    if (_workContext.CurrentCampgroundHost != null)
        //    {
        //        model.SearchCampgroundHostId = _workContext.CurrentCampgroundHost.Id;
        //    }

        //    var categoryIds = new List<int> { model.SearchCategoryId };
        //    //include subcategories
        //    if (model.SearchIncludeSubCategories && model.SearchCategoryId > 0)
        //        categoryIds.AddRange(GetChildCategoryIds(model.SearchCategoryId));

        //    //0 - all (according to "ShowHidden" parameter)
        //    //1 - published only
        //    //2 - unpublished only
        //    bool? overridePublished = null;
        //    if (model.SearchPublishedId == 1)
        //        overridePublished = true;
        //    else if (model.SearchPublishedId == 2)
        //        overridePublished = false;

        //    var campgrounds = _campgroundService.SearchCampgrounds(
        //        categoryIds: categoryIds,
        //        campgroundHostId: model.SearchCampgroundHostId,
        //        campgroundTypeId: model.SearchCampgroundTypeId,
        //        keywords: model.SearchCampgroundName,
        //        showHidden: true,
        //        overridePublished: overridePublished
        //    );

        //    try
        //    {
        //        byte[] bytes;
        //        using (var stream = new MemoryStream())
        //        {
        //            _pdfService.PrintCampgroundsToPdf(stream, campgrounds);
        //            bytes = stream.ToArray();
        //        }
        //        return File(bytes, MimeTypes.ApplicationPdf, "pdfcatalog.pdf");
        //    }
        //    catch (Exception exc)
        //    {
        //        ErrorNotification(exc);
        //        return RedirectToAction("List");
        //    }
        //}

        //[HttpPost, ActionName("List")]
        //[FormValueRequired("exportxml-all")]
        //public virtual IActionResult ExportXmlAll(CampgroundListModel model)
        //{
        //    if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
        //        return AccessDeniedView();

        //    //a campgroundHost should have access only to his campgrounds
        //    if (_workContext.CurrentCampgroundHost != null)
        //    {
        //        model.SearchCampgroundHostId = _workContext.CurrentCampgroundHost.Id;
        //    }

        //    var categoryIds = new List<int> { model.SearchCategoryId };
        //    //include subcategories
        //    if (model.SearchIncludeSubCategories && model.SearchCategoryId > 0)
        //        categoryIds.AddRange(GetChildCategoryIds(model.SearchCategoryId));

        //    //0 - all (according to "ShowHidden" parameter)
        //    //1 - published only
        //    //2 - unpublished only
        //    bool? overridePublished = null;
        //    if (model.SearchPublishedId == 1)
        //        overridePublished = true;
        //    else if (model.SearchPublishedId == 2)
        //        overridePublished = false;

        //    var campgrounds = _campgroundService.SearchCampgrounds(
        //        categoryIds: categoryIds,
        //        campgroundHostId: model.SearchCampgroundHostId,
        //        campgroundTypeId: model.SearchCampgroundTypeId,
        //        keywords: model.SearchCampgroundName,
        //        showHidden: true,
        //        overridePublished: overridePublished
        //    );

        //    try
        //    {
        //        var xml = _exportManager.ExportCampgroundsToXml(campgrounds);

        //        return File(Encoding.UTF8.GetBytes(xml), "application/xml", "campgrounds.xml");
        //    }
        //    catch (Exception exc)
        //    {
        //        ErrorNotification(exc);
        //        return RedirectToAction("List");
        //    }
        //}

        //[HttpPost]
        //public virtual IActionResult ExportXmlSelected(string selectedIds)
        //{
        //    if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
        //        return AccessDeniedView();

        //    var campgrounds = new List<Campground>();
        //    if (selectedIds != null)
        //    {
        //        var ids = selectedIds
        //            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
        //            .Select(x => Convert.ToInt32(x))
        //            .ToArray();
        //        campgrounds.AddRange(_campgroundService.GetCampgroundsByIds(ids));
        //    }
        //    //a campgroundHost should have access only to his campgrounds
        //    if (_workContext.CurrentCampgroundHost != null)
        //    {
        //        campgrounds = campgrounds.Where(p => p.CampgroundHostId == _workContext.CurrentCampgroundHost.Id).ToList();
        //    }

        //    var xml = _exportManager.ExportCampgroundsToXml(campgrounds);

        //    return File(Encoding.UTF8.GetBytes(xml), "application/xml", "campgrounds.xml");
        //}

        //[HttpPost, ActionName("List")]
        //[FormValueRequired("exportexcel-all")]
        //public virtual IActionResult ExportExcelAll(CampgroundListModel model)
        //{
        //    if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
        //        return AccessDeniedView();

        //    //a campgroundHost should have access only to his campgrounds
        //    if (_workContext.CurrentCampgroundHost != null)
        //    {
        //        model.SearchCampgroundHostId = _workContext.CurrentCampgroundHost.Id;
        //    }

        //    var categoryIds = new List<int> { model.SearchCategoryId };
        //    //include subcategories
        //    if (model.SearchIncludeSubCategories && model.SearchCategoryId > 0)
        //        categoryIds.AddRange(GetChildCategoryIds(model.SearchCategoryId));

        //    //0 - all (according to "ShowHidden" parameter)
        //    //1 - published only
        //    //2 - unpublished only
        //    bool? overridePublished = null;
        //    if (model.SearchPublishedId == 1)
        //        overridePublished = true;
        //    else if (model.SearchPublishedId == 2)
        //        overridePublished = false;

        //    var campgrounds = _campgroundService.SearchCampgrounds(
        //        categoryIds: categoryIds,
        //        manufacturerId: model.SearchManufacturerId,
        //        storeId: model.SearchStoreId,
        //        campgroundHostId: model.SearchCampgroundHostId,
        //        warehouseId: model.SearchWarehouseId,
        //        campgroundType: model.SearchCampgroundTypeId > 0 ? (CampgroundType?)model.SearchCampgroundTypeId : null,
        //        keywords: model.SearchCampgroundName,
        //        showHidden: true,
        //        overridePublished: overridePublished
        //    );
        //    try
        //    {
        //        var bytes = _exportManager.ExportCampgroundsToXlsx(campgrounds);

        //        return File(bytes, MimeTypes.TextXlsx, "campgrounds.xlsx");
        //    }
        //    catch (Exception exc)
        //    {
        //        ErrorNotification(exc);
        //        return RedirectToAction("List");
        //    }
        //}

        //[HttpPost]
        //public virtual IActionResult ExportExcelSelected(string selectedIds)
        //{
        //    if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
        //        return AccessDeniedView();

        //    var campgrounds = new List<Campground>();
        //    if (selectedIds != null)
        //    {
        //        var ids = selectedIds
        //            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
        //            .Select(x => Convert.ToInt32(x))
        //            .ToArray();
        //        campgrounds.AddRange(_campgroundService.GetCampgroundsByIds(ids));
        //    }
        //    //a campgroundHost should have access only to his campgrounds
        //    if (_workContext.CurrentCampgroundHost != null)
        //    {
        //        campgrounds = campgrounds.Where(p => p.CampgroundHostId == _workContext.CurrentCampgroundHost.Id).ToList();
        //    }

        //    var bytes = _exportManager.ExportCampgroundsToXlsx(campgrounds);

        //    return File(bytes, MimeTypes.TextXlsx, "campgrounds.xlsx");
        //}

        //[HttpPost]
        //public virtual IActionResult ImportExcel(IFormFile importexcelfile)
        //{
        //    if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
        //        return AccessDeniedView();

        //    if (_workContext.CurrentCampgroundHost != null && !_campgroundHostSettings.AllowCampgroundHostsToImportCampgrounds)
        //        //a campgroundHost can not import campgrounds
        //        return AccessDeniedView();

        //    try
        //    {
        //        if (importexcelfile != null && importexcelfile.Length > 0)
        //        {
        //            _importManager.ImportCampgroundsFromXlsx(importexcelfile.OpenReadStream());
        //        }
        //        else
        //        {
        //            ErrorNotification(_localizationService.GetResource("Admin.Common.UploadFile"));
        //            return RedirectToAction("List");
        //        }

        //        SuccessNotification(_localizationService.GetResource("Admin.Campgrounds.Imported"));
        //        return RedirectToAction("List");
        //    }
        //    catch (Exception exc)
        //    {
        //        ErrorNotification(exc);
        //        return RedirectToAction("List");
        //    }
        //}

        #endregion

        #region Bulk editing

        //public virtual IActionResult BulkEdit()
        //{
        //    if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
        //        return AccessDeniedView();

        //    var model = new BulkEditListModel();
        //    //categories
        //    model.AvailableCategories.Add(new SelectListItem { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
        //    var categories = SelectListHelper.GetCategoryList(_categoryService, _cacheManager, true);
        //    foreach (var c in categories)
        //        model.AvailableCategories.Add(c);

        //    //manufacturers
        //    model.AvailableManufacturers.Add(new SelectListItem { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
        //    var manufacturers = SelectListHelper.GetManufacturerList(_manufacturerService, _cacheManager, true);
        //    foreach (var m in manufacturers)
        //        model.AvailableManufacturers.Add(m);

        //    //campground types
        //model.AvailableCampgroundTypes.Add(new SelectListItem { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
        //    var campgroundTypes = SelectListHelper.GetCampgroundTypeList(_campgroundTypeService, _cacheManager, true);
        //    foreach (var ct in campgroundTypes)
        //        model.AvailableCampgroundTypes.Add(ct);

        //    return View(model);
        //}

        //[HttpPost]
        //public virtual IActionResult BulkEditSelect(DataSourceRequest command, BulkEditListModel model)
        //{
        //    if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
        //        return AccessDeniedKendoGridJson();

        //    var campgroundHostId = 0;
        //    //a campgroundHost should have access only to his campgrounds
        //    if (_workContext.CurrentCampgroundHost != null)
        //        campgroundHostId = _workContext.CurrentCampgroundHost.Id;

        //    var campgrounds = _campgroundService.SearchCampgrounds(categoryIds: new List<int> { model.SearchCategoryId },
        //        manufacturerId: model.SearchManufacturerId,
        //        campgroundHostId: campgroundHostId,
        //        campgroundType: model.SearchCampgroundTypeId > 0 ? (CampgroundType?)model.SearchCampgroundTypeId : null,
        //        keywords: model.SearchCampgroundName,
        //        pageIndex: command.Page - 1,
        //        pageSize: command.PageSize,
        //        showHidden: true);

        //    var gridModel = new DataSourceResult
        //    {
        //        Data = campgrounds.Select(x =>
        //        {
        //            var campgroundModel = new BulkEditCampgroundModel
        //            {
        //                Id = x.Id,
        //                Name = x.Name,
        //                Sku = x.Sku,
        //                OldPrice = x.OldPrice,
        //                Price = x.Price,
        //                ManageInventoryMethod = x.ManageInventoryMethod.GetLocalizedEnum(_localizationService, _workContext.WorkingLanguage.Id),
        //                StockQuantity = x.StockQuantity,
        //                Published = x.Published
        //            };

        //            if (x.ManageInventoryMethod == ManageInventoryMethod.ManageStock && x.UseMultipleWarehouses)
        //            {
        //                //multi-warehouse supported
        //                //TODO localize
        //                campgroundModel.ManageInventoryMethod += " (multi-warehouse)";
        //            }

        //            return campgroundModel;
        //        }),

        //        Total = campgrounds.TotalCount
        //    };

        //    return Json(gridModel);
        //}

        //[HttpPost]
        //public virtual IActionResult BulkEditUpdate(IEnumerable<BulkEditCampgroundModel> campgrounds)
        //{
        //    if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
        //        return AccessDeniedView();

        //    if (campgrounds != null)
        //    {
        //        foreach (var pModel in campgrounds)
        //        {
        //            //update
        //            var campground = _campgroundService.GetCampgroundById(pModel.Id);
        //            if (campground != null)
        //            {
        //                //a campgroundHost should have access only to his campgrounds
        //                if (_workContext.CurrentCampgroundHost != null)
        //                    continue;

        //                var prevTotalStockQuantity = campground.GetTotalStockQuantity();
        //                var previousStockQuantity = campground.StockQuantity;

        //                campground.Name = pModel.Name;
        //                campground.Sku = pModel.Sku;
        //                campground.Price = pModel.Price;
        //                campground.OldPrice = pModel.OldPrice;
        //                campground.StockQuantity = pModel.StockQuantity;
        //                campground.Published = pModel.Published;
        //                campground.UpdatedOnUtc = DateTime.UtcNow;
        //                _campgroundService.UpdateCampground(campground);

        //                //back in stock notifications
        //                if (campground.ManageInventoryMethod == ManageInventoryMethod.ManageStock &&
        //                    campground.BackorderMode == BackorderMode.NoBackorders &&
        //                    campground.AllowBackInStockSubscriptions &&
        //                    campground.GetTotalStockQuantity() > 0 &&
        //                    prevTotalStockQuantity <= 0 &&
        //                    campground.Published &&
        //                    !campground.Deleted)
        //                {
        //                    _backInStockSubscriptionService.SendNotificationsToSubscribers(campground);
        //                }

        //                //quantity change history
        //                _campgroundService.AddStockQuantityHistoryEntry(campground, campground.StockQuantity - previousStockQuantity, campground.StockQuantity,
        //                    campground.WarehouseId, _localizationService.GetResource("Admin.StockQuantityHistory.Messages.Edit"));
        //            }
        //        }
        //    }

        //    return new NullJsonResult();
        //}

        //[HttpPost]
        //public virtual IActionResult BulkEditDelete(IEnumerable<BulkEditCampgroundModel> campgrounds)
        //{
        //    if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
        //        return AccessDeniedView();

        //    if (campgrounds != null)
        //    {
        //        foreach (var pModel in campgrounds)
        //        {
        //            //delete
        //            var campground = _campgroundService.GetCampgroundById(pModel.Id);
        //            if (campground != null)
        //            {
        //                //a campgroundHost should have access only to his campgrounds
        //                if (_workContext.CurrentCampgroundHost != null)
        //                    continue;

        //                _campgroundService.DeleteCampground(campground);
        //            }
        //        }
        //    }
        //    return new NullJsonResult();
        //}

        #endregion

        #region Campground attributes

        [HttpPost]
        public virtual IActionResult CampgroundAttributeMappingList(DataSourceRequest command, int campgroundId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedKendoGridJson();

            var campground = _campgroundService.GetCampgroundById(campgroundId);
            if (campground == null)
                throw new ArgumentException("No campground found with the specified id");

            var attributes = _campgroundAttributeTypeService.GetCampgroundAttributeMappingsByCampgroundId(campgroundId);
            var attributesModel = attributes
                .Select(x =>
                {
                    var attributeModel = new CampgroundModel.CampgroundAttributeMappingModel();
                    PrepareCampgroundAttributeMappingModel(attributeModel, x, x.Campground, false);
                    return attributeModel;
                })
                .ToList();

            var gridModel = new DataSourceResult
            {
                Data = attributesModel,
                Total = attributesModel.Count
            };

            return Json(gridModel);
        }

        public virtual IActionResult CampgroundAttributeMappingCreate(int campgroundId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedView();

            var campground = _campgroundService.GetCampgroundById(campgroundId);
            if (campground == null)
                throw new ArgumentException("No campground found with the specified id");

            var model = new CampgroundModel.CampgroundAttributeMappingModel();
            PrepareCampgroundAttributeMappingModel(model, null, campground, false);

            //locales
            AddLocales(_languageService, model.Locales);
            return View(CAMPGROUND_ATT_CREATE_VIEW, model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult CampgroundAttributeMappingCreate(CampgroundModel.CampgroundAttributeMappingModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedView();

            var campground = _campgroundService.GetCampgroundById(model.CampgroundId);
            if (campground == null)
                throw new ArgumentException("No campground found with the specified id");

            //ensure this attribute is not mapped yet
            if (_campgroundAttributeTypeService.GetCampgroundAttributeMappingsByCampgroundId(campground.Id)
                .Any(x => x.CampgroundAttributeTypeId == model.CampgroundAttributeTypeId))
            {
                //redisplay form
                ErrorNotification(_localizationService.GetResource("Admin.Campgrounds.CampgroundAttributes.Attributes.AlreadyExists"));
                //model
                PrepareCampgroundAttributeMappingModel(model, null, campground, true);

                return View(CAMPGROUND_ATT_CREATE_VIEW, model);
            }

            //insert mapping
            var campgroundAttributeTypeMapping = new CampgroundAttributeMapping
            {
                CampgroundId = model.CampgroundId,
                CampgroundAttributeTypeId = model.CampgroundAttributeTypeId,
                TextPrompt = model.TextPrompt,
                IsRequired = model.IsRequired,
                AttributeControlTypeId = model.AttributeControlTypeId,
                DisplayOrder = model.DisplayOrder,
                ValidationMinLength = model.ValidationMinLength,
                ValidationMaxLength = model.ValidationMaxLength,
                DefaultValue = model.DefaultValue
            };
            _campgroundAttributeTypeService.InsertCampgroundAttributeMapping(campgroundAttributeTypeMapping);
            UpdateLocales(campgroundAttributeTypeMapping, model);

            //predefined values
            var predefinedValues = _campgroundAttributeTypeService.GetPredefinedCampgroundAttributeValues(model.CampgroundAttributeTypeId);
            foreach (var predefinedValue in predefinedValues)
            {
                var cav = new CampgroundAttributeValue
                {
                    CampgroundAttributeMappingId = campgroundAttributeTypeMapping.Id,
                    Name = predefinedValue.Name,
                    PriceAdjustment = predefinedValue.PriceAdjustment,
                    Cost = predefinedValue.Cost,
                    IsPreSelected = predefinedValue.IsPreSelected,
                    DisplayOrder = predefinedValue.DisplayOrder
                };
                _campgroundAttributeTypeService.InsertCampgroundAttributeValue(cav);
                //locales
                var languages = _languageService.GetAllLanguages(true);
                //localization
                foreach (var lang in languages)
                {
                    var name = predefinedValue.GetLocalized(x => x.Name, lang.Id, false, false);
                    if (!string.IsNullOrEmpty(name))
                        _localizedEntityService.SaveLocalizedValue(cav, x => x.Name, name, lang.Id);
                }
            }

            SuccessNotification(_localizationService.GetResource("Admin.Campgrounds.CampgroundAttributes.Attributes.Added"));

            if (continueEditing)
            {
                //selected tab
                SaveSelectedTabName();

                return RedirectToAction("CampgroundAttributeMappingEdit", new { id = campgroundAttributeTypeMapping.Id });
            }

            SaveSelectedTabName("tab-campground-attributes");
            return RedirectToAction("Edit", new { id = campground.Id });
        }

        public virtual IActionResult CampgroundAttributeMappingEdit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedView();

            var pam = _campgroundAttributeTypeService.GetCampgroundAttributeMappingById(id);
            if (pam == null)
                throw new ArgumentException("No campground attribute mapping found with the specified id");

            var campground = _campgroundService.GetCampgroundById(pam.CampgroundId);
            if (campground == null)
                throw new ArgumentException("No campground found with the specified id");

            var model = new CampgroundModel.CampgroundAttributeMappingModel();
            PrepareCampgroundAttributeMappingModel(model, pam, campground, false);

            //locales
            AddLocales(_languageService, model.Locales, (locale, languageId) =>
            {
                locale.TextPrompt = pam.GetLocalized(x => x.TextPrompt, languageId, false, false);
            });

            return View(CAMPGROUND_ATT_EDIT_VIEW, model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult CampgroundAttributeMappingEdit(CampgroundModel.CampgroundAttributeMappingModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedView();

            var campgroundAttributeTypeMapping = _campgroundAttributeTypeService.GetCampgroundAttributeMappingById(model.Id);
            if (campgroundAttributeTypeMapping == null)
                throw new ArgumentException("No campground attribute mapping found with the specified id");

            var campground = _campgroundService.GetCampgroundById(campgroundAttributeTypeMapping.CampgroundId);
            if (campground == null)
                throw new ArgumentException("No campground found with the specified id");

            //ensure this attribute is not mapped yet
            if (_campgroundAttributeTypeService.GetCampgroundAttributeMappingsByCampgroundId(campground.Id)
                .Any(x => x.CampgroundAttributeTypeId == model.CampgroundAttributeTypeId && x.Id != campgroundAttributeTypeMapping.Id))
            {
                //redisplay form
                ErrorNotification(_localizationService.GetResource("Admin.Campgrounds.CampgroundAttributes.Attributes.AlreadyExists"));
                //model
                PrepareCampgroundAttributeMappingModel(model, campgroundAttributeTypeMapping, campground, true);

                return View(CAMPGROUND_ATT_EDIT_VIEW, model);
            }

            campgroundAttributeTypeMapping.CampgroundAttributeTypeId = model.CampgroundAttributeTypeId;
            campgroundAttributeTypeMapping.TextPrompt = model.TextPrompt;
            campgroundAttributeTypeMapping.IsRequired = model.IsRequired;
            campgroundAttributeTypeMapping.AttributeControlTypeId = model.AttributeControlTypeId;
            campgroundAttributeTypeMapping.DisplayOrder = model.DisplayOrder;
            campgroundAttributeTypeMapping.ValidationMinLength = model.ValidationMinLength;
            campgroundAttributeTypeMapping.ValidationMaxLength = model.ValidationMaxLength;
            campgroundAttributeTypeMapping.DefaultValue = model.DefaultValue;
            _campgroundAttributeTypeService.UpdateCampgroundAttributeMapping(campgroundAttributeTypeMapping);

            UpdateLocales(campgroundAttributeTypeMapping, model);

            SuccessNotification(_localizationService.GetResource("Admin.Campgrounds.CampgroundAttributes.Attributes.Updated"));
            if (continueEditing)
            {
                //selected tab
                SaveSelectedTabName();

                return RedirectToAction("CampgroundAttributeMappingEdit", new { id = campgroundAttributeTypeMapping.Id });
            }

            SaveSelectedTabName("tab-campground-attributes");
            return RedirectToAction("Edit", new { id = campground.Id });
        }

        [HttpPost]
        public virtual IActionResult CampgroundAttributeMappingDelete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedView();

            var campgroundAttributeTypeMapping = _campgroundAttributeTypeService.GetCampgroundAttributeMappingById(id);
            if (campgroundAttributeTypeMapping == null)
                throw new ArgumentException("No campground attribute mapping found with the specified id");

            var campgroundId = campgroundAttributeTypeMapping.CampgroundId;
            var campground = _campgroundService.GetCampgroundById(campgroundId);
            if (campground == null)
                throw new ArgumentException("No campground found with the specified id");

            _campgroundAttributeTypeService.DeleteCampgroundAttributeMapping(campgroundAttributeTypeMapping);

            SuccessNotification(_localizationService.GetResource("Admin.Campgrounds.CampgroundAttributes.Attributes.Deleted"));
            SaveSelectedTabName("tab-campground-attributes");
            return RedirectToAction("Edit", new { id = campgroundId });
        }

        [HttpPost]
        public virtual IActionResult CampgroundAttributeValueList(int campgroundAttributeTypeMappingId, DataSourceRequest command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedKendoGridJson();

            var campgroundAttributeTypeMapping = _campgroundAttributeTypeService.GetCampgroundAttributeMappingById(campgroundAttributeTypeMappingId);
            if (campgroundAttributeTypeMapping == null)
                throw new ArgumentException("No campground attribute mapping found with the specified id");

            var campground = _campgroundService.GetCampgroundById(campgroundAttributeTypeMapping.CampgroundId);
            if (campground == null)
                throw new ArgumentException("No campground found with the specified id");

            var values = _campgroundAttributeTypeService.GetCampgroundAttributeValues(campgroundAttributeTypeMappingId);
            var gridModel = new DataSourceResult
            {
                Data = values.Select(x =>
                {
                    Campground associatedCampground = null;
                    var pictureThumbnailUrl = _pictureService.GetPictureUrl(x.PictureId, 75, false);
                    //little hack here. Grid is rendered wrong way with <img> without "src" attribute
                    if (string.IsNullOrEmpty(pictureThumbnailUrl))
                        pictureThumbnailUrl = _pictureService.GetPictureUrl(null, 1, true);
                    return new CampgroundModel.CampgroundAttributeValueModel
                    {
                        Id = x.Id,
                        CampgroundAttributeMappingId = x.CampgroundAttributeMappingId,
                        AttributeValueTypeId = x.CampgroundAttributeValueTypeId,
                        AttributeValueTypeName = x.CampgroundAttributeValueType.GetLocalizedEnum(_localizationService, _workContext),
                        AssociatedCampgroundId = x.AssociatedCampgroundId,
                        AssociatedCampgroundName = associatedCampground != null ? associatedCampground.Name : "",
                        Name = x.CampgroundAttributeMapping.AttributeControlType != AttributeControlType.ColorSquares ? x.Name : $"{x.Name} - {x.ColorSquaresRgb}",
                        ColorSquaresRgb = x.ColorSquaresRgb,
                        ImageSquaresPictureId = x.ImageSquaresPictureId,
                        IsPreSelected = x.IsPreSelected,
                        DisplayOrder = x.DisplayOrder,
                        PictureId = x.PictureId,
                        PictureThumbnailUrl = pictureThumbnailUrl
                    };
                }),
                Total = values.Count
            };

            return Json(gridModel);
        }

        public virtual IActionResult CampgroundAttributeValueCreatePopup(int campgroundAttributeTypeMappingId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedView();

            var campgroundAttributeTypeMapping = _campgroundAttributeTypeService.GetCampgroundAttributeMappingById(campgroundAttributeTypeMappingId);
            if (campgroundAttributeTypeMapping == null)
                throw new ArgumentException("No campground attribute mapping found with the specified id");

            var campground = _campgroundService.GetCampgroundById(campgroundAttributeTypeMapping.CampgroundId);
            if (campground == null)
                throw new ArgumentException("No campground found with the specified id");

            var model = new CampgroundModel.CampgroundAttributeValueModel
            {
                CampgroundAttributeMappingId = campgroundAttributeTypeMappingId,

                //color squares
                DisplayColorSquaresRgb = campgroundAttributeTypeMapping.AttributeControlType == AttributeControlType.ColorSquares,
                //image squares
                DisplayImageSquaresPicture = campgroundAttributeTypeMapping.AttributeControlType == AttributeControlType.ImageSquares,

            };

            //locales
            AddLocales(_languageService, model.Locales);

            //pictures
            model.CampgroundPictureModels = _campgroundService.GetCampgroundPicturesByCampgroundId(campground.Id)
                .Select(x => new CampgroundModel.CampgroundPictureModel
                {
                    Id = x.Id,
                    CampgroundId = x.CampgroundId,
                    PictureId = x.PictureId,
                    PictureUrl = _pictureService.GetPictureUrl(x.PictureId),
                    DisplayOrder = x.DisplayOrder
                })
                .ToList();

            return View(CAMPGROUND_ATT_CREATE_POP_VIEW, model);
        }
        [HttpPost]
        public virtual IActionResult CampgroundAttributeValueCreatePopup(CampgroundModel.CampgroundAttributeValueModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedView();

            var campgroundAttributeTypeMapping = _campgroundAttributeTypeService.GetCampgroundAttributeMappingById(model.CampgroundAttributeMappingId);
            if (campgroundAttributeTypeMapping == null)
                //No campground attribute found with the specified id
                return RedirectToAction("List", "Campground");

            var campground = _campgroundService.GetCampgroundById(campgroundAttributeTypeMapping.CampgroundId);
            if (campground == null)
                throw new ArgumentException("No campground found with the specified id");

            if (campgroundAttributeTypeMapping.AttributeControlType == AttributeControlType.ColorSquares)
            {
                //ensure valid color is chosen/entered
                if (string.IsNullOrEmpty(model.ColorSquaresRgb))
                    ModelState.AddModelError("", "Color is required");
                try
                {
                    //ensure color is valid (can be instanciated)
                    System.Drawing.ColorTranslator.FromHtml(model.ColorSquaresRgb);
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError("", exc.Message);
                }
            }

            //ensure a picture is uploaded
            if (campgroundAttributeTypeMapping.AttributeControlType == AttributeControlType.ImageSquares && model.ImageSquaresPictureId == 0)
            {
                ModelState.AddModelError("", "Image is required");
            }

            if (ModelState.IsValid)
            {
                var cav = new CampgroundAttributeValue
                {
                    CampgroundAttributeMappingId = model.CampgroundAttributeMappingId,
                    CampgroundAttributeValueTypeId = model.AttributeValueTypeId,
                    AssociatedCampgroundId = model.AssociatedCampgroundId,
                    Name = model.Name,
                    ColorSquaresRgb = model.ColorSquaresRgb,
                    ImageSquaresPictureId = model.ImageSquaresPictureId,
                    IsPreSelected = model.IsPreSelected,
                    DisplayOrder = model.DisplayOrder,
                    PictureId = model.PictureId,
                };

                _campgroundAttributeTypeService.InsertCampgroundAttributeValue(cav);
                UpdateLocales(cav, model);

                ViewBag.RefreshPage = true;
                return View(CAMPGROUND_ATT_CREATE_POP_VIEW, model);
            }

            //If we got this far, something failed, redisplay form

            //pictures
            model.CampgroundPictureModels = _campgroundService.GetCampgroundPicturesByCampgroundId(campground.Id)
                .Select(x => new CampgroundModel.CampgroundPictureModel
                {
                    Id = x.Id,
                    CampgroundId = x.CampgroundId,
                    PictureId = x.PictureId,
                    PictureUrl = _pictureService.GetPictureUrl(x.PictureId),
                    DisplayOrder = x.DisplayOrder
                })
                .ToList();

            var associatedCampground = _campgroundService.GetCampgroundById(model.AssociatedCampgroundId);
            model.AssociatedCampgroundName = associatedCampground != null ? associatedCampground.Name : "";

            return View(CAMPGROUND_ATT_CREATE_POP_VIEW, model);
        }

        public virtual IActionResult CampgroundAttributeValueEditPopup(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedView();

            var cav = _campgroundAttributeTypeService.GetCampgroundAttributeValueById(id);
            if (cav == null)
                //No attribute value found with the specified id
                return RedirectToAction("List", "Campground");

            var campground = _campgroundService.GetCampgroundById(cav.CampgroundAttributeMapping.CampgroundId);
            if (campground == null)
                throw new ArgumentException("No campground found with the specified id");

            var associatedCampground = _campgroundService.GetCampgroundById(cav.AssociatedCampgroundId);

            var model = new CampgroundModel.CampgroundAttributeValueModel
            {
                CampgroundAttributeMappingId = cav.CampgroundAttributeMappingId,
                AttributeValueTypeId = cav.CampgroundAttributeValueTypeId,
                AttributeValueTypeName = cav.CampgroundAttributeValueType.GetLocalizedEnum(_localizationService, _workContext),
                AssociatedCampgroundId = cav.AssociatedCampgroundId,
                AssociatedCampgroundName = associatedCampground != null ? associatedCampground.Name : "",
                Name = cav.Name,
                ColorSquaresRgb = cav.ColorSquaresRgb,
                DisplayColorSquaresRgb = cav.CampgroundAttributeMapping.AttributeControlType == AttributeControlType.ColorSquares,
                ImageSquaresPictureId = cav.ImageSquaresPictureId,
                DisplayImageSquaresPicture = cav.CampgroundAttributeMapping.AttributeControlType == AttributeControlType.ImageSquares,
                IsPreSelected = cav.IsPreSelected,
                DisplayOrder = cav.DisplayOrder,
                PictureId = cav.PictureId
            };
            //locales
            AddLocales(_languageService, model.Locales, (locale, languageId) =>
            {
                locale.Name = cav.GetLocalized(x => x.Name, languageId, false, false);
            });
            //pictures
            model.CampgroundPictureModels = _campgroundService.GetCampgroundPicturesByCampgroundId(campground.Id)
                .Select(x => new CampgroundModel.CampgroundPictureModel
                {
                    Id = x.Id,
                    CampgroundId = x.CampgroundId,
                    PictureId = x.PictureId,
                    PictureUrl = _pictureService.GetPictureUrl(x.PictureId),
                    DisplayOrder = x.DisplayOrder
                })
                .ToList();

            return View(CAMPGROUND_ATT_EDIT_POP_VIEW, model);
        }
        [HttpPost]
        public virtual IActionResult CampgroundAttributeValueEditPopup(CampgroundModel.CampgroundAttributeValueModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedView();

            var cav = _campgroundAttributeTypeService.GetCampgroundAttributeValueById(model.Id);
            if (cav == null)
                //No attribute value found with the specified id
                return RedirectToAction("List", "Campground");

            var campground = _campgroundService.GetCampgroundById(cav.CampgroundAttributeMapping.CampgroundId);
            if (campground == null)
                throw new ArgumentException("No campground found with the specified id");

            if (cav.CampgroundAttributeMapping.AttributeControlType == AttributeControlType.ColorSquares)
            {
                //ensure valid color is chosen/entered
                if (string.IsNullOrEmpty(model.ColorSquaresRgb))
                    ModelState.AddModelError("", "Color is required");
                try
                {
                    //ensure color is valid (can be instanciated)
                    System.Drawing.ColorTranslator.FromHtml(model.ColorSquaresRgb);
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError("", exc.Message);
                }
            }

            //ensure a picture is uploaded
            if (cav.CampgroundAttributeMapping.AttributeControlType == AttributeControlType.ImageSquares && model.ImageSquaresPictureId == 0)
            {
                ModelState.AddModelError("", "Image is required");
            }

            if (ModelState.IsValid)
            {
                cav.CampgroundAttributeValueTypeId = model.AttributeValueTypeId;
                cav.AssociatedCampgroundId = model.AssociatedCampgroundId;
                cav.Name = model.Name;
                cav.ColorSquaresRgb = model.ColorSquaresRgb;
                cav.ImageSquaresPictureId = model.ImageSquaresPictureId;
                cav.IsPreSelected = model.IsPreSelected;
                cav.DisplayOrder = model.DisplayOrder;
                cav.PictureId = model.PictureId;
                _campgroundAttributeTypeService.UpdateCampgroundAttributeValue(cav);

                UpdateLocales(cav, model);

                ViewBag.RefreshPage = true;
                return View(CAMPGROUND_ATT_EDIT_POP_VIEW, model);
            }

            //If we got this far, something failed, redisplay form

            //pictures
            model.CampgroundPictureModels = _campgroundService.GetCampgroundPicturesByCampgroundId(campground.Id)
                .Select(x => new CampgroundModel.CampgroundPictureModel
                {
                    Id = x.Id,
                    CampgroundId = x.CampgroundId,
                    PictureId = x.PictureId,
                    PictureUrl = _pictureService.GetPictureUrl(x.PictureId),
                    DisplayOrder = x.DisplayOrder
                })
                .ToList();

            var associatedCampground = _campgroundService.GetCampgroundById(model.AssociatedCampgroundId);
            model.AssociatedCampgroundName = associatedCampground != null ? associatedCampground.Name : "";

            return View(CAMPGROUND_ATT_EDIT_POP_VIEW, model);
        }

        [HttpPost]
        public virtual IActionResult CampgroundAttributeValueDelete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedView();

            var cav = _campgroundAttributeTypeService.GetCampgroundAttributeValueById(id);
            if (cav == null)
                throw new ArgumentException("No campground attribute value found with the specified id");

            var campground = _campgroundService.GetCampgroundById(cav.CampgroundAttributeMapping.CampgroundId);
            if (campground == null)
                throw new ArgumentException("No campground found with the specified id");

            _campgroundAttributeTypeService.DeleteCampgroundAttributeValue(cav);

            return new NullJsonResult();
        }

        public virtual IActionResult AssociateCampgroundToAttributeValuePopup()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedView();

            var model = new CampgroundModel.CampgroundAttributeValueModel.AssociateCampgroundToAttributeValueModel
            {
                //a campgroundHost should have access only to his campgrounds
                IsLoggedInAsCampgroundHost = _workContext.CurrentCampgroundHost != null
            };

            //state categories
            model.AvailableCategories = PrepareAvailableStateCategories();

            //campground hosts
            model.AvailableCampgroundHosts = PrepareAvailableCampgroundHosts();

            //campground types
            model.AvailableCampgroundTypes = PrepareAvailableCampgroundTypes();

            return View(ASSOC_ATT_POP_VIEW, model);
        }

        [HttpPost]
        public virtual IActionResult AssociateCampgroundToAttributeValuePopupList(DataSourceRequest command,
            CampgroundModel.CampgroundAttributeValueModel.AssociateCampgroundToAttributeValueModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedKendoGridJson();

            var campgrounds = _campgroundService.SearchCampgrounds(
                categoryIds: new List<int> { model.SearchCategoryId },
                campgroundHostId: model.SearchCampgroundHostId,
                campgroundTypeId: model.SearchCampgroundTypeId,
                keywords: model.SearchCampgroundName,
                pageIndex: command.Page - 1,
                pageSize: command.PageSize,
                showHidden: true
                );
            var gridModel = new DataSourceResult
            {
                Data = campgrounds.Select(x => x.ToModel()),
                Total = campgrounds.TotalCount
            };

            return Json(gridModel);
        }

        [HttpPost]
        [FormValueRequired("save")]
        public virtual IActionResult AssociateCampgroundToAttributeValuePopup(CampgroundModel.CampgroundAttributeValueModel.AssociateCampgroundToAttributeValueModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedView();

            var associatedCampground = _campgroundService.GetCampgroundById(model.AssociatedToCampgroundId);
            if (associatedCampground == null)
                return Content("Cannot load a campground");

            model.IsLoggedInAsCampgroundHost = _workContext.CurrentCampgroundHost != null;
            ViewBag.RefreshPage = true;
            ViewBag.campgroundId = associatedCampground.Id;
            ViewBag.campgroundName = associatedCampground.Name;
            return View(model);
        }

        //action displaying notification (warning) to a store owner when associating some campground
        public virtual IActionResult AssociatedCampgroundGetWarnings(int campgroundId)
        {
            var associatedCampground = _campgroundService.GetCampgroundById(campgroundId);
            if (associatedCampground != null)
            {
                //attributes
                if (associatedCampground.CampgroundAttributeMappings.Any())
                {
                    if (associatedCampground.CampgroundAttributeMappings.Any(attribute => attribute.IsRequired))
                        return Json(new { Result = _localizationService.GetResource("Admin.Campgrounds.CampgroundAttributes.Attributes.Values.Fields.AssociatedCampground.HasRequiredAttributes") });

                    return Json(new { Result = _localizationService.GetResource("Admin.Campgrounds.CampgroundAttributes.Attributes.Values.Fields.AssociatedCampground.HasAttributes") });
                }
            }

            return Json(new { Result = string.Empty });
        }

        #endregion

        #region Campground editor settings

        [HttpPost]
        public virtual IActionResult SaveCampgroundEditorSettings(CampgroundModel model, string returnUrl = "")
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCampgrounds))
                return AccessDeniedView();

            var campgroundEditorSettings = _settingService.LoadSetting<CampgroundEditorSettings>();
            campgroundEditorSettings = model.CampgroundEditorSettingsModel.ToEntity(campgroundEditorSettings);
            _settingService.SaveSetting(campgroundEditorSettings);

            //campground list
            if (string.IsNullOrEmpty(returnUrl))
                return RedirectToAction("List");

            //prevent open redirection attack
            if (!Url.IsLocalUrl(returnUrl))
                return RedirectToAction("List");

            return Redirect(returnUrl);
        }

        #endregion

    }
}