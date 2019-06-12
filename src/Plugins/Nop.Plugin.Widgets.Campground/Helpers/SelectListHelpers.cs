using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Caching;
using Nop.Plugin.Widgets.Campgrounds.Infrastructure.Cache;
using Nop.Plugin.Campgrounds.Services;
using Nop.Services.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Services.Common;
using Nop.Core.Domain.Customers;

namespace Nop.Plugin.Widgets.Campgrounds.Helpers
{
    /// <summary>
    /// Select list helper
    /// </summary>
    public static class SelectListHelper
    {
        /// <summary>
        /// Get category list
        /// </summary>
        /// <param name="categoryService">Category service</param>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Category list</returns>
        public static List<SelectListItem> GetStateCategoryList(ICategoryService categoryService, ICacheManager cacheManager, bool showHidden = false)
        {
            if (categoryService == null)
                throw new ArgumentNullException(nameof(categoryService));

            if (cacheManager == null)
                throw new ArgumentNullException(nameof(cacheManager));

            var cacheKey = string.Format(ModelCacheEventConsumer.CAMPGROUND_CATEGORIES_LIST_KEY, showHidden);
            var listItems = cacheManager.Get(cacheKey, () =>
            {
                var categories = categoryService.GetAllCategories(showHidden: showHidden);
                return categories.Where(c => c.ParentCategoryId == 2).Select(c => new SelectListItem
                {
                    Text = c.GetFormattedBreadCrumb(categories),
                    Value = c.Id.ToString()
                });
            });

            var result = new List<SelectListItem>();
            //clone the list to ensure that "selected" property is not set
            foreach (var item in listItems)
            {
                result.Add(new SelectListItem
                {
                    Text = item.Text,
                    Value = item.Value
                });
            }

            return result;
        }

        /// <summary>
        /// Get campground host list
        /// </summary>
        /// <param name="campgroundHostService">campgroundHost service</param>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>CampgroundHost list</returns>
        public static List<SelectListItem> GetCampgroundHostList(ICampgroundHostService campgroundHostService, ICacheManager cacheManager, bool showHidden = false, int selectedId = 0)
        {
            if (campgroundHostService == null)
                throw new ArgumentNullException(nameof(campgroundHostService));

            if (cacheManager == null)
                throw new ArgumentNullException(nameof(cacheManager));

            var cacheKey = string.Format(ModelCacheEventConsumer.CAMPGROUND_CAMPGROUNDHOST_LIST_KEY, showHidden);
            var listItems = cacheManager.Get(cacheKey, () =>
            {
                var campgroundHosts = campgroundHostService.GetAllCampgroundHosts(showHidden: showHidden);
                return campgroundHosts.Select(v => new SelectListItem
                {
                    Text = v.Customer.GetAttribute<string>(SystemCustomerAttributeNames.FirstName) + " "  + v.Customer.GetAttribute<string>(SystemCustomerAttributeNames.LastName) + " (" + v.Customer.Email + ")",
                    Value = v.Id.ToString(),
                    Selected = selectedId == v.Id
                });
            });

            var result = new List<SelectListItem>();
            //clone the list to ensure that "selected" property is not set
            foreach (var item in listItems)
            {
                result.Add(new SelectListItem
                {
                    Text = item.Text,
                    Value = item.Value,
                    Selected = item.Selected
                });
            }

            return result;
        }

        /// <summary>
        /// Get campground type list
        /// </summary>
        /// <param name="campgroundTypeService">campgroundType service</param>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>CampgroundType list</returns>
        public static List<SelectListItem> GetCampgroundTypeList(ICampgroundTypeService campgroundTypeService, ICacheManager cacheManager, bool showHidden = false)
        {
            if (campgroundTypeService == null)
                throw new ArgumentNullException(nameof(campgroundTypeService));

            if (cacheManager == null)
                throw new ArgumentNullException(nameof(cacheManager));

            var cacheKey = string.Format(ModelCacheEventConsumer.CAMPGROUND_CAMPGROUNDTYPE_LIST_KEY, showHidden);
            var listItems = cacheManager.Get(cacheKey, () =>
            {
                var campgroundTypes = campgroundTypeService.GetAllCampgroundTypes(showHidden: showHidden);
                return campgroundTypes.Select(v => new SelectListItem
                {
                    Text = v.Description,
                    Value = v.Id.ToString()
                });
            });

            var result = new List<SelectListItem>();
            //clone the list to ensure that "selected" property is not set
            foreach (var item in listItems)
            {
                result.Add(new SelectListItem
                {
                    Text = item.Text,
                    Value = item.Value
                });
            }

            return result;
        }

    }
}
