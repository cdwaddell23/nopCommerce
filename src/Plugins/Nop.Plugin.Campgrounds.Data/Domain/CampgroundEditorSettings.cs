
using Nop.Core;
using Nop.Core.Configuration;

namespace Nop.Plugin.Campgrounds.Data.Domain
{
    public class CampgroundEditorSettings : BaseEntity, ISettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether 'Campground type' field is shown
        /// </summary>
        public bool CampgroundClass { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Campground class' field is shown
        /// </summary>
        public bool VisibleCampgroundClass { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Visible individually' field is shown
        /// </summary>
        public bool VisibleIndividually { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Campground template' field is shown
        /// </summary>
        public bool CampgroundTemplate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Admin comment' feild is shown
        /// </summary>
        public bool AdminComment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Vendor' field is shown
        /// </summary>
        public bool Vendor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Stores' field is shown
        /// </summary>
        public bool Stores { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'ACL' field is shown
        /// </summary>
        public bool ACL { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Show on home page' field is shown
        /// </summary>
        public bool ShowOnHomePage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Display order 'field is shown
        /// </summary>
        public bool DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Allow campground reviews' field is shown
        /// </summary>
        public bool AllowCampgroundReviews { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Campground tags' field is shown
        /// </summary>
        public bool CampgroundTags { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Campground cost' field is shown
        /// </summary>
        public bool CampgroundCost { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Tier prices' field is shown
        /// </summary>
        public bool SeasonalPrices { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Discounts' field is shown
        /// </summary>
        public bool Discounts { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Disable buy button' field is shown
        /// </summary>
        public bool DisableBuyButton { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Disable wishlist button' field is shown
        /// </summary>
        public bool DisableWishlistButton { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Available for pre-order' field is shown
        /// </summary>
        public bool AvailableForPreOrder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Call for price' field is shown
        /// </summary>
        public bool CallForPrice { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Old price' field is shown
        /// </summary>
        public bool OldPrice { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Customer enters price' field is shown
        /// </summary>
        public bool CustomerEntersPrice { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'PAngV' field is shown
        /// </summary>
        public bool PAngV { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Require other campgrounds added to the cart' field is shown
        /// </summary>
        public bool RequireOtherCampgroundsAddedToTheCart { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Is gift card' field is shown
        /// </summary>
        public bool IsGiftCard { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Recurring campground' field is shown
        /// </summary>
        public bool RecurringCampground { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Is rental' field is shown
        /// </summary>
        public bool IsRental { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Campground availability range' field is shown
        /// </summary>
        public bool CampgroundAvailabilityRange { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Display stock availability' field is shown
        /// </summary>
        public bool DisplayStockAvailability { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Display stock quantity' field is shown
        /// </summary>
        public bool DisplayStockQuantity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Minimum stock quantity' field is shown
        /// </summary>
        public bool MinimumStockQuantity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Low stock activity' field is shown
        /// </summary>
        public bool LowStockActivity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Notify admin for quantity below' field is shown
        /// </summary>
        public bool NotifyAdminForQuantityBelow { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Backorders' field is shown
        /// </summary>
        public bool Backorders { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Allow back in stock subscriptions' field is shown
        /// </summary>
        public bool AllowBackInStockSubscriptions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Minimum cart quantity' field is shown
        /// </summary>
        public bool MinimumCartQuantity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Maximum cart quantity' field is shown
        /// </summary>
        public bool MaximumCartQuantity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Allowed quantities' field is shown
        /// </summary>
        public bool AllowedQuantities { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Allow only existing attribute combinations' field is shown
        /// </summary>
        public bool AllowAddingOnlyExistingAttributeCombinations { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Not returnable' field is shown
        /// </summary>
        public bool NotReturnable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Available start date' field is shown
        /// </summary>
        public bool AvailableStartDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Available end date' field is shown
        /// </summary>
        public bool AvailableEndDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Mark as new' field is shown
        /// </summary>
        public bool MarkAsNew { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Mark as new. Start date' field is shown
        /// </summary>
        public bool MarkAsNewStartDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Mark as new. End date' field is shown
        /// </summary>
        public bool MarkAsNewEndDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Published' field is shown
        /// </summary>
        public bool Published { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Created on' field is shown
        /// </summary>
        public bool CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Updated on' field is shown
        /// </summary>
        public bool UpdatedOn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Related campgrounds' block is shown
        /// </summary>
        public bool RelatedCampgrounds { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Cross-sells campgrounds' block is shown
        /// </summary>
        public bool CrossSellsCampgrounds { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'SEO' tab is shown
        /// </summary>
        public bool Seo { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Purchased with orders' tab is shown
        /// </summary>
        public bool PurchasedWithOrders { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether one column is used on the campground details page
        /// </summary>
        public bool OneColumnCampgroundPage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Campground attributes' tab is shown
        /// </summary>
        public bool CampgroundAttributeTypes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Specification attributes' tab is shown
        /// </summary>
        public bool SpecificationAttributes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Stock quantity history' tab is shown
        /// </summary>
        public bool StockQuantityHistory { get; set; }
    }
}