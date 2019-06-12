using System;
using System.Linq;
using Nop.Core;
using Nop.Core.Data;
using Nop.Plugin.Campgrounds.Data.Domain;
using Nop.Services.Events;

namespace Nop.Plugin.Campgrounds.Services
{
    /// <summary>
    /// CampgroundType service
    /// </summary>
    public partial class CampgroundTypeService : ICampgroundTypeService
    {
        #region Fields

        private readonly IRepository<CampgroundType> _campgroundTypeRepository;
        private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="campgroundTypeRepository">CampgroundType repository</param>
        /// <param name="campgroundTypeNoteRepository">CampgroundType note repository</param>
        /// <param name="eventPublisher">Event published</param>
        public CampgroundTypeService(IRepository<CampgroundType> campgroundTypeRepository,
            IEventPublisher eventPublisher)
        {
            this._campgroundTypeRepository = campgroundTypeRepository;
            this._eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods
        
        /// <summary>
        /// Gets a campgroundType by campgroundType identifier
        /// </summary>
        /// <param name="campgroundTypeId">CampgroundType identifier</param>
        /// <returns>CampgroundType</returns>
        public virtual CampgroundType GetCampgroundTypeById(int campgroundTypeId)
        {
            if (campgroundTypeId == 0)
                return null;

            return _campgroundTypeRepository.GetById(campgroundTypeId);
        }

        /// <summary>
        /// Delete a campgroundType
        /// </summary>
        /// <param name="campgroundType">CampgroundType</param>
        public virtual void DeleteCampgroundType(CampgroundType campgroundType)
        {
            if (campgroundType == null)
                throw new ArgumentNullException(nameof(campgroundType));

            _campgroundTypeRepository.Delete(campgroundType);

            //event notification
            _eventPublisher.EntityDeleted(campgroundType);
        }

        /// <summary>
        /// Gets all campgroundTypes
        /// </summary>
        /// <param name="name">CampgroundType name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>CampgroundTypes</returns>
        public virtual IPagedList<CampgroundType> GetAllCampgroundTypes(string description = "",
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var query = _campgroundTypeRepository.Table;
            if (!string.IsNullOrWhiteSpace(description))
                query = query.Where(v => v.Description.Contains(description));
            query = query.OrderBy(v => v.Description);

            var campgroundTypes = new PagedList<CampgroundType>(query, pageIndex, pageSize);
            return campgroundTypes;
        }

        /// <summary>
        /// Inserts a campgroundType
        /// </summary>
        /// <param name="campgroundType">CampgroundType</param>
        public virtual void InsertCampgroundType(CampgroundType campgroundType)
        {
            if (campgroundType == null)
                throw new ArgumentNullException(nameof(campgroundType));

            _campgroundTypeRepository.Insert(campgroundType);

            //event notification
            _eventPublisher.EntityInserted(campgroundType);
        }

        /// <summary>
        /// Updates the campgroundType
        /// </summary>
        /// <param name="campgroundType">CampgroundType</param>
        public virtual void UpdateCampgroundType(CampgroundType campgroundType)
        {
            if (campgroundType == null)
                throw new ArgumentNullException(nameof(campgroundType));

            _campgroundTypeRepository.Update(campgroundType);

            //event notification
            _eventPublisher.EntityUpdated(campgroundType);
        }
        #endregion
    }
}