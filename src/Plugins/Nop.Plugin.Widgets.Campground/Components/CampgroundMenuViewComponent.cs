using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Services.Configuration;
using Nop.Services.Catalog;
using Nop.Plugin.Campgrounds.Services;
using Nop.Web.Framework.Components;
using Nop.Core.Domain.Directory;
using Nop.Core.Data;
using Nop.Plugin.Campgrounds.Data.Domain;
using Nop.Plugin.Widgets.Campgrounds.Factories;
using Nop.Plugin.Widgets.Campgrounds.Infrastructure.Cache;
using Nop.Plugin.Widgets.Campgrounds.Models;

namespace Nop.Plugin.Widgets.Campgrounds.Components
{
    [ViewComponent(Name = "CampgroundMenu")]
    public class CampgroundMenuViewComponent : NopViewComponent
    {
        private readonly IStoreContext _storeContext;
        private readonly IStaticCacheManager _cacheManager;
        private readonly ISettingService _settingService;
        private readonly ICategoryService _categoryService;
        private readonly ICampgroundService _campgroundServices;
        private readonly ICampgroundModelFactory _campgroundModelFactory;
        private readonly IRepository<Campground> _campgroundRepository;
        private readonly IRepository<StateProvince> _stateProvinceRepository;

        public CampgroundMenuViewComponent(
            IStoreContext storeContext,
            IStaticCacheManager cacheManager,
            ISettingService settingService,
            ICategoryService categoryService,
            ICampgroundService campgroundService,
            ICampgroundModelFactory campgroundModelFactory,
            IRepository<Campground> campgroundRepository,
            IRepository<StateProvince> stateProvinceRepository)
        {
            this._storeContext = storeContext;
            this._cacheManager = cacheManager;
            this._settingService = settingService;
            this._categoryService = categoryService;
            this._campgroundServices = campgroundService;
            this._campgroundModelFactory = campgroundModelFactory;
            this._campgroundRepository = campgroundRepository;
            this._stateProvinceRepository = stateProvinceRepository;
        }

        public IViewComponentResult Invoke(CampgroundCategoryModel model, bool topMenu = false, bool supressNearby = false)
        {
            //load and cache them
            var cacheKey = string.Format(ModelCacheEventConsumer.CAMPGROUND_MENU_MODEL_KEY, _storeContext.CurrentStore.Id);
            model = model == null ? _cacheManager.Get(cacheKey, () => _campgroundModelFactory.PrepareCampgroundCategoryModels(2)) : model;

            model.IncludeInTopMenu = topMenu;
            model.SuppressNearby = supressNearby;

            return View("~/Plugins/Campgrounds/Views/Shared/Components/CampgroundMenu/Default.cshtml", model);
        }

    }
}
