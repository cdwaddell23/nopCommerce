using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Mvc.Models;

namespace Nop.Plugin.Widgets.Campgrounds.Models
{
    public partial class CampgroundEditorSettingsModel : BaseNopModel
    {
        [NopResourceDisplayName("Admin.Configuration.Settings.CampgroundEditor.Id")]
        public bool Id { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.CampgroundEditor.CampgroundType")]
        public bool CampgroundType { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.CampgroundEditor.CampgroundHost")]
        public bool CampgroundHost { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.CampgroundEditor.VisibleIndividually")]
        public bool VisibleIndividually { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.CampgroundEditor.CampgroundTemplate")]
        public bool CampgroundTemplate { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.CampgroundEditor.AdminComment")]
        public bool AdminComment { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.CampgroundEditor.ACL")]
        public bool ACL { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.CampgroundEditor.ShowOnHomePage")]
        public bool ShowOnHomePage { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.CampgroundEditor.DisplayOrder")]
        public bool DisplayOrder { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.CampgroundEditor.AllowCampgroundReviews")]
        public bool AllowCampgroundReviews { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.CampgroundEditor.CampgroundTags")]
        public bool CampgroundTags { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.CampgroundEditor.CampgroundCost")]
        public bool CampgroundCost { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.CampgroundEditor.Discounts")]
        public bool Discounts { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.CampgroundEditor.DisableBuyButton")]
        public bool DisableBuyButton { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.CampgroundEditor.DisableWishlistButton")]
        public bool DisableWishlistButton { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.CampgroundEditor.RequireOtherCampgroundsAddedToTheCart")]
        public bool RequireOtherCampgroundsAddedToTheCart { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.CampgroundEditor.RecurringCampground")]
        public bool RecurringCampground { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.CampgroundEditor.IsRental")]
        public bool IsRental { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.CampgroundEditor.CampgroundAvailabilityRange")]
        public bool CampgroundAvailabilityRange { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.CampgroundEditor.AllowBackInStockSubscriptions")]
        public bool AllowBackInStockSubscriptions { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.CampgroundEditor.AllowAddingOnlyExistingAttributeCombinations")]
        public bool AllowAddingOnlyExistingAttributeCombinations { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.CampgroundEditor.AvailableStartDate")]
        public bool AvailableStartDate { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.CampgroundEditor.AvailableEndDate")]
        public bool AvailableEndDate { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.CampgroundEditor.MarkAsNew")]
        public bool MarkAsNew { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.CampgroundEditor.MarkAsNewStartDate")]
        public bool MarkAsNewStartDate { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.CampgroundEditor.MarkAsNewEndDate")]
        public bool MarkAsNewEndDate { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.CampgroundEditor.Published")]
        public bool Published { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.CampgroundEditor.CreatedOn")]
        public bool CreatedOn { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.CampgroundEditor.UpdatedOn")]
        public bool UpdatedOn { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.CampgroundEditor.RelatedCampgrounds")]
        public bool RelatedCampgrounds { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.CampgroundEditor.CrossSellsCampgrounds")]
        public bool CrossSellsCampgrounds { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.CampgroundEditor.Seo")]
        public bool Seo { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.CampgroundEditor.OneColumnCampgroundPage")]
        public bool OneColumnCampgroundPage { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.CampgroundEditor.CampgroundAttributeTypes")]
        public bool CampgroundAttributeTypes { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.CampgroundEditor.SpecificationAttributes")]
        public bool SpecificationAttributes { get; set; }
    }
}