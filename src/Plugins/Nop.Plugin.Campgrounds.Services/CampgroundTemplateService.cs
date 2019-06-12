using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core.Data;
using Nop.Services.Events;
using Nop.Plugin.Campgrounds.Data.Domain;

namespace Nop.Plugin.Campgrounds.Services
{
    /// <summary>
    /// Campground template service
    /// </summary>
    public partial class CampgroundTemplateService : ICampgroundTemplateService
    {
        #region Fields

        private readonly IRepository<CampgroundTemplate> _CampgroundTemplateRepository;
        private readonly IEventPublisher _eventPublisher;

        #endregion
        
        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="CampgroundTemplateRepository">Campground template repository</param>
        /// <param name="eventPublisher">Event published</param>
        public CampgroundTemplateService(IRepository<CampgroundTemplate> CampgroundTemplateRepository,
            IEventPublisher eventPublisher)
        {
            this._CampgroundTemplateRepository = CampgroundTemplateRepository;
            this._eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete Campground template
        /// </summary>
        /// <param name="CampgroundTemplate">Campground template</param>
        public virtual void DeleteCampgroundTemplate(CampgroundTemplate CampgroundTemplate)
        {
            if (CampgroundTemplate == null)
                throw new ArgumentNullException("CampgroundTemplate");

            _CampgroundTemplateRepository.Delete(CampgroundTemplate);

            //event notification
            _eventPublisher.EntityDeleted(CampgroundTemplate);
        }

        /// <summary>
        /// Gets all Campground templates
        /// </summary>
        /// <returns>Campground templates</returns>
        public virtual IList<CampgroundTemplate> GetAllCampgroundTemplates()
        {
            var query = from pt in _CampgroundTemplateRepository.Table
                        orderby pt.DisplayOrder, pt.Id
                        select pt;

            var templates = query.ToList();
            return templates;
        }

        /// <summary>
        /// Gets a Campground template
        /// </summary>
        /// <param name="CampgroundTemplateId">Campground template identifier</param>
        /// <returns>Campground template</returns>
        public virtual CampgroundTemplate GetCampgroundTemplateById(int CampgroundTemplateId)
        {
            if (CampgroundTemplateId == 0)
                return null;

            return _CampgroundTemplateRepository.GetById(CampgroundTemplateId);
        }

        /// <summary>
        /// Inserts Campground template
        /// </summary>
        /// <param name="CampgroundTemplate">Campground template</param>
        public virtual void InsertCampgroundTemplate(CampgroundTemplate CampgroundTemplate)
        {
            if (CampgroundTemplate == null)
                throw new ArgumentNullException("CampgroundTemplate");

            _CampgroundTemplateRepository.Insert(CampgroundTemplate);

            //event notification
            _eventPublisher.EntityInserted(CampgroundTemplate);
        }

        /// <summary>
        /// Updates the Campground template
        /// </summary>
        /// <param name="CampgroundTemplate">Campground template</param>
        public virtual void UpdateCampgroundTemplate(CampgroundTemplate CampgroundTemplate)
        {
            if (CampgroundTemplate == null)
                throw new ArgumentNullException("CampgroundTemplate");

            _CampgroundTemplateRepository.Update(CampgroundTemplate);

            //event notification
            _eventPublisher.EntityUpdated(CampgroundTemplate);
        }
        
        #endregion
    }
}
