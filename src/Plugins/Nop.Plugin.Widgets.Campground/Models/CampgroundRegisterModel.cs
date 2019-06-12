using FluentValidation.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Plugin.Campgrounds.Data.Domain;
using Nop.Plugin.Widgets.Campgrounds.Validators;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Mvc.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nop.Plugin.Widgets.Campgrounds.Models
{
    [Validator(typeof(ApplyCampgroundRegisterValidator))]
    public partial class CampgroundRegisterModel : BaseNopModel
    {
        public CampgroundRegisterModel()
        {
            this.Campgrounds = new List<CampgroundDetailModel>();
        }

        public CampgroundModel Campground { get; set; }

        public IList<CampgroundDetailModel> Campgrounds { get; set; }

        public bool DisplayCaptcha { get; set; }

        public bool TermsOfServiceEnabled { get; set; }
        public bool TermsOfServicePopup { get; set; }

        public bool DisableFormInput { get; set; }
        public string Result { get; set; }

    }
}
