using System.Collections.Generic;
using Microsoft.AspNetCore.Routing;
using Nop.Core;
using Nop.Core.Plugins;
using Nop.Plugin.Campgrounds.Data;
using Nop.Plugin.Campgrounds.Services;
using Nop.Services.Cms;
using Nop.Services.Common;
using Nop.Services.Configuration;

namespace Nop.Plugin.Widgets.Campgrounds.Entity
{
    class CampgroundMenuPlugin : BasePlugin, IWidgetPlugin
    {
        private readonly IWebHelper _webHelper;
        private readonly ISettingService _settingService;
        private readonly CampgroundsObjectContext _campgroundsObjectContext;

        public CampgroundMenuPlugin(IWebHelper webHelper, 
            ISettingService settingService, 
            CampgroundsObjectContext campgroundsObjectContext)
        {
            this._webHelper = webHelper;
            this._settingService = settingService;
            this._campgroundsObjectContext = campgroundsObjectContext;
        }

        public IList<string> GetWidgetZones()
        {
            return new List<string> { "menu_campground_top", "campground_list", "campground_detail" };
        }

        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public override string GetConfigurationPageUrl()
        {
            return _webHelper.GetStoreLocation() + "Admin/CampgroundMenu/Configure";
        }

        public string GetWidgetViewComponentName(string widgetZone)
        {
            if (widgetZone == "menu_campground_top")
                return "CampgroundList";
            else if (widgetZone == "campground_list")
                return "CampgroundList";
            else if (widgetZone == "campground_detail")
                return "CampgroundDetail";
            else
                return string.Empty;
        }

        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "CampgroundMenuController";
            routeValues = new RouteValueDictionary { { "Namespaces", "Nop.Plugin.Widgets.Campgrounds.Modelss.Controllers" }, { "area", "campgrounds" } };
        }

        public override void Install()
        {
            // TODO - INSERT Campgrounds by State into URLRecords table

            //this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.CampgroundMenu", "");

            base.Install();
        }

        public override void Uninstall()
        {
            base.Uninstall();
        }
    }
}
