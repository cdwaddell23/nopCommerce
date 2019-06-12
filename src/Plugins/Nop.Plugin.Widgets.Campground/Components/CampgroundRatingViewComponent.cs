using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Plugin.Campgrounds.Services;
using Nop.Plugin.Widgets.Campgrounds.Factories;
using Nop.Plugin.Widgets.Campgrounds.Infrastructure.Cache;
using Nop.Plugin.Widgets.Campgrounds.Models;
using Nop.Services.Configuration;
using Nop.Web.Framework.Components;
using Nop.Web.Framework.Mvc.Models;

namespace Nop.Plugin.Widgets.Campgrounds.Components
{
    [ViewComponent(Name = "CampgroundRating")]
    public class CampgroundRatingViewComponent : NopViewComponent
    {
        private readonly IStoreContext _storeContext;
        private readonly IStaticCacheManager _cacheManager;
        private readonly ISettingService _settingService;
        private readonly ICampgroundModelFactory _campgroundModelFactory;
        private readonly ICampgroundService _campgroundService;

        public CampgroundRatingViewComponent(
            IStoreContext storeContext,
            IStaticCacheManager cacheManager,
            ISettingService settingService,
            ICampgroundModelFactory campgroundModelFactory,
            ICampgroundService campgroundService)
        {
            this._storeContext = storeContext;
            this._cacheManager = cacheManager;
            this._settingService = settingService;
            this._campgroundModelFactory = campgroundModelFactory;
            this._campgroundService = campgroundService;
        }

        public IViewComponentResult Invoke(CampgroundDetailModel model, int campgroundId)
        {

            //add NULL check
            if (model == null && campgroundId > 0)
            {
                var campground = _campgroundService.GetCampgroundById(campgroundId);
                model = _campgroundModelFactory.PrepareCampgroundDetailModel(campground, null, null, false);
            }
            model.ShowReviewRatingFrame = campgroundId == 0;

            return View(viewName: "~/Plugins/Campgrounds/Views/Shared/Components/CampgroundRating/default.cshtml", model: model);
        }
    }

}
