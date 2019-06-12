using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Plugin.Campgrounds.Data.Domain;
using Nop.Plugin.Campgrounds.Services;
using Nop.Plugin.Widgets.Campgrounds.Factories;
using Nop.Plugin.Widgets.Campgrounds.Infrastructure.Cache;
using Nop.Services.Orders;
using Nop.Services.Security;
using Nop.Services.Stores;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Widgets.Campgrounds.Components
{
    public class NewCampgroundsViewComponent : NopViewComponent
    {
        public const string COMPONENT_VIEW = "~/Plugins/Campgrounds/Views/Shared/Components/NewCampgrounds/Default.cshtml";

        private readonly CampgroundSettings _campgroundSettings;
        private readonly ICampgroundModelFactory _campgroundModelFactory;
        private readonly ICampgroundService _campgroundService;
        private readonly IStoreContext _storeContext;
        private readonly IAclService _aclService;
        private readonly IStoreMappingService _storeMappingService;
        private readonly IOrderReportService _orderReportService;
        private readonly IStaticCacheManager _cacheManager;

        public NewCampgroundsViewComponent(CampgroundSettings campgroundSettings,
            ICampgroundModelFactory campgroundModelFactory,
            ICampgroundService campgroundService,
            IStoreContext storeContext,
            IAclService aclService,
            IStoreMappingService storeMappingService,
            IOrderReportService orderReportService,
            IStaticCacheManager cacheManager)
        {
            this._campgroundSettings = campgroundSettings;
            this._campgroundModelFactory = campgroundModelFactory;
            this._campgroundService = campgroundService;
            this._storeContext = storeContext;
            this._aclService = aclService;
            this._storeMappingService = storeMappingService;
            this._orderReportService = orderReportService;
            this._cacheManager = cacheManager;
        }

        public IViewComponentResult Invoke(int? CampgroundThumbPictureSize)
        {
            if (!_campgroundSettings.ShowNewCampgroundsOnHomepage || _campgroundSettings.NumberOfNewCampgroundsOnHomepage == 0)
                return Content("");

            //load and cache report
            var results = _cacheManager.Get(string.Format(ModelCacheEventConsumer.HOMEPAGE_NEWCAMPGROUNDS_IDS_KEY, _storeContext.CurrentStore.Id),
                () => _campgroundService.GetNewCampgrounds(
                        pageSize: _campgroundSettings.NumberOfNewCampgroundsOnHomepage));

            //load Campgrounds
            var campgrounds = _campgroundService.GetCampgroundsByIds(results.Select(x => x.Id).ToArray());
            //ACL and store mapping
            campgrounds = campgrounds.Where(p => _aclService.Authorize(p) && _storeMappingService.Authorize(p)).ToList();
            //availability dates
            campgrounds = campgrounds.Where(p => p.IsAvailable()).ToList();

            if (!campgrounds.Any())
                return Content("");

            //prepare model
            var model = _campgroundModelFactory.PrepareCampgroundDetailModels(campgrounds).Take(_campgroundSettings.NumberOfNewCampgroundsOnHomepage).ToList();
            return View(COMPONENT_VIEW, model);
        }
    }
}
