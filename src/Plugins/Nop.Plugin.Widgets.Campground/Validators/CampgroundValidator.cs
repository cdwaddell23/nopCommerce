using FluentValidation;
using Nop.Plugin.Widgets.Campgrounds.Models;
using Nop.Data;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;
using Nop.Plugin.Campgrounds.Data.Domain;

namespace Nop.Plugin.Widgets.Campgrounds.Validators
{
    public partial class CampgroundValidator : BaseNopValidator<CampgroundModel>
    {
        public CampgroundValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Campgrounds.Fields.Name.Required"));

            SetDatabaseValidationRules<Campground>(dbContext);
        }
    }
}