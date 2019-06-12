using Nop.Plugin.Widgets.Campgrounds.Models;

namespace Nop.Plugin.Widgets.Campgrounds.Factories
{
    /// <summary>
    /// Represents the interface of the campgroundHost model factory
    /// </summary>
    public partial interface ICampgroundRegisterModelFactory
    {
        /// <summary>
        /// Prepare the apply campgroundHost model
        /// </summary>
        /// <param name="model">The apply campgroundHost model</param>
        /// <param name="validateCampgroundHost">Whether to validate that the customer is already a campgroundHost</param>
        /// <param name="excludeProperties">Whether to exclude populating of model properties from the entity</param>
        /// <returns>The apply campgroundHost model</returns>
        CampgroundRegisterModel PrepareCampgroundRegisterModel(CampgroundRegisterModel model, bool validateCampgroundHost,bool excludeProperties);

        /// <summary>
        /// Prepare the campgroundHost info model
        /// </summary>
        /// <param name="model">CampgroundHost info model</param>
        /// <param name="excludeProperties">Whether to exclude populating of model properties from the entity</param>
        /// <returns>CampgroundHost info model</returns>
        //CampgroundHostInfoModel PrepareCampgroundHostInfoModel(CampgroundHostInfoModel model, bool excludeProperties);
    }
}
