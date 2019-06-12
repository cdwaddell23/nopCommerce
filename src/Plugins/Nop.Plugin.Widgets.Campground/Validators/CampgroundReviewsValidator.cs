using FluentValidation;
using Nop.Plugin.Widgets.Campgrounds.Models;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;

namespace Nop.Plugin.Widgets.Campgrounds.Validators
{
    public partial class CampgroundReviewsValidator : BaseNopValidator<CampgroundReviewsModel>
    {
        public CampgroundReviewsValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.AddCampgroundReview.Title).NotEmpty().WithMessage(localizationService.GetResource("Reviews.Fields.Title.Required")).When(x => x.AddCampgroundReview != null);
            RuleFor(x => x.AddCampgroundReview.Title).Length(1, 200).WithMessage(string.Format(localizationService.GetResource("Reviews.Fields.Title.MaxLengthValidation"), 200)).When(x => x.AddCampgroundReview != null && !string.IsNullOrEmpty(x.AddCampgroundReview.Title));
            RuleFor(x => x.AddCampgroundReview.ReviewText).NotEmpty().WithMessage(localizationService.GetResource("Reviews.Fields.ReviewText.Required")).When(x => x.AddCampgroundReview != null);
        }
    }
}