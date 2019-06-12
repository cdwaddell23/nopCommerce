using Nop.Plugin.Campgrounds.Data.Domain;
using Nop.Plugin.Widgets.Campgrounds.Models;
using Nop.Web.Areas.Admin.Extensions;

namespace Nop.Plugin.Widgets.Campgrounds.Extensions
{
    public static class CampgroundMappingExtensions
    {

        #region Campground
        //Campground
        public static CampgroundModel ToModel(this Campground entity)
        {
            return entity.MapTo<Campground, CampgroundModel>();
        }

        public static Campground ToEntity(this CampgroundModel model)
        {
            return model.MapTo<CampgroundModel, Campground>();
        }

        public static Campground ToEntity(this CampgroundModel model, Campground destination)
        {
            return model.MapTo(destination);
        }
        #endregion

        #region Campground Attributes
        // CampgroundAttributeType Mapping
        public static CampgroundAttributeTypeModel ToModel(this CampgroundAttributeType entity)
        {
            return entity.MapTo<CampgroundAttributeType, CampgroundAttributeTypeModel>();
        }

        public static CampgroundAttributeType ToEntity(this CampgroundAttributeTypeModel model)
        {
            return model.MapTo<CampgroundAttributeTypeModel, CampgroundAttributeType>();
        }

        public static CampgroundAttributeType ToEntity(this CampgroundAttributeTypeModel model, CampgroundAttributeType destination)
        {
            return model.MapTo(destination);
        }
        #endregion

        #region Campground Address
        // CampgroundAttributeType Mapping
        public static CampgroundAddressModel ToModel(this CampgroundAddress entity)
        {
            return entity.MapTo<CampgroundAddress, CampgroundAddressModel>();
        }

        public static CampgroundAddress ToEntity(this CampgroundAddressModel model)
        {
            return model.MapTo<CampgroundAddressModel, CampgroundAddress>();
        }

        public static CampgroundAddress ToEntity(this CampgroundAddressModel model, CampgroundAddress destination)
        {
            return model.MapTo(destination);
        }
        #endregion

        #region Campground Editor Settings
        //CampgroundEditorSettings
        public static CampgroundEditorSettingsModel ToModel(this CampgroundEditorSettings entity)
        {
            return entity.MapTo<CampgroundEditorSettings, CampgroundEditorSettingsModel>();
        }
        public static CampgroundEditorSettings ToEntity(this CampgroundEditorSettingsModel model, CampgroundEditorSettings destination)
        {
            return model.MapTo(destination);
        }
        #endregion

    }
}