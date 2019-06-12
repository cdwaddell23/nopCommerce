using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Blogs;
using Nop.Core.Domain.Catalog;
using Nop.Plugin.Campgrounds.Data.Domain;
using Nop.Plugin.Campgrounds.Services;
using Nop.Plugin.Widgets.Campgrounds.Factories;
using Nop.Plugin.Widgets.Campgrounds.Infrastructure.Cache;
using Nop.Services.Orders;
using Nop.Services.Security;
using Nop.Services.Stores;
using Nop.Web.Factories;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Widgets.Campgrounds.Components
{
    public class CampingBlogsViewComponent : NopViewComponent
    {
        public const string COMPONENT_VIEW = "~/Plugins/Campgrounds/Views/Shared/Components/CampingBlogs/Default.cshtml";

        private readonly ICampgroundModelFactory _campgroundModelFactory;
        private readonly BlogSettings _blogSettings;

        public CampingBlogsViewComponent(ICampgroundModelFactory campgroundModelFactory,
            BlogSettings blogSettings)
        {
            this._campgroundModelFactory = campgroundModelFactory;
            this._blogSettings = blogSettings;
        }

        public IViewComponentResult Invoke(int? CampgroundThumbPictureSize)
        {

            if (!_blogSettings.Enabled)
                return Content("");

            var model = _campgroundModelFactory.PrepareHomePageBlogItemsModel();
            return View(COMPONENT_VIEW, model);

        }
    }
}
