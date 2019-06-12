using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Campgrounds.Data.Domain;
using Nop.Plugin.Campgrounds.Services;
using Nop.Plugin.Widgets.Campgrounds.Factories;
using Nop.Plugin.Widgets.Campgrounds.Models;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Widgets.Campgrounds.Components
{
    public class SearchCampgroundsViewComponent : NopViewComponent
    {
        public const string COMPONENT_VIEW = "~/Plugins/Campgrounds/Views/Shared/Components/SearchCampgrounds/Default.cshtml";

        private readonly CampgroundSettings _campgroundSettings;
        private readonly ICampgroundModelFactory _campgroundModelFactory;
        private readonly ICampgroundService _campgroundService;

        public SearchCampgroundsViewComponent(CampgroundSettings campgroundSettings,
            ICampgroundModelFactory campgroundModelFactory,
            ICampgroundService campgroundService)
        {
            this._campgroundSettings = campgroundSettings;
            this._campgroundModelFactory = campgroundModelFactory;
            this._campgroundService = campgroundService;
        }

        public IViewComponentResult Invoke(int categoryId)
        {
            if (!_campgroundSettings.ShowSearchCampgroundsOnHomepage)
                return Content("");

            //prepare model
            var model = new CampgroundSearchBoxModel();
            var stateId = _campgroundService.GetCampgroundStateIdFromCategoryId(categoryId);

            model.AvailableStates = _campgroundModelFactory.PrepareAvailableStates(Id: stateId);
            return View(COMPONENT_VIEW, model);
        }
    }
}
