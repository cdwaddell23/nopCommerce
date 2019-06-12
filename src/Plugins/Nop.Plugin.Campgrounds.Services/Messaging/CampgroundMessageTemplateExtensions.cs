using System.Collections.Generic;
using Nop.Core.Domain.Messages;
using Nop.Services.Messages;

namespace Nop.Plugin.Campgrounds.Services.Messaging
{
    /// <summary>
    /// Represents message template  extensions
    /// </summary>
    public static class MessageTemplateExtensions
    {
        /// <summary>
        /// Get token groups of message template
        /// </summary>
        /// <param name="messageTemplate">Message template</param>
        /// <returns>Collection of token group names</returns>
        public static IEnumerable<string> GetTokenGroups(this MessageTemplate messageTemplate)
        {
            //groups depend on which tokens are added at the appropriate methods in IWorkflowMessageService
            switch (messageTemplate.Name)
            {
                case CampgroundMessageTemplateSystemNames.NewCampgroundHostAccountApplyStoreOwnerNotification:
                    return new[] { TokenGroupNames.StoreTokens, TokenGroupNames.CustomerTokens, CampgroundTokenGroupNames.CampgroundHostTokens };

                case CampgroundMessageTemplateSystemNames.CampgroundHostInformationChangeNotification:
                    return new[] { TokenGroupNames.StoreTokens, CampgroundTokenGroupNames.CampgroundHostTokens };

                case CampgroundMessageTemplateSystemNames.CampgroundReviewNotification:
                    return new[] { TokenGroupNames.StoreTokens, CampgroundTokenGroupNames.CampgroundReviewTokens, TokenGroupNames.CustomerTokens };

                case CampgroundMessageTemplateSystemNames.ContactCampgroundHostMessage:
                    return new[] { TokenGroupNames.StoreTokens, CampgroundTokenGroupNames.ContactCampgroundHost };

                default:
                    return new string[] { };
            }
        }
    }
}