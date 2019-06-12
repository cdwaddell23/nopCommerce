using AutoMapper;
using Nop.Core.Infrastructure.Mapper;
using Nop.Plugin.Campgrounds.Data.Domain;
using Nop.Plugin.Widgets.Campgrounds.Models;
using Nop.Services.Seo;

namespace Nop.Plugin.Widgets.Campgrounds.Infrastructure.Mapper
{
    /// <summary>
    /// AutoMapper configuration for admin area models
    /// </summary>
    public class AdminMapperConfiguration : Profile, IMapperProfile
    {
        public AdminMapperConfiguration()
        {

            #region Campground Attributes
            CreateMap<CampgroundAttributeType, CampgroundAttributeTypeModel>()
                .ForMember(dest => dest.Locales, mo => mo.Ignore())
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
            CreateMap<CampgroundAttributeTypeModel, CampgroundAttributeType>();
            #endregion


            #region Campground Address
            CreateMap<CampgroundAddress, CampgroundAddressModel>();
                //.ForMember(dest => dest.AddressHtml, mo => mo.Ignore())
                //.ForMember(dest => dest.AvailableCountries, mo => mo.Ignore())
                //.ForMember(dest => dest.AvailableStates, mo => mo.Ignore())
                //.ForMember(dest => dest.FirstNameEnabled, mo => mo.Ignore())
                //.ForMember(dest => dest.FirstNameRequired, mo => mo.Ignore())
                //.ForMember(dest => dest.LastNameEnabled, mo => mo.Ignore())
                //.ForMember(dest => dest.LastNameRequired, mo => mo.Ignore())
                //.ForMember(dest => dest.EmailEnabled, mo => mo.Ignore())
                //.ForMember(dest => dest.EmailRequired, mo => mo.Ignore())
                //.ForMember(dest => dest.CompanyEnabled, mo => mo.Ignore())
                //.ForMember(dest => dest.CompanyRequired, mo => mo.Ignore())
                //.ForMember(dest => dest.CountryEnabled, mo => mo.Ignore())
                //.ForMember(dest => dest.CountryRequired, mo => mo.Ignore())
                //.ForMember(dest => dest.StateProvinceEnabled, mo => mo.Ignore())
                //.ForMember(dest => dest.CityEnabled, mo => mo.Ignore())
                //.ForMember(dest => dest.CityRequired, mo => mo.Ignore())
                //.ForMember(dest => dest.StreetAddressEnabled, mo => mo.Ignore())
                //.ForMember(dest => dest.StreetAddressRequired, mo => mo.Ignore())
                //.ForMember(dest => dest.StreetAddress2Enabled, mo => mo.Ignore())
                //.ForMember(dest => dest.StreetAddress2Required, mo => mo.Ignore())
                //.ForMember(dest => dest.ZipPostalCodeEnabled, mo => mo.Ignore())
                //.ForMember(dest => dest.ZipPostalCodeRequired, mo => mo.Ignore())
                //.ForMember(dest => dest.PhoneEnabled, mo => mo.Ignore())
                //.ForMember(dest => dest.PhoneRequired, mo => mo.Ignore())
                //.ForMember(dest => dest.FaxEnabled, mo => mo.Ignore())
                //.ForMember(dest => dest.FaxRequired, mo => mo.Ignore())
                //.ForMember(dest => dest.Address.Country,
                //    mo => mo.MapFrom(src => src.Country != null ? src.Country.Name : null))
                //.ForMember(dest => dest.Address.StateProvince,
                //    mo => mo.MapFrom(src => src.StateProvince != null ? src.StateProvince.Name : null))
                //.ForMember(dest => dest.CustomProperties, mo => mo.Ignore());

            CreateMap<CampgroundAddressModel, CampgroundAddress>();
                //.ForMember(dest => dest.CreatedOnUtc, mo => mo.Ignore())
                //.ForMember(dest => dest.Country, mo => mo.Ignore())
                //.ForMember(dest => dest.CustomAttributes, mo => mo.Ignore())
                //.ForMember(dest => dest.StateProvince, mo => mo.Ignore());
            #endregion

            #region Campground
            CreateMap<Campground, CampgroundModel>()
                    .ForMember(dest => dest.CampgroundsTypesSupportedByCampgroundTemplates, mo => mo.Ignore())
                    .ForMember(dest => dest.CampgroundTypeName, mo => mo.Ignore())
                    .ForMember(dest => dest.AssociatedToCampgroundId, mo => mo.Ignore())
                    .ForMember(dest => dest.AssociatedToCampgroundName, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.UpdatedOn, mo => mo.Ignore())
                    .ForMember(dest => dest.CampgroundTags, mo => mo.Ignore())
                    .ForMember(dest => dest.PictureThumbnailUrl, mo => mo.Ignore())
                    .ForMember(dest => dest.Locales, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableCategories, mo => mo.Ignore())
                    .ForMember(dest => dest.AddPictureModel, mo => mo.Ignore())
                    .ForMember(dest => dest.CampgroundPictureModels, mo => mo.Ignore())
                    .ForMember(dest => dest.AddSpecificationAttributeModel, mo => mo.Ignore())
                    .ForMember(dest => dest.CopyCampgroundModel, mo => mo.Ignore())
                    .ForMember(dest => dest.IsLoggedInAsCampgroundHost, mo => mo.Ignore())
                    .ForMember(dest => dest.SeName, mo => mo.MapFrom(src => src.GetSeName(0, true, false)))
                    .ForMember(dest => dest.AvailableCustomerRoles, mo => mo.Ignore())
                    .ForMember(dest => dest.SelectedCustomerRoleIds, mo => mo.Ignore())
                    .ForMember(dest => dest.PrimaryStoreCurrencyCode, mo => mo.Ignore())
                    .ForMember(dest => dest.SelectedCategoryIds, mo => mo.Ignore())
                    .ForMember(dest => dest.AvailableCampgroundAvailabilityRanges, mo => mo.Ignore())
                    .ForMember(dest => dest.CampgroundEditorSettingsModel, mo => mo.Ignore())
                    .ForMember(dest => dest.CustomProperties, mo => mo.Ignore())
                    .ForMember(dest => dest.CampgroundAttributeTypesExist, mo => mo.Ignore());
            CreateMap<CampgroundModel, Campground>()
                    .ForMember(dest => dest.CampgroundTags, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOnUtc, mo => mo.Ignore())
                    .ForMember(dest => dest.UpdatedOnUtc, mo => mo.Ignore())
                    .ForMember(dest => dest.ParentGroupedCampgroundId, mo => mo.Ignore())
                    .ForMember(dest => dest.CampgroundType, mo => mo.Ignore())
                    .ForMember(dest => dest.Deleted, mo => mo.Ignore())
                    .ForMember(dest => dest.ApprovedRatingSum, mo => mo.Ignore())
                    .ForMember(dest => dest.NotApprovedRatingSum, mo => mo.Ignore())
                    .ForMember(dest => dest.ApprovedTotalReviews, mo => mo.Ignore())
                    .ForMember(dest => dest.NotApprovedTotalReviews, mo => mo.Ignore())
                    .ForMember(dest => dest.CampgroundCategories, mo => mo.Ignore())
                    .ForMember(dest => dest.CampgroundPictures, mo => mo.Ignore())
                    .ForMember(dest => dest.CampgroundReviews, mo => mo.Ignore())
                    .ForMember(dest => dest.CampgroundSpecificationAttributes, mo => mo.Ignore())
                    .ForMember(dest => dest.HasDiscountsApplied, mo => mo.Ignore())
                    .ForMember(dest => dest.CampgroundAttributeMappings, mo => mo.Ignore())
                    .ForMember(dest => dest.CampgroundAttributeTypeCombinations, mo => mo.Ignore())
                    .ForMember(dest => dest.AppliedDiscounts, mo => mo.Ignore())
                    .ForMember(dest => dest.SubjectToAcl, mo => mo.Ignore())
                    .ForMember(dest => dest.LimitedToStores, mo => mo.Ignore());
            #endregion

            #region Campground Editor Settings
            CreateMap<CampgroundEditorSettings, CampgroundEditorSettingsModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
            CreateMap<CampgroundEditorSettingsModel, CampgroundEditorSettings>();
            #endregion

        }

        /// <summary>
        /// Order of this mapper implementation
        /// </summary>
        public int Order => 0;
    }
}