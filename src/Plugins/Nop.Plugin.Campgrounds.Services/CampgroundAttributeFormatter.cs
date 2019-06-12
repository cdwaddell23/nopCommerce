using System.Net;
using System.Text;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;
using Nop.Core.Html;
using Nop.Plugin.Campgrounds.Data.Domain;
using Nop.Services.Directory;
using Nop.Services.Localization;

namespace Nop.Plugin.Campgrounds.Services
{
    /// <summary>
    /// Campground attribute formatter
    /// </summary>
    public partial class CampgroundAttributeTypeFormatter : ICampgroundAttributeTypeFormatter
    {
        private readonly IWorkContext _workContext;
        private readonly ICampgroundAttributeTypeService _campgroundAttributeTypeService;
        private readonly ICampgroundAttributeTypeParser _campgroundAttributeTypeParser;
        private readonly ICurrencyService _currencyService;
        private readonly ILocalizationService _localizationService;
        private readonly IWebHelper _webHelper;
        private readonly ShoppingCartSettings _shoppingCartSettings;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="workContext">Work context</param>
        /// <param name="campgroundAttributeTypeService">Campground attribute service</param>
        /// <param name="campgroundAttributeTypeParser">Campground attribute parser</param>
        /// <param name="currencyService">Currency service</param>
        /// <param name="localizationService">Localization service</param>
        /// <param name="taxService">Tax service</param>
        /// <param name="priceFormatter"> Price formatter</param>
        /// <param name="downloadService">Download service</param>
        /// <param name="webHelper">Web helper</param>
        /// <param name="priceCalculationService">Price calculation service</param>
        /// <param name="shoppingCartSettings">Shopping cart settings</param>
        public CampgroundAttributeTypeFormatter(IWorkContext workContext,
            ICampgroundAttributeTypeService campgroundAttributeTypeService,
            ICampgroundAttributeTypeParser campgroundAttributeTypeParser,
            ICurrencyService currencyService,
            ILocalizationService localizationService,
            IWebHelper webHelper,
            ShoppingCartSettings shoppingCartSettings)
        {
            this._workContext = workContext;
            this._campgroundAttributeTypeService = campgroundAttributeTypeService;
            this._campgroundAttributeTypeParser = campgroundAttributeTypeParser;
            this._currencyService = currencyService;
            this._localizationService = localizationService;
            this._webHelper = webHelper;
            this._shoppingCartSettings = shoppingCartSettings;
        }

        /// <summary>
        /// Formats attributes
        /// </summary>
        /// <param name="campground">Campground</param>
        /// <param name="attributesXml">Attributes in XML format</param>
        /// <returns>Attributes</returns>
        public virtual string FormatAttributes(Campground campground, string attributesXml)
        {
            var customer = (Customer)_workContext.CurrentCustomer;
            return FormatAttributes(campground, attributesXml, customer);
        }
        
        /// <summary>
        /// Formats attributes
        /// </summary>
        /// <param name="campground">Campground</param>
        /// <param name="attributesXml">Attributes in XML format</param>
        /// <param name="customer">Customer</param>
        /// <param name="separator">Separator</param>
        /// <param name="htmlEncode">A value indicating whether to encode (HTML) values</param>
        /// <param name="renderPrices">A value indicating whether to render prices</param>
        /// <param name="renderCampgroundAttributeTypes">A value indicating whether to render campground attributes</param>
        /// <param name="renderGiftCardAttributes">A value indicating whether to render gift card attributes</param>
        /// <param name="allowHyperlinks">A value indicating whether to HTML hyperink tags could be rendered (if required)</param>
        /// <returns>Attributes</returns>
        public virtual string FormatAttributes(Campground campground, string attributesXml,
            Customer customer, string separator = "<br />", bool htmlEncode = true, bool renderPrices = true,
            bool renderCampgroundAttributeTypes = true, bool renderGiftCardAttributes = true,
            bool allowHyperlinks = true)
        {
            var result = new StringBuilder();

            //attributes
            if (renderCampgroundAttributeTypes)
            {
                foreach (var attribute in _campgroundAttributeTypeParser.ParseCampgroundAttributeMappings(attributesXml))
                {
                    //attributes without values
                    if (!attribute.ShouldHaveValues())
                    {
                        foreach (var value in _campgroundAttributeTypeParser.ParseValues(attributesXml, attribute.Id))
                        {
                            var formattedAttribute = string.Empty;
                            if (attribute.AttributeControlType == AttributeControlType.MultilineTextbox)
                            {
                                //multiline textbox
                                var attributeName = attribute.CampgroundAttributeType.GetLocalized(a => a.Name, _workContext.WorkingLanguage.Id);

                                //encode (if required)
                                if (htmlEncode)
                                    attributeName = WebUtility.HtmlEncode(attributeName);

                                //we never encode multiline textbox input
                                formattedAttribute = $"{attributeName}: {HtmlHelper.FormatText(value, false, true, false, false, false, false)}";
                            }
                            else
                            {
                                //other attributes (textbox, datepicker)
                                formattedAttribute = $"{attribute.CampgroundAttributeType.GetLocalized(a => a.Name, _workContext.WorkingLanguage.Id)}: {value}";

                                //encode (if required)
                                if (htmlEncode)
                                    formattedAttribute = WebUtility.HtmlEncode(formattedAttribute);
                            }

                            if (!string.IsNullOrEmpty(formattedAttribute))
                            {
                                if (result.Length > 0)
                                    result.Append(separator);
                                result.Append(formattedAttribute);
                            }
                        }
                    }
                    //campground attribute values
                    else
                    {
                        foreach (var attributeValue in _campgroundAttributeTypeParser.ParseCampgroundAttributeValues(attributesXml, attribute.Id))
                        {
                            var formattedAttribute = $"{attribute.CampgroundAttributeType.GetLocalized(a => a.Name, _workContext.WorkingLanguage.Id)}: {attributeValue.GetLocalized(a => a.Name, _workContext.WorkingLanguage.Id)}";

                            //display quantity
                            if (_shoppingCartSettings.RenderAssociatedAttributeValueQuantity && attributeValue.CampgroundAttributeValueType == CampgroundAttributeValueType.AssociatedToCampground)
                            {
                                //render only when more than 1
                                if (attributeValue.Quantity > 1)
                                    formattedAttribute += string.Format(_localizationService.GetResource("CampgroundAttributeTypes.Quantity"), attributeValue.Quantity);
                            }

                            //encode (if required)
                            if (htmlEncode)
                                formattedAttribute = WebUtility.HtmlEncode(formattedAttribute);

                            if (!string.IsNullOrEmpty(formattedAttribute))
                            {
                                if (result.Length > 0)
                                    result.Append(separator);
                                result.Append(formattedAttribute);
                            }
                        }
                    }
                }
            }

            return result.ToString();
        }
    }
}
