using System;
using System.Linq;
using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Media;
using Nop.Plugin.Campgrounds.Data.Domain;
using Nop.Plugin.Widgets.Campgrounds.Models;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Web.Framework.Security.Captcha;

namespace Nop.Plugin.Widgets.Campgrounds.Factories
{
    /// <summary>
    /// Represents the CampgroundRegister model factory
    /// </summary>
    public partial class CampgroundRegisterModelFactory : ICampgroundRegisterModelFactory
    {
        #region Fields

        private readonly IWorkContext _workContext;
        private readonly ILocalizationService _localizationService;
        private readonly IPictureService _pictureService;
        
        private readonly CaptchaSettings _captchaSettings;
        private readonly CommonSettings _commonSettings;
        private readonly MediaSettings _mediaSettings;
        private readonly CampgroundHostSettings _campgroundHostSettings;

        #endregion

        #region Ctor

        public CampgroundRegisterModelFactory(IWorkContext workContext,
            ILocalizationService localizationService,
            IPictureService pictureService,
            CaptchaSettings captchaSettings,
            CommonSettings commonSettings,
            MediaSettings mediaSettings,
            CampgroundHostSettings campgroundHostSettings)
        {
            this._workContext = workContext;
            this._localizationService = localizationService;
            this._pictureService = pictureService;
            
            this._captchaSettings = captchaSettings;
            this._commonSettings = commonSettings;
            this._mediaSettings = mediaSettings;
            this._campgroundHostSettings = campgroundHostSettings;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare the apply CampgroundRegister model
        /// </summary>
        /// <param name="model">The apply CampgroundRegister model</param>
        /// <param name="validateCampgroundRegister">Whether to validate that the customer is already a CampgroundRegister</param>
        /// <param name="excludeProperties">Whether to exclude populating of model properties from the entity</param>
        /// <returns>The apply CampgroundRegister model</returns>
        public virtual CampgroundRegisterModel PrepareCampgroundRegisterModel(CampgroundRegisterModel model, bool validateCampgroundRegister, bool excludeProperties)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (validateCampgroundRegister && _workContext.CurrentCustomer.CustomerRoles.FirstOrDefault(cr => cr.SystemName == "CampgroundRegister") != null)
            {
                //already applied for CampgroundRegister account
                model.DisableFormInput = true;
                model.Result = _localizationService.GetResource("CampgroundRegisters.ApplyAccount.AlreadyApplied");
            }

            model.DisplayCaptcha = _captchaSettings.Enabled && _captchaSettings.ShowOnApplyCampgroundPage;
            model.TermsOfServiceEnabled = _campgroundHostSettings.TermsOfServiceEnabled;
            model.TermsOfServicePopup = _commonSettings.PopupForTermsOfServiceLinks;

            if (!excludeProperties)
            {
                model.Campground.CampgroundAddress.Email = _workContext.CurrentCustomer.Email;
            }

            return model;
        }

        /// <summary>
        /// Prepare the CampgroundRegister info model
        /// </summary>
        /// <param name="model">CampgroundRegister info model</param>
        /// <param name="excludeProperties">Whether to exclude populating of model properties from the entity</param>
        /// <returns>CampgroundRegister info model</returns>
        //public virtual CampgroundRegisterInfoModel PrepareCampgroundRegisterInfoModel(CampgroundRegisterInfoModel model, bool excludeProperties)
        //{
        //    if (model == null)
        //        throw new ArgumentNullException(nameof(model));

        //    var CampgroundRegister = _workContext.CurrentCampgroundRegister;
        //    if (!excludeProperties)
        //    {
        //        model.Description = CampgroundRegister.Description;
        //        model.Email = CampgroundRegister.Email;
        //        model.Name = CampgroundRegister.Name;
        //    }

        //    var picture = _pictureService.GetPictureById(CampgroundRegister.PictureId);
        //    var pictureSize = _mediaSettings.AvatarPictureSize;
        //    model.PictureUrl = picture != null ? _pictureService.GetPictureUrl(picture, pictureSize) : string.Empty;

        //    return model;
        //}
        
        #endregion
    }
}
