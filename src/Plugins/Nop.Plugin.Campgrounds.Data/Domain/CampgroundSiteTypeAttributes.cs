﻿using Nop.Core;
using Nop.Core.Domain.Localization;

namespace Nop.Plugin.Campgrounds.Data.Domain
{
    /// <summary>
    /// Represents a location type attribute
    /// </summary>
    public partial class CampgroundSiteTypeAttributes : BaseEntity, ILocalizedEntity
    {
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        public string Description { get; set; }
    }
}
