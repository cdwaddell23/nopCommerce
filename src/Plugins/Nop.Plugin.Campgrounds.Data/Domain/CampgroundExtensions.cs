using System;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Plugin.Campgrounds.Data.Domain
{
    /// <summary>
    /// Campground extensions
    /// </summary>
    public static class CampgroundExtensions
    {
        /// <summary>
        /// Parse "required campground Ids" property
        /// </summary>
        /// <param name="campground">Campground</param>
        /// <returns>A list of required campground IDs</returns>
        //public static int[] ParseRequiredCampgroundIds(this Campground campground)
        //{
        //    if (campground == null)
        //        throw new ArgumentNullException("campground");

        //    if (String.IsNullOrEmpty(campground.RequiredCampgroundIds))
        //        return new int[0];

        //    var ids = new List<int>();

        //    foreach (var idStr in campground.RequiredCampgroundIds
        //        .Split(new [] { ',' }, StringSplitOptions.RemoveEmptyEntries)
        //        .Select(x => x.Trim()))
        //    {
        //        int id;
        //        if (int.TryParse(idStr, out id))
        //            ids.Add(id);
        //    }

        //    return ids.ToArray();
        //}

        /// <summary>
        /// Get a value indicating whether a campground is available now (availability dates)
        /// </summary>
        /// <param name="campground">Campground</param>
        /// <returns>Result</returns>
        public static bool IsAvailable(this Campground campground)
        {
            return IsAvailable(campground, DateTime.UtcNow);
        }

        /// <summary>
        /// Get a value indicating whether a campground is available now (availability dates)
        /// </summary>
        /// <param name="campground">Campground</param>
        /// <param name="dateTime">Datetime to check</param>
        /// <returns>Result</returns>
        public static bool IsAvailable(this Campground campground, DateTime dateTime)
        {
            if (campground == null)
                throw new ArgumentNullException("campground");

            if (campground.AvailableStartDateTimeUtc.HasValue && campground.AvailableStartDateTimeUtc.Value > dateTime)
            {
                return false;
            }

            if (campground.AvailableEndDateTimeUtc.HasValue && campground.AvailableEndDateTimeUtc.Value < dateTime)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Remove address
        /// </summary>
        /// <param name="campground"></param>
        /// <param name="campgroundAddress"></param>
        public static void RemoveAddress(this Campground campground, CampgroundAddress campgroundAddress)
        {
            if (campground.BillingAddress == campgroundAddress) campground.BillingAddress = null;
            if (campground.CampgroundAddress == campgroundAddress) campground.CampgroundAddress = null;
        }


    }
}
