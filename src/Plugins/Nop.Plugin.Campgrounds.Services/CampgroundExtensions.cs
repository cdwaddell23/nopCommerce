using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Nop.Core.Html;
using Nop.Plugin.Campgrounds.Data.Domain;
using Nop.Services.Localization;
using Nop.Services.Shipping.Date;

namespace Nop.Plugin.Campgrounds.Services
{
    /// <summary>
    /// Extensions
    /// </summary>
    public static class CampgroundExtensions
    {
        #region Utilities
        public static void FormatCampgroundURL(this Campground campground)
        {
            if (campground == null)
                throw new ArgumentNullException(nameof(campground));
            
            if (!string.IsNullOrEmpty(campground.Website))
            {
                var text = campground.Website.Contains('?') ? campground.Website.Split('?')[0].Replace("https://", string.Empty).Replace("http://", string.Empty).TrimEnd('/') : campground.Website.Replace("https://", string.Empty).Replace("http://", string.Empty).TrimEnd('/');
                campground.Website = text;
            }
        }

        public static string ToUrlSlug(this string value)
        {

            //First to lower case 
            value = value.ToLowerInvariant();

            //Remove all accents
            var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(value);

            value = Encoding.ASCII.GetString(bytes);

            //Replace spaces 
            value = Regex.Replace(value, @"\s", "-", RegexOptions.Compiled);

            //Remove invalid chars 
            value = Regex.Replace(value, @"[^\w\s\p{Pd}]", "", RegexOptions.Compiled);

            //Trim dashes from end 
            value = value.Trim('-', '_');

            //Replace double occurences of - or \_ 
            value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);

            return value;
        }
        #endregion

        #region Methods


        /// <summary>
        /// Finds a related campground item by specified identifiers
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="campgroundId1">The first campground identifier</param>
        /// <param name="campgroundId2">The second campground identifier</param>
        /// <returns>Related campground</returns>
        public static RelatedCampground FindRelatedCampground(this IList<RelatedCampground> source,
            int campgroundId1, int campgroundId2)
        {
            foreach (var relatedCampground in source)
                if (relatedCampground.CampgroundId1 == campgroundId1 && relatedCampground.CampgroundId2 == campgroundId2)
                    return relatedCampground;
            return null;
        }

        /// <summary>
        /// Finds a cross-sell campground item by specified identifiers
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="campgroundId1">The first campground identifier</param>
        /// <param name="campgroundId2">The second campground identifier</param>
        /// <returns>Cross-sell campground</returns>
        public static CrossSellCampground FindCrossSellCampground(this IList<CrossSellCampground> source,
            int campgroundId1, int campgroundId2)
        {
            foreach (var crossSellCampground in source)
                if (crossSellCampground.CampgroundId1 == campgroundId1 && crossSellCampground.CampgroundId2 == campgroundId2)
                    return crossSellCampground;
            return null;
        }

        /// <summary>
        /// Indicates whether a campground tag exists
        /// </summary>
        /// <param name="campground">Campground</param>
        /// <param name="campgroundTagId">Campground tag identifier</param>
        /// <returns>Result</returns>
        public static bool CampgroundTagExists(this Campground campground,
            int campgroundTagId)
        {
            if (campground == null)
                throw new ArgumentNullException(nameof(campground));

            var result = campground.CampgroundTags.ToList().Find(pt => pt.Id == campgroundTagId) != null;
            return result;
        }

        /// <summary>
        /// Formats start/end date for rental campground
        /// </summary>
        /// <param name="campground">Campground</param>
        /// <param name="date">Date</param>
        /// <returns>Formatted date</returns>
        public static string FormatRentalDate(this Campground campground, DateTime date)
        {
            if (campground == null)
                throw new ArgumentNullException(nameof(campground));

            if (!campground.IsRental)
                return null;

            return date.ToShortDateString();
        }

        #endregion
    }
}
