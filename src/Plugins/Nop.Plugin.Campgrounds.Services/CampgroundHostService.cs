using System;
using System.Linq;
using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.Customers;
using Nop.Plugin.Campgrounds.Data.Domain;
using Nop.Services.Events;

namespace Nop.Plugin.Campgrounds.Services
{
    /// <summary>
    /// CampgroundHost service
    /// </summary>
    public partial class CampgroundHostService : ICampgroundHostService
    {
        #region Fields

        private readonly IRepository<CampgroundHost> _campgroundHostRepository;
        private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="campgroundHostRepository">CampgroundHost repository</param>
        /// <param name="campgroundHostNoteRepository">CampgroundHost note repository</param>
        /// <param name="eventPublisher">Event published</param>
        public CampgroundHostService(IRepository<CampgroundHost> campgroundHostRepository,
            IEventPublisher eventPublisher)
        {
            this._campgroundHostRepository = campgroundHostRepository;
            this._eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Gets a campgroundHost by campgroundHost identifier
        /// </summary>
        /// <param name="campgroundHostId">CampgroundHost identifier</param>
        /// <returns>CampgroundHost</returns>
        public virtual CampgroundHost GetCampgroundHostById(int campgroundHostId)
        {
            if (campgroundHostId == 0)
                return null;

            return _campgroundHostRepository.GetById(campgroundHostId);
        }

        /// <summary>
        /// Gets a campgroundHost by customer identifier
        /// </summary>
        /// <param name="customerId">Customer identifier</param>
        /// <returns>CampgroundHost</returns>
        public virtual CampgroundHost GetCampgroundHostByCustomerId(int customerId)
        {
            if (customerId == 0)
                return null;

            var query = from ch in _campgroundHostRepository.Table
                        where ch.Customer.Id == customerId
                        select ch;

            return query.FirstOrDefault();
        }

        /// <summary>
        /// Gets a campgroundHost by campgroundHost identifier
        /// </summary>
        /// <param name="campgroundHostId">CampgroundHost identifier</param>
        /// <returns>CampgroundHost</returns>
        public virtual CampgroundHost GetCampgroundHostByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return null;

            var query = from ch in _campgroundHostRepository.Table
                        where ch.Customer.Email == email
                        select ch;

            return query.FirstOrDefault();
        }

        /// <summary>
        /// Delete a campgroundHost
        /// </summary>
        /// <param name="campgroundHost">CampgroundHost</param>
        public virtual void DeleteCampgroundHost(CampgroundHost campgroundHost)
        {
            if (campgroundHost == null)
                throw new ArgumentNullException(nameof(campgroundHost));

            campgroundHost.Deleted = true;
            UpdateCampgroundHost(campgroundHost);

            //event notification
            _eventPublisher.EntityDeleted(campgroundHost);
        }

        /// <summary>
        /// Gets all campgroundHosts
        /// </summary>
        /// <param name="name">CampgroundHost name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>CampgroundHosts</returns>
        public virtual IPagedList<CampgroundHost> GetAllCampgroundHosts(string email = "",
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var query = _campgroundHostRepository.Table;
            if (!string.IsNullOrWhiteSpace(email))
                query = query.Where(v => v.Customer.Email.Contains(email));
            if (!showHidden)
                query = query.Where(v => v.Active);
            query = query.Where(v => !v.Deleted);
            query = query.OrderBy(v => v.DisplayOrder).ThenBy(v => v.Customer.Email);

            var campgroundHosts = new PagedList<CampgroundHost>(query, pageIndex, pageSize);
            return campgroundHosts;
        }

        /// <summary>
        /// Inserts a campgroundHost
        /// </summary>
        /// <param name="campgroundHost">CampgroundHost</param>
        public virtual void InsertCampgroundHost(CampgroundHost campgroundHost)
        {
            if (campgroundHost == null)
                throw new ArgumentNullException(nameof(campgroundHost));

            _campgroundHostRepository.Insert(campgroundHost);

            //event notification
            _eventPublisher.EntityInserted(campgroundHost);
        }

        /// <summary>
        /// Updates the campgroundHost
        /// </summary>
        /// <param name="campgroundHost">CampgroundHost</param>
        public virtual void UpdateCampgroundHost(CampgroundHost campgroundHost)
        {
            if (campgroundHost == null)
                throw new ArgumentNullException(nameof(campgroundHost));

            _campgroundHostRepository.Update(campgroundHost);

            //event notification
            _eventPublisher.EntityUpdated(campgroundHost);
        }
        #endregion
    }
}