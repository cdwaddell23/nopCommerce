using Nop.Plugin.Campgrounds.Data.Domain;

namespace Nop.Plugin.Campgrounds.Services
{
    /// <summary>
    /// CampgroundAddress service interface
    /// </summary>
    public partial interface ICampgroundAddressService
    {
        /// <summary>
        /// Deletes an campgroundAddress
        /// </summary>
        /// <param name="campgroundAddress">CampgroundAddress</param>
        void DeleteCampgroundAddress(CampgroundAddress campgroundAddress);

        /// <summary>
        /// Gets total number of campgroundAddresses by country identifier
        /// </summary>
        /// <param name="countryId">Country identifier</param>
        /// <returns>Number of campgroundAddresses</returns>
        int GetCampgroundAddressTotalByCountryId(int countryId);

        /// <summary>
        /// Gets total number of campgroundAddresses by state/province identifier
        /// </summary>
        /// <param name="stateProvinceId">State/province identifier</param>
        /// <returns>Number of campgroundAddresses</returns>
        int GetCampgroundAddressTotalByStateProvinceId(int stateProvinceId);

        /// <summary>
        /// Gets an campgroundAddress by campgroundAddress identifier
        /// </summary>
        /// <param name="campgroundAddressId">CampgroundAddress identifier</param>
        /// <returns>CampgroundAddress</returns>
        CampgroundAddress GetCampgroundAddressById(int campgroundAddressId);

        /// <summary>
        /// Inserts an campgroundAddress
        /// </summary>
        /// <param name="campgroundAddress">CampgroundAddress</param>
        void InsertCampgroundAddress(CampgroundAddress campgroundAddress);

        /// <summary>
        /// Updates the campgroundAddress
        /// </summary>
        /// <param name="campgroundAddress">CampgroundAddress</param>
        void UpdateCampgroundAddress(CampgroundAddress campgroundAddress);

        /// <summary>
        /// Gets a value indicating whether campgroundAddress is valid (can be saved)
        /// </summary>
        /// <param name="campgroundAddress">CampgroundAddress to validate</param>
        /// <returns>Result</returns>
        bool IsCampgroundAddressValid(CampgroundAddress campgroundAddress);
    }
}