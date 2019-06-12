

using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Stores;
using Nop.Core.Extensions;
using Nop.Data;
using Nop.Plugin.Campgrounds.Data.Domain;
using Nop.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Plugin.Campgrounds.Services
{
    /// <summary>
    /// Campground tag service
    /// </summary>
    public partial class CampgroundTagService : ICampgroundTagService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : store ID
        /// </remarks>
        private const string CAMPGROUNDTAG_COUNT_KEY = "Nop.campgroundtag.count-{0}";

        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string CAMPGROUNDTAG_PATTERN_KEY = "Nop.campgroundtag.";

        #endregion

        #region Fields

        private readonly IRepository<CampgroundTag> _campgroundTagRepository;
        private readonly IRepository<StoreMapping> _storeMappingRepository;
        private readonly IDataProvider _dataProvider;
        private readonly IDbContext _dbContext;
        private readonly CommonSettings _commonSettings;
        private readonly CampgroundSettings _campgroundSettings;
        private readonly IStaticCacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICampgroundService _campgroundService;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="campgroundTagRepository">Campground tag repository</param>
        /// <param name="dataProvider">Data provider</param>
        /// <param name="dbContext">Database Context</param>
        /// <param name="commonSettings">Common settings</param>
        /// <param name="cacheManager">Static cache manager</param>
        /// <param name="eventPublisher">Event published</param>
        /// <param name="storeMappingRepository">Store mapping repository</param>
        /// <param name="catalogSettings">Catalog settings</param>
        /// <param name="campgroundService">Campground service</param>
        public CampgroundTagService(IRepository<CampgroundTag> campgroundTagRepository,
            IRepository<StoreMapping> storeMappingRepository,
            IDataProvider dataProvider,
            IDbContext dbContext,
            CommonSettings commonSettings,
            CampgroundSettings campgroundSettings,
            IStaticCacheManager cacheManager,
            IEventPublisher eventPublisher,
            ICampgroundService campgroundService)
        {
            this._campgroundTagRepository = campgroundTagRepository;
            this._storeMappingRepository = storeMappingRepository;
            this._dataProvider = dataProvider;
            this._dbContext = dbContext;
            this._commonSettings = commonSettings;
            this._campgroundSettings = campgroundSettings;
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._campgroundService = campgroundService;
        }

        #endregion

        #region Nested classes

        private class CampgroundTagWithCount
        {
            public int CampgroundTagId { get; set; }
            public int CampgroundCount { get; set; }
        }

        #endregion
        
        #region Utilities

        /// <summary>
        /// Get campground count for each of existing campground tag
        /// </summary>
        /// <param name="storeId">Store identifier</param>
        /// <returns>Dictionary of "campground tag ID : campground count"</returns>
        private Dictionary<int, int> GetCampgroundCount(int storeId)
        {
            var key = string.Format(CAMPGROUNDTAG_COUNT_KEY, storeId);
            return _cacheManager.Get(key, () =>
            {
                //stored procedures are enabled and supported by the database. 
                //It's much faster than the LINQ implementation below 
                if (_commonSettings.UseStoredProceduresIfSupported && _dataProvider.StoredProceduredSupported)
                {
                    //prepare parameters
                    var pStoreId = _dataProvider.GetInt32Parameter("StoreId", storeId);

                    //invoke stored procedure
                    var result = _dbContext.SqlQuery<CampgroundTagWithCount>("Exec CampgroundTagCountLoadAll @StoreId", pStoreId);

                    return result.ToDictionary(item => item.CampgroundTagId, item => item.CampgroundCount);
                }

                //stored procedures aren't supported. Use LINQ
                var query = _campgroundTagRepository.Table.Select(pt => new
                {
                    pt.Id,
                    CampgroundCount = (storeId == 0 || _campgroundSettings.IgnoreStoreLimitations) ?
                        pt.Campgrounds.Count(p => !p.Deleted && p.Published)
                        : (from p in pt.Campgrounds
                            join sm in _storeMappingRepository.Table
                                on new { p1 = p.Id, p2 = "Campground" } equals new { p1 = sm.EntityId, p2 = sm.EntityName } into p_sm
                            from sm in p_sm.DefaultIfEmpty()
                            where (!p.LimitedToStores || storeId == sm.StoreId) && !p.Deleted && p.Published
                            select p).Count()
                });
                var dictionary = new Dictionary<int, int>();
                foreach (var item in query)
                    dictionary.Add(item.Id, item.CampgroundCount);
                return dictionary;
            });
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete a campground tag
        /// </summary>
        /// <param name="campgroundTag">Campground tag</param>
        public virtual void DeleteCampgroundTag(CampgroundTag campgroundTag)
        {
            if (campgroundTag == null)
                throw new ArgumentNullException(nameof(campgroundTag));

            _campgroundTagRepository.Delete(campgroundTag);

            //cache
            _cacheManager.RemoveByPattern(CAMPGROUNDTAG_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(campgroundTag);
        }

        /// <summary>
        /// Gets all campground tags
        /// </summary>
        /// <returns>Campground tags</returns>
        public virtual IList<CampgroundTag> GetAllCampgroundTags()
        {
            var query = _campgroundTagRepository.Table;
            var campgroundTags = query.ToList();
            return campgroundTags;
        }

        /// <summary>
        /// Gets campground tag
        /// </summary>
        /// <param name="campgroundTagId">Campground tag identifier</param>
        /// <returns>Campground tag</returns>
        public virtual CampgroundTag GetCampgroundTagById(int campgroundTagId)
        {
            if (campgroundTagId == 0)
                return null;

            return _campgroundTagRepository.GetById(campgroundTagId);
        }

        /// <summary>
        /// Gets campground tag by name
        /// </summary>
        /// <param name="name">Campground tag name</param>
        /// <returns>Campground tag</returns>
        public virtual CampgroundTag GetCampgroundTagByName(string name)
        {
            var query = from pt in _campgroundTagRepository.Table
                        where pt.Name == name
                        select pt;

            var campgroundTag = query.FirstOrDefault();
            return campgroundTag;
        }

        /// <summary>
        /// Inserts a campground tag
        /// </summary>
        /// <param name="campgroundTag">Campground tag</param>
        public virtual void InsertCampgroundTag(CampgroundTag campgroundTag)
        {
            if (campgroundTag == null)
                throw new ArgumentNullException(nameof(campgroundTag));

            _campgroundTagRepository.Insert(campgroundTag);

            //cache
            _cacheManager.RemoveByPattern(CAMPGROUNDTAG_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(campgroundTag);
        }

        /// <summary>
        /// Updates the campground tag
        /// </summary>
        /// <param name="campgroundTag">Campground tag</param>
        public virtual void UpdateCampgroundTag(CampgroundTag campgroundTag)
        {
            if (campgroundTag == null)
                throw new ArgumentNullException(nameof(campgroundTag));

            _campgroundTagRepository.Update(campgroundTag);

            //cache
            _cacheManager.RemoveByPattern(CAMPGROUNDTAG_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(campgroundTag);
        }

        /// <summary>
        /// Get number of campgrounds
        /// </summary>
        /// <param name="campgroundTagId">Campground tag identifier</param>
        /// <param name="storeId">Store identifier</param>
        /// <returns>Number of campgrounds</returns>
        public virtual int GetCampgroundCount(int campgroundTagId, int storeId)
        {
            var dictionary = GetCampgroundCount(storeId);
            if (dictionary.ContainsKey(campgroundTagId))
                return dictionary[campgroundTagId];
            
            return 0;
        }

        /// <summary>
        /// Update campground tags
        /// </summary>
        /// <param name="campground">Campground for update</param>
        /// <param name="campgroundTags">Campground tags</param>
        public virtual void UpdateCampgroundTags(Campground campground, string[] campgroundTags)
        {
            if (campground == null)
                throw new ArgumentNullException(nameof(campground));

            //campground tags
            var existingCampgroundTags = campground.CampgroundTags.ToList();
            var campgroundTagsToRemove = new List<CampgroundTag>();
            foreach (var existingCampgroundTag in existingCampgroundTags)
            {
                var found = false;
                foreach (var newCampgroundTag in campgroundTags)
                {
                    if (existingCampgroundTag.Name.Equals(newCampgroundTag, StringComparison.InvariantCultureIgnoreCase))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    campgroundTagsToRemove.Add(existingCampgroundTag);
                }
            }
            foreach (var campgroundTag in campgroundTagsToRemove)
            {
                campground.CampgroundTags.Remove(campgroundTag);
                _campgroundService.UpdateCampground(campground);
            }
            foreach (var campgroundTagName in campgroundTags)
            {
                CampgroundTag campgroundTag;
                var campgroundTag2 = GetCampgroundTagByName(campgroundTagName);
                if (campgroundTag2 == null)
                {
                    //add new campground tag
                    campgroundTag = new CampgroundTag
                    {
                        Name = campgroundTagName
                    };
                    InsertCampgroundTag(campgroundTag);
                }
                else
                {
                    campgroundTag = campgroundTag2;
                }
                if (!campground.CampgroundTagExists(campgroundTag.Id))
                {
                    campground.CampgroundTags.Add(campgroundTag);
                    _campgroundService.UpdateCampground(campground);
                }
            }
        }

        #endregion
    }
}
