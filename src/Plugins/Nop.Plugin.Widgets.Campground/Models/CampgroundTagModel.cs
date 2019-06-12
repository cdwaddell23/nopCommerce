using System.Collections.Generic;
using FluentValidation.Attributes;
using Nop.Plugin.Widgets.Campgrounds.Validators;
using Nop.Web.Framework.Localization;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Mvc.Models;

namespace Nop.Plugin.Widgets.Campgrounds.Models
{
    [Validator(typeof(CampgroundTagValidator))]
    public partial class CampgroundTagModel : BaseNopEntityModel, ILocalizedModel<CampgroundTagLocalizedModel>
    {
        public CampgroundTagModel()
        {
            Locales = new List<CampgroundTagLocalizedModel>();
        }
        [NopResourceDisplayName("Admin.Catalog.CampgroundTags.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.Catalog.CampgroundTags.Fields.CampgroundCount")]
        public int CampgroundCount { get; set; }

        public IList<CampgroundTagLocalizedModel> Locales { get; set; }
    }

    public partial class CampgroundTagLocalizedModel : ILocalizedModelLocal
    {
        public int LanguageId { get; set; }

        [NopResourceDisplayName("Admin.Catalog.CampgroundTags.Fields.Name")]
        public string Name { get; set; }
    }
}