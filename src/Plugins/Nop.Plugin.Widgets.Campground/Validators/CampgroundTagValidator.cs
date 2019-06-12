using FluentValidation;
using Nop.Data;
using Nop.Plugin.Campgrounds.Data.Domain;
using Nop.Plugin.Widgets.Campgrounds.Models;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;

namespace Nop.Plugin.Widgets.Campgrounds.Validators
{
    public partial class CampgroundTagValidator : BaseNopValidator<CampgroundTagModel>
    {
        public CampgroundTagValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Catalog.CampgroundTags.Fields.Name.Required"));

            SetDatabaseValidationRules<CampgroundTag>(dbContext);
        }
    }
}