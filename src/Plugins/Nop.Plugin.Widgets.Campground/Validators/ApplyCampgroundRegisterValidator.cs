using FluentValidation;
using Nop.Plugin.Widgets.Campgrounds.Models;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;

namespace Nop.Plugin.Widgets.Campgrounds.Validators
{
    public partial class ApplyCampgroundRegisterValidator : BaseNopValidator<CampgroundRegisterModel>
    {
        public ApplyCampgroundRegisterValidator(ILocalizationService localizationService)
        {
            //RuleFor(x => x.Campground.CampgroundAddress.Address1).NotEmpty().WithMessage(localizationService.GetResource("CampgroundRegister.Address1.Required"));
            //RuleFor(x => x.Campground.CampgroundAddress.City).NotEmpty().WithMessage(localizationService.GetResource("CampgroundRegister.City.Required"));
            //RuleFor(x => x.Campground.CampgroundAddress.StateProvinceId).NotEmpty().WithMessage(localizationService.GetResource("CampgroundRegister.State.Required"));
            //RuleFor(x => x.Campground.CampgroundAddress.ZipPostalCode).NotEmpty().WithMessage(localizationService.GetResource("CampgroundRegister.ZipPostalCode.Required"));
            //RuleFor(x => x.Campground.ShortDescription).NotEmpty().WithMessage(localizationService.GetResource("CampgroundRegister.Description.Required"));

            //RuleFor(x => x.Campground.Name).NotEmpty().WithMessage(localizationService.GetResource("CampgroundRegister.CampgroundName.Required"));
            //RuleFor(x => x.Campground.CampgroundAddress.FirstName).NotEmpty().WithMessage(localizationService.GetResource("CampgroundRegister.FirstName.Required"));
            //RuleFor(x => x.Campground.CampgroundAddress.LastName).NotEmpty().WithMessage(localizationService.GetResource("CampgroundRegister.LastName.Required"));

            //RuleFor(x => x.Campground.CampgroundAddress.Email).NotEmpty().WithMessage(localizationService.GetResource("CampgroundRegister.Email.Required"));
            //RuleFor(x => x.Campground.CampgroundAddress.Email).EmailAddress().WithMessage(localizationService.GetResource("Common.WrongEmail"));
        }
    }
}