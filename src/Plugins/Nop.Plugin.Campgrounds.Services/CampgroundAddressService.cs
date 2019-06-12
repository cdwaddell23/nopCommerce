using System;
using System.Linq;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Domain.Common;
using Nop.Plugin.Campgrounds.Data.Domain;
using Nop.Services.Common;
using Nop.Services.Directory;
using Nop.Services.Events;

namespace Nop.Plugin.Campgrounds.Services
{
    /// <summary>
    /// CampgroundAddress service
    /// </summary>
    public partial class CampgroundAddressService : ICampgroundAddressService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : address ID
        /// </remarks>
        private const string ADDRESSES_BY_ID_KEY = "Nop.campgroundAddress.id-{0}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string ADDRESSES_PATTERN_KEY = "Nop.campgroundAddress.";

        #endregion

        #region Fields

        private readonly IRepository<CampgroundAddress> _campgroundAddressRepository;
        private readonly ICountryService _countryService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly IAddressAttributeService _addressAttributeService;
        private readonly IEventPublisher _eventPublisher;
        private readonly AddressSettings _addressSettings;
        private readonly ICacheManager _cacheManager;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="campgroundAddressRepository">CampgroundAddress repository</param>
        /// <param name="countryService">Country service</param>
        /// <param name="stateProvinceService">State/province service</param>
        /// <param name="addressAttributeService">CampgroundAddress attribute service</param>
        /// <param name="eventPublisher">Event publisher</param>
        /// <param name="addressSettings">CampgroundAddress settings</param>
        public CampgroundAddressService(ICacheManager cacheManager,
            IRepository<CampgroundAddress> campgroundAddressRepository,
            ICountryService countryService, 
            IStateProvinceService stateProvinceService,
            IAddressAttributeService addressAttributeService,
            IEventPublisher eventPublisher, 
            AddressSettings addressSettings)
        {
            this._cacheManager = cacheManager;
            this._campgroundAddressRepository = campgroundAddressRepository;
            this._countryService = countryService;
            this._stateProvinceService = stateProvinceService;
            this._addressAttributeService = addressAttributeService;
            this._eventPublisher = eventPublisher;
            this._addressSettings = addressSettings;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes an campgroundAddress
        /// </summary>
        /// <param name="campgroundAddress">CampgroundAddress</param>
        public virtual void DeleteCampgroundAddress(CampgroundAddress campgroundAddress)
        {
            if (campgroundAddress == null)
                throw new ArgumentNullException(nameof(campgroundAddress));

            _campgroundAddressRepository.Delete(campgroundAddress);

            //cache
            _cacheManager.RemoveByPattern(ADDRESSES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(campgroundAddress);
        }

        /// <summary>
        /// Gets total number of campgroundAddresses by country identifier
        /// </summary>
        /// <param name="countryId">Country identifier</param>
        /// <returns>Number of campgroundAddresses</returns>
        public virtual int GetCampgroundAddressTotalByCountryId(int countryId)
        {
            if (countryId == 0)
                return 0;

            var query = from a in _campgroundAddressRepository.Table
                        where a.CountryId == countryId
                        select a;
            return query.Count();
        }

        /// <summary>
        /// Gets total number of campgroundAddresses by state/province identifier
        /// </summary>
        /// <param name="stateProvinceId">State/province identifier</param>
        /// <returns>Number of campgroundAddresses</returns>
        public virtual int GetCampgroundAddressTotalByStateProvinceId(int stateProvinceId)
        {
            if (stateProvinceId == 0)
                return 0;

            var query = from a in _campgroundAddressRepository.Table
                        where a.StateProvinceId == stateProvinceId
                        select a;
            return query.Count();
        }

        /// <summary>
        /// Gets an campgroundAddress by campgroundAddress identifier
        /// </summary>
        /// <param name="campgroundAddressId">CampgroundAddress identifier</param>
        /// <returns>CampgroundAddress</returns>
        public virtual CampgroundAddress GetCampgroundAddressById(int campgroundAddressId)
        {
            if (campgroundAddressId == 0)
                return null;

            var key = string.Format(ADDRESSES_BY_ID_KEY, campgroundAddressId);
            return _cacheManager.Get(key, () => _campgroundAddressRepository.GetById(campgroundAddressId));
        }

        /// <summary>
        /// Inserts an campgroundAddress
        /// </summary>
        /// <param name="campgroundAddress">CampgroundAddress</param>
        public virtual void InsertCampgroundAddress(CampgroundAddress campgroundAddress)
        {
            if (campgroundAddress == null)
                throw new ArgumentNullException(nameof(campgroundAddress));
            
            campgroundAddress.CreatedOnUtc = DateTime.UtcNow;

            //some validation
            if (campgroundAddress.CountryId == 0)
                campgroundAddress.CountryId = null;
            if (campgroundAddress.StateProvinceId == 0)
                campgroundAddress.StateProvinceId = null;

            _campgroundAddressRepository.Insert(campgroundAddress);

            //cache
            _cacheManager.RemoveByPattern(ADDRESSES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(campgroundAddress);
        }

        /// <summary>
        /// Updates the campgroundAddress
        /// </summary>
        /// <param name="campgroundAddress">CampgroundAddress</param>
        public virtual void UpdateCampgroundAddress(CampgroundAddress campgroundAddress)
        {
            if (campgroundAddress == null)
                throw new ArgumentNullException(nameof(campgroundAddress));

            //some validation
            if (campgroundAddress.CountryId == 0)
                campgroundAddress.CountryId = null;
            if (campgroundAddress.StateProvinceId == 0)
                campgroundAddress.StateProvinceId = null;

            _campgroundAddressRepository.Update(campgroundAddress);

            //cache
            _cacheManager.RemoveByPattern(ADDRESSES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(campgroundAddress);
        }
        
        /// <summary>
        /// Gets a value indicating whether campgroundAddress is valid (can be saved)
        /// </summary>
        /// <param name="campgroundAddress">CampgroundAddress to validate</param>
        /// <returns>Result</returns>
        public virtual bool IsCampgroundAddressValid(CampgroundAddress campgroundAddress)
        {
            if (campgroundAddress == null)
                throw new ArgumentNullException(nameof(campgroundAddress));

            if (string.IsNullOrWhiteSpace(campgroundAddress.FirstName))
                return false;

            if (string.IsNullOrWhiteSpace(campgroundAddress.LastName))
                return false;

            if (string.IsNullOrWhiteSpace(campgroundAddress.Email))
                return false;

            if (_addressSettings.CompanyEnabled &&
                _addressSettings.CompanyRequired &&
                string.IsNullOrWhiteSpace(campgroundAddress.Company))
                return false;

            if (_addressSettings.StreetAddressEnabled &&
                _addressSettings.StreetAddressRequired &&
                string.IsNullOrWhiteSpace(campgroundAddress.Address1))
                return false;

            if (_addressSettings.StreetAddress2Enabled &&
                _addressSettings.StreetAddress2Required &&
                string.IsNullOrWhiteSpace(campgroundAddress.Address2))
                return false;

            if (_addressSettings.ZipPostalCodeEnabled &&
                _addressSettings.ZipPostalCodeRequired &&
                string.IsNullOrWhiteSpace(campgroundAddress.ZipPostalCode))
                return false;

            if (_addressSettings.CountryEnabled)
            {
                if (campgroundAddress.CountryId == null || campgroundAddress.CountryId.Value == 0)
                    return false;

                var country = _countryService.GetCountryById(campgroundAddress.CountryId.Value);
                if (country == null)
                    return false;

                if (_addressSettings.StateProvinceEnabled)
                {
                    var states = _stateProvinceService.GetStateProvincesByCountryId(country.Id);
                    if (states.Any())
                    {
                        if (campgroundAddress.StateProvinceId == null || campgroundAddress.StateProvinceId.Value == 0)
                            return false;

                        var state = states.FirstOrDefault(x => x.Id == campgroundAddress.StateProvinceId.Value);
                        if (state == null)
                            return false;
                    }
                }
            }

            if (_addressSettings.CityEnabled &&
                _addressSettings.CityRequired &&
                string.IsNullOrWhiteSpace(campgroundAddress.City))
                return false;

            if (_addressSettings.PhoneEnabled &&
                _addressSettings.PhoneRequired &&
                string.IsNullOrWhiteSpace(campgroundAddress.PhoneNumber))
                return false;

            if (_addressSettings.FaxEnabled &&
                _addressSettings.FaxRequired &&
                string.IsNullOrWhiteSpace(campgroundAddress.FaxNumber))
                return false;

            var attributes = _addressAttributeService.GetAllAddressAttributes();
            if (attributes.Any(x => x.IsRequired))
                return false;

            return true;
        }
        
        #endregion
    }
}