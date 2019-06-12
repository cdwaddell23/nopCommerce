using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Plugin.Campgrounds.Data.Domain;
using Nop.Plugin.Campgrounds.Services;
using Nop.Plugin.Widgets.Campgrounds.Factories;
using Nop.Plugin.Widgets.Campgrounds.Models;
using Nop.Services.Localization;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Widgets.Campgrounds.Components
{
    public class CampgroundReviewContentViewComponent : NopViewComponent
    {
        public const string COMPONENT_VIEW = "~/Plugins/Campgrounds/Views/Shared/Components/CampgroundReviewContent/Default.cshtml";

        private readonly CampgroundSettings _campgroundSettings;
        private readonly ICampgroundModelFactory _campgroundModelFactory;
        private readonly ICampgroundService _campgroundService;
        private readonly IWorkContext _workContext;
        private readonly ILocalizationService _localizationService;

        public CampgroundReviewContentViewComponent(CampgroundSettings campgroundSettings,
            ICampgroundModelFactory campgroundModelFactory,
            ICampgroundService campgroundService,
            ILocalizationService localizationService,
            IWorkContext workContext)
        {
            this._campgroundSettings = campgroundSettings;
            this._campgroundModelFactory = campgroundModelFactory;
            this._campgroundService = campgroundService;
            this._localizationService = localizationService;
            this._workContext = workContext;
        }

        public IViewComponentResult Invoke(int campgroundId)
        {
            var campground = _campgroundService.GetCampgroundById(campgroundId);

            var model = new CampgroundReviewsModel();
            model = _campgroundModelFactory.PrepareCampgroundReviewsModel(model, campground);
            //only registered users can leave reviews
            if (_workContext.CurrentCustomer.IsGuest() && !_campgroundSettings.AllowAnonymousUsersToReviewCampground)
                ModelState.AddModelError("", _localizationService.GetResource("Reviews.OnlyRegisteredUsersCanWriteReviews"));

            //default value
            model.AddCampgroundReview.Rating = _campgroundSettings.DefaultCampgroundRatingValue;
            return View(COMPONENT_VIEW, model);
        }
    }
}
