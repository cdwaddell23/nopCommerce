using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Messages;
using Nop.Core.Domain.Stores;
using Nop.Plugin.Campgrounds.Data.Domain;
using Nop.Services.Events;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Seo;

namespace Nop.Plugin.Campgrounds.Services.Messaging
{
    /// <summary>
    /// Message token provider
    /// </summary>
    public partial class CampgroundMessageTokenProvider : ICampgroundMessageTokenProvider
    {
        #region Fields

        private readonly ICampgroundService _campgroundService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly ICampgroundAttributeTypeParser _campgroundAttributeTypeParser;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly IEventPublisher _eventPublisher;

        private readonly MessageTemplatesSettings _templatesSettings;

        private Dictionary<string, IEnumerable<string>> _allowedTokens;

        #endregion

        #region Ctor

        public CampgroundMessageTokenProvider(
            ICampgroundService campgroundService,
            ILanguageService languageService,
            ILocalizationService localizationService, 
            IDateTimeHelper dateTimeHelper,
            ICampgroundAttributeTypeParser campgroundAttributeTypeParser,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor,
            IEventPublisher eventPublisher, 
            MessageTemplatesSettings templatesSettings)
        {
            this._campgroundService = campgroundService;
            this._languageService = languageService;
            this._localizationService = localizationService;
            this._campgroundAttributeTypeParser = campgroundAttributeTypeParser;
            this._urlHelperFactory = urlHelperFactory;
            this._actionContextAccessor = actionContextAccessor;
            this._templatesSettings = templatesSettings;
            this._eventPublisher = eventPublisher;
        }

        #endregion

        #region Allowed tokens

        /// <summary>
        /// Get all available tokens by token groups
        /// </summary>
        protected Dictionary<string, IEnumerable<string>> AllowedTokens
        {
            get
            {
                if (_allowedTokens != null)
                    return _allowedTokens;

                _allowedTokens = new Dictionary<string, IEnumerable<string>>();

                //campground tokens
                _allowedTokens.Add(CampgroundTokenGroupNames.CampgroundTokens, new[]
                {
                    "%Campground.ID%",
                    "%Campground.CampgroundName%",
                    "%Campground.ShortDescription%",
                    "%Campground.CampgroundURL%",
                });

                //campground review tokens
                _allowedTokens.Add(CampgroundTokenGroupNames.CampgroundReviewTokens, new[]
                {
                    "%CampgroundReview.CampgroundName%"
                });


                //contact campground host tokens
                _allowedTokens.Add(CampgroundTokenGroupNames.ContactCampgroundHost, new[]
                {
                    "%CampgroundHost.Name%",
                    "%CampgroundHost.Email%"
                });

                return _allowedTokens;
            }
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Get UrlHelper
        /// </summary>
        /// <returns>UrlHelper</returns>
        protected virtual IUrlHelper GetUrlHelper()
        {
            return _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);
        }

        /// <summary>
        /// Get store URL
        /// </summary>
        /// <param name="storeId">Store identifier; Pass 0 to load URL of the current store</param>
        /// <param name="removeTailingSlash">A value indicating whether to remove a tailing slash</param>
        /// <returns>Store URL</returns>
        protected virtual string GetCampgroundUrl(int campgroundId = 0, bool removeTailingSlash = true)
        {
            var campground = _campgroundService.GetCampgroundById(campgroundId) ?? null;

            if (campground == null)
                throw new Exception("No campground could be loaded");

            var url = campground.Website;
            if (string.IsNullOrEmpty(url))
                throw new Exception("URL cannot be null");

            if (url.EndsWith("/"))
                url = url.Remove(url.Length - 1);

            return url;
        }

        #endregion

        #region Methods


        /// <summary>
        /// Add campgroundHost tokens
        /// </summary>
        /// <param name="tokens">List of already added tokens</param>
        /// <param name="campgroundHost">CampgroundHost</param>
        public virtual void AddCampgroundHostTokens(IList<Token> tokens, CampgroundHost campgroundHost)
        {
            tokens.Add(new Token("CampgroundHost.Username", campgroundHost.Customer.Username));
            tokens.Add(new Token("CampgroundHost.Email", campgroundHost.Customer.Email));

            //event notification
            _eventPublisher.EntityTokensAdded(campgroundHost, tokens);
        }

        /// <summary>
        /// Add campground review tokens
        /// </summary>
        /// <param name="tokens">List of already added tokens</param>
        /// <param name="campgroundReview">Campground review</param>
        public virtual void AddCampgroundReviewTokens(IList<Token> tokens, CampgroundReview campgroundReview)
        {
            tokens.Add(new Token("CampgroundReview.CampgroundName", campgroundReview.Campground.Name));

            //event notification
            _eventPublisher.EntityTokensAdded(campgroundReview, tokens);
        }

         /// <summary>
        /// Add campground tokens
        /// </summary>
        /// <param name="tokens">List of already added tokens</param>
        /// <param name="campground">Campground</param>
        /// <param name="languageId">Language identifier</param>
        public virtual void AddCampgroundTokens(IList<Token> tokens, Campground campground, int languageId)
        {
            tokens.Add(new Token("Campground.ID", campground.Id));
            tokens.Add(new Token("Campground.CampgroundName", campground.GetLocalized(x => x.Name, languageId)));
            tokens.Add(new Token("Campground.ShortDescription", campground.GetLocalized(x => x.ShortDescription, languageId), true));
            
            var campgroundUrl = $"{GetCampgroundUrl(campground.Id)}{GetUrlHelper().RouteUrl("Campground", new { SeName = campground.GetSeName() })}";
            tokens.Add(new Token("Campground.CampgroundURL", campgroundUrl, true));

            //event notification
            _eventPublisher.EntityTokensAdded(campground, tokens);
        }



        /// <summary>
        /// Get collection of allowed (supported) message tokens for campaigns
        /// </summary>
        /// <returns>Collection of allowed (supported) message tokens for campaigns</returns>
        public virtual IEnumerable<string> GetListOfCampaignAllowedTokens()
        {
            var additionTokens = new CampaignAdditionTokensAddedEvent();
            _eventPublisher.Publish(additionTokens);

            var allowedTokens = GetListOfAllowedTokens(new[] { CampgroundTokenGroupNames.CampgroundTokens, TokenGroupNames.SubscriptionTokens }).ToList();
            allowedTokens.AddRange(additionTokens.AdditionTokens);

            return allowedTokens.Distinct();
        }

        /// <summary>
        /// Get collection of allowed (supported) message tokens
        /// </summary>
        /// <param name="tokenGroups">Collection of token groups; pass null to get all available tokens</param>
        /// <returns>Collection of allowed message tokens</returns>
        public virtual IEnumerable<string> GetListOfAllowedTokens(IEnumerable<string> tokenGroups = null)
        {
            var additionTokens = new AdditionTokensAddedEvent();
            _eventPublisher.Publish(additionTokens);

            var allowedTokens = AllowedTokens.Where(x => tokenGroups == null || tokenGroups.Contains(x.Key))
                .SelectMany(x => x.Value).ToList();

            allowedTokens.AddRange(additionTokens.AdditionTokens);

            return allowedTokens.Distinct();
        }
        
        #endregion
    }
}
