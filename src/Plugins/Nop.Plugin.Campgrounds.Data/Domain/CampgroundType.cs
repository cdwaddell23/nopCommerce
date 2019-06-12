using Nop.Core;
using Nop.Core.Domain.Localization;
using System.Collections.Generic;

namespace Nop.Plugin.Campgrounds.Data.Domain
{
    /// <summary>
    /// Represents a Campground Type
    /// </summary>
    public partial class CampgroundType : BaseEntity, ILocalizedEntity
    {
        private ICollection<Campground> _campgrounds;

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Campgrounds
        /// </summary>
        public virtual ICollection<Campground> Campgrounds
        {
            get { return _campgrounds ?? (_campgrounds = new List<Campground>()); }
            protected set { _campgrounds = value; }
        }
    }
}