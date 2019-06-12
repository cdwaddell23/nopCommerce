using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Mvc.Models;

namespace Nop.Plugin.Widgets.Campgrounds.Models
{
    public partial class CopyCampgroundModel : BaseNopEntityModel
    {

        [NopResourceDisplayName("Admin.Campgrounds.Copy.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Copy.CopyImages")]
        public bool CopyImages { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Copy.Published")]
        public bool Published { get; set; }
    }
}