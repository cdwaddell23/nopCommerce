using System;
using System.Collections.Generic;
using Nop.Core.Domain.Discounts;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Security;
using Nop.Core.Domain.Seo;
using Nop.Core.Domain.Stores;
using Nop.Core;
using Nop.Core.Domain.Catalog;

namespace Nop.Plugin.Campgrounds.Data.Domain
{
    /// <summary>
    /// Represents a Campground
    /// </summary>
    public partial class Campground : BaseEntity, ILocalizedEntity, ISlugSupported, IAclSupported, IStoreMappingSupported
    {
        private ICollection<CampgroundCategory> _campgroundCategories;
        private ICollection<CampgroundPicture> _campgroundPictures;
        private ICollection<CampgroundReview> _campgroundReviews;
        private ICollection<CampgroundAddress> _campgroundAddresses;
        private ICollection<CampgroundSpecificationAttribute> _campgroundSpecificationAttributes;
        private ICollection<CampgroundTag> _campgroundTags;
        private ICollection<CampgroundAttributeMapping> _campgroundAttributeMappings;
        private ICollection<CampgroundAttributeCombination> _campgroundAttributeTypeCombinations;
        private ICollection<SeasonalPrice> _SeasonalPrices;
        private ICollection<Discount> _appliedDiscounts;
        private ICollection<CampgroundType> _campgroundType;
        private ICollection<CampgroundHost> _campgroundHost;
        //private ICollection<CampgroundSite> _campgroundSites;
        //private ICollection<CampgroundSiteBookings> _campgroundSiteBookings;


        #region Description and Notes
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the short description
        /// </summary>
        public string ShortDescription { get; set; }
        /// <summary>
        /// Gets or sets the full description
        /// </summary>
        public string FullDescription { get; set; }
        /// <summary>
        /// Gets or sets the full description
        /// </summary>
        public string Website { get; set; }
        public string FullUrl { get; set; }
        /// <summary>
        /// Gets or sets the admin comment
        /// </summary>
        public string AdminComment { get; set; }

        public int? CampgroundAddressId { get; set; }
        public int? BillingAddressId { get;set;}

        public decimal Distance { get; set; }

        #endregion

        #region SEO
        /// <summary>
        /// Gets or sets the meta keywords
        /// </summary>
        public string MetaKeywords { get; set; }
        /// <summary>
        /// Gets or sets the meta description
        /// </summary>
        public string MetaDescription { get; set; }
        /// <summary>
        /// Gets or sets the meta title
        /// </summary>
        public string MetaTitle { get; set; }
        #endregion

        #region Reviews
        /// <summary>
        /// Gets or sets a value indicating whether the Campground allows customer reviews
        /// </summary>
        public bool AllowCampgroundReviews { get; set; }
        /// <summary>
        /// Gets or sets the rating sum (approved reviews)
        /// </summary>
        public int ApprovedRatingSum { get; set; }
        /// <summary>
        /// Gets or sets the rating sum (not approved reviews)
        /// </summary>
        public int NotApprovedRatingSum { get; set; }
        /// <summary>
        /// Gets or sets the total rating votes (approved reviews)
        /// </summary>
        public int ApprovedTotalReviews { get; set; }
        /// <summary>
        /// Gets or sets the total rating votes (not approved reviews)
        /// </summary>
        public int NotApprovedTotalReviews { get; set; }
        #endregion

        #region Campground Agreements

        /// <summary>
        /// Gets or sets the has user agreement
        /// </summary>
        public bool HasUserAgreement { get; set; }
        /// <summary>
        /// Gets or sets the text of license agreement
        /// </summary>
        public string UserAgreementText { get; set; }
        #endregion

        #region Featured Available
        /// <summary>
        /// Gets or sets a value indicating whether the Campground is rental
        /// </summary>
        public bool IsFeatured { get; set; }
        /// <summary>
        /// Gets or sets the rental length for some period (price for this period)
        /// </summary>
        public DateTime? FeaturedStartDate { get; set; }
        /// <summary>
        /// Gets or sets the rental period (price for this period)
        /// </summary>
        public DateTime? FeaturedEndDate { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the Campground is marked as tax exempt
        /// </summary>
        public Decimal FeaturedPrice { get; set; }
        #endregion

        #region Rentals Available
        /// <summary>
        /// Gets or sets a value indicating whether the Campground is rental
        /// </summary>
        public bool IsRental { get; set; }
        /// <summary>
        /// Gets or sets the rental length for some period (price for this period)
        /// </summary>
        public int RentalPriceLength { get; set; }
        /// <summary>
        /// Gets or sets the rental period (price for this period)
        /// </summary>
        public int RentalPricePeriodId { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the Campground is marked as tax exempt
        /// </summary>
        #endregion

        #region Campground Inventory

        /// <summary>
        /// Gets or sets a value indicating whether this item is available for registration
        /// </summary>
        public bool RegistrationsEnabled { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this item is available for Pre-Order
        /// </summary>
        public int AvailableCampsites { get; set; }
        /// <summary>
        /// Gets or sets a value backorder mode identifier
        /// </summary>
        public int BackorderModeId { get; set; }
        #endregion

        #region Buy and Wishlist Buttons
        /// <summary>
        /// Gets or sets a value indicating whether to disable buy (Add to cart) button
        /// </summary>
        public bool DisableBuyButton { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether to disable "Add to wishlist" button
        /// </summary>
        public bool DisableWishlistButton { get; set; }
        #endregion
        
        #region Pricing, Dates and Quantities
        /// <summary>
        /// Gets or sets a value indicating whether to show "Call for Pricing" or "Call for quote" instead of price
        /// </summary>
        public bool CallForPrice { get; set; }
        /// <summary>
        /// Gets or sets the price
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Gets or sets the old price
        /// </summary>
        public decimal OldPrice { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether base price (PAngV) is enabled. Used by German users.
        /// </summary>
        public bool BasepriceEnabled { get; set; }
        /// <summary>
        /// Gets or sets an amount in Campground for PAngV
        /// </summary>
        public decimal BasepriceAmount { get; set; }
        /// <summary>
        /// Gets or sets a unit of Campground for PAngV (MeasureWeight entity)
        /// </summary>
        public int BasepriceUnitId { get; set; }
        /// <summary>
        /// Gets or sets a reference amount for PAngV
        /// </summary>
        public decimal BasepriceBaseAmount { get; set; }
        /// <summary>
        /// Gets or sets a reference unit for PAngV (MeasureWeight entity)
        /// </summary>
        public int BasepriceBaseUnitId { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this Campground has Seasonal prices configured
        /// <remarks>The same as if we run this.SeasonalPrices.Count > 0
        /// We use this property for performance optimization:
        /// if this property is set to false, then we do not need to load Seasonal prices navigation property
        /// </remarks>
        /// </summary>
        public bool HasSeasonalPrices { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this Campground has discounts applied
        /// <remarks>The same as if we run this.AppliedDiscounts.Count > 0
        /// We use this property for performance optimization:
        /// if this property is set to false, then we do not need to load Applied Discounts navigation property
        /// </remarks>
        /// </summary>
        public bool HasDiscountsApplied { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether we allow adding to the cart/wishlist only attribute combinations that exist and have stock greater than zero.
        /// This option is used only when we have "manage inventory" set to "track inventory by Campground attributes"
        /// </summary>
        public bool AllowAddingOnlyExistingAttributeCombinations { get; set; }
        /// <summary>
        /// Gets or sets the available start date and time
        /// </summary>
        public DateTime? AvailableStartDateTimeUtc { get; set; }
        /// <summary>
        /// Gets or sets the available end date and time
        /// </summary>
        public DateTime? AvailableEndDateTimeUtc { get; set; }
        #endregion

        #region Miscelaneous
        /// <summary>
        /// Gets or sets the Campground code (sku)
        /// </summary>
        public string CampgroundCode { get; set; }
        /// <summary>
        /// Gets or sets the Campground class identifier
        /// </summary>
        public int CampgroundClassId { get; set; }
        /// <summary>
        /// Gets or sets the parent Campground identifier. It's used to identify associated Campgrounds (only with "grouped" Campgrounds)
        /// </summary>
        public int ParentGroupedCampgroundId { get; set; }
        /// <summary>
        /// Gets or sets the values indicating whether this Campground is visible in catalog or search results.
        /// It's used when this Campground is associated to some "grouped" one
        /// This way associated Campgrounds could be accessed/added/etc only from a grouped Campground details page
        /// </summary>
        public bool VisibleIndividually { get; set; }
        /// <summary>
        /// Gets or sets a value of used Campground template identifier
        /// </summary>
        public int CampgroundTemplateId { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether to show the campground on home page
        /// </summary>
        public bool ShowOnHomePage { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this Campground is marked as new
        /// </summary>
        public bool MarkAsNew { get; set; }
        /// <summary>
        /// Gets or sets the start date and time of the new Campground (set Campground as "New" from date). Leave empty to ignore this property
        /// </summary>
        public DateTime? MarkAsNewStartDateTimeUtc { get; set; }
        /// <summary>
        /// Gets or sets the end date and time of the new Campground (set Campground as "New" to date). Leave empty to ignore this property
        /// </summary>
        public DateTime? MarkAsNewEndDateTimeUtc { get; set; }
        /// <summary>
        /// Gets or sets a display order.
        /// This value is used when sorting associated Campgrounds (used with "grouped" Campgrounds)
        /// This value is used when sorting home page Campgrounds
        /// </summary>
        public int DisplayOrder { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the entity is published
        /// </summary>
        public bool Published { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the entity has been deleted
        /// </summary>
        public bool Deleted { get; set; }
        /// <summary>
        /// Gets or sets a ACL identifier
        /// </summary>
        public bool SubjectToAcl { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the entity is limited/restricted to certain stores
        /// </summary>
        public bool LimitedToStores { get; set; }
        #endregion

        #region Audit Fields
        /// <summary>
        /// Gets or sets the date and time of Campground creation
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }
        /// <summary>
        /// Gets or sets the date and time of Campground update
        /// </summary>
        public DateTime? UpdatedOnUtc { get; set; }
        #endregion

        #region Paging
        /// <summary>
        /// Gets or sets the page size
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether customers can select the page size
        /// </summary>
        public bool AllowCustomersToSelectPageSize { get; set; }

        /// <summary>
        /// Gets or sets the available customer selectable page size options
        /// </summary>
        public string PageSizeOptions { get; set; }


        #endregion

        /// <summary>
        /// Gets or sets the Campground class (State, Federal, Private)
        /// </summary>
        public CampgroundClass CampgroundClass
        {
            get
            {
                return (CampgroundClass)this.CampgroundClassId;
            }
            set
            {
                this.CampgroundClassId = (int)value;
            }
        }


        /// <summary>
        /// Gets or sets the billing address
        /// </summary>
        public virtual CampgroundAddress BillingAddress { get; set; }

        /// <summary>
        /// Gets or sets the campground address
        /// </summary>
        public virtual CampgroundAddress CampgroundAddress { get; set; }

        /// <summary>
        /// Gets or sets customer addresses
        /// </summary>
        public virtual ICollection<CampgroundAddress> Addresses
        {
            get { return _campgroundAddresses ?? (_campgroundAddresses = new List<CampgroundAddress>()); }
            protected set { _campgroundAddresses = value; }
        }

        ///// <summary>
        ///// Gets or sets the collection of CampgroundCategory
        ///// </summary>
        //public virtual ICollection<CampgroundSite> CampgroundSites
        //{
        //    get { return _campgroundSites ?? (_campgroundSites = new List<CampgroundSite>()); }
        //    protected set { _campgroundSites = value; }
        //}

        ///// <summary>
        ///// Gets or sets the collection of booked sites
        ///// </summary>
        //public virtual ICollection<CampgroundSiteBookings> SiteBookings
        //{
        //    get { return _campgroundSiteBookings ?? (_campgroundSiteBookings = new List<CampgroundSiteBookings>()); }
        //    protected set { _campgroundSiteBookings = value; }
        //}

        /// <summary>
        /// Gets or sets the collection of CampgroundCategory
        /// </summary>
        public virtual ICollection<CampgroundCategory> CampgroundCategories
        {
            get { return _campgroundCategories ?? (_campgroundCategories = new List<CampgroundCategory>()); }
            protected set { _campgroundCategories = value; }
        }

        /// <summary>
        /// Gets or sets the collection of CampgroundPicture
        /// </summary>
        public virtual ICollection<CampgroundPicture> CampgroundPictures
        {
            get { return _campgroundPictures ?? (_campgroundPictures = new List<CampgroundPicture>()); }
            protected set { _campgroundPictures = value; }
        }

        /// <summary>
        /// Gets or sets the collection of Campground reviews
        /// </summary>
        public virtual ICollection<CampgroundReview> CampgroundReviews
        {
            get { return _campgroundReviews ?? (_campgroundReviews = new List<CampgroundReview>()); }
            protected set { _campgroundReviews = value; }
        }

        /// <summary>
        /// Gets or sets the collection of Campground types
        /// </summary>
        public virtual ICollection<CampgroundType> CampgroundType
        {
            get { return _campgroundType ?? (_campgroundType = new List<CampgroundType>()); }
            protected set { _campgroundType = value; }
        }

        /// <summary>
        /// Gets or sets the collection of Campground types
        /// </summary>
        public virtual ICollection<CampgroundHost> CampgroundHost
        {
            get { return _campgroundHost ?? (_campgroundHost = new List<CampgroundHost>()); }
            protected set { _campgroundHost = value; }
        }

        /// <summary>
        /// Gets or sets the Campground specification attribute
        /// </summary>
        public virtual ICollection<CampgroundSpecificationAttribute> CampgroundSpecificationAttributes
        {
            get { return _campgroundSpecificationAttributes ?? (_campgroundSpecificationAttributes = new List<CampgroundSpecificationAttribute>()); }
            protected set { _campgroundSpecificationAttributes = value; }
        }

        /// <summary>
        /// Gets or sets the Campground tags
        /// </summary>
        public virtual ICollection<CampgroundTag> CampgroundTags
        {
            get { return _campgroundTags ?? (_campgroundTags = new List<CampgroundTag>()); }
            protected set { _campgroundTags = value; }
        }

        /// <summary>
        /// Gets or sets the Campground attribute mappings
        /// </summary>
        public virtual ICollection<CampgroundAttributeMapping> CampgroundAttributeMappings
        {
            get { return _campgroundAttributeMappings ?? (_campgroundAttributeMappings = new List<CampgroundAttributeMapping>()); }
            protected set { _campgroundAttributeMappings = value; }
        }

        /// <summary>
        /// Gets or sets the Campground attribute combinations
        /// </summary>
        public virtual ICollection<CampgroundAttributeCombination> CampgroundAttributeTypeCombinations
        {
            get { return _campgroundAttributeTypeCombinations ?? (_campgroundAttributeTypeCombinations = new List<CampgroundAttributeCombination>()); }
            protected set { _campgroundAttributeTypeCombinations = value; }
        }

        /// <summary>
        /// Gets or sets the Seasonal prices
        /// </summary>
        public virtual ICollection<SeasonalPrice> SeasonalPrices
        {
            get { return _SeasonalPrices ?? (_SeasonalPrices = new List<SeasonalPrice>()); }
            protected set { _SeasonalPrices = value; }
        }

        /// <summary>
        /// Gets or sets the collection of applied discounts
        /// </summary>
        public virtual ICollection<Discount> AppliedDiscounts
        {
            get { return _appliedDiscounts ?? (_appliedDiscounts = new List<Discount>()); }
            protected set { _appliedDiscounts = value; }
        }
    }
}