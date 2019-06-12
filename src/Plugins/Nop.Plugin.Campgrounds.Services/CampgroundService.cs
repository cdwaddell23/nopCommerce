using System;
using System.Collections.Generic;
using System.Linq;
//using Nop.Data;
using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Caching;
using Nop.Core.Domain.Security;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Localization;
using Nop.Core.Extensions;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Events;
using Nop.Services.Security;
using Nop.Services.Customers;
using Nop.Services.Logging;
using Nop.Plugin.Campgrounds.Data.Domain;
using Nop.Plugin.Campgrounds.Data;
using System.Net;
using System.Data.Entity.SqlServer;
using Nop.Services.Media;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Stores;
using Nop.Services.Stores;
using Nop.Services.Helpers;
using Nop.Services.Directory;

namespace Nop.Plugin.Campgrounds.Services
{
    /// <summary>
    /// Campground service
    /// </summary>
    public partial class CampgroundService : ICampgroundService
    {
        #region Constants
        /// <summary>
        /// Key for caching
        private const string CAMPGROUNDS_BY_ID_KEY = "Nop.Campground.id-{0}";
        private const string CAMPGROUNDS_BY_CATEGORYID_KEY = "Nop.Campground.Categoryid-{0}";
        private const string CAMPGROUNDSTATEID_BY_CATEGORYID_KEY = "Nop.CampgroundStateId.Categoryid-{0}";
        private const string CAMPGROUNDS_PATTERN_KEY = "Nop.Campground.";
        private const string CAMPGROUNDHOST_PATTERN_KEY = "Nop.CampgroundHost.";
        private const string CATEGORIES_PATTERN_KEY = "Nop.campground.category.";
        private const string CAMPGROUNDCATEGORIES_PATTERN_KEY = "Nop.campgroundcategory.";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// {1} : category ID
        /// {2} : page index
        /// {3} : page size
        /// {4} : current customer ID
        /// {5} : store ID
        /// </remarks>
        private const string CAMPGROUNDCATEGORIES_ALLBYCATEGORYID_KEY = "Nop.campgroundcategory.allbycategoryid-{0}-{1}-{2}-{3}-{4}-{5}";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// {1} : campground ID
        /// {2} : current customer ID
        /// {3} : store ID
        /// </remarks>
        private const string CAMPGROUNDCATEGORIES_ALLBYCAMPGROUNDID_KEY = "Nop.campgroundcategory.allbycampgroundid-{0}-{1}-{2}-{3}";
        #endregion

        #region Fields

        private readonly ICacheManager _cacheManager;
        private readonly IRepository<Campground> _campgroundRepository;
        private readonly IRepository<CampgroundAddress> _campgroundAddressRepository;
        private readonly IRepository<CampgroundPicture> _campgroundPictureRepository;
        private readonly IRepository<CampgroundCategory> _campgroundCategoryRepository;
        private readonly IRepository<CampgroundReview> _campgroundReviewRepository;
        private readonly IRepository<CampgroundHost> _campgroundHostRepository;
        private readonly IRepository<RelatedCampground> _relatedCampgroundRepository;
        private readonly IRepository<CrossSellCampground> _crossSellCampgroundRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<StoreMapping> _storeMappingRepository;
        private readonly IRepository<AclRecord> _aclRepository;
        private readonly IStoreContext _storeContext;
        private readonly IStoreMappingService _storeMappingService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly IPictureService _pictureService;
        private readonly ILanguageService _languageService;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly IDataProvider _dataProvider;
        private readonly CampgroundsObjectContext _dbContext;
        private readonly IWorkContext _workContext;
        private readonly LocalizationSettings _localizationSettings;
        private readonly CommonSettings _commonSettings;
        private readonly CampgroundSettings _campgroundSettings;
        private readonly CatalogSettings _catalogSettings;
        private readonly IEventPublisher _eventPublisher;
        private readonly IAclService _aclService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly ILogger _logger;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="CampgroundRepository">Campground repository</param>
        /// <param name="CampgroundPictureRepository">Campground picture repository</param>
        /// <param name="languageService">Language service</param>
        /// <param name="workflowMessageService">Workflow message service</param>
        /// <param name="dataProvider">Data provider</param>
        /// <param name="dbContext">Database Context</param>
        /// <param name="workContext">Work context</param>
        /// <param name="localizationSettings">Localization settings</param>
        /// <param name="commonSettings">Common settings</param>
        /// <param name="CampgroundSettings">Campgrounds settings</param>
        /// <param name="eventPublisher">Event published</param>
        /// <param name="aclService">ACL service</param>
        public CampgroundService(ICacheManager cacheManager,
            IRepository<Campground> campgroundRepository,
            IRepository<CampgroundAddress> campgroundAddressRepository,
            IRepository<CampgroundCategory> campgroundCategoryRepository,
            IRepository<CampgroundPicture> campgroundPictureRepository,
            IRepository<CampgroundReview> campgroundReviewRepository,
            IRepository<CampgroundHost> campgroundHostRepository,
            IRepository<RelatedCampground> relatedCampgroundRepository,
            IRepository<CrossSellCampground> crossSellCampgroundRepository,
            IRepository<Category> categoryRepository,
            IRepository<StoreMapping> storeMappingRepository,
            IStoreContext storeContext,
            IStoreMappingService storeMappingService,
            IStateProvinceService stateProvinceService,
            IRepository<AclRecord> aclRepository,
            IPictureService pictureService,
            ILanguageService languageService,
            IWorkflowMessageService workflowMessageService,
            IDataProvider dataProvider,
            CampgroundsObjectContext dbContext,
            IWorkContext workContext,
            LocalizationSettings localizationSettings,
            CommonSettings commonSettings,
            CampgroundSettings CampgroundSettings,
            CatalogSettings catalogSettings,
            IEventPublisher eventPublisher,
            IAclService aclService,
            IDateTimeHelper dateTimeHelper,
            ILogger logger)
        {
            this._cacheManager = cacheManager;
            this._campgroundRepository = campgroundRepository;
            this._campgroundAddressRepository = campgroundAddressRepository;
            this._campgroundCategoryRepository = campgroundCategoryRepository;
            this._campgroundPictureRepository = campgroundPictureRepository;
            this._campgroundReviewRepository = campgroundReviewRepository;
            this._campgroundHostRepository = campgroundHostRepository;
            this._relatedCampgroundRepository = relatedCampgroundRepository;
            this._crossSellCampgroundRepository = crossSellCampgroundRepository;
            this._categoryRepository = categoryRepository;
            this._storeMappingRepository = storeMappingRepository;
            this._storeContext = storeContext;
            this._storeMappingService = storeMappingService;
            this._stateProvinceService = stateProvinceService;
            this._aclRepository = aclRepository;
            this._pictureService = pictureService;
            this._languageService = languageService;
            this._workflowMessageService = workflowMessageService;
            this._dataProvider = dataProvider;
            this._dbContext = dbContext;
            this._workContext = workContext;
            this._localizationSettings = localizationSettings;
            this._commonSettings = commonSettings;
            this._campgroundSettings = CampgroundSettings;
            this._catalogSettings = catalogSettings;
            this._eventPublisher = eventPublisher;
            this._aclService = aclService;
            this._dateTimeHelper = dateTimeHelper;
            this._logger = logger;
        }

        #endregion

        #region Methods

        #region Campground Utilities

        public Location GetCampgroundLatLong(CampgroundAddress campgroundAddress)
        {
            var googleAPI = campgroundAddress.Address1?.Length > 0 ? string.Format("https://maps.googleapis.com/maps/api/geocode/json?address={0}&key=AIzaSyBatV_B_a-477kd8IN_wmvDEbJdIF9LQ8c", campgroundAddress.Address1?.Trim().Replace(" ", "+") + ",+" + campgroundAddress.City?.Trim().Replace(" ", "+") + ",+" + campgroundAddress.StateProvince?.Name.Trim().Replace(" ", "+") + ",+" + campgroundAddress.ZipPostalCode?.Trim().Replace(" ", "+")): string.Format("https://maps.googleapis.com/maps/api/geocode/json?latlng={0},{1}&key=AIzaSyBatV_B_a-477kd8IN_wmvDEbJdIF9LQ8c", campgroundAddress.Latitude, campgroundAddress.Longitude);
            var returnValue = new Location();
            try
            {
                var response = new WebClient().DownloadString(googleAPI);
                var root = GoogleGeocode.FromJson(response);

                if (root.Status.ToUpper() == "OK")
                {
                    returnValue.Lat = root.Results.FirstOrDefault(g => g.Geometry.Location.Lat != 0).Geometry.Location.Lat;
                    returnValue.Lng = root.Results.FirstOrDefault(g => g.Geometry.Location.Lng != 0).Geometry.Location.Lng;
                    returnValue.PlaceId = root.Results.FirstOrDefault(g => !string.IsNullOrEmpty(g.PlaceId)).PlaceId;
                    returnValue.StateProvinceId = _stateProvinceService.GetStateProvinceByAbbreviation(root.Results.Select(a => a.AddressComponents.FirstOrDefault(t => t.Types.Contains("administrative_area_level_1")))?.First()?.ShortName).Id;
                    returnValue.GeocodeURL = googleAPI;
                    returnValue.Found = true;
                }
                else
                {
                    returnValue.Found = true;
                    _logger.Warning($"Google GEO Location Warning: " + root.Status, null, _workContext.CurrentCustomer);
                }
            }
            catch
            {
                returnValue.Found = false;
            }

            return returnValue;
        }

        private void UpdateCampgroundGeoLocation(CampgroundAddress campgroundAddress)
        {
            if ((campgroundAddress.ZipPostalCode == null || campgroundAddress.GooglePlaceId == null) || (campgroundAddress.Latitude == null && campgroundAddress.Longitude == null))
            {
                // GooglePlacesAPi Key = AIzaSyDYig6UG2pNzek85kR6OTqQnJvnLZIoTPw
                // GeocodeAPI Key = AIzaSyBatV_B_a-477kd8IN_wmvDEbJdIF9LQ8c
                // 1 mile = 1609.34 meters
                //https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=-33.8670522,151.1957362&radius=500&type=restaurant&keyword=cruise&key=YOUR_API_KEY
                var googleAPI = string.Empty;

                if (campgroundAddress.Latitude != null && campgroundAddress.Longitude != null)
                    googleAPI = string.Format("https://maps.googleapis.com/maps/api/geocode/json?latlng={0},{1}&key=AIzaSyBatV_B_a-477kd8IN_wmvDEbJdIF9LQ8c", campgroundAddress.Latitude.ToString().Trim(), campgroundAddress.Longitude.ToString().Trim());
                else
                    googleAPI = string.Format("https://maps.googleapis.com/maps/api/geocode/json?address={0}&key=AIzaSyBatV_B_a-477kd8IN_wmvDEbJdIF9LQ8c", campgroundAddress.Address1?.Trim().Replace(" ", "+") + ",+" + campgroundAddress.City?.Trim().Replace(" ", "+") + ",+" + campgroundAddress.StateProvince?.Name.Trim().Replace(" ", "+") + ",+" + campgroundAddress.ZipPostalCode?.Trim().Replace(" ", "+"));


                var response = new WebClient().DownloadString(googleAPI);
                var root = GoogleGeocode.FromJson(response);

                try
                {
                    if (root.Status.ToUpper() == "OK")
                    {
                        if (campgroundAddress.Latitude != null && campgroundAddress.Longitude != null)
                        {
                            campgroundAddress.Longitude = root.Results.FirstOrDefault(g => g.Geometry.Location.Lng != 0).Geometry.Location.Lng;
                            campgroundAddress.Latitude = root.Results.FirstOrDefault(g => g.Geometry.Location.Lat != 0).Geometry.Location.Lat;
                            campgroundAddress.Address1 = root.Results.Select(a => a.AddressComponents.FirstOrDefault(t => t.Types.Contains("route")))?.First()?.ShortName;
                            campgroundAddress.City = root.Results.Select(a => a.AddressComponents.FirstOrDefault(t => t.Types.Contains("locality")))?.First()?.LongName;
                            campgroundAddress.ZipPostalCode = root.Results.Select(a => a.AddressComponents.FirstOrDefault(t => t.Types.Contains("postal_code")))?.First()?.LongName;
                            campgroundAddress.GooglePlaceId = root.Results.FirstOrDefault(g => g.PlaceId != String.Empty).PlaceId;
                            campgroundAddress.GoogleGeocodeURL = googleAPI;
                            campgroundAddress.FormattedAddress = root.Results.FirstOrDefault(g => g.FormattedAddress != String.Empty).FormattedAddress;
                        }
                        else
                        {
                            campgroundAddress.Longitude = root.Results.FirstOrDefault(g => g.Geometry.Location.Lng != 0).Geometry.Location.Lng;
                            campgroundAddress.Latitude = root.Results.FirstOrDefault(g => g.Geometry.Location.Lat != 0).Geometry.Location.Lat;
                            campgroundAddress.GooglePlaceId = root.Results.FirstOrDefault(g => g.PlaceId != String.Empty).PlaceId;
                            campgroundAddress.GoogleGeocodeURL = googleAPI;
                            campgroundAddress.FormattedAddress = root.Results.FirstOrDefault(g => g.FormattedAddress != String.Empty).FormattedAddress;
                        }

                        _campgroundAddressRepository.Update(campgroundAddress);
                    }
                    else
                        _logger.Warning($"Google GEO Location Warning: " + root.Status, null, _workContext.CurrentCustomer);

                }
                catch (Exception exception)
                {
                    //log full error
                    _logger.Error($"Google GEO Location Exception: {exception.Message}.", exception, _workContext.CurrentCustomer);
                }
            }
        }

        public byte[] SaveCampgroundStaticMap(Campground campground)
        {
            //https://maps.googleapis.com/maps/api/staticmap?size=350x350&zoom=12&center=@Model.CampgroundAddress.Latitude,@Model.CampgroundAddress.Longitude&maptype=terrain&markers=icon:http%3A%2F%2Fwww.camptale.com%2Fthemes%2Fnopelectro%2Fcontent%2Fimages%2Fmaps%2Fcamping-logo.png|label:X|@Model.CampgroundAddress.Latitude,@Model.CampgroundAddress.Longitude&format=png&style=feature:road.highway%7Celement:geometry%7Cvisibility:simplified%7Ccolor:0xc280e9&style=feature:transit.line%7Cvisibility:simplified%7Ccolor:0xbababa&style=feature:road.highway%7Celement:labels.text.stroke%7Cvisibility:on%7Ccolor:0xb06eba&style=feature:road.highway%7Celement:labels.text.fill%7Cvisibility:on%7Ccolor:0xffffff&key=AIzaSyBatV_B_a-477kd8IN_wmvDEbJdIF9LQ8c

            var staticMapAPI = string.Empty;

            if (campground.CampgroundAddress?.Latitude != null && campground.CampgroundAddress?.Longitude != null)
                staticMapAPI = string.Format("https://maps.googleapis.com/maps/api/staticmap?size=350x350&zoom=12&center={0},{1}&maptype=terrain&format=jpg&markers=icon:http%3A%2F%2Fwww.camptale.com%2Fthemes%2Fnopelectro%2Fcontent%2Fimages%2Fmaps%2Fcamping-logo.png|label:X|{0},{1}&style=feature:road.highway%7Celement:geometry%7Cvisibility:simplified%7Ccolor:0xc280e9&style=feature:transit.line%7Cvisibility:simplified%7Ccolor:0xbababa&style=feature:road.highway%7Celement:labels.text.stroke%7Cvisibility:on%7Ccolor:0xb06eba&style=feature:road.highway%7Celement:labels.text.fill%7Cvisibility:on%7Ccolor:0xffffff&key=AIzaSyBatV_B_a-477kd8IN_wmvDEbJdIF9LQ8c", campground.CampgroundAddress.Latitude.ToString().Trim(), campground.CampgroundAddress.Longitude.ToString().Trim());

            var client = new WebClient();

            var response = _pictureService.ValidatePicture(client.DownloadData(staticMapAPI), MimeTypes.ImageJpeg);

            return response;
        }
        #endregion

        #region Campgrounds

        /// <summary>
        /// Delete a Campground
        /// </summary>
        /// <param name="Campground">Campground</param>
        public virtual void DeleteCampground(Campground campgrounds)
        {
            if (campgrounds == null)
                throw new ArgumentNullException("Campground");

            campgrounds.Deleted = true;
            //delete Campground
            UpdateCampground(campgrounds);

            //event notification
            _eventPublisher.EntityDeleted(campgrounds);
        }

        /// <summary>
        /// Delete Campgrounds
        /// </summary>
        /// <param name="Campgrounds">Campgrounds</param>
        public virtual void DeleteCampgrounds(IList<Campground> campgrounds)
        {
            if (campgrounds == null)
                throw new ArgumentNullException("Campgrounds");

            foreach (var campground in campgrounds)
            {
                campground.Deleted = true;
            }

            //delete Campground
            UpdateCampgrounds(campgrounds);

            foreach (var Campground in campgrounds)
            {
                //event notification
                _eventPublisher.EntityDeleted(Campground);
            }
        }

        /// <summary>
        /// Gets all Campgrounds displayed on the home page
        /// </summary>
        /// <returns>Campgrounds</returns>
        public virtual IList<Campground> GetAllCampgroundsDisplayedOnHomePage()
        {
            var query = from c in _campgroundRepository.Table
                        orderby c.DisplayOrder, c.Id
                        where c.Published &&
                        !c.Deleted &&
                        c.ShowOnHomePage
                        select c;
            var Campgrounds = query.ToList();
            return Campgrounds;
        }

        /// <summary>
        /// Gets Campground
        /// </summary>
        /// <param name="campgroundId">Campground identifier</param>
        /// <returns>Campground</returns>
        public virtual Campground GetCampgroundById(int campgroundId)
        {
            if (campgroundId == 0)
                return null;

            string key = string.Format(CAMPGROUNDS_BY_ID_KEY, campgroundId);
            return _cacheManager.Get(key, () => _campgroundRepository.GetById(campgroundId));
        }

        /// <summary>
        /// Get all campground availability ranges
        /// </summary>
        /// <returns>Campground availability ranges</returns>
        //public virtual IList<CampgroundAvailabilityRange> GetAllCampgroundAvailabilityRanges()
        //{
        //    var query = from par in _campgroundAvailabilityRangeRepository.Table
        //                orderby par.DisplayOrder, par.Id
        //                select par;
        //    return query.ToList();
        //}

        /// <summary>
        /// Get Campground Classes
        /// </summary>
        /// <returns>Campground Classes</returns>
        public virtual Dictionary<int, string> GetCampgroundClass()
        {
            string[] campgroundClass = Enum.GetNames(typeof(CampgroundClass));

            var Campgrounds = campgroundClass.Select((value, key) =>
            new { value, key }).ToDictionary(x => x.key + 1, x => x.value);

            return Campgrounds;
        }


        public virtual IList<Campground> GetNewCampgrounds(int pageSize = 6)
        {
            var query = from c in _campgroundRepository.Table
                        orderby c.Id descending
                        where c.Published &&
                        !c.Deleted &&
                        c.MarkAsNew
                        select c;
            return query.ToList();
        }

        /// <summary>
        /// Inserts a campground category mapping
        /// </summary>
        /// <param name="campgroundCategory">>Campground category mapping</param>
        public virtual void InsertCampgroundCategory(CampgroundCategory campgroundCategory)
        {
            if (campgroundCategory == null)
                throw new ArgumentNullException(nameof(campgroundCategory));

            _campgroundCategoryRepository.Insert(campgroundCategory);

            //cache
            _cacheManager.RemoveByPattern(CATEGORIES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDCATEGORIES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(campgroundCategory);
        }

        /// <summary>
        /// Updates the campground category mapping 
        /// </summary>
        /// <param name="campgroundCategory">>Campground category mapping</param>
        public virtual void UpdateCampgroundCategory(CampgroundCategory campgroundCategory)
        {
            if (campgroundCategory == null)
                throw new ArgumentNullException(nameof(campgroundCategory));

            _campgroundCategoryRepository.Update(campgroundCategory);

            //cache
            _cacheManager.RemoveByPattern(CATEGORIES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDCATEGORIES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(campgroundCategory);
        }

        /// <summary>
        /// Deletes a campground category mapping
        /// </summary>
        /// <param name="campgroundCategory">Campground category</param>
        public virtual void DeleteCampgroundCategory(CampgroundCategory campgroundCategory)
        {
            if (campgroundCategory == null)
                throw new ArgumentNullException(nameof(campgroundCategory));

            _campgroundCategoryRepository.Delete(campgroundCategory);

            //cache
            _cacheManager.RemoveByPattern(CATEGORIES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDCATEGORIES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(campgroundCategory);
        }

        /// <summary>
        /// Gets campground category mapping collection
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Campground a category mapping collection</returns>
        public virtual IPagedList<CampgroundCategory> GetCampgroundCategoriesByCategoryId(int categoryId,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            if (categoryId == 0)
                return new PagedList<CampgroundCategory>(new List<CampgroundCategory>(), pageIndex, pageSize);

            var key = string.Format(CAMPGROUNDCATEGORIES_ALLBYCATEGORYID_KEY, showHidden, categoryId, pageIndex, pageSize, _workContext.CurrentCustomer.Id, _storeContext.CurrentStore.Id);
            return _cacheManager.Get(key, () =>
            {
                var query = from cr in _campgroundCategoryRepository.Table
                            join c in _campgroundRepository.Table on cr.CampgroundId equals c.Id
                            where cr.CategoryId == categoryId &&
                                  !c.Deleted &&
                                  (showHidden || c.Published)
                            orderby cr.DisplayOrder, cr.Id
                            select cr;

                if (!showHidden && (!_catalogSettings.IgnoreAcl || !_catalogSettings.IgnoreStoreLimitations))
                {
                    if (!_catalogSettings.IgnoreAcl)
                    {
                        //ACL (access control list)
                        var allowedCustomerRolesIds = _workContext.CurrentCustomer.GetCustomerRoleIds();
                        query = from pc in query
                                join c in _categoryRepository.Table on pc.CategoryId equals c.Id
                                join acl in _aclRepository.Table
                                on new { c1 = c.Id, c2 = "Category" } equals new { c1 = acl.EntityId, c2 = acl.EntityName } into c_acl
                                from acl in c_acl.DefaultIfEmpty()
                                where !c.SubjectToAcl || allowedCustomerRolesIds.Contains(acl.CustomerRoleId)
                                select pc;
                    }
                    if (!_catalogSettings.IgnoreStoreLimitations)
                    {
                        //Store mapping
                        var currentStoreId = _storeContext.CurrentStore.Id;
                        query = from pc in query
                                join c in _categoryRepository.Table on pc.CategoryId equals c.Id
                                join sm in _storeMappingRepository.Table
                                on new { c1 = c.Id, c2 = "Category" } equals new { c1 = sm.EntityId, c2 = sm.EntityName } into c_sm
                                from sm in c_sm.DefaultIfEmpty()
                                where !c.LimitedToStores || currentStoreId == sm.StoreId
                                select pc;
                    }
                    //only distinct categories (group by ID)
                    query = from c in query
                            group c by c.Id
                            into cGroup
                            orderby cGroup.Key
                            select cGroup.FirstOrDefault();
                    query = query.OrderBy(pc => pc.DisplayOrder).ThenBy(pc => pc.Id);
                }

                var campgroundCategories = new PagedList<CampgroundCategory>(query, pageIndex, pageSize);
                return campgroundCategories;
            });
        }

        /// <summary>
        /// Gets a campground category mapping collection
        /// </summary>
        /// <param name="campgroundId">Campground identifier</param>
        /// <param name="showHidden"> A value indicating whether to show hidden records</param>
        /// <returns> Campground category mapping collection</returns>
        public virtual IList<CampgroundCategory> GetCampgroundCategoriesByCampgroundId(int campgroundId, bool showHidden = false)
        {
            return GetCampgroundCategoriesByCampgroundId(campgroundId, _storeContext.CurrentStore.Id, showHidden);
        }

        /// <summary>
        /// Gets a campground category mapping collection
        /// </summary>
        /// <param name="campgroundId">Campground identifier</param>
        /// <param name="storeId">Store identifier (used in multi-store environment). "showHidden" parameter should also be "true"</param>
        /// <param name="showHidden"> A value indicating whether to show hidden records</param>
        /// <returns> Campground category mapping collection</returns>
        public virtual IList<CampgroundCategory> GetCampgroundCategoriesByCampgroundId(int campgroundId, int storeId, bool showHidden = false)
        {
            if (campgroundId == 0)
                return new List<CampgroundCategory>();

            var key = string.Format(CAMPGROUNDCATEGORIES_ALLBYCAMPGROUNDID_KEY, showHidden, campgroundId, _workContext.CurrentCustomer.Id, storeId);
            return _cacheManager.Get(key, () =>
            {
                var categories = (from c in _categoryRepository.Table
                                  select c).ToArray();

                var query = from pc in _campgroundCategoryRepository.Table
                                //join c in categories on pc.CategoryId equals c.Id
                            where pc.CampgroundId == campgroundId //&&
                                  //!c.Deleted &&
                                  //(showHidden || c.Published)
                            orderby pc.DisplayOrder//, pc.Id
                            select pc;

                var allCampgroundCategories = query.ToList();
                var result = new List<CampgroundCategory>();
                if (!showHidden)
                {
                    foreach (var pc in allCampgroundCategories)
                    {
                        //ACL (access control list) and store mapping
                        var category = pc.Category;
                        if (_aclService.Authorize(category) && _storeMappingService.Authorize(category, storeId))
                            result.Add(pc);
                    }
                }
                else
                {
                    //no filtering
                    result.AddRange(allCampgroundCategories);
                }
                return result;
            });
        }

        /// <summary>
        /// Gets associated campgrounds
        /// </summary>
        /// <param name="parentGroupedCampgroundId">Parent campground identifier (used with grouped campgrounds)</param>
        /// <param name="storeId">Store identifier; 0 to load all records</param>
        /// <param name="campgroundHostId">CampgroundHost identifier; 0 to load all records</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Campgrounds</returns>
        public virtual IList<Campground> GetAssociatedCampgrounds(int parentGroupedCampgroundId,
            int storeId = 0, int campgroundHostId = 0, bool showHidden = false)
        {
            var query = _campgroundRepository.Table;
            query = query.Where(x => x.ParentGroupedCampgroundId == parentGroupedCampgroundId);
            if (!showHidden)
            {
                query = query.Where(x => x.Published);

                //The function 'CurrentUtcDateTime' is not supported by SQL Server Compact. 
                //That's why we pass the date value
                var nowUtc = DateTime.UtcNow;
                //available dates
                query = query.Where(p =>
                    (!p.AvailableStartDateTimeUtc.HasValue || p.AvailableStartDateTimeUtc.Value < nowUtc) &&
                    (!p.AvailableEndDateTimeUtc.HasValue || p.AvailableEndDateTimeUtc.Value > nowUtc));
            }
            //campgroundHost filtering
            //if (campgroundHostId > 0)
            //{
            //    query = query.Where(p => p.CampgroundHost.Where(ch => ch.Id == campgroundHostId));
            //}
            query = query.Where(x => !x.Deleted);
            query = query.OrderBy(x => x.DisplayOrder).ThenBy(x => x.Id);

            var campgrounds = query.ToList();

            //ACL mapping
            if (!showHidden)
            {
                campgrounds = campgrounds.Where(x => _aclService.Authorize(x)).ToList();
            }
            //Store mapping
            if (!showHidden && storeId > 0)
            {
                campgrounds = campgrounds.Where(x => _storeMappingService.Authorize(x, storeId)).ToList();
            }

            return campgrounds;
        }

        /// <summary>
        /// Gets campground by category and name
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <param name="campgroundName"></param>
        /// <returns></returns>
        public virtual Campground GetCampgroundByCategoryIdName(int categoryId, string campgroundName)
        {
            if (categoryId == 0)
                return null;

            string key = string.Format(CAMPGROUNDSTATEID_BY_CATEGORYID_KEY, categoryId);
            var campgroundStateId = _cacheManager.Get(key, () => GetCampgroundStateIdFromCategoryId(categoryId));

            key = string.Format(CAMPGROUNDS_BY_CATEGORYID_KEY, categoryId);
            return _cacheManager.Get(key, () => GetCampgroundsByName(campgroundStateId, campgroundName));
        }

        /// <summary>
        /// Get Campgrounds by StateID and Campground Name
        /// </summary>
        /// <param name="CampgroundStateId"></param>
        /// <param name="campgroundName"></param>
        /// <returns></returns>
        public virtual Campground GetCampgroundsByName(int CampgroundStateId, string campgroundName)
        {
            var query = _campgroundRepository.Table;
            query = query.Where(x => x.CampgroundAddress.StateProvinceId == CampgroundStateId);
            query = query.Where(x => x.Name == campgroundName);
            query = query.Where(x => !x.Deleted);
            query = query.OrderBy(x => x.Name);

            return query.FirstOrDefault();
        }

        /// <summary>
        /// Get campground address by campground ID
        /// </summary>
        /// <param name="campgroundId"></param>
        /// <returns></returns>
        public virtual CampgroundAddress GetCampgroundAddressByCampgroundId(int campgroundId)
        {
            var query = from campground in _campgroundRepository.Table
                        join address in _campgroundAddressRepository.Table on campground.CampgroundAddressId equals address.Id
                        where campground.Id == campgroundId
                        select address;

            var campgroundAddress = query.FirstOrDefault();

            UpdateCampgroundGeoLocation(campgroundAddress);

            return campgroundAddress;
        }


        public virtual IList<Campground> GetNearbyCampgrounds(decimal? sourceLatitude, decimal? sourceLongitude, int Distance = 25, bool useStoreProc = true)
        {
            if ((sourceLatitude == null) || (sourceLongitude == null))
                return null;

            List<Campground> campgrounds = new List<Campground>();
            if (useStoreProc) {
                var pSourceLatitude = _dataProvider.GetDecimalParameter("startlat", sourceLatitude);
                var pSourceLongitude = _dataProvider.GetDecimalParameter("startlng", sourceLongitude);
                var pDistance = _dataProvider.GetInt32Parameter("Distance", Distance);

                var results = _dbContext.ExecuteStoredProcedureList<Campground>(
                        "GetNearbyAddresses",
                        pSourceLatitude,
                        pSourceLongitude,
                        pDistance).ToList();
                foreach (var campground in results)
                {
                    Campground c = new Campground
                    {
                        Id = campground.Id,
                        Name = campground.GetLocalized(x => x.Name),
                        ShortDescription = campground.GetLocalized(x => x.ShortDescription),
                        FullDescription = campground.GetLocalized(x => x.FullDescription),
                        MetaDescription = campground.GetLocalized(x => x.MetaDescription),
                        MetaKeywords = campground.GetLocalized(x => x.MetaKeywords),
                        MetaTitle = campground.GetLocalized(x => x.MetaTitle),
                        AvailableCampsites = campground.AvailableCampsites,
                        Distance = campground.Distance,
                        CampgroundAddress = GetCampgroundAddressByCampgroundId(campground.Id),
                        AllowCampgroundReviews = campground.AllowCampgroundReviews
                    };
                    campgrounds.Add(c);
                }
            }
            else
            {

                var nearby = from c in _campgroundRepository.Table
                             where (3959 * SqlFunctions.Acos(SqlFunctions.Cos(SqlFunctions.Radians(sourceLatitude)) * SqlFunctions.Cos(SqlFunctions.Radians(c.CampgroundAddress.Latitude)) * SqlFunctions.Cos(SqlFunctions.Radians(c.CampgroundAddress.Longitude) - SqlFunctions.Radians(sourceLongitude)) + SqlFunctions.Sin(SqlFunctions.Radians(sourceLatitude)) * SqlFunctions.Sin(SqlFunctions.Radians(c.CampgroundAddress.Latitude)))) <= Distance
                             select new { c.Id, Distance = (3959 * SqlFunctions.Acos(SqlFunctions.Cos(SqlFunctions.Radians(sourceLatitude)) * SqlFunctions.Cos(SqlFunctions.Radians(c.CampgroundAddress.Latitude)) * SqlFunctions.Cos(SqlFunctions.Radians(c.CampgroundAddress.Longitude) - SqlFunctions.Radians(sourceLongitude)) + SqlFunctions.Sin(SqlFunctions.Radians(sourceLatitude)) * SqlFunctions.Sin(SqlFunctions.Radians(c.CampgroundAddress.Latitude)))) };

                var results = from campground in _campgroundRepository.Table
                            join nc in nearby on campground.Id equals nc.Id
                            where campground.Deleted == false &&
                              nc.Distance > 0
                            orderby nc.Distance
                            select new { campground, nc.Distance };

                foreach (var item in results)
                {
                    Campground c = new Campground
                    {
                        Id = item.campground.Id,
                        Name = item.campground.GetLocalized(x => x.Name),
                        ShortDescription = item.campground.GetLocalized(x => x.ShortDescription),
                        FullDescription = item.campground.GetLocalized(x => x.FullDescription),
                        MetaDescription = item.campground.GetLocalized(x => x.MetaDescription),
                        MetaKeywords = item.campground.GetLocalized(x => x.MetaKeywords),
                        MetaTitle = item.campground.GetLocalized(x => x.MetaTitle),
                        AvailableCampsites = item.campground.AvailableCampsites,
                        Distance = (decimal)item.campground.Distance,
                        AllowCampgroundReviews = item.campground.AllowCampgroundReviews
                    };
                    campgrounds.Add(c);
                }
            }

            return campgrounds;
        }
        /// <summary>
        /// Get Campgrounds by category id
        /// </summary>
        /// <param name="CategoryId">CategoryId</param>
        /// <returns>Campgrounds</returns>
        public virtual IPagedList<Campground> GetCampgroundsByCategoryId(int CategoryId, int pageIndex, int pageSize)
        {
            if (CategoryId == 0)
                return null;

            string key = string.Format(CAMPGROUNDSTATEID_BY_CATEGORYID_KEY, CategoryId);
            var campgroundStateId = _cacheManager.Get(key, () => GetCampgroundStateIdFromCategoryId(CategoryId));

            key = string.Format(CAMPGROUNDS_BY_CATEGORYID_KEY, CategoryId);
            return _cacheManager.Get(key, () => GetCampgroundsByStateId(campgroundStateId, pageIndex, pageSize));
        }

        /// <summary>
        /// Get Campgrounds by state id
        /// </summary>
        /// <param name="CampgroundStateId">Campground state identifiers</param>
        /// <returns>Campgrounds</returns>
        public virtual IPagedList<Campground> GetCampgroundsByStateId(int CampgroundStateId, int pageIndex, int pageSize)
        {
            var query = _campgroundRepository.Table;
            query = query.Where(x => x.CampgroundAddress.StateProvinceId == CampgroundStateId);
            query = query.Where(x => !x.Deleted);
            query = query.OrderBy(x => x.Name);

            var campground = new PagedList<Campground>(query, pageIndex, pageSize);

            return campground;
        }

        /// <summary>
        /// Gets campgrounds by campground attribute
        /// </summary>
        /// <param name="campgroundAttributeTypeId">Campground attribute identifier</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Campgrounds</returns>
        public virtual IPagedList<Campground> GetCampgroundsByCampgroundAtributeId(int campgroundAttributeTypeId,
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _campgroundRepository.Table;
            query = query.Where(x => x.CampgroundAttributeMappings.Any(y => y.CampgroundAttributeTypeId == campgroundAttributeTypeId));
            query = query.Where(x => !x.Deleted);
            query = query.OrderBy(x => x.Name);

            var campgrounds = new PagedList<Campground>(query, pageIndex, pageSize);
            return campgrounds;
        }

        /// <summary>
        /// Get stateId from categroyId
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        public virtual int GetCampgroundStateIdFromCategoryId(int CategoryId)
        {
            var stateId = from campground in _campgroundRepository.Table
                          join address in _campgroundAddressRepository.Table on campground.CampgroundAddressId equals address.Id
                          join categories in _campgroundCategoryRepository.Table on campground.Id equals categories.CampgroundId
                          where (categories.CategoryId == CategoryId)
                          select address.StateProvinceId;

            return stateId.FirstOrDefault() == null ? 0 : stateId.FirstOrDefault().Value;
        }

        /// <summary>
        /// Get campground categoryId by state/province identifier
        /// </summary>
        /// <param name="stateProvinceId">State/province identifier</param>
        /// <returns>Number of campgroundAddresses</returns>
        public virtual int GetCampgroundCategoryIdFromStateId(int stateProvinceId)
        {
            if (stateProvinceId == 0)
                return 0;

            var query = from c in _campgroundRepository.Table
                        join ca in _campgroundAddressRepository.Table on c.CampgroundAddressId equals ca.Id
                        join cr in _campgroundCategoryRepository.Table on c.Id equals cr.CampgroundId
                        where ca.StateProvinceId == stateProvinceId
                        select cr.CategoryId;

            return query.FirstOrDefault();
        }

        /// <summary>
        /// Get Campgrounds by identifiers
        /// </summary>
        /// <param name="campgroundIds">Campground identifiers</param>
        /// <returns>Campgrounds</returns>
        public virtual IList<Campground> GetCampgroundsByIds(int[] campgroundIds)
        {
            if (campgroundIds == null || campgroundIds.Length == 0)
                return new List<Campground>();

            var query = from c in _campgroundRepository.Table
                        where campgroundIds.Contains(c.Id) && !c.Deleted
                        select c;
            var Campgrounds = query.ToList();
            //sort by passed identifiers
            var sortedCampgrounds = new List<Campground>();
            foreach (int id in campgroundIds)
            {
                var Campground = Campgrounds.Find(x => x.Id == id);
                if (Campground != null)
                    sortedCampgrounds.Add(Campground);
            }
            return sortedCampgrounds;
        }

        /// <summary>
        /// Get featured Campgrounds 
        /// </summary>
        /// <returns>Campgrounds</returns>
        public virtual IPagedList<Campground> GetFeaturedCampgrounds(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _campgroundRepository.Table;
            query = query.Where(x => x.IsFeatured);
            query = query.Where(x => !x.Deleted);
            query = query.OrderBy(x => x.Name);

            var campground = new PagedList<Campground>(query, pageIndex, pageSize);
            return campground;
        }

        /// <summary>
        /// Inserts a Campground
        /// </summary>
        /// <param name="Campground">Campground</param>
        public virtual void InsertCampground(Campground campground)
        {
            if (campground == null)
                throw new ArgumentNullException("Campground");

            if (campground.CreatedOnUtc == null || campground.CreatedOnUtc == DateTime.MinValue)
                campground.CreatedOnUtc = _dateTimeHelper.ConvertToUserTime(DateTime.Now, DateTimeKind.Utc);


            //insert
            _campgroundRepository.Insert(campground);

            //clear cache
            _cacheManager.RemoveByPattern(CAMPGROUNDS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(campground);
        }

        public virtual void InsertCampgroundHost(CampgroundHost campgroundHost)
        {
            if (campgroundHost == null)
                throw new ArgumentNullException("CampgroundHost");

            if (campgroundHost.CreatedOnUtc == null || campgroundHost.CreatedOnUtc == DateTime.MinValue)
                campgroundHost.CreatedOnUtc = _dateTimeHelper.ConvertToUserTime(DateTime.Now, DateTimeKind.Utc);

            //insert
            _campgroundHostRepository.Insert(campgroundHost);

            //clear cache
            _cacheManager.RemoveByPattern(CAMPGROUNDHOST_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(campgroundHost);
        }

        /// <summary>
        /// Updates the Campground
        /// </summary>
        /// <param name="Campground">Campground</param>
        public virtual void UpdateCampground(Campground campground)
        {
            if (campground == null)
                throw new ArgumentNullException("Campground");

            campground.UpdatedOnUtc = _dateTimeHelper.ConvertToUserTime(DateTime.Now, DateTimeKind.Utc);

            //update
            _campgroundRepository.Update(campground);

            //cache
            _cacheManager.RemoveByPattern(CAMPGROUNDS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(campground);
        }

        public virtual void UpdateCampgrounds(IList<Campground> campgrounds)
        {
            if (campgrounds == null)
                throw new ArgumentNullException("Campgrounds");

            foreach (var campground in campgrounds)
                campground.UpdatedOnUtc = _dateTimeHelper.ConvertToUserTime(DateTime.Now, DateTimeKind.Utc);

            //update
            _campgroundRepository.Update(campgrounds);

            //cache
            _cacheManager.RemoveByPattern(CAMPGROUNDS_PATTERN_KEY);

            //event notification
            foreach (var Campground in campgrounds)
            {
                _eventPublisher.EntityUpdated(Campground);
            }
        }

        /// <summary>
        /// Get number of Campground (published and visible) in certain category
        /// </summary>
        /// <param name="categoryIds">Category identifiers</param>
        /// <param name="storeId">Store identifier; 0 to load all records</param>
        /// <returns>Number of Campgrounds</returns>
        public virtual int GetNumberOfCampgroundsInCategory(IList<int> categoryIds = null, int storeId = 0)
        {
            //validate "categoryIds" parameter
            if (categoryIds != null && categoryIds.Contains(0))
                categoryIds.Remove(0);

            var query = _campgroundRepository.Table;
            query = query.Where(c => !c.Deleted && c.Published && c.VisibleIndividually);

            //category filtering
            if (categoryIds != null && categoryIds.Any())
            {
                query = from c in query
                        from cc in c.CampgroundCategories.Where(cc => categoryIds.Contains(cc.CategoryId))
                        where c.Deleted == false
                        select c;
            }
            else
            {
                query = from c in query
                        where c.Deleted == false
                        select c;
            }

            if (!_campgroundSettings.IgnoreAcl)
            {
                //Access control list. Allowed customer roles
                var allowedCustomerRolesIds = _workContext.CurrentCustomer.GetCustomerRoleIds();

                query = from c in query
                        join acl in _aclRepository.Table
                        on new { c1 = c.Id, c2 = "Campground" } equals new { c1 = acl.EntityId, c2 = acl.EntityName } into c_acl
                        from acl in c_acl.DefaultIfEmpty()
                        where !c.SubjectToAcl || allowedCustomerRolesIds.Contains(acl.CustomerRoleId)
                        select c;
            }

            //if (storeId > 0 && !_campgroundSettings.IgnoreStoreLimitations)
            //{
            //    query = from c in query
            //            join sm in _storeMappingRepository.Table
            //            on new { c1 = c.Id, c2 = "Campground" } equals new { c1 = sm.EntityId, c2 = sm.EntityName } into c_sm
            //            from sm in c_sm.DefaultIfEmpty()
            //            where !c.LimitedToStores || storeId == sm.StoreId
            //            select c;
            //}

            //only distinct Campgrounds
            var result = query.Select(c => c.Id).Distinct().Count();
            return result;
        }


        public virtual int GetCampgroundsParentCategoryId(Campground campground)
        {
            var query = from cc in _campgroundCategoryRepository.Table
                        //join c in _categoryRepository.Table on cc.CategoryId equals c.Id
                        where cc.CampgroundId == campground.Id
                        select cc.CategoryId;

            return (int)query.FirstOrDefault();
        }


        /// <summary>
        /// Update Campground review totals
        /// </summary>
        /// <param name="Campground">Campground</param>
        public virtual void UpdateCampgroundReviewTotals(Campground campground)
        {
            if (campground == null)
                throw new ArgumentNullException("Campground");

            int approvedRatingSum = 0;
            int notApprovedRatingSum = 0;
            int approvedTotalReviews = 0;
            int notApprovedTotalReviews = 0;
            var reviews = campground.CampgroundReviews;
            foreach (var cr in reviews)
            {
                if (cr.IsApproved)
                {
                    approvedRatingSum += cr.Rating;
                    approvedTotalReviews++;
                }
                else
                {
                    notApprovedRatingSum += cr.Rating;
                    notApprovedTotalReviews++;
                }
            }

            campground.ApprovedRatingSum = approvedRatingSum;
            campground.NotApprovedRatingSum = notApprovedRatingSum;
            campground.ApprovedTotalReviews = approvedTotalReviews;
            campground.NotApprovedTotalReviews = notApprovedTotalReviews;
            UpdateCampground(campground);
        }

        /// <summary>
        /// Gets number of Campgrounds by campgroundHost identifier
        /// </summary>
        /// <param name="campgroundHostId">CampgroundHost identifier</param>
        /// <returns>Number of Campgrounds</returns>
        public int GetNumberOfCampgroundsByCustomerId(int customerId)
        {
            if (customerId == 0)
                return 0;

            return _campgroundHostRepository.Table.Select(ch => ch.CustomerId == customerId && !ch.Deleted).Count();
        }

        #endregion

        #region Campground pictures

        /// <summary>
        /// Deletes a Campground picture
        /// </summary>
        /// <param name="CampgroundPicture">Campground picture</param>
        public virtual void DeleteCampgroundPicture(CampgroundPicture campgroundPicture)
        {
            if (campgroundPicture == null)
                throw new ArgumentNullException("CampgroundPicture");

            _campgroundPictureRepository.Delete(campgroundPicture);

            //event notification
            _eventPublisher.EntityDeleted(campgroundPicture);
        }

        /// <summary>
        /// Gets a Campground pictures by Campground identifier
        /// </summary>
        /// <param name="campgroundId">The Campground identifier</param>
        /// <returns>Campground pictures</returns>
        public virtual IList<CampgroundPicture> GetCampgroundPicturesByCampgroundId(int campgroundId)
        {
            var query = from pp in _campgroundPictureRepository.Table
                        where pp.CampgroundId == campgroundId
                        orderby pp.DisplayOrder, pp.Id
                        select pp;
            var CampgroundPictures = query.ToList();
            return CampgroundPictures;
        }

        /// <summary>
        /// Gets a Campground picture
        /// </summary>
        /// <param name="CampgroundPictureId">Campground picture identifier</param>
        /// <returns>Campground picture</returns>
        public virtual CampgroundPicture GetCampgroundPictureById(int CampgroundPictureId)
        {
            if (CampgroundPictureId == 0)
                return null;

            return _campgroundPictureRepository.GetById(CampgroundPictureId);
        }

        /// <summary>
        /// Inserts a Campground picture
        /// </summary>
        /// <param name="CampgroundPicture">Campground picture</param>
        public virtual void InsertCampgroundPicture(CampgroundPicture campgroundPicture)
        {
            if (campgroundPicture == null)
                throw new ArgumentNullException("CampgroundPicture");

            _campgroundPictureRepository.Insert(campgroundPicture);

            //event notification
            _eventPublisher.EntityInserted(campgroundPicture);
        }

        /// <summary>
        /// Updates a Campground picture
        /// </summary>
        /// <param name="CampgroundPicture">Campground picture</param>
        public virtual void UpdateCampgroundPicture(CampgroundPicture campgroundPicture)
        {
            if (campgroundPicture == null)
                throw new ArgumentNullException("CampgroundPicture");

            _campgroundPictureRepository.Update(campgroundPicture);

            //event notification
            _eventPublisher.EntityUpdated(campgroundPicture);
        }

        /// <summary>
        /// Get the IDs of all Campground images 
        /// </summary>
        /// <param name="campgroundIds">Campground IDs</param>
        /// <returns>All picture identifiers grouped by Campground ID</returns>
        public IDictionary<int, int[]> GetCampgroundsImagesIds(int[] campgroundIds)
        {
            return _campgroundPictureRepository.Table.Where(c => campgroundIds.Contains(c.CampgroundId))
                .GroupBy(c => c.CampgroundId).ToDictionary(c => c.Key, c => c.Select(p1 => p1.PictureId).ToArray());
        }


        #endregion

        #region Campground reviews

        /// <summary>
        /// Get all campground reviews
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="approved"></param>
        /// <param name="fromUtc"></param>
        /// <param name="toUtc"></param>
        /// <param name="message"></param>
        /// <param name="storeId"></param>
        /// <param name="campgroundId"></param>
        /// <param name="campgroundHostId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public virtual IPagedList<CampgroundReview> GetAllCampgroundReviews(int customerId, bool? approved,
            DateTime? fromUtc = null, DateTime? toUtc = null,
            string message = null, int storeId = 0, int campgroundId = 0, int campgroundHostId = 0,
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _campgroundReviewRepository.Table;
            if (approved.HasValue)
                query = query.Where(cr => cr.IsApproved == approved);
            if (customerId > 0)
                query = query.Where(cr => cr.CustomerId == customerId);
            if (fromUtc.HasValue)
                query = query.Where(cr => fromUtc.Value <= cr.CreatedOnUtc);
            if (toUtc.HasValue)
                query = query.Where(cr => toUtc.Value >= cr.CreatedOnUtc);
            if (!string.IsNullOrEmpty(message))
                query = query.Where(cr => cr.Title.Contains(message) || cr.ReviewText.Contains(message));
            if (storeId > 0)
                query = query.Where(cr => cr.StoreId == storeId);
            if (campgroundId > 0)
                query = query.Where(cr => cr.CampgroundId == campgroundId);
            //if (campgroundHostId > 0)
            //    query = query.Where(cr => cr.Campground.CampgroundHostId == campgroundHostId);

            //ignore deleted campgrounds
            query = query.Where(cr => cr.Campground != null && !cr.Campground.Deleted);
            query = query.Where(cr => !cr.Deleted);

            query = query.OrderByDescending(cr => cr.CreatedOnUtc).ThenBy(cr => cr.Id);

            var campgroundReviews = new PagedList<CampgroundReview>(query, pageIndex, pageSize);

            return campgroundReviews;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="campgroundReviewId"></param>
        /// <returns></returns>
        public virtual CampgroundReview GetCampgroundReviewById(int campgroundReviewId)
        {
            if (campgroundReviewId == 0)
                return null;

            return _campgroundReviewRepository.GetById(campgroundReviewId);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="campgroundReviewId"></param>
        /// <returns></returns>
        public virtual CampgroundReview GetCampgroundReviewByCustomer(int customerId, int campgroundId)
        {
            if (customerId == 0 && campgroundId == 0)
                return null;

            var query = _campgroundReviewRepository.Table;
            if (customerId > 0)
                query = query.Where(cr => cr.CustomerId == customerId);
            if (campgroundId > 0)
                query = query.Where(cr => cr.CampgroundId == campgroundId);
            query = query.Where(cr => !cr.Deleted);
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Get Campground reviews by identifiers
        /// </summary>
        /// <param name="CampgroundReviewIds">Campground review identifiers</param>
        /// <returns>Campground reviews</returns>
        public virtual IList<CampgroundReview> GetCampgroundReviewsByIds(int[] CampgroundReviewIds)
        {
            if (CampgroundReviewIds == null || CampgroundReviewIds.Length == 0)
                return new List<CampgroundReview>();

            var query = from cr in _campgroundReviewRepository.Table
                        where CampgroundReviewIds.Contains(cr.Id)
                          && cr.Deleted == false
                        select cr;
            var CampgroundReviews = query.ToList();
            //sort by passed identifiers
            var sortedCampgroundReviews = new List<CampgroundReview>();
            foreach (int id in CampgroundReviewIds)
            {
                var CampgroundReview = CampgroundReviews.Find(x => x.Id == id);
                if (CampgroundReview != null)
                    sortedCampgroundReviews.Add(CampgroundReview);
            }
            return sortedCampgroundReviews;
        }

        /// <summary>
        /// Deletes a Campground review
        /// </summary>
        /// <param name="CampgroundReview">Campground review</param>
        public virtual void DeleteCampgroundReview(CampgroundReview campgroundReview)
        {
            if (campgroundReview == null)
                throw new ArgumentNullException("CampgroundReview");

            campgroundReview.Deleted = true;
            //delete Campground
            UpdateCampground(campgroundReview.Campground);

            _cacheManager.RemoveByPattern(CAMPGROUNDS_PATTERN_KEY);
            //event notification
            _eventPublisher.EntityDeleted(campgroundReview);
        }

        /// <summary>
        /// Deletes Campground reviews
        /// </summary>
        /// <param name="CampgroundReviews">Campground reviews</param>
        public virtual void DeleteCampgroundReviews(IList<CampgroundReview> campgroundReviews)
        {
            if (campgroundReviews == null)
                throw new ArgumentNullException("CampgroundReviews");
            foreach (var campgroundReview in campgroundReviews)
                DeleteCampgroundReview(campgroundReview);

            _cacheManager.RemoveByPattern(CAMPGROUNDS_PATTERN_KEY);
            //event notification
            foreach (var campgroundReview in campgroundReviews)
            {
                _eventPublisher.EntityDeleted(campgroundReview);
            }
        }

        #endregion

        #region Campground Search

        public IList<Campground> FindByName(string name = null, bool exactMatch = false, bool isPublished = true)
        {
            var query = _campgroundRepository.Table;

            if (exactMatch)
                query = query.Where(c => c.Name == name);
            else
                foreach (var item in name.Split())
                    query = query.Where(c => c.Name.Contains(item));

            query = from c in query
                    join ca in _campgroundAddressRepository.Table on c.CampgroundAddressId equals ca.Id
                    where(c.Published == isPublished)
                    orderby c.Name, ca.StateProvince.Name
                    select c;

            return query.ToList();
        }

        public IPagedList<Campground> SearchCampgrounds(string keywords = null,
            int pageIndex = 0,
            int pageSize = int.MaxValue,
            IList<int> categoryIds = null,
            bool markedAsNewOnly = false,
            bool? featuredCampgrounds = null,
            int campgroundTagId = 0,
            bool searchDescriptions = false,
            bool searchCampgroundTags = false,
            int languageId = 0,
            int campgroundTypeId = 0,
            int campgroundHostId = 0,
            IList<int> filteredSpecs = null,
            CampgroundSortingEnum orderBy = CampgroundSortingEnum.Type,
            bool showHidden = false,
            bool? overridePublished = null,
            int maxRecords = 100)
        {

            var searchLocalizedValue = false;
            if (languageId > 0)
            {
                if (showHidden)
                {
                    searchLocalizedValue = true;
                }
                else
                {
                    //ensure that we have at least two published languages
                    var totalPublishedLanguages = _languageService.GetAllLanguages().Count;
                    searchLocalizedValue = totalPublishedLanguages >= 2;
                }
            }

            //validate "categoryIds" parameter
            if (categoryIds != null && categoryIds.Contains(0))
                categoryIds.Remove(0);

            //Access control list. Allowed customer roles
            var allowedCustomerRolesIds = _workContext.CurrentCustomer.GetCustomerRoleIds();

            //campgrounds
            var query = _campgroundRepository.Table;
            query = query.Where(c => !c.Deleted);
            if (!overridePublished.HasValue)
            {
                //process according to "showHidden"
                if (!showHidden)
                {
                    query = query.Where(c => c.Published);
                }
            }
            else if (overridePublished.Value)
            {
                //published only
                query = query.Where(c => c.Published);
            }
            else if (!overridePublished.Value)
            {
                //unpublished only
                query = query.Where(c => !c.Published);
            }

            //searching by keyword
            if (!string.IsNullOrWhiteSpace(keywords))
            {
                query = from c in query
                            //join lp in _localizedPropertyRepository.Table on c.Id equals lp.EntityId into p_lp
                            //from lp in p_lp.DefaultIfEmpty()
                        join ca in _campgroundAddressRepository.Table on c.CampgroundAddressId equals ca.Id
                        from ct in c.CampgroundTags.DefaultIfEmpty()
                        where(c.Name.Contains(keywords)) ||
                              (searchDescriptions && c.ShortDescription.Contains(keywords)) ||
                              (searchDescriptions && c.FullDescription.Contains(keywords)) ||
                              (ca.City.Contains(keywords)) ||
                              (ca.StateProvince.Name.Contains(keywords)) ||
                              (ca.ZipPostalCode.Contains(keywords)) ||
                              //campground tags (exact match)
                              (searchCampgroundTags && ct.Name == keywords)
                        //||
                        //localized values
                        //(searchLocalizedValue && lp.LanguageId == languageId && lp.LocaleKeyGroup == "Campground" &&
                        // lp.LocaleKey == "Name" && lp.LocaleValue.Contains(keywords)) ||
                        //(searchDescriptions && searchLocalizedValue && lp.LanguageId == languageId &&
                        // lp.LocaleKeyGroup == "Campground" && lp.LocaleKey == "ShortDescription" &&
                        // lp.LocaleValue.Contains(keywords)) ||
                        //(searchDescriptions && searchLocalizedValue && lp.LanguageId == languageId &&
                        // lp.LocaleKeyGroup == "Campground" && lp.LocaleKey == "FullDescription" &&
                        // lp.LocaleValue.Contains(keywords))
                        select c;
            }

            if (!showHidden && !_campgroundSettings.IgnoreAcl)
            {
                //ACL (access control list)
                query = from c in query
                        join acl in _aclRepository.Table
                            on new { c1 = c.Id, c2 = "Campground" } equals new { c1 = acl.EntityId, c2 = acl.EntityName } into p_acl
                        from acl in p_acl.DefaultIfEmpty()
                        where !c.SubjectToAcl || allowedCustomerRolesIds.Contains(acl.CustomerRoleId)
                        select c;
            }

            //category filtering
            if (categoryIds != null && categoryIds.Any())
            {
                query = from c in query
                        from cc in c.CampgroundCategories.Where(cc => categoryIds.Contains(cc.CategoryId))
                        where (!featuredCampgrounds.HasValue || featuredCampgrounds.Value == cc.IsFeaturedCampground)
                        select c;
            }

            //tag filtering
            if (campgroundTagId > 0)
            {
                query = from c in query
                        from ct in c.CampgroundTags.Where(ct => ct.Id == campgroundTagId)
                        select c;
            }

            if (campgroundTypeId > 0)
            {
                query = from c in query
                        from ct in c.CampgroundType.Where(ct => ct.Id == campgroundTypeId)
                        select c;
            }

            //if (campgroundHostId > 0)
            //{
            //    query = from c in query
            //            from ch in c.CCampgroundHost.Where(ch => ch.Id == campgroundHostId)
            //            select c;
            //}

            //only distinct campgrounds (group by ID)
            //if we use standard Distinct() method, then all fields will be compared (low performance)
            //it'll not work in SQL Server Compact when searching campgrounds by a keyword)
            query = from c in query
                    group c by c.Id
                    into cGroup
                    orderby cGroup.Key
                    select cGroup.FirstOrDefault();

            //sort campgrounds
            if (orderBy == CampgroundSortingEnum.Type && categoryIds != null && categoryIds.Any())
            {
                //category position
                var firstCategoryId = categoryIds[0];
                query = query.OrderBy(c =>
                    c.CampgroundCategories.FirstOrDefault(pc => pc.CategoryId == firstCategoryId).DisplayOrder);
            }
            else if (orderBy == CampgroundSortingEnum.Position)
            {
                //otherwise sort by name
                query = query.OrderBy(c => c.Name);
            }
            else if (orderBy == CampgroundSortingEnum.NameAsc)
            {
                //Name: A to Z
                query = query.OrderBy(c => c.Name);
            }
            else if (orderBy == CampgroundSortingEnum.NameDesc)
            {
                //Name: Z to A
                query = query.OrderByDescending(c => c.Name);
            }
            else if (orderBy == CampgroundSortingEnum.CreatedOn)
            {
                //creation date
                query = query.OrderByDescending(c => c.CreatedOnUtc);
            }
            else
            {
                //actually this code is not reachable
                query = query.OrderBy(c => c.Name);
            }

            var campgrounds = new PagedList<Campground>(query.Take(maxRecords), pageIndex, pageSize);

            //return campgrounds
            return campgrounds;
        }

        #endregion

        #region Related campgrounds

        /// <summary>
        /// Deletes a related campground
        /// </summary>
        /// <param name="relatedCampground">Related campground</param>
        public virtual void DeleteRelatedCampground(RelatedCampground relatedCampground)
        {
            if (relatedCampground == null)
                throw new ArgumentNullException(nameof(relatedCampground));

            _relatedCampgroundRepository.Delete(relatedCampground);

            //event notification
            _eventPublisher.EntityDeleted(relatedCampground);
        }

        /// <summary>
        /// Gets related campgrounds by campground identifier
        /// </summary>
        /// <param name="campgroundId1">The first campground identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Related campgrounds</returns>
        public virtual IList<RelatedCampground> GetRelatedCampgroundsByCampgroundId1(int campgroundId1, bool showHidden = false)
        {
            var query = from rp in _relatedCampgroundRepository.Table
                        join p in _campgroundRepository.Table on rp.CampgroundId2 equals p.Id
                        where rp.CampgroundId1 == campgroundId1 &&
                        !p.Deleted &&
                        (showHidden || p.Published)
                        orderby rp.DisplayOrder, rp.Id
                        select rp;
            var relatedCampgrounds = query.ToList();

            return relatedCampgrounds;
        }

        /// <summary>
        /// Gets a related campground
        /// </summary>
        /// <param name="relatedCampgroundId">Related campground identifier</param>
        /// <returns>Related campground</returns>
        public virtual RelatedCampground GetRelatedCampgroundById(int relatedCampgroundId)
        {
            if (relatedCampgroundId == 0)
                return null;

            return _relatedCampgroundRepository.GetById(relatedCampgroundId);
        }

        /// <summary>
        /// Inserts a related campground
        /// </summary>
        /// <param name="relatedCampground">Related campground</param>
        public virtual void InsertRelatedCampground(RelatedCampground relatedCampground)
        {
            if (relatedCampground == null)
                throw new ArgumentNullException(nameof(relatedCampground));

            _relatedCampgroundRepository.Insert(relatedCampground);

            //event notification
            _eventPublisher.EntityInserted(relatedCampground);
        }

        /// <summary>
        /// Updates a related campground
        /// </summary>
        /// <param name="relatedCampground">Related campground</param>
        public virtual void UpdateRelatedCampground(RelatedCampground relatedCampground)
        {
            if (relatedCampground == null)
                throw new ArgumentNullException(nameof(relatedCampground));

            _relatedCampgroundRepository.Update(relatedCampground);

            //event notification
            _eventPublisher.EntityUpdated(relatedCampground);
        }

        #endregion

        #region Cross-sell campgrounds

        /// <summary>
        /// Deletes a cross-sell campground
        /// </summary>
        /// <param name="crossSellCampground">Cross-sell identifier</param>
        public virtual void DeleteCrossSellCampground(CrossSellCampground crossSellCampground)
        {
            if (crossSellCampground == null)
                throw new ArgumentNullException(nameof(crossSellCampground));

            _crossSellCampgroundRepository.Delete(crossSellCampground);

            //event notification
            _eventPublisher.EntityDeleted(crossSellCampground);
        }

        /// <summary>
        /// Gets cross-sell campgrounds by campground identifier
        /// </summary>
        /// <param name="campgroundId1">The first campground identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Cross-sell campgrounds</returns>
        public virtual IList<CrossSellCampground> GetCrossSellCampgroundsByCampgroundId1(int campgroundId1, bool showHidden = false)
        {
            return GetCrossSellCampgroundsByCampgroundIds(new[] { campgroundId1 }, showHidden);
        }

        /// <summary>
        /// Gets cross-sell campgrounds by campground identifier
        /// </summary>
        /// <param name="campgroundIds">The first campground identifiers</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Cross-sell campgrounds</returns>
        public virtual IList<CrossSellCampground> GetCrossSellCampgroundsByCampgroundIds(int[] campgroundIds, bool showHidden = false)
        {
            if (campgroundIds == null || campgroundIds.Length == 0)
                return new List<CrossSellCampground>();

            var query = from csp in _crossSellCampgroundRepository.Table
                        join p in _campgroundRepository.Table on csp.CampgroundId2 equals p.Id
                        where campgroundIds.Contains(csp.CampgroundId1) &&
                              !p.Deleted &&
                              (showHidden || p.Published)
                        orderby csp.Id
                        select csp;
            var crossSellCampgrounds = query.ToList();
            return crossSellCampgrounds;
        }

        /// <summary>
        /// Gets a cross-sell campground
        /// </summary>
        /// <param name="crossSellCampgroundId">Cross-sell campground identifier</param>
        /// <returns>Cross-sell campground</returns>
        public virtual CrossSellCampground GetCrossSellCampgroundById(int crossSellCampgroundId)
        {
            if (crossSellCampgroundId == 0)
                return null;

            return _crossSellCampgroundRepository.GetById(crossSellCampgroundId);
        }

        /// <summary>
        /// Inserts a cross-sell campground
        /// </summary>
        /// <param name="crossSellCampground">Cross-sell campground</param>
        public virtual void InsertCrossSellCampground(CrossSellCampground crossSellCampground)
        {
            if (crossSellCampground == null)
                throw new ArgumentNullException(nameof(crossSellCampground));

            _crossSellCampgroundRepository.Insert(crossSellCampground);

            //event notification
            _eventPublisher.EntityInserted(crossSellCampground);
        }

        /// <summary>
        /// Updates a cross-sell campground
        /// </summary>
        /// <param name="crossSellCampground">Cross-sell campground</param>
        public virtual void UpdateCrossSellCampground(CrossSellCampground crossSellCampground)
        {
            if (crossSellCampground == null)
                throw new ArgumentNullException(nameof(crossSellCampground));

            _crossSellCampgroundRepository.Update(crossSellCampground);

            //event notification
            _eventPublisher.EntityUpdated(crossSellCampground);
        }


        #endregion


        #endregion
    }
    public static class CategoryExtensions
    {
        /// <summary>
        /// Find campground category
        /// </summary>
        /// <param name="source"></param>
        /// <param name="campgroundId"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public static CampgroundCategory FindCampgroundCategory(this IList<CampgroundCategory> source,
            int campgroundId, int categoryId)
        {
            foreach (var campgroundCategory in source)
                if (campgroundCategory.CampgroundId == campgroundId && campgroundCategory.CategoryId == categoryId)
                    return campgroundCategory;

            return null;
        }

    }
}