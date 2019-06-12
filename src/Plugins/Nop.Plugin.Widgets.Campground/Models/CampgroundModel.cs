using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Plugin.Campgrounds.Data.Domain;
using Nop.Plugin.Widgets.Campgrounds.Validators;
using Nop.Web.Framework.Localization;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Mvc.Models;

namespace Nop.Plugin.Widgets.Campgrounds.Models
{
    [Validator(typeof(CampgroundValidator))]
    public partial class CampgroundModel : BaseNopEntityModel, ILocalizedModel<CampgroundLocalizedModel>
    {
        public CampgroundModel()
        {
            Locales = new List<CampgroundLocalizedModel>();
            CampgroundPictureModels = new List<CampgroundPictureModel>();
            CopyCampgroundModel = new CopyCampgroundModel();
            AddPictureModel = new CampgroundPictureModel();
            CampgroundAddress = new CampgroundAddressModel();
            AddSpecificationAttributeModel = new AddCampgroundSpecificationAttributeModel();
            CampgroundEditorSettingsModel = new CampgroundEditorSettingsModel();

            AvailableCampgroundTemplates = new List<SelectListItem>();
            AvailableCampgroundAvailabilityRanges = new List<SelectListItem>();
            CampgroundsTypesSupportedByCampgroundTemplates = new Dictionary<int, IList<SelectListItem>>();

            AvailableCampgroundHosts = new List<SelectListItem>();
            AvailableCampgroundTypes = new List<SelectListItem>();

            SelectedCategoryIds = new List<int>();
            AvailableCategories = new List<SelectListItem>();

            SelectedCustomerRoleIds = new List<int>();
            AvailableCustomerRoles = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.ID")]
        public override int Id { get; set; }

        //picture thumbnail
        [NopResourceDisplayName("Admin.Campgrounds.Fields.PictureThumbnailUrl")]
        public string PictureThumbnailUrl { get; set; }

        //campground types
        [NopResourceDisplayName("Admin.Campgrounds.Fields.CampgroundType")]
        public int CampgroundTypeId { get; set; }
        [NopResourceDisplayName("Admin.Campgrounds.Fields.CampgroundType")]
        public string CampgroundTypeName { get; set; }
        [NopResourceDisplayName("Admin.Campgrounds.Fields.CampgroundType")]
        public IList<SelectListItem> AvailableCampgroundTypes { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.AssociatedToCampgroundName")]
        public int AssociatedToCampgroundId { get; set; }
        [NopResourceDisplayName("Admin.Campgrounds.Fields.AssociatedToCampgroundName")]
        public string AssociatedToCampgroundName { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.CampgroundTemplate")]
        public int CampgroundTemplateId { get; set; }
        public IList<SelectListItem> AvailableCampgroundTemplates { get; set; }
        //<campground type ID, list of supported campground template IDs>
        public Dictionary<int, IList<SelectListItem>> CampgroundsTypesSupportedByCampgroundTemplates { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.ShortDescription")]
        public string ShortDescription { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.FullDescription")]
        public string FullDescription { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.AdminComment")]
        public string AdminComment { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.ShowOnHomePage")]
        public bool ShowOnHomePage { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.MetaKeywords")]
        public string MetaKeywords { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.MetaDescription")]
        public string MetaDescription { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.MetaTitle")]
        public string MetaTitle { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.SeName")]
        public string SeName { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.CampgroundSeName")]
        public string CampgroundSeName { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.Website")]
        public string Website { get; set; }
        [NopResourceDisplayName("Admin.Campgrounds.Fields.FullUrl")]
        public string FullUrl { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.Distance")]
        public double Distance { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.AllowCampgroundReviews")]
        public bool AllowCampgroundReviews { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.CampgroundTags")]
        public string CampgroundTags { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.RequireOtherCampgrounds")]
        public bool RequireOtherCampgrounds { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.RequiredCampgroundIds")]
        public string RequiredCampgroundIds { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.AutomaticallyAddRequiredCampgrounds")]
        public bool AutomaticallyAddRequiredCampgrounds { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.HasUserAgreement")]
        public bool HasUserAgreement { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.UserAgreementText")]
        public string UserAgreementText { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.IsRecurring")]
        public bool IsRecurring { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.AvailableCampsites")]
        public int AvailableCampsites { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.RecurringCycleLength")]
        public int RecurringCycleLength { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.RecurringCyclePeriod")]
        public int RecurringCyclePeriodId { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.RecurringTotalCycles")]
        public int RecurringTotalCycles { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.IsRental")]
        public bool IsRental { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.RentalPriceLength")]
        public int RentalPriceLength { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.RentalPricePeriod")]
        public int RentalPricePeriodId { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.CampgroundAvailabilityRange")]
        public int CampgroundAvailabilityRangeId { get; set; }
        public IList<SelectListItem> AvailableCampgroundAvailabilityRanges { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.BackorderMode")]
        public int BackorderModeId { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.AllowBackInStockSubscriptions")]
        public bool AllowBackInStockSubscriptions { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.OrderMinimumQuantity")]
        public int OrderMinimumQuantity { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.OrderMaximumQuantity")]
        public int OrderMaximumQuantity { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.AllowedQuantities")]
        public string AllowedQuantities { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.AllowAddingOnlyExistingAttributeCombinations")]
        public bool AllowAddingOnlyExistingAttributeCombinations { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.NotReturnable")]
        public bool NotReturnable { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.DisableBuyButton")]
        public bool DisableBuyButton { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.DisableWishlistButton")]
        public bool DisableWishlistButton { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.AvailableForPreOrder")]
        public bool AvailableForPreOrder { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.PreOrderAvailabilityStartDateTimeUtc")]
        [UIHint("DateTimeNullable")]
        public DateTime? PreOrderAvailabilityStartDateTimeUtc { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.CallForPrice")]
        public bool CallForPrice { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.MarkAsNew")]
        public bool MarkAsNew { get; set; }
        [NopResourceDisplayName("Admin.Campgrounds.Fields.MarkAsNewStartDateTimeUtc")]
        [UIHint("DateTimeNullable")]
        public DateTime? MarkAsNewStartDateTimeUtc { get; set; }
        [NopResourceDisplayName("Admin.Campgrounds.Fields.MarkAsNewEndDateTimeUtc")]
        [UIHint("DateTimeNullable")]
        public DateTime? MarkAsNewEndDateTimeUtc { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.AvailableStartDateTime")]
        [UIHint("DateTimeNullable")]
        public DateTime? AvailableStartDateTimeUtc { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.AvailableEndDateTime")]
        [UIHint("DateTimeNullable")]
        public DateTime? AvailableEndDateTimeUtc { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.Published")]
        public bool Published { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.CreatedOn")]
        public DateTime? CreatedOn { get; set; }
        [NopResourceDisplayName("Admin.Campgrounds.Fields.UpdatedOn")]
        public DateTime? UpdatedOn { get; set; }

        public string PrimaryStoreCurrencyCode { get; set; }

        public IList<CampgroundLocalizedModel> Locales { get; set; }

        //ACL (customer roles)
        [NopResourceDisplayName("Admin.Campgrounds.Fields.AclCustomerRoles")]
        public IList<int> SelectedCustomerRoleIds { get; set; }
        public IList<SelectListItem> AvailableCustomerRoles { get; set; }

        //categories
        [NopResourceDisplayName("Admin.Campgrounds.Fields.Categories")]
        public IList<int> SelectedCategoryIds { get; set; }
        public IList<SelectListItem> AvailableCategories { get; set; }

        //campground hosts
        [NopResourceDisplayName("Admin.Campgrounds.Fields.CampgroundHost")]
        public int CampgroundHostId { get; set; }
        public IList<SelectListItem> AvailableCampgroundHosts { get; set; }

        //campground host
        public bool IsLoggedInAsCampgroundHost { get; set; }

        //campground address
        public CampgroundAddressModel CampgroundAddress { get; set; }

        //pictures
        public CampgroundPictureModel AddPictureModel { get; set; }
        public IList<CampgroundPictureModel> CampgroundPictureModels { get; set; }

        //campground attributes
        public bool CampgroundAttributeTypesExist { get; set; }

        //add specification attribute model
        public AddCampgroundSpecificationAttributeModel AddSpecificationAttributeModel { get; set; }

        //copy campground
        public CopyCampgroundModel CopyCampgroundModel { get; set; }

        //editor settings
        public CampgroundEditorSettingsModel CampgroundEditorSettingsModel { get; set; }

        #region Nested classes

        public partial class AddRequiredCampgroundModel : BaseNopModel
        {
            public AddRequiredCampgroundModel()
            {
                AvailableCategories = new List<SelectListItem>();
                AvailableCampgroundHosts = new List<SelectListItem>();
                AvailableCampgroundTypes = new List<SelectListItem>();
            }

            [NopResourceDisplayName("Admin.Campgrounds.List.SearchCampgroundName")]
            public string SearchCampgroundName { get; set; }
            [NopResourceDisplayName("Admin.Campgrounds.List.SearchCategory")]
            public int SearchCategoryId { get; set; }
            [NopResourceDisplayName("Admin.Campgrounds.List.SearchCampgroundHost")]
            public int SearchCampgroundHostId { get; set; }
            [NopResourceDisplayName("Admin.Campgrounds.List.SearchCampgroundType")]
            public int SearchCampgroundTypeId { get; set; }

            public IList<SelectListItem> AvailableCategories { get; set; }
            public IList<SelectListItem> AvailableCampgroundHosts { get; set; }
            public IList<SelectListItem> AvailableCampgroundTypes { get; set; }

            //campground
            public bool IsLoggedInAsCampgroundHost { get; set; }
        }

        public partial class AddCampgroundSpecificationAttributeModel : BaseNopModel
        {
            public AddCampgroundSpecificationAttributeModel()
            {
                AvailableAttributes = new List<SelectListItem>();
                AvailableOptions = new List<SelectListItem>();
            }
            
            [NopResourceDisplayName("Admin.Campgrounds.SpecificationAttributes.Fields.SpecificationAttribute")]
            public int SpecificationAttributeId { get; set; }

            [NopResourceDisplayName("Admin.Campgrounds.SpecificationAttributes.Fields.AttributeType")]
            public int AttributeTypeId { get; set; }

            [NopResourceDisplayName("Admin.Campgrounds.SpecificationAttributes.Fields.SpecificationAttributeOption")]
            public int SpecificationAttributeOptionId { get; set; }
            
            [NopResourceDisplayName("Admin.Campgrounds.SpecificationAttributes.Fields.CustomValue")]
            public string CustomValue { get; set; }

            [NopResourceDisplayName("Admin.Campgrounds.SpecificationAttributes.Fields.AllowFiltering")]
            public bool AllowFiltering { get; set; }

            [NopResourceDisplayName("Admin.Campgrounds.SpecificationAttributes.Fields.ShowOnCampgroundPage")]
            public bool ShowOnCampgroundPage { get; set; }

            [NopResourceDisplayName("Admin.Campgrounds.SpecificationAttributes.Fields.DisplayOrder")]
            public int DisplayOrder { get; set; }

            public IList<SelectListItem> AvailableAttributes { get; set; }
            public IList<SelectListItem> AvailableOptions { get; set; }
        }
        
        public partial class CampgroundPictureModel : BaseNopEntityModel
        {
            public int CampgroundId { get; set; }

            [UIHint("Picture")]
            [NopResourceDisplayName("Admin.Campgrounds.Pictures.Fields.Picture")]
            public int PictureId { get; set; }

            [NopResourceDisplayName("Admin.Campgrounds.Pictures.Fields.Picture")]
            public string PictureUrl { get; set; }

            [NopResourceDisplayName("Admin.Campgrounds.Pictures.Fields.DisplayOrder")]
            public int DisplayOrder { get; set; }

            [NopResourceDisplayName("Admin.Campgrounds.Pictures.Fields.OverrideAltAttribute")]
            public string OverrideAltAttribute { get; set; }

            [NopResourceDisplayName("Admin.Campgrounds.Pictures.Fields.OverrideTitleAttribute")]
            public string OverrideTitleAttribute { get; set; }
        }

        public partial class RelatedCampgroundModel : BaseNopEntityModel
        {
            public int CampgroundId2 { get; set; }

            [NopResourceDisplayName("Admin.Campgrounds.RelatedCampgrounds.Fields.Campground")]
            public string Campground2Name { get; set; }
            
            [NopResourceDisplayName("Admin.Campgrounds.RelatedCampgrounds.Fields.DisplayOrder")]
            public int DisplayOrder { get; set; }
        }

        public partial class AddRelatedCampgroundModel : BaseNopModel
        {
            public AddRelatedCampgroundModel()
            {
                AvailableCategories = new List<SelectListItem>();
                AvailableCampgroundHosts = new List<SelectListItem>();
                AvailableCampgroundTypes = new List<SelectListItem>();
            }

            [NopResourceDisplayName("Admin.Campgrounds.List.SearchCampgroundName")]
            public string SearchCampgroundName { get; set; }
            [NopResourceDisplayName("Admin.Campgrounds.List.SearchCategory")]
            public int SearchCategoryId { get; set; }
            [NopResourceDisplayName("Admin.Campgrounds.List.SearchCampgroundHost")]
            public int SearchCampgroundHostId { get; set; }
            [NopResourceDisplayName("Admin.Campgrounds.List.SearchCampgroundType")]
            public int SearchCampgroundTypeId { get; set; }

            public IList<SelectListItem> AvailableCategories { get; set; }
            public IList<SelectListItem> AvailableCampgroundHosts { get; set; }
            public IList<SelectListItem> AvailableCampgroundTypes { get; set; }

            public int CampgroundId { get; set; }

            public int[] SelectedCampgroundIds { get; set; }

            //campground
            public bool IsLoggedInAsCampgroundHost { get; set; }
        }

        public partial class AssociatedCampgroundModel : BaseNopEntityModel
        {
            [NopResourceDisplayName("Admin.Campgrounds.AssociatedCampgrounds.Fields.Campground")]
            public string CampgroundName { get; set; }
            [NopResourceDisplayName("Admin.Campgrounds.AssociatedCampgrounds.Fields.DisplayOrder")]
            public int DisplayOrder { get; set; }
        }

        public partial class AddAssociatedCampgroundModel : BaseNopModel
        {
            public AddAssociatedCampgroundModel()
            {
                AvailableCategories = new List<SelectListItem>();
                AvailableCampgroundHosts = new List<SelectListItem>();
                AvailableCampgroundTypes = new List<SelectListItem>();
            }

            [NopResourceDisplayName("Admin.Campgrounds.List.SearchCampgroundName")]
            public string SearchCampgroundName { get; set; }
            [NopResourceDisplayName("Admin.Campgrounds.List.SearchCategory")]
            public int SearchCategoryId { get; set; }
            [NopResourceDisplayName("Admin.Campgrounds.List.SearchCampgroundHost")]
            public int SearchCampgroundHostId { get; set; }
            [NopResourceDisplayName("Admin.Campgrounds.List.SearchCampgroundType")]
            public int SearchCampgroundTypeId { get; set; }

            public IList<SelectListItem> AvailableCategories { get; set; }
            public IList<SelectListItem> AvailableCampgroundHosts { get; set; }
            public IList<SelectListItem> AvailableCampgroundTypes { get; set; }

            public int CampgroundId { get; set; }

            public int[] SelectedCampgroundIds { get; set; }

            //campground
            public bool IsLoggedInAsCampgroundHost { get; set; }
        }

        public partial class CrossSellCampgroundModel : BaseNopEntityModel
        {
            public int CampgroundId2 { get; set; }

            [NopResourceDisplayName("Admin.Campgrounds.CrossSells.Fields.Campground")]
            public string Campground2Name { get; set; }
        }

        public partial class AddCrossSellCampgroundModel : BaseNopModel
        {
            public AddCrossSellCampgroundModel()
            {
                AvailableCategories = new List<SelectListItem>();
                AvailableCampgroundHosts = new List<SelectListItem>();
                AvailableCampgroundTypes = new List<SelectListItem>();
            }

            [NopResourceDisplayName("Admin.Campgrounds.List.SearchCampgroundName")]
            public string SearchCampgroundName { get; set; }
            [NopResourceDisplayName("Admin.Campgrounds.List.SearchCategory")]
            public int SearchCategoryId { get; set; }
            [NopResourceDisplayName("Admin.Campgrounds.List.SearchCampgroundHost")]
            public int SearchCampgroundHostId { get; set; }
            [NopResourceDisplayName("Admin.Campgrounds.List.SearchCampgroundType")]
            public int SearchCampgroundTypeId { get; set; }

            public IList<SelectListItem> AvailableCategories { get; set; }
            public IList<SelectListItem> AvailableCampgroundHosts { get; set; }
            public IList<SelectListItem> AvailableCampgroundTypes { get; set; }

            public int CampgroundId { get; set; }

            public int[] SelectedCampgroundIds { get; set; }

            //campground
            public bool IsLoggedInAsCampgroundHost { get; set; }
        }

        public partial class CampgroundAttributeMappingModel : BaseNopEntityModel, ILocalizedModel<CampgroundAttributeMappingLocalizedModel>
        {
            public CampgroundAttributeMappingModel()
            {
                AvailableCampgroundAttributeTypes = new List<SelectListItem>();
                Locales = new List<CampgroundAttributeMappingLocalizedModel>();
            }

            public int CampgroundId { get; set; }

            [NopResourceDisplayName("Admin.Campgrounds.CampgroundAttributes.Attributes.Fields.Attribute")]
            public int CampgroundAttributeTypeId { get; set; }
            [NopResourceDisplayName("Admin.Campgrounds.CampgroundAttributes.Attributes.Fields.Attribute")]
            public string CampgroundAttributeType { get; set; }
            public IList<SelectListItem> AvailableCampgroundAttributeTypes { get; set; }

            [NopResourceDisplayName("Admin.Campgrounds.CampgroundAttributes.Attributes.Fields.TextPrompt")]
            public string TextPrompt { get; set; }

            [NopResourceDisplayName("Admin.Campgrounds.CampgroundAttributes.Attributes.Fields.IsRequired")]
            public bool IsRequired { get; set; }

            [NopResourceDisplayName("Admin.Campgrounds.CampgroundAttributes.Attributes.Fields.AttributeControlType")]
            public int AttributeControlTypeId { get; set; }
            [NopResourceDisplayName("Admin.Campgrounds.CampgroundAttributes.Attributes.Fields.AttributeControlType")]
            public string AttributeControlType { get; set; }

            [NopResourceDisplayName("Admin.Campgrounds.CampgroundAttributes.Attributes.Fields.DisplayOrder")]
            public int DisplayOrder { get; set; }

            //validation fields
            [NopResourceDisplayName("Admin.Campgrounds.CampgroundAttributes.Attributes.ValidationRules.MinLength")]
            [UIHint("Int32Nullable")]
            public int? ValidationMinLength { get; set; }
            [NopResourceDisplayName("Admin.Campgrounds.CampgroundAttributes.Attributes.ValidationRules.MaxLength")]
            [UIHint("Int32Nullable")]
            public int? ValidationMaxLength { get; set; }
            [NopResourceDisplayName("Admin.Campgrounds.CampgroundAttributes.Attributes.ValidationRules.FileAllowedExtensions")]
            public string ValidationFileAllowedExtensions { get; set; }
            [NopResourceDisplayName("Admin.Campgrounds.CampgroundAttributes.Attributes.ValidationRules.FileMaximumSize")]
            [UIHint("Int32Nullable")]
            public int? ValidationFileMaximumSize { get; set; }
            [NopResourceDisplayName("Admin.Campgrounds.CampgroundAttributes.Attributes.ValidationRules.DefaultValue")]
            public string DefaultValue { get; set; }
            public string ValidationRulesString { get; set; }
            
            //condition
            //[NopResourceDisplayName("Admin.Campgrounds.CampgroundAttributes.Attributes.Condition")]
            //public bool ConditionAllowed { get; set; }
            //public string ConditionString { get; set; }
            //public CampgroundAttributeTypeConditionModel ConditionModel { get; set; }

            public IList<CampgroundAttributeMappingLocalizedModel> Locales { get; set; }
        }

        public partial class CampgroundAttributeMappingLocalizedModel : ILocalizedModelLocal
        {
            public int LanguageId { get; set; }

            [NopResourceDisplayName("Admin.Campgrounds.CampgroundAttributes.Attributes.Fields.TextPrompt")]
            public string TextPrompt { get; set; }
        }

        [Validator(typeof(CampgroundAttributeValueModelValidator))]
        public partial class CampgroundAttributeValueModel : BaseNopEntityModel, ILocalizedModel<CampgroundAttributeValueLocalizedModel>
        {
            public CampgroundAttributeValueModel()
            {
                CampgroundPictureModels = new List<CampgroundPictureModel>();
                Locales = new List<CampgroundAttributeValueLocalizedModel>();
            }

            public int CampgroundAttributeMappingId { get; set; }

            [NopResourceDisplayName("Admin.Campgrounds.CampgroundAttributes.Attributes.Values.Fields.AttributeValueType")]
            public int AttributeValueTypeId { get; set; }
            [NopResourceDisplayName("Admin.Campgrounds.CampgroundAttributes.Attributes.Values.Fields.AttributeValueType")]
            public string AttributeValueTypeName { get; set; }

            [NopResourceDisplayName("Admin.Campgrounds.CampgroundAttributes.Attributes.Values.Fields.AssociatedCampground")]
            public int AssociatedCampgroundId { get; set; }
            [NopResourceDisplayName("Admin.Campgrounds.CampgroundAttributes.Attributes.Values.Fields.AssociatedCampground")]
            public string AssociatedCampgroundName { get; set; }

            [NopResourceDisplayName("Admin.Campgrounds.CampgroundAttributes.Attributes.Values.Fields.Name")]
            public string Name { get; set; }
            
            [NopResourceDisplayName("Admin.Campgrounds.CampgroundAttributes.Attributes.Values.Fields.ColorSquaresRgb")]
            public string ColorSquaresRgb { get; set; }
            public bool DisplayColorSquaresRgb { get; set; }

            [NopResourceDisplayName("Admin.Campgrounds.CampgroundAttributes.Attributes.Values.Fields.ImageSquaresPicture")]
            [UIHint("Picture")]
            public int ImageSquaresPictureId { get; set; }
            public bool DisplayImageSquaresPicture { get; set; }

            [NopResourceDisplayName("Admin.Campgrounds.CampgroundAttributes.Attributes.Values.Fields.IsPreSelected")]
            public bool IsPreSelected { get; set; }

            [NopResourceDisplayName("Admin.Campgrounds.CampgroundAttributes.Attributes.Values.Fields.DisplayOrder")]
            public int DisplayOrder { get; set; }

            [NopResourceDisplayName("Admin.Campgrounds.CampgroundAttributes.Attributes.Values.Fields.PriceAdjustment")]
            public decimal PriceAdjustment { get; set; }
            [NopResourceDisplayName("Admin.Campgrounds.CampgroundAttributes.Attributes.Values.Fields.PriceAdjustment")]
            //used only on the values list page
            public string PriceAdjustmentStr { get; set; }

            [NopResourceDisplayName("Admin.Campgrounds.CampgroundAttributes.Attributes.Values.Fields.WeightAdjustment")]
            public decimal WeightAdjustment { get; set; }
            [NopResourceDisplayName("Admin.Campgrounds.CampgroundAttributes.Attributes.Values.Fields.WeightAdjustment")]
            //used only on the values list page
            public string WeightAdjustmentStr { get; set; }

            [NopResourceDisplayName("Admin.Campgrounds.CampgroundAttributes.Attributes.Values.Fields.Cost")]
            public decimal Cost { get; set; }
            [NopResourceDisplayName("Admin.Campgrounds.CampgroundAttributes.Attributes.Values.Fields.CustomerEntersQty")]
            public bool CustomerEntersQty { get; set; }
            [NopResourceDisplayName("Admin.Campgrounds.CampgroundAttributes.Attributes.Values.Fields.Quantity")]
            public int Quantity { get; set; }
            [NopResourceDisplayName("Admin.Campgrounds.CampgroundAttributes.Attributes.Values.Fields.Picture")]
            public int PictureId { get; set; }
            [NopResourceDisplayName("Admin.Campgrounds.CampgroundAttributes.Attributes.Values.Fields.PictureThumbnailUrl")]
            public string PictureThumbnailUrl { get; set; }

            public IList<CampgroundPictureModel> CampgroundPictureModels { get; set; }
            public IList<CampgroundAttributeValueLocalizedModel> Locales { get; set; }

            #region Nested classes

            public partial class AssociateCampgroundToAttributeValueModel : BaseNopModel
            {
                public AssociateCampgroundToAttributeValueModel()
                {
                    AvailableCategories = new List<SelectListItem>();
                    AvailableCampgroundHosts = new List<SelectListItem>();
                    AvailableCampgroundTypes = new List<SelectListItem>();
                }

                [NopResourceDisplayName("Admin.Campgrounds.List.SearchCampgroundName")]
                public string SearchCampgroundName { get; set; }
                [NopResourceDisplayName("Admin.Campgrounds.List.SearchCategory")]
                public int SearchCategoryId { get; set; }
                [NopResourceDisplayName("Admin.Campgrounds.List.SearchCampgroundHost")]
                public int SearchCampgroundHostId { get; set; }
                [NopResourceDisplayName("Admin.Campgrounds.List.SearchCampgroundType")]
                public int SearchCampgroundTypeId { get; set; }

                public IList<SelectListItem> AvailableCategories { get; set; }
                public IList<SelectListItem> AvailableCampgroundHosts { get; set; }
                public IList<SelectListItem> AvailableCampgroundTypes { get; set; }
                
                //campground
                public bool IsLoggedInAsCampgroundHost { get; set; }

                public int AssociatedToCampgroundId { get; set; }
            }

            #endregion
        }

        public partial class CampgroundAttributeValueLocalizedModel : ILocalizedModelLocal
        {
            public int LanguageId { get; set; }

            [NopResourceDisplayName("Admin.Campgrounds.CampgroundAttributes.Attributes.Values.Fields.Name")]
            public string Name { get; set; }
        }

        #endregion
    }

    public partial class CampgroundLocalizedModel : ILocalizedModelLocal
    {
        public int LanguageId { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.ShortDescription")]
        public string ShortDescription { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.FullDescription")]
        public string FullDescription { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.MetaKeywords")]
        public string MetaKeywords { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.MetaDescription")]
        public string MetaDescription { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.MetaTitle")]
        public string MetaTitle { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.SeName")]
        public string SeName { get; set; }

        [NopResourceDisplayName("Admin.Campgrounds.Fields.Website")]
        public string Website { get; set; }
    }
}