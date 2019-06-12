using Nop.Core.Domain.Customers;
using Nop.Plugin.Campgrounds.Data.Domain;

namespace Nop.Plugin.Campgrounds.Services.Messaging
{
    /// <summary>
    /// Workflow message service
    /// </summary>
    public partial interface ICampgroundWorkflowMessageService
    {
        #region Misc

        /// <summary>
        /// Sends 'New vendor account submitted' message to a store owner
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="vendor">Vendor</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        int SendNewCampgroundAccountApplyStoreOwnerNotification(Customer customer, CampgroundHost campgroundHost, int languageId);

        /// <summary>
        /// Sends 'Vendor information change' message to a store owner
        /// </summary>
        /// <param name="vendor">Vendor</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        int SendCampgroundInformationChangeNotification(CampgroundHost campgroundHost, int languageId);

        /// <summary>
        /// Sends a product review notification message to a store owner
        /// </summary>
        /// <param name="productReview">Product review</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        int SendCampgroundReviewNotificationMessage(CampgroundReview campgroundReview, int languageId);

        #endregion
    }
}
