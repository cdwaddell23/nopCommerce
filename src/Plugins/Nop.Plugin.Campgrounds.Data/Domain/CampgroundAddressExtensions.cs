using System;

namespace Nop.Plugin.Campgrounds.Data.Domain
{
    /// <summary>
    /// Campground extensions
    /// </summary>
    public static class CampgroundAddressExtensions
    {
        private static string FormatPhoneNumber(string phoneNumber)
        {
            if (phoneNumber == null)
                return phoneNumber;

            phoneNumber = new System.Text.RegularExpressions.Regex(@"\D")
                    .Replace(phoneNumber, string.Empty);

            if (phoneNumber.Length == 7)
                return Convert.ToInt64(phoneNumber).ToString("###-####");
            if (phoneNumber.Length == 10)
                return Convert.ToInt64(phoneNumber).ToString("(###)###-####");
            if (phoneNumber.Length > 10)
                return Convert.ToInt64(phoneNumber)
                    .ToString("###-###-#### Ext: " + new String('#', (phoneNumber.Length - 10)));

            return phoneNumber;
        }

        /// <summary>
        /// Format phone number to standard (###) ###-#### format
        /// </summary>
        /// <param name="campgroundAddress"></param>
        /// <returns></returns>
        public static string FormatPhone(this CampgroundAddress campgroundAddress)
        {
            return FormatPhoneNumber(campgroundAddress.PhoneNumber);
        }

        public static string FormatFax(this CampgroundAddress campgroundAddress)
        {
            return FormatPhoneNumber(campgroundAddress.FaxNumber);
        }

    }
}
