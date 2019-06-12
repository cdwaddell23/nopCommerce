using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Domain.Catalog;
using Nop.Plugin.Campgrounds.Data.Domain;
using Nop.Services.Events;

namespace Nop.Plugin.Campgrounds.Services
{
    /// <summary>
    /// Campground attribute service
    /// </summary>
    public partial class CampgroundAttributeTypeService : ICampgroundAttributeTypeService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : page index
        /// {1} : page size
        /// </remarks>
        private const string CAMPGROUNDATTRIBUTES_ALL_KEY = "Nop.campgroundattribute.all-{0}-{1}";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : campground attribute ID
        /// </remarks>
        private const string CAMPGROUNDATTRIBUTES_BY_ID_KEY = "Nop.campgroundattribute.id-{0}";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : campground ID
        /// </remarks>
        private const string CAMPGROUNDATTRIBUTEMAPPINGS_ALL_KEY = "Nop.campgroundattributemapping.all-{0}";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : campground attribute mapping ID
        /// </remarks>
        private const string CAMPGROUNDATTRIBUTEMAPPINGS_BY_ID_KEY = "Nop.campgroundattributemapping.id-{0}";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : campground attribute mapping ID
        /// </remarks>
        private const string CAMPGROUNDATTRIBUTEVALUES_ALL_KEY = "Nop.campgroundattributevalue.all-{0}";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : campground attribute value ID
        /// </remarks>
        private const string CAMPGROUNDATTRIBUTEVALUES_BY_ID_KEY = "Nop.campgroundattributevalue.id-{0}";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : campground ID
        /// </remarks>
        private const string CAMPGROUNDATTRIBUTECOMBINATIONS_ALL_KEY = "Nop.campgroundattributecombination.all-{0}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string CAMPGROUNDATTRIBUTES_PATTERN_KEY = "Nop.campgroundattribute.";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string CAMPGROUNDATTRIBUTEMAPPINGS_PATTERN_KEY = "Nop.campgroundattributemapping.";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string CAMPGROUNDATTRIBUTEVALUES_PATTERN_KEY = "Nop.campgroundattributevalue.";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string CAMPGROUNDATTRIBUTECOMBINATIONS_PATTERN_KEY = "Nop.campgroundattributecombination.";

        #endregion

        #region Fields

        private readonly IRepository<CampgroundAttributeType> _campgroundAttributeTypeRepository;
        private readonly IRepository<CampgroundAttributeMapping> _campgroundAttributeTypeMappingRepository;
        private readonly IRepository<CampgroundAttributeCombination> _campgroundAttributeTypeCombinationRepository;
        private readonly IRepository<CampgroundAttributeValue> _campgroundAttributeTypeValueRepository;
        private readonly IRepository<PredefinedCampgroundAttributeValue> _predefinedCampgroundAttributeValueRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICacheManager _cacheManager;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="campgroundAttributeTypeRepository">Campground attribute repository</param>
        /// <param name="campgroundAttributeTypeMappingRepository">Campground attribute mapping repository</param>
        /// <param name="campgroundAttributeTypeCombinationRepository">Campground attribute combination repository</param>
        /// <param name="campgroundAttributeTypeValueRepository">Campground attribute value repository</param>
        /// <param name="predefinedCampgroundAttributeValueRepository">Predefined campground attribute value repository</param>
        /// <param name="eventPublisher">Event published</param>
        public CampgroundAttributeTypeService(ICacheManager cacheManager,
            IRepository<CampgroundAttributeType> campgroundAttributeTypeRepository,
            IRepository<CampgroundAttributeMapping> campgroundAttributeTypeMappingRepository,
            IRepository<CampgroundAttributeCombination> campgroundAttributeTypeCombinationRepository,
            IRepository<CampgroundAttributeValue> campgroundAttributeTypeValueRepository,
            IRepository<PredefinedCampgroundAttributeValue> predefinedCampgroundAttributeValueRepository,
            IEventPublisher eventPublisher)
        {
            this._cacheManager = cacheManager;
            this._campgroundAttributeTypeRepository = campgroundAttributeTypeRepository;
            this._campgroundAttributeTypeMappingRepository = campgroundAttributeTypeMappingRepository;
            this._campgroundAttributeTypeCombinationRepository = campgroundAttributeTypeCombinationRepository;
            this._campgroundAttributeTypeValueRepository = campgroundAttributeTypeValueRepository;
            this._predefinedCampgroundAttributeValueRepository = predefinedCampgroundAttributeValueRepository;
            this._eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        #region Campground attributes

        /// <summary>
        /// Deletes a campground attribute
        /// </summary>
        /// <param name="campgroundAttributeType">Campground attribute</param>
        public virtual void DeleteCampgroundAttributeType(CampgroundAttributeType campgroundAttributeType)
        {
            if (campgroundAttributeType == null)
                throw new ArgumentNullException(nameof(campgroundAttributeType));

            _campgroundAttributeTypeRepository.Delete(campgroundAttributeType);

            //cache
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTEMAPPINGS_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTEVALUES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTECOMBINATIONS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(campgroundAttributeType);
        }

        /// <summary>
        /// Gets all campground attributes
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Campground attributes</returns>
        public virtual IPagedList<CampgroundAttributeType> GetAllCampgroundAttributeTypes(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var key = string.Format(CAMPGROUNDATTRIBUTES_ALL_KEY, pageIndex, pageSize);
            return _cacheManager.Get(key, () =>
            {
                var query = from pa in _campgroundAttributeTypeRepository.Table
                            orderby pa.Name
                            select pa;
                var campgroundAttributeTypes = new PagedList<CampgroundAttributeType>(query, pageIndex, pageSize);
                return campgroundAttributeTypes;
            });
        }

        /// <summary>
        /// Gets a campground attribute 
        /// </summary>
        /// <param name="campgroundAttributeTypeId">Campground attribute identifier</param>
        /// <returns>Campground attribute </returns>
        public virtual CampgroundAttributeType GetCampgroundAttributeTypeById(int campgroundAttributeTypeId)
        {
            if (campgroundAttributeTypeId == 0)
                return null;

            var key = string.Format(CAMPGROUNDATTRIBUTES_BY_ID_KEY, campgroundAttributeTypeId);
            return _cacheManager.Get(key, () => _campgroundAttributeTypeRepository.GetById(campgroundAttributeTypeId));
        }

        /// <summary>
        /// Inserts a campground attribute
        /// </summary>
        /// <param name="campgroundAttributeType">Campground attribute</param>
        public virtual void InsertCampgroundAttributeType(CampgroundAttributeType campgroundAttributeType)
        {
            if (campgroundAttributeType == null)
                throw new ArgumentNullException(nameof(campgroundAttributeType));

            _campgroundAttributeTypeRepository.Insert(campgroundAttributeType);

            //cache
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTEMAPPINGS_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTEVALUES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTECOMBINATIONS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(campgroundAttributeType);
        }

        /// <summary>
        /// Updates the campground attribute
        /// </summary>
        /// <param name="campgroundAttributeType">Campground attribute</param>
        public virtual void UpdateCampgroundAttributeType(CampgroundAttributeType campgroundAttributeType)
        {
            if (campgroundAttributeType == null)
                throw new ArgumentNullException(nameof(campgroundAttributeType));

            _campgroundAttributeTypeRepository.Update(campgroundAttributeType);

            //cache
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTEMAPPINGS_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTEVALUES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTECOMBINATIONS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(campgroundAttributeType);
        }

        /// <summary>
        /// Returns a list of IDs of not existing attributes
        /// </summary>
        /// <param name="attributeId">The IDs of the attributes to check</param>
        /// <returns>List of IDs not existing attributes</returns>
        public virtual int[] GetNotExistingAttributes(int[] attributeId)
        {
            if (attributeId == null)
                throw new ArgumentNullException(nameof(attributeId));

            var query = _campgroundAttributeTypeRepository.Table;
            var queryFilter = attributeId.Distinct().ToArray();
            var filter = query.Select(a => a.Id).Where(m => queryFilter.Contains(m)).ToList();
            return queryFilter.Except(filter).ToArray();
        }

        #endregion

        #region Campground attributes mappings

        /// <summary>
        /// Deletes a campground attribute mapping
        /// </summary>
        /// <param name="campgroundAttributeTypeMapping">Campground attribute mapping</param>
        public virtual void DeleteCampgroundAttributeMapping(CampgroundAttributeMapping campgroundAttributeTypeMapping)
        {
            if (campgroundAttributeTypeMapping == null)
                throw new ArgumentNullException(nameof(campgroundAttributeTypeMapping));

            _campgroundAttributeTypeMappingRepository.Delete(campgroundAttributeTypeMapping);

            //cache
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTEMAPPINGS_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTEVALUES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTECOMBINATIONS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(campgroundAttributeTypeMapping);
        }

        /// <summary>
        /// Gets campground attribute mappings by campground identifier
        /// </summary>
        /// <param name="campgroundId">The campground identifier</param>
        /// <returns>Campground attribute mapping collection</returns>
        public virtual IList<CampgroundAttributeMapping> GetCampgroundAttributeMappingsByCampgroundId(int campgroundId)
        {
            var key = string.Format(CAMPGROUNDATTRIBUTEMAPPINGS_ALL_KEY, campgroundId);

            return _cacheManager.Get(key, () =>
            {
                var query = from pam in _campgroundAttributeTypeMappingRepository.Table
                            orderby pam.DisplayOrder, pam.Id
                            where pam.CampgroundId == campgroundId
                            select pam;
                var campgroundAttributeTypeMappings = query.ToList();
                return campgroundAttributeTypeMappings;
            });
        }

        /// <summary>
        /// Gets a campground attribute mapping
        /// </summary>
        /// <param name="campgroundAttributeTypeMappingId">Campground attribute mapping identifier</param>
        /// <returns>Campground attribute mapping</returns>
        public virtual CampgroundAttributeMapping GetCampgroundAttributeMappingById(int campgroundAttributeTypeMappingId)
        {
            if (campgroundAttributeTypeMappingId == 0)
                return null;

            var key = string.Format(CAMPGROUNDATTRIBUTEMAPPINGS_BY_ID_KEY, campgroundAttributeTypeMappingId);
            return _cacheManager.Get(key, () => _campgroundAttributeTypeMappingRepository.GetById(campgroundAttributeTypeMappingId));
        }

        /// <summary>
        /// Inserts a campground attribute mapping
        /// </summary>
        /// <param name="campgroundAttributeTypeMapping">The campground attribute mapping</param>
        public virtual void InsertCampgroundAttributeMapping(CampgroundAttributeMapping campgroundAttributeTypeMapping)
        {
            if (campgroundAttributeTypeMapping == null)
                throw new ArgumentNullException(nameof(campgroundAttributeTypeMapping));

            _campgroundAttributeTypeMappingRepository.Insert(campgroundAttributeTypeMapping);

            //cache
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTEMAPPINGS_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTEVALUES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTECOMBINATIONS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(campgroundAttributeTypeMapping);
        }

        /// <summary>
        /// Updates the campground attribute mapping
        /// </summary>
        /// <param name="campgroundAttributeTypeMapping">The campground attribute mapping</param>
        public virtual void UpdateCampgroundAttributeMapping(CampgroundAttributeMapping campgroundAttributeTypeMapping)
        {
            if (campgroundAttributeTypeMapping == null)
                throw new ArgumentNullException(nameof(campgroundAttributeTypeMapping));

            _campgroundAttributeTypeMappingRepository.Update(campgroundAttributeTypeMapping);

            //cache
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTEMAPPINGS_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTEVALUES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTECOMBINATIONS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(campgroundAttributeTypeMapping);
        }

        #endregion

        #region Campground attribute values

        /// <summary>
        /// Deletes a campground attribute value
        /// </summary>
        /// <param name="campgroundAttributeTypeValue">Campground attribute value</param>
        public virtual void DeleteCampgroundAttributeValue(CampgroundAttributeValue campgroundAttributeTypeValue)
        {
            if (campgroundAttributeTypeValue == null)
                throw new ArgumentNullException(nameof(campgroundAttributeTypeValue));

            _campgroundAttributeTypeValueRepository.Delete(campgroundAttributeTypeValue);

            //cache
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTEMAPPINGS_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTEVALUES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTECOMBINATIONS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(campgroundAttributeTypeValue);
        }

        /// <summary>
        /// Gets campground attribute values by campground attribute mapping identifier
        /// </summary>
        /// <param name="campgroundAttributeTypeMappingId">The campground attribute mapping identifier</param>
        /// <returns>Campground attribute mapping collection</returns>
        public virtual IList<CampgroundAttributeValue> GetCampgroundAttributeValues(int campgroundAttributeTypeMappingId)
        {
            var key = string.Format(CAMPGROUNDATTRIBUTEVALUES_ALL_KEY, campgroundAttributeTypeMappingId);
            return _cacheManager.Get(key, () =>
            {
                var query = from pav in _campgroundAttributeTypeValueRepository.Table
                            orderby pav.DisplayOrder, pav.Id
                            where pav.CampgroundAttributeMappingId == campgroundAttributeTypeMappingId
                            select pav;
                var campgroundAttributeTypeValues = query.ToList();
                return campgroundAttributeTypeValues;
            });
        }

        /// <summary>
        /// Gets a campground attribute value
        /// </summary>
        /// <param name="campgroundAttributeTypeValueId">Campground attribute value identifier</param>
        /// <returns>Campground attribute value</returns>
        public virtual CampgroundAttributeValue GetCampgroundAttributeValueById(int campgroundAttributeTypeValueId)
        {
            if (campgroundAttributeTypeValueId == 0)
                return null;
            
           var key = string.Format(CAMPGROUNDATTRIBUTEVALUES_BY_ID_KEY, campgroundAttributeTypeValueId);
           return _cacheManager.Get(key, () => _campgroundAttributeTypeValueRepository.GetById(campgroundAttributeTypeValueId));
        }

        /// <summary>
        /// Inserts a campground attribute value
        /// </summary>
        /// <param name="campgroundAttributeTypeValue">The campground attribute value</param>
        public virtual void InsertCampgroundAttributeValue(CampgroundAttributeValue campgroundAttributeTypeValue)
        {
            if (campgroundAttributeTypeValue == null)
                throw new ArgumentNullException(nameof(campgroundAttributeTypeValue));

            _campgroundAttributeTypeValueRepository.Insert(campgroundAttributeTypeValue);

            //cache
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTEMAPPINGS_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTEVALUES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTECOMBINATIONS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(campgroundAttributeTypeValue);
        }

        /// <summary>
        /// Updates the campground attribute value
        /// </summary>
        /// <param name="campgroundAttributeTypeValue">The campground attribute value</param>
        public virtual void UpdateCampgroundAttributeValue(CampgroundAttributeValue campgroundAttributeTypeValue)
        {
            if (campgroundAttributeTypeValue == null)
                throw new ArgumentNullException(nameof(campgroundAttributeTypeValue));

            _campgroundAttributeTypeValueRepository.Update(campgroundAttributeTypeValue);

            //cache
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTEMAPPINGS_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTEVALUES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTECOMBINATIONS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(campgroundAttributeTypeValue);
        }

        #endregion

        #region Predefined campground attribute values

        /// <summary>
        /// Deletes a predefined campground attribute value
        /// </summary>
        /// <param name="ppav">Predefined campground attribute value</param>
        public virtual void DeletePredefinedCampgroundAttributeValue(PredefinedCampgroundAttributeValue ppav)
        {
            if (ppav == null)
                throw new ArgumentNullException(nameof(ppav));

            _predefinedCampgroundAttributeValueRepository.Delete(ppav);

            //cache
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTEMAPPINGS_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTEVALUES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTECOMBINATIONS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(ppav);
        }

        /// <summary>
        /// Gets predefined campground attribute values by campground attribute identifier
        /// </summary>
        /// <param name="campgroundAttributeTypeId">The campground attribute identifier</param>
        /// <returns>Campground attribute mapping collection</returns>
        public virtual IList<PredefinedCampgroundAttributeValue> GetPredefinedCampgroundAttributeValues(int campgroundAttributeTypeId)
        {
            var query = from ppav in _predefinedCampgroundAttributeValueRepository.Table
                        orderby ppav.DisplayOrder, ppav.Id
                        where ppav.CampgroundAttributeTypeId == campgroundAttributeTypeId
                        select ppav;
            var values = query.ToList();
            return values;
        }

        /// <summary>
        /// Gets a predefined campground attribute value
        /// </summary>
        /// <param name="id">Predefined campground attribute value identifier</param>
        /// <returns>Predefined campground attribute value</returns>
        public virtual PredefinedCampgroundAttributeValue GetPredefinedCampgroundAttributeValueById(int id)
        {
            if (id == 0)
                return null;

            return _predefinedCampgroundAttributeValueRepository.GetById(id);
        }

        /// <summary>
        /// Inserts a predefined campground attribute value
        /// </summary>
        /// <param name="ppav">The predefined campground attribute value</param>
        public virtual void InsertPredefinedCampgroundAttributeValue(PredefinedCampgroundAttributeValue ppav)
        {
            if (ppav == null)
                throw new ArgumentNullException(nameof(ppav));

            _predefinedCampgroundAttributeValueRepository.Insert(ppav);

            //cache
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTEMAPPINGS_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTEVALUES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTECOMBINATIONS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(ppav);
        }

        /// <summary>
        /// Updates the predefined campground attribute value
        /// </summary>
        /// <param name="ppav">The predefined campground attribute value</param>
        public virtual void UpdatePredefinedCampgroundAttributeValue(PredefinedCampgroundAttributeValue ppav)
        {
            if (ppav == null)
                throw new ArgumentNullException(nameof(ppav));

            _predefinedCampgroundAttributeValueRepository.Update(ppav);

            //cache
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTEMAPPINGS_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTEVALUES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTECOMBINATIONS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(ppav);
        }

        #endregion

        #region Campground attribute combinations

        /// <summary>
        /// Deletes a campground attribute combination
        /// </summary>
        /// <param name="combination">Campground attribute combination</param>
        public virtual void DeleteCampgroundAttributeTypeCombination(CampgroundAttributeCombination combination)
        {
            if (combination == null)
                throw new ArgumentNullException(nameof(combination));

            _campgroundAttributeTypeCombinationRepository.Delete(combination);

            //cache
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTEMAPPINGS_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTEVALUES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTECOMBINATIONS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(combination);
        }

        /// <summary>
        /// Gets all campground attribute combinations
        /// </summary>
        /// <param name="campgroundId">Campground identifier</param>
        /// <returns>Campground attribute combinations</returns>
        public virtual IList<CampgroundAttributeCombination> GetAllCampgroundAttributeTypeCombinations(int campgroundId)
        {
            if (campgroundId == 0)
                return new List<CampgroundAttributeCombination>();

            var key = string.Format(CAMPGROUNDATTRIBUTECOMBINATIONS_ALL_KEY, campgroundId);

            return _cacheManager.Get(key, () =>
            {
                var query = from c in _campgroundAttributeTypeCombinationRepository.Table
                            orderby c.Id
                            where c.CampgroundId == campgroundId
                            select c;
                var combinations = query.ToList();
                return combinations;
            });
        }

        /// <summary>
        /// Gets a campground attribute combination
        /// </summary>
        /// <param name="campgroundAttributeTypeCombinationId">Campground attribute combination identifier</param>
        /// <returns>Campground attribute combination</returns>
        public virtual CampgroundAttributeCombination GetCampgroundAttributeTypeCombinationById(int campgroundAttributeTypeCombinationId)
        {
            if (campgroundAttributeTypeCombinationId == 0)
                return null;
            
            return _campgroundAttributeTypeCombinationRepository.GetById(campgroundAttributeTypeCombinationId);
        }

        /// <summary>
        /// Inserts a campground attribute combination
        /// </summary>
        /// <param name="combination">Campground attribute combination</param>
        public virtual void InsertCampgroundAttributeTypeCombination(CampgroundAttributeCombination combination)
        {
            if (combination == null)
                throw new ArgumentNullException(nameof(combination));

            _campgroundAttributeTypeCombinationRepository.Insert(combination);

            //cache
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTEMAPPINGS_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTEVALUES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTECOMBINATIONS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(combination);
        }

        /// <summary>
        /// Updates a campground attribute combination
        /// </summary>
        /// <param name="combination">Campground attribute combination</param>
        public virtual void UpdateCampgroundAttributeTypeCombination(CampgroundAttributeCombination combination)
        {
            if (combination == null)
                throw new ArgumentNullException(nameof(combination));

            _campgroundAttributeTypeCombinationRepository.Update(combination);

            //cache
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTEMAPPINGS_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTEVALUES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDATTRIBUTECOMBINATIONS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(combination);
        }

        #endregion

        #endregion
    }
}
