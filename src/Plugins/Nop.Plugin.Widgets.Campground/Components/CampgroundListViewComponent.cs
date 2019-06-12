using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Plugin.Widgets.Campgrounds.Factories;
using Nop.Plugin.Widgets.Campgrounds.Infrastructure.Cache;
using Nop.Services.Configuration;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Widgets.Campgrounds.Components
{
    [ViewComponent(Name = "CampgroundList")]
    public class CampgroundListViewComponent : NopViewComponent
    {
        private readonly IStoreContext _storeContext;
        private readonly IStaticCacheManager _cacheManager;
        private readonly ISettingService _settingService;
        private readonly ICampgroundModelFactory _campgroundModelFactory;

        public CampgroundListViewComponent(
            IStoreContext storeContext,
            IStaticCacheManager cacheManager,
            ISettingService settingService,
            ICampgroundModelFactory campgroundModelFactory)
        {
            this._storeContext = storeContext;
            this._cacheManager = cacheManager;
            this._settingService = settingService;
            this._campgroundModelFactory = campgroundModelFactory;
        }

        public IViewComponentResult Invoke(string widgetZone, object additionalData)
        {
            var cacheKey = string.Format(ModelCacheEventConsumer.CAMPGROUND_MENU_MODEL_KEY, _storeContext.CurrentStore.Id);
            var model = _cacheManager.Get(cacheKey, () => _campgroundModelFactory.PrepareCampgroundCategoryModels(2));

            return View(viewName: "~/Plugins/Campgrounds/Views/Shared/_CampgroundLine.TopMenu.cshtml", model: model);
        }
    }
}
