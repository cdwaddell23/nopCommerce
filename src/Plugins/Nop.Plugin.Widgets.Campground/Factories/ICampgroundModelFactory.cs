using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Domain.Catalog;
using Nop.Plugin.Campgrounds.Data.Domain;
using Nop.Plugin.Widgets.Campgrounds.Models;
using Nop.Web.Models.Media;
using System.Collections.Generic;

namespace Nop.Plugin.Widgets.Campgrounds.Factories
{
    /// <summary>
    /// Represents the interface of the campground model factory
    /// </summary>
    public partial interface ICampgroundModelFactory
    {
        #region Common

        /// <summary>
        /// Prepare sorting options
        /// </summary>
        /// <param name="pagingFilteringModel">Catalog paging filtering model</param>
        /// <param name="command">Catalog paging filtering command</param>
        void PrepareSortingOptions(CampgroundPagingFilteringModel pagingFilteringModel, CampgroundPagingFilteringModel command);

        /// <summary>
        /// Get a Select Items list of available states
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="showHidden"></param>
        /// <returns></returns>
        IList<SelectListItem> PrepareAvailableStates(int Id = 0, bool showHidden = false);

        IList<SelectListItem> PrepareAvailableCampgroundHosts(bool showHidden = true, int selectedId = 0);

        IList<SelectListItem> PrepareAvailableCampgroundTypes(bool showHidden = true);

        IList<SelectListItem> PrepareAvailableStateCategories(bool showHidden = true);

        /// <summary>
        /// Prepare view modes
        /// </summary>
        /// <param name="pagingFilteringModel">Catalog paging filtering model</param>
        /// <param name="command">Catalog paging filtering command</param>
        void PrepareViewModes(CampgroundPagingFilteringModel pagingFilteringModel, CampgroundPagingFilteringModel command);

        /// <summary>
        /// Prepare page size options
        /// </summary>
        /// <param name="pagingFilteringModel">Catalog paging filtering model</param>
        /// <param name="command">Catalog paging filtering command</param>
        /// <param name="allowCustomersToSelectPageSize">Are customers allowed to select page size?</param>
        /// <param name="pageSizeOptions">Page size options</param>
        /// <param name="fixedPageSize">Fixed page size</param>
        void PreparePageSizeOptions(CampgroundPagingFilteringModel pagingFilteringModel, CampgroundPagingFilteringModel command,
            bool allowCustomersToSelectPageSize, string pageSizeOptions, int fixedPageSize);

        #endregion

        #region Campgrounds

        PictureModel PrepareCampgroundDefaultPictureModel(Campground campground, int pictureSize = 450, int thumbPictureSize = 150);
        PictureModel PrepareCampgroundDefaultPictureModel(Category category);

        HomePageBlogItemsModel PrepareHomePageBlogItemsModel();

        /// <summary>
        /// Prepare campground model
        /// </summary>
        /// <param name="model"></param>
        /// <param name="campground"></param>
        /// <param name="setPredefinedValues"></param>
        /// <param name="excludeProperties"></param>
        void PrepareCampgroundModel(CampgroundModel model, Campground campground, bool setPredefinedValues = false, bool excludeProperties = false);

        /// <summary>
        /// Prepare campground model
        /// </summary>
        /// <param name="category">Category</param>
        /// <param name="command">Catalog paging filtering command</param>
        /// <returns>Campground model</returns>
        CampgroundDetailModel PrepareCampgroundDetailModel(Campground campground, CampgroundPagingFilteringModel command, Category category = null, bool searchNearby = true);
        /// <summary>
        /// Prepare campground detail list
        /// </summary>
        /// <param name="campgrounds"></param>
        /// <returns></returns>
        IEnumerable<CampgroundDetailModel> PrepareCampgroundDetailModels(IEnumerable<Campground> campgrounds, Category category = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="command"></param>
        /// <param name="category"></param>
        /// <param name="searchNearby"></param>
        /// <returns></returns>
        CampgroundDetailModel PrepareCampingNearby(decimal latitude, decimal longitude, CampgroundPagingFilteringModel command, Campground campground = null, CampgroundDetailModel campgroundDetail = null, Category category = null, bool searchNearby = true);

        /// <summary>
        /// Prepare campground menu model
        /// </summary>
        /// <param name="parentCategoryId"></param>
        /// <returns></returns>
        CampgroundCategoryModel PrepareCampgroundCategoryModels(int categoryId);

        /// <summary>
        /// Prepare campground address model
        /// </summary>
        /// <param name="model"></param>
        /// <param name="campground"></param>
        /// <param name="setPredefinedValues"></param>
        /// <param name="excludeProperties"></param>
        void PrepareCampgroundAddressModel(CampgroundModel model, Campground campground, bool setPredefinedValues = false, bool excludeProperties = false);

        /// <summary>
        /// Prepare campground overview model
        /// </summary>
        /// <param name="category">Category</param>
        /// <param name="command">Catalog paging filtering command</param>
        /// <returns>Campground model</returns>
        CampgroundOverviewModel PrepareCampgroundOverviewModel(Category category, CampgroundPagingFilteringModel command);

        /// <summary>
        /// Get the campground template view path
        /// </summary>
        /// <param name="campground">Campground</param>
        /// <returns>View path</returns>
        string PrepareCampgroundTemplateViewPath(Campground campground);

        /// <summary>
        /// Prepare the customer campground reviews
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        CustomerCampgroundReviewsModel PrepareCustomerCampgroundReviewsModel(int? page);

        /// <summary>
        /// Get the campground template view path
        /// </summary>
        /// <param name="currentCategoryId">currentCategoryId</param>
        /// <param name="currentCampgroundId">currentCampgroundId</param>
        /// <returns>View path</returns>
        CampgroundNavigationModel PrepareCampgroundNavigationModel(int currentCategoryId, int currentCampgroundId);

        /// <summary>
        /// Prepare top menu model
        /// </summary>
        /// <returns>Top menu model</returns>
        CampgroundCategoryModel PrepareCampgroundCategoryModel();

        /// <summary>
        /// Prepare homepage campground models
        /// </summary>
        /// <returns>List of homepage campground models</returns>
        List<CampgroundDetailModel> PrepareHomepageCampgroundDetailModels();

        /// <summary>
        /// Campground reviews
        /// </summary>
        /// <param name="model"></param>
        /// <param name="campground"></param>
        /// <returns></returns>
        CampgroundReviewsModel PrepareCampgroundReviewsModel(CampgroundReviewsModel model, Campground campground);

        /// <summary>
        /// Searh Campgrounds
        /// </summary>
        /// <param name="sTerms"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        CampgroundOverviewModel PrepareCampgroundSearchModel(Category category, string sTerms, CampgroundPagingFilteringModel command);

        /// <summary>
        /// Prepare Campground Host Apply Model
        /// </summary>
        /// <param name="model"></param>
        /// <param name="validateVendor"></param>
        /// <param name="excludeProperties"></param>
        /// <returns></returns>
        CampgroundRegisterModel PrepareCampgroundRegisterModel(CampgroundRegisterModel model, bool validateVendor, bool excludeProperties);
        #endregion

        #region Categories


        /// <summary>
        /// Prepare campground (simple) models
        /// </summary>
        /// <returns>List of campground (simple) models</returns>
        //List<CampgroundSimpleModel> PrepareCampgroundSimpleModels();

        /// <summary>
        /// Prepare campground (simple) models
        /// </summary>
        /// <param name="rootCampgroundId">Root campground identifier</param>
        /// <param name="loadSubCategories">A value indicating whether subcategories should be loaded</param>
        /// <param name="allCategories">All available categories; pass null to load them internally</param>
        /// <returns>List of campground (simple) models</returns>
        //List<CampgroundSimpleModel> PrepareCampgroundSimpleModels(int rootCampgroundId, bool loadSubCategories = true, IList<Campground> allCategories = null);

        #endregion

    }
}
