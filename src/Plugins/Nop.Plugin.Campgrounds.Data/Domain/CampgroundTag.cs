using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Domain.Localization;

namespace Nop.Plugin.Campgrounds.Data.Domain
{
    /// <summary>
    /// Represents a Campground tag
    /// </summary>
    public partial class CampgroundTag : BaseEntity, ILocalizedEntity
    {
        private ICollection<Campground> _campgrounds;

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

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
