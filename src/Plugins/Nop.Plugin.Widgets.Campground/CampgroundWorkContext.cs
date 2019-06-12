using Microsoft.AspNetCore.Http;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Tax;
using Nop.Plugin.Campgrounds.Data.Domain;
using Nop.Plugin.Campgrounds.Services;
using Nop.Services.Authentication;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Stores;
using Nop.Services.Vendors;
using Nop.Web.Framework;

namespace Nop.Plugin.Widgets.Campgrounds
{
    /// <summary>
    /// Represents work context for web application
    /// </summary>
    public partial class CampgroundWorkContext : WebWorkContext, ICampgroundWorkContext
    {
        #region Const
        
        private const string CUSTOMER_COOKIE_NAME = ".Nop.Customer";

        #endregion

        #region Fields

        private readonly ICampgroundHostService _campgroundHostService;

        private CampgroundHost _cachedCampgroundHost;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="campgroundHostService"></param>
        public CampgroundWorkContext(ICampgroundHostService campgroundHostService,
            IHttpContextAccessor httpContextAccessor,
            CurrencySettings currencySettings,
            IAuthenticationService authenticationService,
            ICurrencyService currencyService,
            ICustomerService customerService,
            IGenericAttributeService genericAttributeService,
            ILanguageService languageService,
            IStoreContext storeContext,
            IStoreMappingService storeMappingService,
            IUserAgentHelper userAgentHelper,
            IVendorService vendorService,
            LocalizationSettings localizationSettings,
            TaxSettings taxSettings) : base(httpContextAccessor,currencySettings,authenticationService,currencyService,customerService,genericAttributeService,languageService,storeContext,storeMappingService,userAgentHelper,vendorService,localizationSettings,taxSettings)
        {
            this._campgroundHostService = campgroundHostService;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the current vendor (logged-in manager)
        /// </summary>
        public virtual CampgroundHost CurrentCampgroundHost
        {
            get
            {
                //whether there is a cached value
                if (_cachedCampgroundHost != null)
                    return _cachedCampgroundHost;

                if (this.CurrentCustomer == null)
                    return null;

                //try to get campground host
                var campgroundHost = _campgroundHostService.GetCampgroundHostByCustomerId(this.CurrentCustomer.Id);

                //check vendor availability
                if (campgroundHost == null || campgroundHost.Deleted || !campgroundHost.Active)
                    return null;

                //cache the found campground host
                _cachedCampgroundHost = campgroundHost;

                return _cachedCampgroundHost;
            }
        }


        #endregion
    }
}
