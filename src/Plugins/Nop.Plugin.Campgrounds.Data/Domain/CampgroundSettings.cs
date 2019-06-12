using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Configuration;

namespace Nop.Plugin.Campgrounds.Data.Domain
{
    /// <summary>
    /// Campground settings
    /// </summary>
    public class CampgroundSettings : ISettings
    {
        public CampgroundSettings()
        {
            CampgroundSortingEnumDisabled = new List<int>();
            CampgroundSortingEnumDisplayOrder= new Dictionary<int, int>();
        }

        /// <summary>
        /// Gets or sets a value indicating details pages of unpublished Campground details pages could be open (for SEO optimization)
        /// </summary>
        public bool AllowViewUnpublishedCampgroundPage { get; set; }
        /// <summary>
        /// Gets or sets a value indicating customers should see "discontinued" message when visibting details pages of unpublished Campgrounds (if "AllowViewUnpublishedCampgroundPage" is "true)
        /// </summary>
        public bool DisplayDiscontinuedMessageForUnpublishedCampgrounds { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether "Published" or "Disable buy/wishlist buttons" flags should be updated after order cancellation (deletion).
        /// Of course, when qty > configured minimum stock level
        /// </summary>
        //public bool PublishBackCampgroundWhenCancellingOrders { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Campground sorting is enabled
        /// </summary>
        public bool AllowCampgroundSorting { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Campground State Province is enabled
        /// </summary>
        public bool StateProvinceEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Campground Company is enabled
        /// </summary>
        public bool CompanyEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Campground Street Address is enabled
        /// </summary>
        public bool StreetAddressEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Campground Street Address 2 is enabled
        /// </summary>
        public bool StreetAddress2Enabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Campground City is enabled
        /// </summary>
        public bool CityEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Campground Zip Postal Code is enabled
        /// </summary>
        public bool ZipPostalCodeEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Campground Country is enabled
        /// </summary>
        public bool CountryEnabled { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether customers can apply for campground Allow Customers To Apply For Campground accounts
        /// </summary>
        public bool AllowCustomersToApplyForCampgroundAccount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether customers are allowed to change Campground view mode
        /// </summary>
        public bool AllowCampgroundViewModeChanging { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether customers are allowed to change Campground view mode
        /// </summary>
        public string DefaultViewMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a category details page should include Campgrounds from subcategories
        /// </summary>
        public bool ShowCampgroundsFromSubcategories { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether number of Campgrounds should be displayed beside each category
        /// </summary>
        public bool ShowCategoryCampgroundNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether we include subcategories (when 'ShowCategoryCampgroundNumber' is 'true')
        /// </summary>
        public bool ShowCategoryCampgroundNumberIncludingSubcategories { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether category breadcrumb is enabled
        /// </summary>
        public bool CategoryBreadcrumbEnabled { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether a 'Share button' is enabled
        /// </summary>
        public bool ShowShareButton { get; set; }

        /// <summary>
        /// Gets or sets a share code (e.g. AddThis button code)
        /// </summary>
        public string PageShareCode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating Campground reviews must be approved
        /// </summary>
        public bool CampgroundReviewsMustBeApproved { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the default rating value of the Campground reviews
        /// </summary>
        public int DefaultCampgroundRatingValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow anonymous users write Campground reviews.
        /// </summary>
        public bool AllowAnonymousUsersToReviewCampground { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Campground can be reviewed only by customer who have already ordered it
        /// </summary>
        public bool CampgroundReviewPossibleOnlyAfterPurchasing { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether notification of a store owner about new Campground reviews is enabled
        /// </summary>
        public bool NotifyCampgroundHostAboutNewCampgroundReviews { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if store owners should get confirmation emails when adding campgrounds.
        /// </summary>
        public bool NotifyCampgroundHostAboutNewCampground { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Campground reviews will be filtered per store
        /// </summary>
        public bool ShowCampgroundReviewsPerStore { get; set; }

        /// <summary>
        /// Gets or sets a show Campground reviews tab on account page
        /// </summary>
        public bool ShowCampgroundReviewsTabOnAccountPage { get; set; }

        /// <summary>
        /// Gets or sets the page size for Campground reviews in account page
        /// </summary>
        public int CampgroundReviewsPageSizeOnAccountPage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Campground 'Email a friend' feature is enabled
        /// </summary>
        public bool EmailAFriendEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow anonymous users to email a friend.
        /// </summary>
        public bool AllowAnonymousUsersToEmailAFriend { get; set; }

        /// <summary>
        /// Gets or sets a number of "Recently viewed Campgrounds"
        /// </summary>
        public int RecentlyViewedCampgroundsNumber { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether "Recently viewed Campgrounds" feature is enabled
        /// </summary>
        public bool RecentlyViewedCampgroundsEnabled { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether "Recently viewed Campgrounds" feature is enabled
        /// </summary>
        public bool VerifyExistingCampgroundsEnabled { get; set; }
        /// <summary>
        /// Gets or sets a number of Campgrounds on the "New Campgrounds" page
        /// </summary>
        public int NewCampgroundsNumber { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether "New Campgrounds" page is enabled
        /// </summary>
        public bool NewCampgroundsEnabled { get; set; }

        /// <summary>
        /// Gets or sets value indicating if new campground additions are displayed on home page
        /// </summary>
        public bool ShowNewCampgroundsOnHomepage { get; set; }

        /// <summary>
        /// Gets or sets the number of new campground results to diplay on home page.
        /// </summary>
        public int NumberOfNewCampgroundsOnHomepage { get; set; }

        /// <summary>
        /// Gets or sets the number of new campground results to diplay on home page.
        /// </summary>
        public int NumberOfNewBlogPostsOnHomepage { get; set; }
        /// <summary>
        /// Gets or sets a value indicating if the search box is enabled on the home page
        /// </summary>
        public bool ShowSearchCampgroundsOnHomepage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether "Compare Campgrounds" feature is enabled
        /// </summary>
        public bool CompareCampgroundsEnabled { get; set; }
        /// <summary>
        /// Gets or sets an allowed number of Campgrounds to be compared
        /// </summary>
        public int CompareCampgroundsNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether autocomplete is enabled
        /// </summary>
        public bool CampgroundSearchAutoCompleteEnabled { get; set; }
        /// <summary>
        /// Gets or sets a number of Campgrounds to return when using "autocomplete" feature
        /// </summary>
        public int CampgroundSearchAutoCompleteNumberOfCampgrounds { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether to show Campground images in the auto complete search
        /// </summary>
        public bool ShowCampgroundImagesInSearchAutoComplete { get; set; }
        /// <summary>
        /// Gets or sets a minimum search term length
        /// </summary>
        public int CampgroundSearchTermMinimumLength { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether campground host has to accept terms of service during campground registration
        /// </summary>
        public bool TermsOfServiceEnabled { get; set; }

        public bool TermsOfServicePopup { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show bestsellers on home page
        /// </summary>
        public bool ShowBestsellersOnHomepage { get; set; }
        /// <summary>
        /// Gets or sets a number of bestsellers on home page
        /// </summary>
        public int NumberOfBestsellersOnHomepage { get; set; }

        /// <summary>
        /// Gets or sets a number of Campgrounds per page on the search Campgrounds page
        /// </summary>
        public int SearchPageCampgroundsPerPage { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether customers are allowed to select page size on the search Campgrounds page
        /// </summary>
        public bool SearchPageAllowCustomersToSelectPageSize { get; set; }
        /// <summary>
        /// Gets or sets the available customer selectable page size options on the search Campgrounds page
        /// </summary>
        public string SearchPagePageSizeOptions { get; set; }

        /// <summary>
        /// Gets or sets "List of Campgrounds purchased by other customers who purchased the above" option is enable
        /// </summary>
        public bool CampgroundsAlsoPurchasedEnabled { get; set; }

        /// <summary>
        /// Gets or sets a number of Campgrounds also purchased by other customers to display
        /// </summary>
        public int CampgroundsAlsoPurchasedNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether we should process attribute change using AJAX. It's used for dynamical attribute change, SKU/GTIN update of combinations, conditional attributes
        /// </summary>
        public bool AjaxProcessAttributeChange { get; set; }
        
        /// <summary>
        /// Gets or sets a number of Campground tags that appear in the tag cloud
        /// </summary>
        public int NumberOfCampgroundTags { get; set; }

        /// <summary>
        /// Gets or sets a number of Campgrounds per page on 'Campgrounds by tag' page
        /// </summary>
        public int CampgroundsByTagPageSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether customers can select the page size for 'Campgrounds by tag'
        /// </summary>
        public bool CampgroundsByTagAllowCustomersToSelectPageSize { get; set; }

        /// <summary>
        /// Gets or sets the available customer selectable page size options for 'Campgrounds by tag'
        /// </summary>
        public string CampgroundsByTagPageSizeOptions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include "Short description" in compare Campgrounds
        /// </summary>
        public bool IncludeShortDescriptionInCompareCampgrounds { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether to include "Full description" in compare Campgrounds
        /// </summary>
        public bool IncludeFullDescriptionInCompareCampgrounds { get; set; }
        /// <summary>
        /// An option indicating whether Campgrounds on category and manufacturer pages should include featured Campgrounds as well
        /// </summary>
        public bool IncludeFeaturedCampgroundsInNormalLists { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether tier prices should be displayed with applied discounts (if available)
        /// </summary>
        public bool DisplayTierPricesWithDiscounts { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether to ignore discounts (side-wide). It can significantly improve performance when enabled.
        /// </summary>
        public bool IgnoreDiscounts { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether to ignore featured Campgrounds (side-wide). It can significantly improve performance when enabled.
        /// </summary>
        public bool IgnoreFeaturedCampgrounds { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether to ignore ACL rules (side-wide). It can significantly improve performance when enabled.
        /// </summary>
        public bool IgnoreAcl { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether to ignore "limit per store" rules (side-wide). It can significantly improve performance when enabled.
        /// </summary>
        public bool IgnoreStoreLimitations { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether to cache Campground prices. It can significantly improve performance when enabled.
        /// </summary>
        public bool CacheCampgroundPrices { get; set; }

        /// <summary>
        /// Gets or sets the default value to use for Category page size options (for new categories)
        /// </summary>
        public string DefaultCampgroundPageSizeOptions { get; set; }
        /// <summary>
        /// Gets or sets the default value to use for Category page size (for new categories)
        /// </summary>
        public int DefaultCampgroundPageSize { get; set; }

        /// <summary>
        /// Gets or sets a list of disabled values of CampgroundSortingEnum
        /// </summary>
        public List<int> CampgroundSortingEnumDisabled { get; set; }

        /// <summary>
        /// Gets or sets a display order of CampgroundSortingEnum values 
        /// </summary>
        public Dictionary<int, int> CampgroundSortingEnumDisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Campgrounds need to be exported/imported with their attributes
        /// </summary>
        public bool ExportImportCampgroundAttributeTypes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether need create dropdown list for export
        /// </summary>
        public bool ExportImportUseDropdownlistsForAssociatedEntities { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the Google API key used for all map api calls
        /// </summary>
        public string GoogleMapsApi { get; set; }
    }
}