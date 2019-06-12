using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Infrastructure;
using Nop.Plugin.Campgrounds.Services;
using Nop.Plugin.Widgets.Campgrounds.Factories;
using Nop.Plugin.Widgets.Campgrounds.Models;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Seo;
using Nop.Services.Stores;
using Nop.Web.Controllers;
using Nop.Web.Framework;

namespace Nop.Plugin.Widgets.Campgrounds.Controllers
{
    public class CampgroundMenuController : BasePublicController
    {
        #region Fields
        private readonly ICampgroundService _campgroundService;
        private readonly ICategoryService _categoryService;
        private readonly IWorkContext _workContext;
        private readonly ICampgroundModelFactory _CampgroundModelFactory;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly ILocalizationService _localizationService;
        private readonly IStoreService _storeService;
        #endregion

        #region Ctor
        public CampgroundMenuController(IWorkContext workContext,
            IStoreService storeService,
            ICampgroundModelFactory CampgroundModelFactory,
            ICategoryService categoryService,
            ICustomerActivityService customerActivityService,
            ILocalizationService localizationService,
            ICampgroundService campgroundService)
        {
            this._workContext = workContext;
            this._storeService = storeService;
            this._CampgroundModelFactory = CampgroundModelFactory;
            this._categoryService = categoryService;
            this._customerActivityService = customerActivityService;
            this._localizationService = localizationService;
            this._campgroundService = campgroundService;
        }

        #endregion

        #region Utilities

        #endregion

        #region Methods

        [Area(AreaNames.Admin)]
        public IActionResult Configure()
        {
            return View("~/Plugins/Campgrounds/Views/Configure.cshtml");
        }

        public IActionResult PublicInfo(string widgetZone)
        {
            return View("~/Plugins/Campgrounds/Views/PublicInfo.cshtml");
        }

        public IActionResult CampgroundsByCategory(string SeName, CampgroundPagingFilteringModel command)
        {
            var urlRecordService = EngineContext.Current.Resolve<IUrlRecordService>();
            var urlRecord = urlRecordService.GetBySlugCached(SeName);

            var category = urlRecord != null ? _categoryService.GetCategoryById(urlRecord.EntityId): null;
            if (category == null || category.Deleted)
                return InvokeHttp404();

            //activity log
            _customerActivityService.InsertActivity("PublicStore.ViewCampground", _localizationService.GetResource("ActivityLog.PublicStore.ViewCategory"), category.Name);

            var model = _CampgroundModelFactory.PrepareCampgroundOverviewModel(category, command);
            //return View(templateViewPath, model);

            return View("~/Plugins/Campgrounds/Views/Campground/CampgroundSummary.InGridOrLines.cshtml", model);
        }


        #endregion

    }
}
