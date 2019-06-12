using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Plugin.Campgrounds.Data.Domain;
using Nop.Plugin.Campgrounds.Services;
using Nop.Plugin.Widgets.Campgrounds.Infrastructure.Cache;
using Nop.Plugin.Widgets.Campgrounds.Models;
using Nop.Services.Catalog;
using Nop.Services.Customers;
using Nop.Services.Common;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Seo;
using Nop.Services.Security;
using Nop.Services.Tax;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Directory;
using Nop.Core.Data;
using Nop.Core.Domain.Customers;
using Nop.Web.Framework.Security.Captcha;
using Nop.Services.Media;
using Nop.Web.Models.Media;
using Nop.Core.Domain.Common;
using Nop.Services.Directory;
using Nop.Plugin.Widgets.Campgrounds.Helpers;
using Nop.Services.Configuration;
using Nop.Plugin.Widgets.Campgrounds.Extensions;
using Nop.Web.Models.Blogs;
using Nop.Services.Blogs;
using Nop.Web.Factories;

namespace Nop.Plugin.Widgets.Campgrounds.Factories
{
    /// <summary>
    /// Represents the campground model factory
    /// </summary>
    public partial class CampgroundModelFactory : ICampgroundModelFactory
    {
        #region Fields

        private readonly ICampgroundService _campgroundService;
        private readonly ICampgroundTemplateService _campgroundTemplateService;
        private readonly ICampgroundAttributeTypeService _campgroundAttributeTypeService;
        private readonly ICampgroundHostService _campgroundHostService;
        private readonly ICampgroundTypeService _campgroundTypeService;
        private readonly ICategoryService _categoryService;
        private readonly IBlogService _blogService;
        private readonly IBlogModelFactory _blogModelFactory;
        private readonly CommonSettings _commonSettings;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly ICampgroundWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly ITaxService _taxService;
        private readonly ILocalizationService _localizationService;
        private readonly IWebHelper _webHelper;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IAclService _aclService;
        private readonly ISearchTermService _searchTermService;
        private readonly MediaSettings _mediaSettings;
        private readonly CampgroundSettings _campgroundSettings;
        private readonly IStaticCacheManager _cacheManager;
        private readonly IRepository<StateProvince> _stateProvinceRepository;
        private readonly IPictureService _pictureService;
        private readonly AddressSettings _addressSettings;
        private readonly ICurrencyService _currencyService;
        private readonly CurrencySettings _currencySettings;
        private readonly ISpecificationAttributeService _specificationAttributeService;
        private readonly CustomerSettings _customerSettings;
        private readonly CaptchaSettings _captchaSettings;
        private readonly ISettingService _settingService;
        private readonly ILogger _logger;

        #endregion

        #region Ctor

        public CampgroundModelFactory(ICampgroundService campgroundService,
            ICampgroundTemplateService campgroundTemplateService,
            ICampgroundAttributeTypeService campgroundAttributeTypeService,
            ICampgroundHostService campgroundHostService,
            ICampgroundTypeService campgroundTypeService,
            ICategoryService categoryService,
            IBlogService blogService,
            IBlogModelFactory blogModelFactory,
            IStateProvinceService stateProvinceService,
            CommonSettings commonSettings,
            ICampgroundWorkContext workContext,
            IStoreContext storeContext,
            ITaxService taxService,
            ILocalizationService localizationService,
            IWebHelper webHelper,
            IDateTimeHelper dateTimeHelper,
            IAclService aclService,
            ISearchTermService searchTermService,
            MediaSettings mediaSettings,
            CampgroundSettings campgroundSettings,
            IStaticCacheManager cacheManager,
            IRepository<StateProvince> stateProvinceRepository,
            IPictureService pictureService,
            AddressSettings addressSettings,
            ICurrencyService currencyService,
            CurrencySettings currencySettings,
            CustomerSettings customerSettings,
            ISpecificationAttributeService specificationAttributeService,
            CaptchaSettings captchaSettings,
            ISettingService settingService,
            ILogger logger)
        {
            this._campgroundService = campgroundService;
            this._campgroundTemplateService = campgroundTemplateService;
            this._campgroundAttributeTypeService = campgroundAttributeTypeService;
            this._campgroundHostService = campgroundHostService;
            this._campgroundTypeService = campgroundTypeService;
            this._categoryService = categoryService;
            this._blogService = blogService;
            this._blogModelFactory = blogModelFactory;
            this._stateProvinceService = stateProvinceService;
            this._commonSettings = commonSettings;
            this._workContext = workContext;
            this._storeContext = storeContext;
            this._taxService = taxService;
            this._localizationService = localizationService;
            this._webHelper = webHelper;
            this._dateTimeHelper = dateTimeHelper;
            this._aclService = aclService;
            this._searchTermService = searchTermService;
            this._mediaSettings = mediaSettings;
            this._campgroundSettings = campgroundSettings;
            this._cacheManager = cacheManager;
            this._stateProvinceRepository = stateProvinceRepository;
            this._pictureService = pictureService;
            this._addressSettings = addressSettings;
            this._currencyService = currencyService;
            this._currencySettings = currencySettings;
            this._customerSettings = customerSettings;
            this._specificationAttributeService = specificationAttributeService;
            this._captchaSettings = captchaSettings;
            this._settingService = settingService;
            this._logger = logger;
        }

        #endregion

        #region Utilities
        protected int GetCampgroundCount(int categoryId)
        {
            string cacheKey = string.Format(ModelCacheEventConsumer.CAMPGROUND_NUMBER_OF_SITES_KEY, categoryId);
            return _cacheManager.Get(cacheKey, () =>
            {
                if (categoryId != 0)
                {
                    var categoryIds = new List<int>
                    {
                        categoryId
                    };
                    return _campgroundService.GetNumberOfCampgroundsInCategory(categoryIds, _storeContext.CurrentStore.Id);
                }
                else
                {
                    return _campgroundService.GetNumberOfCampgroundsInCategory(null, _storeContext.CurrentStore.Id);
                }
            });

        }

        protected int GetCampgroundParentCategoryId(Campground campground)
        {
            if (campground == null)
                return 2;

            string cacheKey = string.Format(ModelCacheEventConsumer.CAMPGROUND_PARENTCATEGORY_KEY, campground.Id);
            return _cacheManager.Get(cacheKey, () => _campgroundService.GetCampgroundsParentCategoryId(campground));
        }

        /// <summary>
        /// Gets a state/province 
        /// </summary>
        /// <param name="name">The state/province name</param>
        /// <returns>State/province</returns>
        public virtual StateProvince GetStateProvinceByName(string name)
        {
            var query = from sp in _stateProvinceRepository.Table
                        where sp.Name == name
                        select sp;
            var stateProvince = query.FirstOrDefault();
            return stateProvince;
        }

        public virtual IList<SelectListItem> PrepareAvailableStates(int Id = 0, bool showHidden = false)
        {
            var AvailableStates = new List<SelectListItem>();
            if (_campgroundSettings.StateProvinceEnabled)
            {
                //states
                var states = _stateProvinceService.GetStateProvincesByCountryId(1).ToList();
                if (states.Any())
                {
                    AvailableStates.Add(new SelectListItem { Text = _localizationService.GetResource("Admin.Address.SelectState"), Value = "0" });

                    foreach (var s in states)
                    {
                        AvailableStates.Add(new SelectListItem { Text = s.Name, Value = s.Id.ToString(), Selected = (s.Id == Id) });
                    }
                }
            }
            return AvailableStates;
        }

        public virtual IList<SelectListItem> PrepareAvailableStateCategories(bool showHidden = true)
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

        public virtual IList<SelectListItem> PrepareAvailableCampgroundHosts(bool showHidden = true, int selectedId = 0)
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

        public virtual IList<SelectListItem> PrepareAvailableCampgroundTypes(bool showHidden = true)
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

        #region Common

        /// <summary>
        /// Prepare sorting options
        /// </summary>
        /// <param name="pagingFilteringModel">Campground paging filtering model</param>
        /// <param name="command">Campground paging filtering command</param>
        public void PrepareSortingOptions(CampgroundPagingFilteringModel pagingFilteringModel, CampgroundPagingFilteringModel command)
        {
            if (pagingFilteringModel == null)
                throw new ArgumentNullException(nameof(pagingFilteringModel));

            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var allDisabled = _campgroundSettings.CampgroundSortingEnumDisabled.Count == Enum.GetValues(typeof(CampgroundSortingEnum)).Length;
            pagingFilteringModel.AllowSorting = _campgroundSettings.AllowCampgroundSorting && !allDisabled;

            var activeOptions = Enum.GetValues(typeof(CampgroundSortingEnum)).Cast<int>()
                .Except(_campgroundSettings.CampgroundSortingEnumDisabled)
                .Select((idOption) => new KeyValuePair<int, int>(idOption, _campgroundSettings.CampgroundSortingEnumDisplayOrder.TryGetValue(idOption, out int order) ? order : idOption))
                .OrderBy(x => x.Value);
            if (command.OrderBy == null)
                command.OrderBy = allDisabled ? 0 : activeOptions.First().Key;

            if (pagingFilteringModel.AllowSorting)
            {
                foreach (var option in activeOptions)
                {
                    var currentPageUrl = _webHelper.GetThisPageUrl(true);
                    var sortUrl = _webHelper.ModifyQueryString(currentPageUrl, "orderby=" + (option.Key).ToString(), null);
                    var sortValue = ((CampgroundSortingEnum)option.Key).GetLocalizedEnum(_localizationService, _workContext);
                    pagingFilteringModel.AvailableSortOptions.Add(new SelectListItem
                    {
                        Text = sortValue,
                        Value = sortUrl,
                        Selected = option.Key == command.OrderBy
                    });
                }
            }
        }

        /// <summary>
        /// Prepare view modes
        /// </summary>
        /// <param name="pagingFilteringModel">Campground paging filtering model</param>
        /// <param name="command">Campground paging filtering command</param>
        public void PrepareViewModes(CampgroundPagingFilteringModel pagingFilteringModel, CampgroundPagingFilteringModel command)
        {
            if (pagingFilteringModel == null)
                throw new ArgumentNullException(nameof(pagingFilteringModel));

            if (command == null)
                throw new ArgumentNullException(nameof(command));

            pagingFilteringModel.AllowViewModeChanging = _campgroundSettings.AllowCampgroundViewModeChanging;

            var viewMode = !string.IsNullOrEmpty(command.ViewMode) ? command.ViewMode : _campgroundSettings.DefaultViewMode;

            pagingFilteringModel.ViewMode = viewMode;
            if (pagingFilteringModel.AllowViewModeChanging)
            {
                var currentPageUrl = _webHelper.GetThisPageUrl(true);
                //grid
                pagingFilteringModel.AvailableViewModes.Add(new SelectListItem
                {
                    Text = _localizationService.GetResource("Catalog.ViewMode.Grid"),
                    Value = _webHelper.ModifyQueryString(currentPageUrl, "viewmode=grid", null),
                    Selected = viewMode == "grid"
                });
                //list
                pagingFilteringModel.AvailableViewModes.Add(new SelectListItem
                {
                    Text = _localizationService.GetResource("Catalog.ViewMode.List"),
                    Value = _webHelper.ModifyQueryString(currentPageUrl, "viewmode=list", null),
                    Selected = viewMode == "list"
                });
            }
        }

        /// <summary>
        /// Prepare page size options
        /// </summary>
        /// <param name="pagingFilteringModel">Campground paging filtering model</param>
        /// <param name="command">Campground paging filtering command</param>
        /// <param name="allowCustomersToSelectPageSize">Are customers allowed to select page size?</param>
        /// <param name="pageSizeOptions">Page size options</param>
        /// <param name="fixedPageSize">Fixed page size</param>
        public void PreparePageSizeOptions(CampgroundPagingFilteringModel pagingFilteringModel, CampgroundPagingFilteringModel command, bool allowCustomersToSelectPageSize, string pageSizeOptions, int fixedPageSize)
        {
            if (pagingFilteringModel == null)
                throw new ArgumentNullException(nameof(pagingFilteringModel));

            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (command.PageNumber <= 0)
            {
                command.PageNumber = 1;
            }
            pagingFilteringModel.AllowCustomersToSelectPageSize = allowCustomersToSelectPageSize;
            if (allowCustomersToSelectPageSize && pageSizeOptions != null)
            {
                var pageSizes = pageSizeOptions.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (pageSizes.Any())
                {
                    // get the first page size entry to use as the default (category page load) or if customer enters invalid value via query string
                    if (command.PageSize <= 0 || !pageSizes.Contains(command.PageSize.ToString()))
                    {
                        if (int.TryParse(pageSizes.FirstOrDefault(), out int temp))
                        {
                            if (temp > 0)
                            {
                                command.PageSize = temp;
                            }
                        }
                    }

                    var currentPageUrl = _webHelper.GetThisPageUrl(true);
                    var sortUrl = _webHelper.ModifyQueryString(currentPageUrl, "pagesize={0}", null);
                    sortUrl = _webHelper.RemoveQueryString(sortUrl, "pagenumber");

                    foreach (var pageSize in pageSizes)
                    {
                        if (!int.TryParse(pageSize, out int temp))
                        {
                            continue;
                        }
                        if (temp <= 0)
                        {
                            continue;
                        }

                        pagingFilteringModel.PageSizeOptions.Add(new SelectListItem
                        {
                            Text = pageSize,
                            Value = string.Format(sortUrl, pageSize),
                            Selected = pageSize.Equals(command.PageSize.ToString(), StringComparison.InvariantCultureIgnoreCase)
                        });
                    }

                    if (pagingFilteringModel.PageSizeOptions.Any())
                    {
                        pagingFilteringModel.PageSizeOptions = pagingFilteringModel.PageSizeOptions.OrderBy(x => int.Parse(x.Text)).ToList();
                        pagingFilteringModel.AllowCustomersToSelectPageSize = true;

                        if (command.PageSize <= 0)
                        {
                            command.PageSize = int.Parse(pagingFilteringModel.PageSizeOptions.First().Text);
                        }
                    }
                }
            }
            else
            {
                //customer is not allowed to select a page size
                command.PageSize = fixedPageSize;
            }

            //ensure pge size is specified
            if (command.PageSize <= 0)
            {
                command.PageSize = fixedPageSize;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the campground template view path
        /// </summary>
        /// <param name="campground">Campground</param>
        /// <returns>View path</returns>
        public virtual string PrepareCampgroundTemplateViewPath(Campground campground)
        {
            if (campground == null)
                throw new ArgumentNullException(nameof(campground));

            var templateCacheKey = string.Format(ModelCacheEventConsumer.CAMPGROUND_TEMPLATE_MODEL_KEY, campground.CampgroundTemplateId);
            var campgroundTemplateViewPath = _cacheManager.Get(templateCacheKey, () =>
            {
                var template = _campgroundTemplateService.GetCampgroundTemplateById(campground.CampgroundTemplateId);
                if (template == null)
                    template = _campgroundTemplateService.GetAllCampgroundTemplates().FirstOrDefault();
                if (template == null)
                    throw new Exception("No default template could be loaded");
                return template.ViewPath;
            });

            return campgroundTemplateViewPath;
        }

        /// <summary>
        /// Prepare the home page news items model
        /// </summary>
        /// <returns>Home page news items model</returns>
        public virtual HomePageBlogItemsModel PrepareHomePageBlogItemsModel()
        {
            var cacheKey = string.Format(ModelCacheEventConsumer.HOMEPAGE_BLOGMODEL_KEY, _workContext.WorkingLanguage.Id, _storeContext.CurrentStore.Id);
            var cachedModel = _cacheManager.Get(cacheKey, () =>
            {
                var blogItems = _blogService.GetAllBlogPosts(_storeContext.CurrentStore.Id, _workContext.WorkingLanguage.Id);
                return new HomePageBlogItemsModel
                {
                    WorkingLanguageId = _workContext.WorkingLanguage.Id,
                    BlogItems = blogItems
                        .Select(x =>
                        {
                            var blogModel = new BlogPostModel();
                            _blogModelFactory.PrepareBlogPostModel(blogModel, x, false);
                            return blogModel;
                        })
                        .Take(_campgroundSettings.NumberOfNewBlogPostsOnHomepage)
                        .ToList()
                };
            });
            var model = (HomePageBlogItemsModel)cachedModel.Clone();
            foreach (var blogItemModel in model.BlogItems)
                blogItemModel.Comments.Clear();
            return model;
        }
        
        /// <summary>
        /// Prepare Campground Category Model
        /// </summary>
        /// <returns></returns>
        public virtual CampgroundCategoryModel PrepareCampgroundCategoryModel()
        {
            var results = new CampgroundCategoryModel();

            var topicCampgroundMenuKey = string.Format(ModelCacheEventConsumer.CAMPGROUND_TOP_MENU_MODEL_KEY,
                _workContext.WorkingLanguage.Id,
                _storeContext.CurrentStore.Id,
                string.Join(",", _workContext.CurrentCustomer.GetCustomerRoleIds()));

            var cachedcampgroundCategories = _cacheManager.Get(topicCampgroundMenuKey, () =>
                _categoryService.GetAllCategoriesByParentCategoryId(2) //Need to move this to configuration
                .Where(t => t.IncludeInTopMenu)
                .ToList()
            );

            foreach (var category in cachedcampgroundCategories)
            {
                var model = new CampgroundCategoryModel();
                if (category.Id != 2)
                {
                    model.Name = category.Name;
                    model.MetaDescription = category.MetaDescription;
                }
                results.SubCategories.Add(model);
            }

            return results;
        }

        /// <summary>
        /// Prepare campground category m,odel
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public virtual CampgroundCategoryModel PrepareCampgroundCategoryModels(int categoryId)
        {
            var model = new CampgroundCategoryModel();
            var parentCategory = _categoryService.GetCategoryById(categoryId);
            var parentCategoryId = parentCategory.ParentCategoryId == 0 ? categoryId : 0;

            model.Name = parentCategory.Name;
            model.MetaKeywords = parentCategory.MetaKeywords;
            model.MetaDescription = parentCategory.MetaDescription;
            model.MetaTitle = parentCategory.MetaTitle;
            model.SeName = parentCategory.Name;
            model.NumberOfCampgrounds = 0;
            model.Level = 0;


            var categories = _categoryService.GetAllCategoriesByParentCategoryId(parentCategoryId, showHidden: true, includeAllLevels: true);
            foreach (var category in categories)
            {
                var state = GetStateProvinceByName(category.Name);
                var subParent = new CampgroundCategoryModel
                {
                    Name = category.Name,
                    MetaKeywords = category.MetaKeywords,
                    MetaDescription = category.MetaDescription,
                    MetaTitle = category.MetaTitle,
                    SeName = SeoExtensions.GetSeName(category.Id, "Campgrounds", 0),
                    NumberOfCampgrounds = GetCampgroundCount(category.Id),
                    Level = 1
                };
                model.SubCategories.Add(subParent);
            }

            return model;
        }

        public virtual void PrepareCampgroundModel(CampgroundModel model, Campground campground, bool setPredefinedValues = false, bool excludeProperties = false)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (campground == null)
                throw new ArgumentNullException(nameof(campground));

            if (campground != null)
            {
                var parentGroupedCampground = _campgroundService.GetCampgroundById(campground.ParentGroupedCampgroundId);
                if (parentGroupedCampground != null)
                {
                    model.AssociatedToCampgroundId = campground.ParentGroupedCampgroundId;
                    model.AssociatedToCampgroundName = parentGroupedCampground.Name;
                }

                model.CreatedOn = _dateTimeHelper.ConvertToUserTime(campground.CreatedOnUtc, DateTimeKind.Utc);
                var updatedOn = campground.UpdatedOnUtc ?? DateTime.Now;
                model.UpdatedOn = _dateTimeHelper.ConvertToUserTime(updatedOn, DateTimeKind.Utc);

                PrepareCampgroundAddressModel(model, campground, true, false);
                model.SeName = SeoExtensions.GetSeName(GetCampgroundParentCategoryId(campground), "Campgrounds", 0);
                model.CampgroundSeName = campground.GetSeName();
            }

            model.PrimaryStoreCurrencyCode = _currencyService.GetCurrencyById(_currencySettings.PrimaryStoreCurrencyId).CurrencyCode;

            //little performance hack here
            //there's no need to load attributes when creating a new campground
            //anyway they're not used (you need to save a campground before you map them)
            if (campground != null)
            {
                //campground attributes
                model.CampgroundAttributeTypesExist = _campgroundAttributeTypeService.GetAllCampgroundAttributeTypes().Any();
                //specification attributes
                model.AddSpecificationAttributeModel.AvailableAttributes = _cacheManager
                    .Get(ModelCacheEventConsumer.SPEC_ATTRIBUTES_MODEL_KEY, () =>
                    {
                        var availableSpecificationAttributes = new List<SelectListItem>();
                        foreach (var sa in _specificationAttributeService.GetSpecificationAttributes())
                        {
                            availableSpecificationAttributes.Add(new SelectListItem
                            {
                                Text = sa.Name,
                                Value = sa.Id.ToString()
                            });
                        }
                        return availableSpecificationAttributes;
                    });

                //options of preselected specification attribute
                if (model.AddSpecificationAttributeModel.AvailableAttributes.Any())
                {
                    var selectedAttributeId = int.Parse(model.AddSpecificationAttributeModel.AvailableAttributes.First().Value);
                    foreach (var sao in _specificationAttributeService.GetSpecificationAttributeOptionsBySpecificationAttribute(selectedAttributeId))
                        model.AddSpecificationAttributeModel.AvailableOptions.Add(new SelectListItem
                        {
                            Text = sao.Name,
                            Value = sao.Id.ToString()
                        });
                }
                //default specs values
                model.AddSpecificationAttributeModel.ShowOnCampgroundPage = true;
            }

            //copy campground
            if (campground != null)
            {
                model.CopyCampgroundModel.Id = campground.Id;
                model.CopyCampgroundModel.Name = string.Format(_localizationService.GetResource("Admin.Campgrounds.Copy.Name.New"), campground.Name);
                model.CopyCampgroundModel.Published = true;
                model.CopyCampgroundModel.CopyImages = true;
            }


            //campgroundHosts
            model.IsLoggedInAsCampgroundHost = _workContext.CurrentCampgroundHost != null;
            model.AvailableCampgroundHosts = PrepareAvailableCampgroundHosts(selectedId: _workContext.CurrentCampgroundHost != null ? _workContext.CurrentCampgroundHost.Id : 0);
            model.CampgroundHostId = _workContext.CurrentCampgroundHost != null ? _workContext.CurrentCampgroundHost.Id : 0;

            //campground tags
            if (campground != null)
            {
                var result = new StringBuilder();
                for (var i = 0; i < campground.CampgroundTags.Count; i++)
                {
                    var pt = campground.CampgroundTags.ToList()[i];
                    result.Append(pt.Name);
                    if (i != campground.CampgroundTags.Count - 1)
                        result.Append(", ");
                }
                model.CampgroundTags = result.ToString();
            }

            //default values
            if (setPredefinedValues)
            {
                model.RecurringCycleLength = 100;
                model.RecurringTotalCycles = 10;
                model.RentalPriceLength = 1;
                model.OrderMinimumQuantity = 1;
                model.OrderMaximumQuantity = 10000;
                model.AllowCampgroundReviews = true;
                model.Published = true;
            }

            //editor settings
            var campgroundEditorSettings = _settingService.LoadSetting<CampgroundEditorSettings>();
            model.CampgroundEditorSettingsModel = campgroundEditorSettings.ToModel();
        }

        public virtual void PrepareCampgroundAddressModel(CampgroundModel model, Campground campground, bool setPredefinedValues = false, bool excludeProperties = false)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (campground == null)
                throw new ArgumentNullException(nameof(campground));

            var campgroundAddress = model.Id > 0 ? _campgroundService.GetCampgroundAddressByCampgroundId(model.Id) : new CampgroundAddress();

            if (campground != null)
            {
                model.CampgroundAddress = campgroundAddress.ToModel();
                model.CampgroundAddress.AvailableStates = PrepareAvailableStates(Id: model.CampgroundAddress.StateProvinceId.GetValueOrDefault());
                model.CampgroundAddress.PhoneNumber = campgroundAddress.FormatPhone();
                model.CampgroundAddress.FaxNumber = campgroundAddress.FormatFax();

                var addressHtmlSb = new StringBuilder("<div>");
                if (_campgroundSettings.StreetAddressEnabled && !string.IsNullOrEmpty(model.CampgroundAddress.Address1))
                    addressHtmlSb.AppendFormat("{0}<br />", WebUtility.HtmlEncode(model.CampgroundAddress.Address1));
                if (_campgroundSettings.StreetAddress2Enabled && !string.IsNullOrEmpty(model.CampgroundAddress.Address2))
                    addressHtmlSb.AppendFormat("{0}<br />", WebUtility.HtmlEncode(model.CampgroundAddress.Address2));
                if (_campgroundSettings.CityEnabled && !string.IsNullOrEmpty(model.CampgroundAddress.City))
                    addressHtmlSb.AppendFormat("{0},", WebUtility.HtmlEncode(model.CampgroundAddress.City));
                if (_campgroundSettings.StateProvinceEnabled && !string.IsNullOrEmpty(model.CampgroundAddress.StateProvinceName))
                    addressHtmlSb.AppendFormat("{0},", WebUtility.HtmlEncode(model.CampgroundAddress.StateProvinceName));
                if (_campgroundSettings.ZipPostalCodeEnabled && !string.IsNullOrEmpty(model.CampgroundAddress.ZipPostalCode))
                    addressHtmlSb.AppendFormat("{0}<br />", WebUtility.HtmlEncode(model.CampgroundAddress.ZipPostalCode));
                if (_campgroundSettings.CountryEnabled && !string.IsNullOrEmpty(model.CampgroundAddress.CountryName))
                    addressHtmlSb.AppendFormat("{0}", WebUtility.HtmlEncode(model.CampgroundAddress.CountryName));
                addressHtmlSb.Append("</div>");

                model.CampgroundAddress.AddressHtml = addressHtmlSb.ToString();
            }

            if (setPredefinedValues)
            {
                model.CampgroundAddress.StreetAddressRequired = true;
                model.CampgroundAddress.CityRequired = true;
                model.CampgroundAddress.ZipPostalCodeRequired = true;
                model.CampgroundAddress.FirstNameRequired = true;
                model.CampgroundAddress.LastNameRequired = true;
                model.CampgroundAddress.EmailRequired = true;
            }
        }


        /// <summary>
        /// Prepare Campground Default Picture Model
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public virtual PictureModel PrepareCampgroundDefaultPictureModel(Campground campground, int pictureSize = 450, int thumbPictureSize = 150)
        {
            //picture
            var defaultCampgroundPicture = _campgroundService.GetCampgroundPicturesByCampgroundId(campground.Id).FirstOrDefault();
            if (defaultCampgroundPicture != null)
            {
                var picture = _pictureService.GetPictureById(defaultCampgroundPicture.PictureId);

                var pictureModel = new PictureModel
                {
                    ImageUrl = defaultCampgroundPicture != null ? _pictureService.GetPictureUrl(picture.Id, pictureSize, false) : null,
                    ThumbImageUrl = defaultCampgroundPicture != null ? _pictureService.GetPictureUrl(picture.Id, thumbPictureSize, false) : null,
                    AlternateText = string.Format("Scenic {0} camping picture", campground.Name),
                    Title = string.Format("Scenic {0} camping picture", campground.Name),
                };
                return pictureModel;
            }
            else return new PictureModel();
        }
        public virtual PictureModel PrepareCampgroundDefaultPictureModel(Category category)
        {
            var pictureModel = new PictureModel
            {
                ImageUrl = _pictureService.GetPictureUrl(category.PictureId),
                AlternateText = string.Format("Scenic {0} camping picture", category.Name),
                Title = string.Format("Scenic {0} camping picture", category.Name),
            };

            return pictureModel;
        }

        /// <summary>
        /// Prepare campground detail model
        /// </summary>
        /// <param name="campgrounds"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public virtual IEnumerable<CampgroundDetailModel> PrepareCampgroundDetailModels(IEnumerable<Campground> campgrounds, Category category = null)
        {
            //IPagedList<Campground> campground;
            var models = new List<CampgroundDetailModel>();

            foreach (var campground in campgrounds)
            {
                var detailModel = PrepareCampgroundDetailModel(campground, null, category, false);

                models.Add(detailModel);
            }

            return models;
        }

        /// <summary>
        /// Prepare campground detail model
        /// </summary>
        /// <param name="campground"></param>
        /// <param name="command"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public CampgroundDetailModel PrepareCampgroundDetailModel(Campground campground, CampgroundPagingFilteringModel command, Category category = null, bool searchNearby = true)
        {
            var campgroundAddress = _campgroundService.GetCampgroundAddressByCampgroundId(campground.Id);

            if (campground.CampgroundPictures.Count() == 0)
            {
                var picture = _pictureService.InsertPicture(_campgroundService.SaveCampgroundStaticMap(campground), MimeTypes.ImageJpeg, campground.Name.ToUrlSlug() + "-camping-picture", titleAttribute: campground.Name + " camping picture");

                _campgroundService.InsertCampgroundPicture(new CampgroundPicture
                {
                    PictureId = picture.Id,
                    CampgroundId = campground.Id,
                    DisplayOrder = -1,
                    IsDefault = true
                });

            }

            var detailModel = new CampgroundDetailModel
            {
                Id = campground.Id,
                Name = campground.GetLocalized(x => x.Name),
                Description = campground.GetLocalized(x => x.ShortDescription),
                FullDescription = campground.GetLocalized(x => x.FullDescription),
                MetaDescription = campground.GetLocalized(x => x.MetaDescription),
                MetaKeywords = campground.GetLocalized(x => x.MetaKeywords),
                MetaTitle = campground.GetLocalized(x => x.MetaTitle),
                Website = campground.GetLocalized(x => x.Website),
                CampgroundAddress = campgroundAddress,
                Phone = campgroundAddress.FormatPhone(),
                AvailableCampsites = campground.AvailableCampsites,
                SeName = SeoExtensions.GetSeName(GetCampgroundParentCategoryId(campground), "Campgrounds", 0),
                CampgroundSeName = campground.GetSeName(),
                DefaultPictureModel = PrepareCampgroundDefaultPictureModel(campground),
                Distance = campground.Distance,
                MarkAsNewStartDate = campground.MarkAsNewStartDateTimeUtc,
            };

            if (campgroundAddress.Latitude != null && campgroundAddress.Longitude != null && searchNearby)
                detailModel = PrepareCampingNearby(campgroundAddress.Latitude.GetValueOrDefault(), campgroundAddress.Longitude.GetValueOrDefault(), command, campground, detailModel);
            

            detailModel.CampgroundReviewOverviewModel = PrepareCampgroundReviewOverviewModel(campground);

            return detailModel;
        }

        public CampgroundDetailModel PrepareCampingNearby(decimal latitude, decimal longitude, CampgroundPagingFilteringModel command, Campground campground = null, CampgroundDetailModel campgroundDetail = null, Category category = null, bool searchNearby = true)
        {
            if (campgroundDetail == null)
                campgroundDetail = new CampgroundDetailModel();

            try
            {
                var nearbyCampground = _campgroundService.GetNearbyCampgrounds(latitude, longitude).ToList();

                foreach (var nearby in nearbyCampground)
                {
                    try
                    {
                        var nearbyModel = new CampgroundDetailModel
                        {
                            Id = nearby.Id,
                            Name = nearby.GetLocalized(x => x.Name),
                            Description = nearby.GetLocalized(x => x.ShortDescription),
                            Website = nearby.GetLocalized(x => x.Website),
                            FullDescription = nearby.GetLocalized(x => x.FullDescription),
                            MetaDescription = nearby.GetLocalized(x => x.MetaDescription),
                            MetaKeywords = nearby.GetLocalized(x => x.MetaKeywords),
                            MetaTitle = nearby.GetLocalized(x => x.MetaTitle),
                            CampgroundAddress = _campgroundService.GetCampgroundAddressByCampgroundId(nearby.Id),
                            AvailableCampsites = nearby.AvailableCampsites,
                            SeName = SeoExtensions.GetSeName(GetCampgroundParentCategoryId(campground), "Campgrounds", 0),
                            CampgroundSeName = nearby.GetSeName(),
                            DefaultPictureModel = PrepareCampgroundDefaultPictureModel(nearby),
                            Distance = nearby.Distance,
                        };


                        if (nearbyModel.DefaultPictureModel == null)
                        {
                            var picture = _pictureService.InsertPicture(_campgroundService.SaveCampgroundStaticMap(nearby), MimeTypes.ImageJpeg, nearby.Name + "Picture", titleAttribute: nearby.Name + "Picture");

                            _campgroundService.InsertCampgroundPicture(new CampgroundPicture
                            {
                                PictureId = picture.Id,
                                CampgroundId = nearby.Id,
                                DisplayOrder = -1,
                                IsDefault = true
                            });

                        }

                        nearbyModel.CampgroundReviewOverviewModel = PrepareCampgroundReviewOverviewModel(nearby);

                        campgroundDetail.NearbyCampgrounds.Add(nearbyModel);
                    }
                    catch (Exception exc)
                    {
                        _logger.Warning(exc.Message + ": CampgroundModelFactory.PrepareCampingNearby(" + latitude + ", " + longitude + ") Error adding nearbyModel - ", exc, _workContext.CurrentCustomer);
                    }
                }

                campgroundDetail.NearbyCampgrounds.OrderBy(x => x.Distance);
            }
            catch (Exception exc)
            {
                _logger.Warning(exc.Message + " CampgroundModelFactory.PrepareCampingNearby(" + latitude + ", " + longitude + ")", exc, _workContext.CurrentCustomer);
            }

            return campgroundDetail;

        }

        /// <summary>
        /// Prepare Campground Search Model
        /// </summary>
        /// <param name="category"></param>
        /// <param name="sTerms"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public CampgroundOverviewModel PrepareCampgroundSearchModel(Category category, string sTerms, CampgroundPagingFilteringModel command)
        {
            var campgrounds = _campgroundService.SearchCampgrounds(
                keywords: sTerms,
                categoryIds: category != null && category.Id > 2 ? new List<int>(new int[] { category.Id }) : null,
                languageId: _workContext.WorkingLanguage.Id);

            var model = new CampgroundOverviewModel
            {
                Id = category != null ? category.Id : 0,
                Name = "Search for " + sTerms,
                Description = category != null ? category.GetLocalized(x => x.Description) : "Search for " + sTerms,
                MetaKeywords = category != null ? category.GetLocalized(x => x.MetaKeywords) : string.Empty,
                MetaDescription = category != null ? category.GetLocalized(x => x.MetaDescription) : "Campground Search - " + sTerms,
                MetaTitle = category != null ? category.GetLocalized(x => x.MetaTitle) : "Campground Search - " + sTerms,
                SeName = category != null ? SeoExtensions.GetSeName(category.Id, "Campgrounds", 0) : string.Empty,
                DefaultPictureModel = category != null && category.PictureId != 0 ? PrepareCampgroundDefaultPictureModel(category) : null,
                ParentCategoryId = campgrounds.Count() == 0 ? category.ParentCategoryId : -1, //Set to negative number to enable title
                NumberOfCampgrounds = campgrounds.Count()
            };

            //sorting
            PrepareSortingOptions(model.PagingFilteringContext, command);
            //view mode
            PrepareViewModes(model.PagingFilteringContext, command);
            //page size
            PreparePageSizeOptions(model.PagingFilteringContext, command,
                category != null ? category.AllowCustomersToSelectPageSize : true,
                category != null ? category.PageSizeOptions : "10, 25, 50",
                category != null ? category.PageSize : 10);

            model.Campgrounds = PrepareCampgroundDetailModels(campgrounds, category).ToList();

            model.PagingFilteringContext.LoadPagedList(campgrounds);

            return model;
        }

        /// <summary>
        /// Prepare campground customer review model
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public virtual CustomerCampgroundReviewsModel PrepareCustomerCampgroundReviewsModel(int? page)
        {
            var pageSize = _campgroundSettings.CampgroundReviewsPageSizeOnAccountPage;
            var pageIndex = 0;

            if (page > 0)
            {
                pageIndex = page.Value - 1;
            }

            var list = _campgroundService.GetAllCampgroundReviews(customerId: _workContext.CurrentCustomer.Id,
                approved: null,
                pageIndex: pageIndex,
                pageSize: pageSize);

            var campgroundReviews = new List<CustomerCampgroundReviewModel>();

            foreach (var review in list)
            {
                var campground = review.Campground;
                var campgroundReviewModel = new CustomerCampgroundReviewModel
                {
                    Title = review.Title,
                    CampgroundId = campground.Id,
                    CampgroundName = campground.GetLocalized(p => p.Name),
                    CampgroundSeName = campground.GetSeName(),
                    Rating = review.Rating,
                    ReviewText = review.ReviewText,
                    ReplyText = review.ReplyText,
                    WrittenOnStr = _dateTimeHelper.ConvertToUserTime(campground.CreatedOnUtc, DateTimeKind.Utc).ToString("g")
                };

                if (_campgroundSettings.CampgroundReviewsMustBeApproved)
                {
                    campgroundReviewModel.ApprovalStatus = review.IsApproved
                        ? _localizationService.GetResource("Account.CustomerCampgroundReviews.ApprovalStatus.Approved")
                        : _localizationService.GetResource("Account.CustomerCampgroundReviews.ApprovalStatus.Pending");
                }
                campgroundReviews.Add(campgroundReviewModel);
            }

            var pagerModel = new Web.Models.Common.PagerModel
            {
                PageSize = list.PageSize,
                TotalRecords = list.TotalCount,
                PageIndex = list.PageIndex,
                ShowTotalSummary = false,
                RouteActionName = "CustomerCampgroundReviewsPaged",
                UseRouteLinks = true,
                RouteValues = new CustomerCampgroundReviewsModel.CustomerCampgroundReviewsRouteValues { pageNumber = pageIndex }
            };

            var model = new CustomerCampgroundReviewsModel
            {
                CampgroundReviews = campgroundReviews,
                PagerModel = pagerModel
            };

            return model;
        }

        /// <summary>
        /// Prepare Campground Overview Model
        /// </summary>
        /// <param name="category"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public CampgroundOverviewModel PrepareCampgroundOverviewModel(Category category, CampgroundPagingFilteringModel command)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            var model = new CampgroundOverviewModel
            {
                Id = category.Id,
                Name = category.GetLocalized(x => x.Name),
                Description = category.GetLocalized(x => x.Description),
                MetaKeywords = category.GetLocalized(x => x.MetaKeywords),
                MetaDescription = category.GetLocalized(x => x.MetaDescription),
                MetaTitle = category.GetLocalized(x => x.MetaTitle),
                DefaultPictureModel = category.PictureId != 0 ? PrepareCampgroundDefaultPictureModel(category) : null,
                SeName = SeoExtensions.GetSeName(category.Id, "Campgrounds", 0),
                ParentCategoryId = category.ParentCategoryId,
                NumberOfCampgrounds = GetCampgroundCount(0)
            };

            //sorting
            PrepareSortingOptions(model.PagingFilteringContext, command);
            //view mode
            PrepareViewModes(model.PagingFilteringContext, command);
            //page size
            PreparePageSizeOptions(model.PagingFilteringContext, command,
                category.AllowCustomersToSelectPageSize,
                category.PageSizeOptions,
                category.PageSize);

            IPagedList<Campground> campgrounds;
            if (model.ParentCategoryId == 0)
            {
                campgrounds = _campgroundService.GetFeaturedCampgrounds(pageIndex: command.PageNumber - 1, pageSize: command.PageSize);
                var cacheKey = string.Format(ModelCacheEventConsumer.CAMPGROUND_MENU_MODEL_KEY, _storeContext.CurrentStore.Id);
                model.CampgroundCategory = _cacheManager.Get(cacheKey, () => PrepareCampgroundCategoryModels(model.Id));
            }
            else
                campgrounds = _campgroundService.GetCampgroundsByCategoryId(category.Id, pageIndex: command.PageNumber - 1, pageSize: command.PageSize);

            model.Campgrounds = PrepareCampgroundDetailModels(campgrounds, category).ToList();

            model.PagingFilteringContext.LoadPagedList(campgrounds);

            return model;
        }

        /// <summary>
        /// Prepare the campground review overview model
        /// </summary>
        /// <param name="campground"></param>
        /// <returns></returns>
        protected virtual CampgroundReviewOverviewModel PrepareCampgroundReviewOverviewModel(Campground campground)
        {
            CampgroundReviewOverviewModel campgroundReview;

            if (_campgroundSettings.ShowCampgroundReviewsPerStore)
            {
                var cacheKey = string.Format(ModelCacheEventConsumer.CAMPGROUND_REVIEWS_MODEL_KEY, campground.Id, _storeContext.CurrentStore.Id);

                campgroundReview = _cacheManager.Get(cacheKey, () =>
                {
                    return new CampgroundReviewOverviewModel
                    {
                        RatingSum = campground.CampgroundReviews
                                .Where(cr => cr.IsApproved && cr.StoreId == _storeContext.CurrentStore.Id)
                                .Sum(cr => cr.Rating),
                        TotalReviews = campground
                                .CampgroundReviews
                                .Count(cr => cr.IsApproved && cr.StoreId == _storeContext.CurrentStore.Id)
                    };
                });
            }
            else
            {
                campgroundReview = new CampgroundReviewOverviewModel()
                {
                    RatingSum = campground.ApprovedRatingSum,
                    TotalReviews = campground.ApprovedTotalReviews
                };
            }
            if (campgroundReview != null)
            {
                campgroundReview.CampgroundId = campground.Id;
                campgroundReview.AllowCampgroundReviews = campground.AllowCampgroundReviews;
            }
            return campgroundReview;
        }

        /// <summary>
        /// Prepare Campground Reviews Model
        /// </summary>
        /// <param name="model"></param>
        /// <param name="campground"></param>
        /// <returns></returns>
        public virtual CampgroundReviewsModel PrepareCampgroundReviewsModel(CampgroundReviewsModel model, Campground campground)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (campground == null)
                throw new ArgumentNullException(nameof(campground));

            model.CampgroundId = campground.Id;
            model.CampgroundName = campground.GetLocalized(x => x.Name);
            model.Description = campground.GetLocalized(x => x.FullDescription);
            model.CampgroundSeName = campground.GetSeName();
            model.SeName = SeoExtensions.GetSeName(GetCampgroundParentCategoryId(campground), "Campgrounds", 0);

            var campgroundReviews = _campgroundSettings.ShowCampgroundReviewsPerStore
                ? campground.CampgroundReviews.Where(cr => cr.IsApproved && cr.StoreId == _storeContext.CurrentStore.Id).OrderBy(cr => cr.CreatedOnUtc)
                : campground.CampgroundReviews.Where(cr => cr.IsApproved).OrderBy(cr => cr.CreatedOnUtc);
            foreach (var cr in campgroundReviews)
            {
                var customer = cr.Customer;
                model.Items.Add(new CampgroundReviewModel
                {
                    Id = cr.Id,
                    CustomerId = cr.CustomerId,
                    //CustomerName = customer.FormatUserName(),
                    AllowViewingProfiles = _customerSettings.AllowViewingProfiles && customer != null, // && !customer.IsGuest(),
                    Title = cr.Title,
                    ReviewText = cr.ReviewText,
                    ReplyText = cr.ReplyText,
                    Rating = cr.Rating,
                    Helpfulness = new CampgroundReviewHelpfulnessModel
                    {
                        CampgroundReviewId = cr.Id,
                        HelpfulYesTotal = cr.HelpfulYesTotal,
                        HelpfulNoTotal = cr.HelpfulNoTotal,
                    },
                    Pictures = campground.CampgroundPictures.Where(pic => pic.ReviewId == cr.Id)
                        .Select(x => new PictureModel
                        {
                            ImageUrl = _pictureService.GetPictureUrl(x.Picture.Id),
                            ThumbImageUrl = _pictureService.GetPictureUrl(x.Picture.Id, 150),
                            AlternateText = (x.Picture != null && !string.IsNullOrEmpty(x.Picture.AltAttribute)) ? x.Picture.AltAttribute : "Campground Review Picture",
                        }).ToList(),
                    WrittenOnStr = _dateTimeHelper.ConvertToUserTime(cr.CreatedOnUtc, DateTimeKind.Utc).ToString("D"),
                });
            }

            model.AddCampgroundReview.CanCurrentCustomerLeaveReview = _campgroundSettings.AllowAnonymousUsersToReviewCampground || !_workContext.CurrentCustomer.IsGuest();
            model.AddCampgroundReview.DisplayCaptcha = _captchaSettings.Enabled && _captchaSettings.ShowOnCampgroundReviewPage;

            return model;
        }

        /// <summary>
        /// Prepare Campground Navigation Model
        /// </summary>
        /// <param name="currentCategoryId"></param>
        /// <param name="currentCampgroundId"></param>
        /// <returns></returns>
        public virtual CampgroundNavigationModel PrepareCampgroundNavigationModel(int currentCategoryId, int currentCampgroundId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Prepare Homepage Campground Detail Models
        /// </summary>
        /// <returns></returns>
        public virtual List<CampgroundDetailModel> PrepareHomepageCampgroundDetailModels()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Prepare the apply campground model
        /// </summary>
        /// <param name="model">The apply campground model</param>
        /// <param name="validateCampground">Whether to validate that the customer is already a campground</param>
        /// <param name="excludeProperties">Whether to exclude populating of model properties from the entity</param>
        /// <returns>The apply campground model</returns>
        public virtual CampgroundRegisterModel PrepareCampgroundRegisterModel(CampgroundRegisterModel model, bool validateCampground, bool excludeProperties)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (validateCampground)
            {
                //already applied for campground account
                model.DisableFormInput = true;
                model.Result = _localizationService.GetResource("CampgroundHost.ApplyAccount.AlreadyApplied");
            }

            model.Campground = new CampgroundModel();
            PrepareCampgroundModel(model.Campground, new Campground());
            model.DisplayCaptcha = _captchaSettings.Enabled && _captchaSettings.ShowOnApplyCampgroundPage;
            model.TermsOfServiceEnabled = _campgroundSettings.TermsOfServiceEnabled;
            model.TermsOfServicePopup = _campgroundSettings.TermsOfServicePopup;

            if (!excludeProperties)
            {
                model.Campground.CampgroundAddress.Email = _workContext.CurrentCustomer.Email;
            }

            return model;
        }

        //#region Campground tags

        ///// <summary>
        ///// Prepare popular campground tags model
        ///// </summary>
        ///// <returns>Campground tags model</returns>
        //public virtual PopularCampgroundTagsModel PreparePopularCampgroundTagsModel()
        //{
        //    var cacheKey = string.Format(ModelCacheEventConsumer.CAMPGROUNDTAG_POPULAR_MODEL_KEY, _workContext.WorkingLanguage.Id, _storeContext.CurrentStore.Id);
        //    var cachedModel = _cacheManager.Get(cacheKey, () =>
        //    {
        //        var model = new PopularCampgroundTagsModel();

        //        //get all tags
        //        var allTags = _campgroundTagService
        //            .GetAllCampgroundTags()
        //            //filter by current store
        //            .Where(x => _campgroundTagService.GetCampgroundCount(x.Id, _storeContext.CurrentStore.Id) > 0)
        //            //order by campground count
        //            .OrderByDescending(x => _campgroundTagService.GetCampgroundCount(x.Id, _storeContext.CurrentStore.Id))
        //            .ToList();

        //        var tags = allTags
        //            .Take(_catalogSettings.NumberOfCampgroundTags)
        //            .ToList();
        //        //sorting
        //        tags = tags.OrderBy(x => x.GetLocalized(y => y.Name)).ToList();

        //        model.TotalTags = allTags.Count;

        //        foreach (var tag in tags)
        //            model.Tags.Add(new CampgroundTagModel
        //            {
        //                Id = tag.Id,
        //                Name = tag.GetLocalized(y => y.Name),
        //                SeName = tag.GetSeName(),
        //                CampgroundCount = _campgroundTagService.GetCampgroundCount(tag.Id, _storeContext.CurrentStore.Id)
        //            });
        //        return model;
        //    });

        //    return cachedModel;
        //}

        ///// <summary>
        ///// Prepare campgrounds by tag model
        ///// </summary>
        ///// <param name="campgroundTag">Campground tag</param>
        ///// <param name="command">Catalog paging filtering command</param>
        ///// <returns>Campgrounds by tag model</returns>
        //public virtual CampgroundsByTagModel PrepareCampgroundsByTagModel(CampgroundTag campgroundTag, CatalogPagingFilteringModel command)
        //{
        //    if (campgroundTag == null)
        //        throw new ArgumentNullException(nameof(campgroundTag));

        //    var model = new CampgroundsByTagModel
        //    {
        //        Id = campgroundTag.Id,
        //        TagName = campgroundTag.GetLocalized(y => y.Name),
        //        TagSeName = campgroundTag.GetSeName()
        //    };

        //    //sorting
        //    PrepareSortingOptions(model.PagingFilteringContext, command);
        //    //view mode
        //    PrepareViewModes(model.PagingFilteringContext, command);
        //    //page size
        //    PreparePageSizeOptions(model.PagingFilteringContext, command,
        //        _catalogSettings.CampgroundsByTagAllowCustomersToSelectPageSize,
        //        _catalogSettings.CampgroundsByTagPageSizeOptions,
        //        _catalogSettings.CampgroundsByTagPageSize);

        //    //campgrounds
        //    var campgrounds = _campgroundService.SearchCampgrounds(
        //        storeId: _storeContext.CurrentStore.Id,
        //        campgroundTagId: campgroundTag.Id,
        //        visibleIndividuallyOnly: true,
        //        orderBy: (CampgroundSortingEnum)command.OrderBy,
        //        pageIndex: command.PageNumber - 1,
        //        pageSize: command.PageSize);
        //    model.Campgrounds = _campgroundModelFactory.PrepareCampgroundOverviewModels(campgrounds).ToList();

        //    model.PagingFilteringContext.LoadPagedList(campgrounds);
        //    return model;
        //}

        ///// <summary>
        ///// Prepare campground tags all model
        ///// </summary>
        ///// <returns>Popular campground tags model</returns>
        //public virtual PopularCampgroundTagsModel PrepareCampgroundTagsAllModel()
        //{
        //    var model = new PopularCampgroundTagsModel
        //    {
        //        Tags = _campgroundTagService
        //        .GetAllCampgroundTags()
        //        //filter by current store
        //        .Where(x => _campgroundTagService.GetCampgroundCount(x.Id, _storeContext.CurrentStore.Id) > 0)
        //        //sort by name
        //        .OrderBy(x => x.GetLocalized(y => y.Name))
        //        .Select(x =>
        //        {
        //            var ptModel = new CampgroundTagModel
        //            {
        //                Id = x.Id,
        //                Name = x.GetLocalized(y => y.Name),
        //                SeName = x.GetSeName(),
        //                CampgroundCount = _campgroundTagService.GetCampgroundCount(x.Id, _storeContext.CurrentStore.Id)
        //            };
        //            return ptModel;
        //        })
        //        .ToList()
        //    };
        //    return model;
        //}

        //#endregion

        #endregion

    }
}
