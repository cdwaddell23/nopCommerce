using FluentValidation;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;
using Nop.Plugin.Widgets.Campgrounds.Models;

namespace Nop.Plugin.Widgets.Campgrounds.Validators
{
    public partial class PredefinedCampgroundAttributeValueModelValidator : BaseNopValidator<PredefinedCampgroundAttributeValueModel>
    {
        public PredefinedCampgroundAttributeValueModelValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Admin.Catalog.Attributes.CampgroundAttributes.PredefinedValues.Fields.Name.Required"));
        }
    }
}