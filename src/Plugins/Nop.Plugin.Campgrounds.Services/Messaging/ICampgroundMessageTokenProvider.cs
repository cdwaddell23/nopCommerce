using System.Collections.Generic;
using Nop.Plugin.Campgrounds.Data.Domain;
using Nop.Services.Messages;

namespace Nop.Plugin.Campgrounds.Services.Messaging
{
    /// <summary>
    /// Message token provider
    /// </summary>
    public partial interface ICampgroundMessageTokenProvider
    {
        /// <summary>
        /// Add campgroundHost tokens
        /// </summary>
        /// <param name="tokens">List of already added tokens</param>
        /// <param name="campgroundHost">CampgroundHost</param>
        void AddCampgroundHostTokens(IList<Token> tokens, CampgroundHost campgroundHost);

        /// <summary>
        /// Add campground review tokens
        /// </summary>
        /// <param name="tokens">List of already added tokens</param>
        /// <param name="campgroundReview">Campground review</param>
        void AddCampgroundReviewTokens(IList<Token> tokens, CampgroundReview campgroundReview);

        /// <summary>
        /// Add campground tokens
        /// </summary>
        /// <param name="tokens">List of already added tokens</param>
        /// <param name="campground">Campground</param>
        /// <param name="languageId">Language identifier</param>
        void AddCampgroundTokens(IList<Token> tokens, Campground campground, int languageId);


        /// <summary>
        /// Get collection of allowed (supported) message tokens for campaigns
        /// </summary>
        /// <returns>Collection of allowed (supported) message tokens for campaigns</returns>
        IEnumerable<string> GetListOfCampaignAllowedTokens();

        /// <summary>
        /// Get collection of allowed (supported) message tokens
        /// </summary>
        /// <param name="tokenGroups">Collection of token groups; pass null to get all available tokens</param>
        /// <returns>Collection of allowed message tokens</returns>
        IEnumerable<string> GetListOfAllowedTokens(IEnumerable<string> tokenGroups = null);
    }
}
