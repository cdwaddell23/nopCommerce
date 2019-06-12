using Nop.Core;
using Nop.Plugin.Campgrounds.Data.Domain;

namespace Nop.Plugin.Widgets.Campgrounds
{
    public interface ICampgroundWorkContext : IWorkContext
    {
        /// <summary>
        /// Gets the current vendor (logged-in manager)
        /// </summary>
        CampgroundHost CurrentCampgroundHost { get; }

    }
}