using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Campgrounds.Data.Domain;
using Nop.Plugin.Widgets.Campgrounds.Factories;
using Nop.Plugin.Widgets.Campgrounds.Models;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Widgets.Campgrounds.Components
{
    public class CampingNearbyViewComponent : NopViewComponent
    {
        public const string COMPONENT_VIEW = "~/Plugins/Campgrounds/Views/Shared/Components/CampingNearby/Default.cshtml";

        private readonly CampgroundSettings _campgroundSettings;
        private readonly ICampgroundModelFactory _campgroundModelFactory;

        public CampingNearbyViewComponent(CampgroundSettings campgroundSettings,
            ICampgroundModelFactory campgroundModelFactory)
        {
            this._campgroundSettings = campgroundSettings;
            this._campgroundModelFactory = campgroundModelFactory;
        }

        public IViewComponentResult Invoke()
        {
            //prepare model
            var model = new CampgroundAddressModel();

            return View(COMPONENT_VIEW, model);
        }
    }
}
