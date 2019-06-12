using FluentValidation;
using Nop.Plugin.Widgets.Campgrounds.Models;
using Nop.Core.Domain.Catalog;
using Nop.Data;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;
using Nop.Plugin.Campgrounds.Data.Domain;

namespace Nop.Plugin.Widgets.Campgrounds.Validators
{
    public partial class CampgroundAttributeValueModelValidator : BaseNopValidator<CampgroundModel.CampgroundAttributeValueModel>
    {
        public CampgroundAttributeValueModelValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Admin.Campgrounds.CampgroundAttributes.Attributes.Values.Fields.Name.Required"));

            RuleFor(x => x.AssociatedCampgroundId)
                .GreaterThanOrEqualTo(1)
                .WithMessage(localizationService.GetResource("Admin.Campgrounds.CampgroundAttributes.Attributes.Values.Fields.AssociatedCampground.Choose"))
                .When(x => x.AttributeValueTypeId == (int)CampgroundAttributeValueType.AssociatedToCampground);

            SetDatabaseValidationRules<CampgroundAttributeValue>(dbContext);
        }
    }
}