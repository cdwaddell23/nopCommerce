using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework.Localization;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.Widgets.Campgrounds.Infrastructure
{
    public partial class RouteProvider : IRouteProvider
    {
        /// <summary>
        /// Register routes
        /// </summary>
        /// <param name="routeBuilder">Route builder</param>
        public void RegisterRoutes(IRouteBuilder routeBuilder)
        {
            //angular app routing
            //routeBuilder.MapRoute("API_AddRating", "camping/api/campgrounds/review/add-rating",
            //    new { controller = "Review", action = "AddRating" });
            //routeBuilder.MapRoute("API_AddReview", "camping/api/campgrounds/review/add-review",
            //    new { controller = "Review", action = "AddReview" });
            //routeBuilder.MapRoute("API_AddPhotos", "camping/api/campgrounds/review/add-photo",
            //    new { controller = "Review", action = "AddPhoto" });
            //routeBuilder.MapRoute("API_Root", "camping/api/campgrounds/review/{id?}",
            //    new { controller = "Review", action = "Index" });
            routeBuilder.MapSpaFallbackRoute(
                name: "spa-fallback",
                defaults: new { controller = "Api", action = "Index" });

            //pinterest-83023.html
            routeBuilder.MapRoute("pinterest-83023.html", "pinterest-83023.html",
                new { controller = "Common", action = "PinterestVerify" });

            routeBuilder.MapRoute("offline.html", "offline",
                new { controller = "Campground", action = "Offline" });

            //add route for the access token callback
            routeBuilder.MapRoute("Campgrounds", "camping",
                new { controller = "Campground", action = "Campgrounds" });

            routeBuilder.MapRoute("CampgroundNearby", "camping/nearby",
                new { controller = "Campground", action = "CampgroundNearby" });

            //set review helpfulness (AJAX link)
            routeBuilder.MapRoute("SetCampgroundReviewHelpfulness", "camping/setcampgroundreviewhelpfulness",
                new { controller = "Campground", action = "SetCampgroundReviewHelpfulness" });

            routeBuilder.MapLocalizedRoute("CampgroundRegister", "camping/campgrounds/add-campground",
                new { controller = "CampgroundRegister", action = "CampgroundRegister" });

            routeBuilder.MapLocalizedRoute("CampgroundRegisterVerifyAddress", "camping/campgrounds/verify-address",
                new { controller = "CampgroundRegister", action = "VerifyAddress" });

            routeBuilder.MapLocalizedRoute("CampgroundSearch", "camping/search/{sTerm?}",
                new { controller = "Campground", action = "SearchCampgrounds" });

            routeBuilder.MapLocalizedRoute("CampgroundCategorySearch", "camping/{SeName}/search/{sTerm?}",
                new { controller = "Campground", action = "SearchCampgrounds" });

            routeBuilder.MapLocalizedRoute("CampgroundSearchAutoComplete", "camping/{SeName}/searchtermautocomplete",
                new { controller = "Campground", action = "SearchCampgroundsTermAutoComplete" });

            routeBuilder.MapRoute("CampgroundCategories", "camping/{SeName}",
                new { controller = "CampgroundMenu", action = "CampgroundsByCategory" });

            routeBuilder.MapRoute("CampgroundDetail", "camping/{SeName}/{campgroundSeName}",
                new { controller = "Campground", action = "CampgroundDetail" } );

            //routeBuilder.MapRoute("CampgroundReviewsId", "camping/{SeName}/{campgroundSeName}/reviews/{campgroundId}",
            //    new { controller = "Campground", action = "CampgroundReviews" });

            routeBuilder.MapRoute("CampgroundReviews", "camping/{SeName}/{campgroundSeName}/reviews/{campgroundId?}",
                new { controller = "Campground", action = "CampgroundReviews" });


            ////Campground tags
            //routeBuilder.MapLocalizedRoute("CampgroundTagsAll", "camping/campgroundtag/all/",
            //    new { controller = "Campground", action = "CampgroundTagsAll" });

            ////Campground tags
            //routeBuilder.MapLocalizedRoute("CampgroundsByTag", "camping/campgroundtag/{campgroundTagId:min(0)}/{SeName?}",
            //    new { controller = "Campground", action = "CampgroundsByTag" });


            #region Admin Routes
            /***************** ADMIN ROUTING *****************/
            #region Campground
            routeBuilder.MapAreaRoute("AdminCampgroundList", "Admin",
                template: "Admin/{controller=CampgroundController}/{action=List}");

            routeBuilder.MapAreaRoute("AdminCampgroundEdit", "Admin",
                template: "Admin/{controller=CampgroundController}/{action=Edit}");

            routeBuilder.MapAreaRoute("AdminCampgroundDelete", "Admin",
                template: "Admin/{controller=CampgroundController}/{action=Delete}");

            routeBuilder.MapAreaRoute("AdminCampgroundCreate", "Admin",
                template: "Admin/{controller=CampgroundController}/{action=Create}");

            routeBuilder.MapAreaRoute("AdminCampgroundVerifyAddress", "Admin",
                template: "{controller=CampgroundRegister}/{action=VerifyAddress}");

            #endregion

            #region Campground Address
            routeBuilder.MapAreaRoute("AdminCampgroundAddressesSelect", "Admin",
                template: "Admin/{controller=CampgroundController}/{action=CampgroundAddressesSelect}/{campgroundId?}");

            routeBuilder.MapAreaRoute("AdminCampgroundAddressCreate", "Admin",
                template: "Admin/{controller=CampgroundController}/{action=CampgroundAddressCreate}/{campgroundId?}");

            routeBuilder.MapAreaRoute("AdminCampgroundAddressEdit", "Admin",
                template: "Admin/{controller=CampgroundController}/{action=CampgroundAddressEdit}/{campgroundId?}");

            routeBuilder.MapAreaRoute("Admin CampgroundAddressDelete", "Admin",
                template: "Admin/{controller=CampgroundController}/{action= CampgroundAddressDelete}/{campgroundId?}");
            #endregion

            #region Campground Attributes
            routeBuilder.MapAreaRoute("AdminCampgroundAttributeTypeList", "Admin",
                template: "Admin/{controller=CampgroundAttributeController}/{action=List}");

            routeBuilder.MapAreaRoute("AdminCampgroundAttributeTypeEdit", "Admin",
                template: "Admin/{controller=CampgroundAttributeController}/{action=Edit}");

            routeBuilder.MapAreaRoute("AdminCampgroundAttributeTypeCreate", "Admin",
                template: "Admin/{controller=CampgroundAttributeController}/{action=Create}");
            #endregion

            #region Campground Attribute Mapping
            routeBuilder.MapAreaRoute("AdminCampgroundAttributeMappingList", "Admin",
                template: "Admin/{controller=CampgroundController}/{action=CampgroundAttributeMappingList}/{campgroundId?}");

            routeBuilder.MapAreaRoute("AdminCampgroundAttributeMappingCreate", "Admin",
                template: "Admin/{controller=CampgroundController}/{action=CampgroundAttributeMappingCreate}/{campgroundId?}");

            routeBuilder.MapAreaRoute("AdminCampgroundAttributeMappingEdit", "Admin",
                template: "Admin/{controller=CampgroundController}/{action=CampgroundAttributeMappingEdit}/{campgroundId?}");

            routeBuilder.MapAreaRoute("AdminCampgroundAttributeMappingDelete", "Admin",
                template: "Admin/{controller=CampgroundController}/{action=CampgroundAttributeMappingDelete}/{campgroundId?}");
            #endregion

            #region Associated Campground
            routeBuilder.MapAreaRoute("AdminAssociatedCampgroundList", "Admin",
                template: "Admin/{controller=CampgroundController}/{action=AssociatedCampgroundList}");

            routeBuilder.MapAreaRoute("AdminAssociatedCampgroundUpdate", "Admin",
                template: "Admin/{controller=CampgroundController}/{action=AssociatedCampgroundUpdate}");

            routeBuilder.MapAreaRoute("AdminAssociatedCampgroundDelete", "Admin",
                template: "Admin/{controller=CampgroundController}/{action=AssociatedCampgroundDelete}");
            #endregion

            #region Related Campground
            routeBuilder.MapAreaRoute("AdminRelatedCampgroundList", "Admin",
                template: "Admin/{controller=CampgroundController}/{action=RelatedCampgroundList}/{campgroundId?}");

            routeBuilder.MapAreaRoute("AdminRelatedCampgroundUpdate", "Admin",
                template: "Admin/{controller=CampgroundController}/{action=RelatedCampgroundUpdate}/{campgroundId?}");

            routeBuilder.MapAreaRoute("AdminRelatedCampgroundDelete", "Admin",
                template: "Admin/{controller=CampgroundController}/{action=RelatedCampgroundDelete}/{campgroundId?}");
            #endregion

            #region Misc Popups
            routeBuilder.MapAreaRoute("AdminAssociatedCampgroundAddPopup", "Admin",
                template: "Admin/{controller=CampgroundController}/{action=AssociatedCampgroundAddPopup}/{campgroundId?}");

            routeBuilder.MapAreaRoute("AdminRelatedCampgroundAddPopup", "Admin",
                template: "Admin/{controller=CampgroundController}/{action=RelatedCampgroundAddPopup}/{campgroundId?}");

            routeBuilder.MapAreaRoute("AdminCrossSellCampgroundAddPopup", "Admin",
                template: "Admin/{controller=CampgroundController}/{action=CrossSellCampgroundAddPopup}/{campgroundId?}");
            #endregion

            #region Cross-Sell Campground
            routeBuilder.MapAreaRoute("AdminCrossSellCampgroundList", "Admin",
                template: "Admin/{controller=CampgroundController}/{action=CrossSellCampgroundList}/{campgroundId?}");

            routeBuilder.MapAreaRoute("AdminCrossSellCampgroundDelete", "Admin",
                template: "Admin/{controller=CampgroundController}/{action=CrossSellCampgroundDelete}/{campgroundId?}");
            #endregion
            /*************** END ADMIN ROUTING ***************/
            #endregion
        }

        /// <summary>
        /// Gets a priority of route provider
        /// </summary>
        public int Priority
        {
            get { return -1; }
        }
    }
}