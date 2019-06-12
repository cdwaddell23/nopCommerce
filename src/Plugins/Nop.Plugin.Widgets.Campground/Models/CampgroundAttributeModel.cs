using System.Collections.Generic;
using FluentValidation.Attributes;
using Nop.Plugin.Widgets.Campgrounds.Validators;
using Nop.Web.Framework.Localization;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Mvc.Models;

namespace Nop.Plugin.Widgets.Campgrounds.Models
{
    [Validator(typeof(CampgroundAttributeTypeValidator))]
    public partial class CampgroundAttributeTypeModel : BaseNopEntityModel, ILocalizedModel<CampgroundAttributeTypeLocalizedModel>
    {
        public CampgroundAttributeTypeModel()
        {
            Locales = new List<CampgroundAttributeTypeLocalizedModel>();
        }

        [NopResourceDisplayName("Admin.Catalog.Attributes.CampgroundAttributes.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Attributes.CampgroundAttributes.Fields.Description")]
        public string Description { get; set; }

        public IList<CampgroundAttributeTypeLocalizedModel> Locales { get; set; }

        #region Nested classes

        public partial class UsedByCampgroundModel : BaseNopEntityModel
        {
            [NopResourceDisplayName("Admin.Catalog.Attributes.CampgroundAttributes.UsedByCampgrounds.Campground")]
            public string CampgroundName { get; set; }
            [NopResourceDisplayName("Admin.Catalog.Attributes.CampgroundAttributes.UsedByCampgrounds.Published")]
            public bool Published { get; set; }
        }

        #endregion
    }

    public partial class CampgroundAttributeTypeLocalizedModel : ILocalizedModelLocal
    {
        public int LanguageId { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Attributes.CampgroundAttributes.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Attributes.CampgroundAttributes.Fields.Description")]
        public string Description { get; set; }
    }

    [Validator(typeof(PredefinedCampgroundAttributeValueModelValidator))]
    public partial class PredefinedCampgroundAttributeValueModel : BaseNopEntityModel, ILocalizedModel<PredefinedCampgroundAttributeValueLocalizedModel>
    {
        public PredefinedCampgroundAttributeValueModel()
        {
            Locales = new List<PredefinedCampgroundAttributeValueLocalizedModel>();
        }

        public int CampgroundAttributeTypeId { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Attributes.CampgroundAttributes.PredefinedValues.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Attributes.CampgroundAttributes.PredefinedValues.Fields.PriceAdjustment")]
        public decimal PriceAdjustment { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Attributes.CampgroundAttributes.PredefinedValues.Fields.PriceAdjustment")]
        //used only on the values list page
        public string PriceAdjustmentStr { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Attributes.CampgroundAttributes.PredefinedValues.Fields.WeightAdjustment")]
        public decimal WeightAdjustment { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Attributes.CampgroundAttributes.PredefinedValues.Fields.WeightAdjustment")]
        //used only on the values list page
        public string WeightAdjustmentStr { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Attributes.CampgroundAttributes.PredefinedValues.Fields.Cost")]
        public decimal Cost { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Attributes.CampgroundAttributes.PredefinedValues.Fields.IsPreSelected")]
        public bool IsPreSelected { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Attributes.CampgroundAttributes.PredefinedValues.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        public IList<PredefinedCampgroundAttributeValueLocalizedModel> Locales { get; set; }
    }

    public partial class PredefinedCampgroundAttributeValueLocalizedModel : ILocalizedModelLocal
    {
        public int LanguageId { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Attributes.CampgroundAttributes.PredefinedValues.Fields.Name")]
        public string Name { get; set; }
    }
}