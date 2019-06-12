using FluentValidation;
using Nop.Data;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;
using Nop.Plugin.Campgrounds.Data.Domain;
using Nop.Plugin.Widgets.Campgrounds.Models;

namespace Nop.Plugin.Widgets.Campgrounds.Validators
{
    public partial class CampgroundAttributeTypeValidator : BaseNopValidator<CampgroundAttributeTypeModel>
    {
        public CampgroundAttributeTypeValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Catalog.Attributes.CampgroundAttributes.Fields.Name.Required"));
            SetDatabaseValidationRules<CampgroundAttributeType>(dbContext);
        }
    }
}