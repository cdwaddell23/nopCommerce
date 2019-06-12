using System;
using System.Collections.Generic;
using Nop.Core;
using Nop.Plugin.Campgrounds.Data.Domain;

namespace Nop.Plugin.Campgrounds.Services
{
    /// <summary>
    /// Campground service
    /// </summary>
    public partial interface ICampgroundService
    {
        #region Campgrounds

        /// <summary>
        /// Delete a Campground
        /// </summary>
        /// <param name="Campground">Campground</param>
        void DeleteCampground(Campground Campground);

        /// <summary>
        /// Delete Campgrounds
        /// </summary>
        /// <param name="Campgrounds">Campgrounds</param>
        void DeleteCampgrounds(IList<Campground> Campgrounds);

        /// <summary>
        /// Get the lat and long of address
        /// </summary>
        /// <param name="Address"></param>
        /// <returns></returns>
        Location GetCampgroundLatLong(CampgroundAddress Address);

        /// <summary>
        /// Gets all Campgrounds displayed on the home page
        /// </summary>
        /// <returns>Campgrounds</returns>
        IList<Campground> GetAllCampgroundsDisplayedOnHomePage();

        /// <summary>
        /// Gets Campground
        /// </summary>
        /// <param name="CampgroundId">Campground identifier</param>
        /// <returns>Campground</returns>
        Campground GetCampgroundById(int CampgroundId);

        /// <summary>
        /// Gets the parent category for a campground
        /// </summary>
        /// <param name="campground"></param>
        /// <returns></returns>
        int GetCampgroundsParentCategoryId(Campground campground);
        
        /// <summary>
        /// Gets Campground Type
        /// </summary>
        /// <returns>Dictionary</returns>
        Dictionary<int, string> GetCampgroundClass();

        /// <summary>
        /// Get new campgrounds, page size of -1 returns all
        /// </summary>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IList<Campground> GetNewCampgrounds(int pageSize = 6);

        /// <summary>
        /// Gets campground categories by category Id
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="showHidden"></param>
        /// <returns></returns>
        IPagedList<CampgroundCategory> GetCampgroundCategoriesByCategoryId(int categoryId,
                    int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

        /// <summary>
        /// Inserts a campground category mapping
        /// </summary>
        /// <param name="campgroundCategory">>Campground category mapping</param>
        void InsertCampgroundCategory(CampgroundCategory campgroundCategory);

        /// <summary>
        /// Updates the campground category mapping 
        /// </summary>
        /// <param name="campgroundCategory">>Campground category mapping</param>
        void UpdateCampgroundCategory(CampgroundCategory campgroundCategory);

        /// <summary>
        /// Deletes a campground category mapping
        /// </summary>
        /// <param name="campgroundCategory">Campground category</param>
        void DeleteCampgroundCategory(CampgroundCategory campgroundCategory);

        /// <summary>
        /// Gets categories by campgroundId
        /// </summary>
        /// <param name="campgroundId"></param>
        /// <param name="showHidden"></param>
        /// <returns></returns>
        IList<CampgroundCategory> GetCampgroundCategoriesByCampgroundId(int campgroundId, bool showHidden = false);

        /// <summary>
        /// Gets a campground category mapping collection
        /// </summary>
        /// <param name="campgroundId">Campground identifier</param>
        /// <param name="storeId">Store identifier (used in multi-store environment). "showHidden" parameter should also be "true"</param>
        /// <param name="showHidden"> A value indicating whether to show hidden records</param>
        /// <returns> Campground category mapping collection</returns>
        IList<CampgroundCategory> GetCampgroundCategoriesByCampgroundId(int campgroundId, int storeId, bool showHidden = false);

        /// <summary>
        /// Get Associated Campgrounds
        /// </summary>
        /// <param name="parentGroupedCampgroundId"></param>
        /// <param name="storeId"></param>
        /// <param name="campgroundHostId"></param>
        /// <param name="showHidden"></param>
        /// <returns></returns>
        IList<Campground> GetAssociatedCampgrounds(int parentGroupedCampgroundId,
            int storeId = 0, int campgroundHostId = 0, bool showHidden = false);

        /// <summary>
        /// Gets Campground by category
        /// </summary>
        /// <param name="CategoryId">CategoryId</param>
        /// <returns>IList Campground</returns>
        IPagedList<Campground> GetCampgroundsByCategoryId(int CampgroundStateId, int pageIndex, int pageSize);
        /// <summary>
        /// Gets Campground by state
        /// </summary>
        /// <param name="StateId">StateId</param>
        /// <returns>IList Campground</returns>
        IPagedList<Campground> GetCampgroundsByStateId(int CampgroundStateId, int pageIndex, int pageSize);


        /// <summary>
        /// Get CategoryId by StateProvinceId
        /// </summary>
        /// <param name="stateProvinceId"></param>
        /// <returns></returns>
        int GetCampgroundCategoryIdFromStateId(int stateProvinceId);

        /// <summary>
        /// Gets StateID from CategoryId
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        int GetCampgroundStateIdFromCategoryId(int CategoryId);
        /// <summary>
        /// Get campgrounds by within a category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="campgroundName"></param>
        /// <returns></returns>
        Campground GetCampgroundByCategoryIdName(int categoryId, string campgroundName);

        /// <summary>
        /// Gets Campgrounds by identifier
        /// </summary>
        /// <param name="CampgroundIds">Campground identifiers</param>
        /// <returns>IList Campgrounds</returns>
        IList<Campground> GetCampgroundsByIds(int[] CampgroundIds);

        /// <summary>
        /// Get Campground by attribute identifier
        /// </summary>
        /// <returns>IPagedList Campground</Campground></returns>
        IPagedList<Campground> GetCampgroundsByCampgroundAtributeId(int campgroundAttributeTypeId,
                    int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Get Campground address
        /// </summary>
        /// <returns>CampgroundAddress</returns>
        CampgroundAddress GetCampgroundAddressByCampgroundId(int CampgroundId);

        /// <summary>
        /// Get nearby campgrounds
        /// </summary>
        /// <param name="Latitude"></param>
        /// <param name="Longitude"></param>
        /// <returns></returns>
        IList<Campground> GetNearbyCampgrounds(decimal? sourceLatitude, decimal? sourceLongitude, int Distance = 25, bool useStoreProc = true);

        /// <summary>
        /// Get featured Campgrounds 
        /// </summary>
        /// <returns>Campgrounds</returns>
        IPagedList<Campground> GetFeaturedCampgrounds(int pageIndex, int pageSize);

        /// <summary>
        /// Inserts a Campground
        /// </summary>
        /// <param name="Campground">Campground</param>
        void InsertCampground(Campground Campground);

        /// <summary>
        /// Insert Campground Host
        /// </summary>
        /// <param name="CampgroundHost"></param>
        void InsertCampgroundHost(CampgroundHost CampgroundHost);

        /// <summary>
        /// Updates the Campground
        /// </summary>
        /// <param name="Campground">Campground</param>
        void UpdateCampground(Campground Campground);

        /// <summary>
        /// Updates the Campgrounds
        /// </summary>
        /// <param name="Campgrounds">Campground</param>
        void UpdateCampgrounds(IList<Campground> Campgrounds);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="campground"></param>
        /// <returns></returns>
        byte[] SaveCampgroundStaticMap(Campground campground);

        /// <summary>
        /// Get number of Campground (published and visible) in certain category
        /// </summary>
        /// <param name="categoryIds">Category identifiers</param>
        /// <param name="storeId">Store identifier; 0 to load all records</param>
        /// <returns>Number of Campgrounds</returns>
        int GetNumberOfCampgroundsInCategory(IList<int> categoryIds = null, int storeId = 0);

        /// <summary>
        /// Update Campground review totals
        /// </summary>
        /// <param name="Campground">Campground</param>
        void UpdateCampgroundReviewTotals(Campground Campground);

        /// <summary>
        /// Gets number of Campgrounds by campgroundHost identifier
        /// </summary>
        /// <param name="campgroundHostId">CampgroundHost identifier</param>
        /// <returns>Number of Campgrounds</returns>
        int GetNumberOfCampgroundsByCustomerId(int customerId);

        #endregion

        #region Campground pictures

        /// <summary>
        /// Deletes a Campground picture
        /// </summary>
        /// <param name="CampgroundPicture">Campground picture</param>
        void DeleteCampgroundPicture(CampgroundPicture CampgroundPicture);

        /// <summary>
        /// Gets a Campground pictures by Campground identifier
        /// </summary>
        /// <param name="CampgroundId">The Campground identifier</param>
        /// <returns>Campground pictures</returns>
        IList<CampgroundPicture> GetCampgroundPicturesByCampgroundId(int CampgroundId);

        /// <summary>
        /// Gets a Campground picture
        /// </summary>
        /// <param name="CampgroundPictureId">Campground picture identifier</param>
        /// <returns>Campground picture</returns>
        CampgroundPicture GetCampgroundPictureById(int CampgroundPictureId);

        /// <summary>
        /// Inserts a Campground picture
        /// </summary>
        /// <param name="CampgroundPicture">Campground picture</param>
        void InsertCampgroundPicture(CampgroundPicture CampgroundPicture);

        /// <summary>
        /// Updates a Campground picture
        /// </summary>
        /// <param name="CampgroundPicture">Campground picture</param>
        void UpdateCampgroundPicture(CampgroundPicture CampgroundPicture);

        /// <summary>
        /// Get the IDs of all Campground images 
        /// </summary>
        /// <param name="CampgroundsIds">Campgrounds IDs</param>
        /// <returns>All picture identifiers grouped by Campground ID</returns>
        IDictionary<int, int[]> GetCampgroundsImagesIds(int[] CampgroundsIds);

        #endregion

        #region Campground reviews

        /// <summary>
        /// Gets all Campground reviews
        /// </summary>
        /// <param name="customerId">Customer identifier (who wrote a review); 0 to load all records</param>
        /// <param name="approved">A value indicating whether to content is approved; null to load all records</param> 
        /// <param name="fromUtc">Item creation from; null to load all records</param>
        /// <param name="toUtc">Item item creation to; null to load all records</param>
        /// <param name="message">Search title or review text; null to load all records</param>
        /// <param name="storeId">The store identifier; pass 0 to load all records</param>
        /// <param name="CampgroundId">The Campground identifier; pass 0 to load all records</param>
        /// <param name="campgroundHostId">The campgroundHost identifier (limit to Campgrounds of this campgroundHost); pass 0 to load all records</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Reviews</returns>
        IPagedList<CampgroundReview> GetAllCampgroundReviews(int customerId, bool? approved,
                    DateTime? fromUtc = null, DateTime? toUtc = null,
                    string message = null, int storeId = 0, int campgroundId = 0, int campgroundHostId = 0,
                    int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Get campground reviews by customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="campgroundId"></param>
        /// <returns></returns>
        CampgroundReview GetCampgroundReviewByCustomer(int customerId, int campgroundId);

        /// <summary>
        /// Gets Campground review
        /// </summary>
        /// <param name="CampgroundReviewId">Campground review identifier</param>
        /// <returns>Campground review</returns>
        CampgroundReview GetCampgroundReviewById(int CampgroundReviewId);

        /// <summary>
        /// Get Campground reviews by identifiers
        /// </summary>
        /// <param name="CampgroundReviewIds">Campground review identifiers</param>
        /// <returns>Campground reviews</returns>
        IList<CampgroundReview> GetCampgroundReviewsByIds(int[] CampgroundReviewIds);

        /// <summary>
        /// Deletes a Campground review
        /// </summary>
        /// <param name="CampgroundReview">Campground review</param>
        void DeleteCampgroundReview(CampgroundReview CampgroundReview);

        /// <summary>
        /// Deletes Campground reviews
        /// </summary>
        /// <param name="CampgroundReviews">Campground reviews</param>
        void DeleteCampgroundReviews(IList<CampgroundReview> CampgroundReviews);

        #endregion

        #region Campground Search

        /// <summary>
        /// Find Campground by Name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IList<Campground> FindByName(string name = null, bool exactMatch = false, bool isPublished = true);

        /// <summary>
        /// Search campgrounds by name, city zip
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="categoryIds"></param>
        /// <param name="visibleIndividuallyOnly"></param>
        /// <param name="markedAsNewOnly"></param>
        /// <param name="featuredCampgrounds"></param>
        /// <param name="campgroundTagId"></param>
        /// <param name="searchDescriptions"></param>
        /// <param name="searchSku"></param>
        /// <param name="searchCampgroundTags"></param>
        /// <param name="languageId"></param>
        /// <param name="filteredSpecs"></param>
        /// <param name="orderBy"></param>
        /// <param name="showHidden"></param>
        /// <param name="overridePublished"></param>
        /// <returns></returns>
        IPagedList<Campground> SearchCampgrounds(
            string keywords = null,
            int pageIndex = 0,
            int pageSize = int.MaxValue,
            IList<int> categoryIds = null,
            bool markedAsNewOnly = false,
            bool? featuredCampgrounds = null,
            int campgroundTagId = 0,
            bool searchDescriptions = false,
            bool searchCampgroundTags = false,
            int languageId = 0,
            int campgroundTypeId = 0,
            int campgroundHostId = 0,
            IList<int> filteredSpecs = null,
            CampgroundSortingEnum orderBy = CampgroundSortingEnum.Type,
            bool showHidden = false,
            bool? overridePublished = null,
            int maxRecords = 100);


        #endregion

        #region Related campgrounds

        /// <summary>
        /// Deletes a related campground
        /// </summary>
        /// <param name="relatedCampground">Related campground</param>
        void DeleteRelatedCampground(RelatedCampground relatedCampground);

        /// <summary>
        /// Gets related campgrounds by campground identifier
        /// </summary>
        /// <param name="campgroundId1">The first campground identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Related campgrounds</returns>
        IList<RelatedCampground> GetRelatedCampgroundsByCampgroundId1(int campgroundId1, bool showHidden = false);

        /// <summary>
        /// Gets a related campground
        /// </summary>
        /// <param name="relatedCampgroundId">Related campground identifier</param>
        /// <returns>Related campground</returns>
        RelatedCampground GetRelatedCampgroundById(int relatedCampgroundId);

        /// <summary>
        /// Inserts a related campground
        /// </summary>
        /// <param name="relatedCampground">Related campground</param>
        void InsertRelatedCampground(RelatedCampground relatedCampground);

        /// <summary>
        /// Updates a related campground
        /// </summary>
        /// <param name="relatedCampground">Related campground</param>
        void UpdateRelatedCampground(RelatedCampground relatedCampground);

        #endregion

        #region Cross-sell campgrounds

        /// <summary>
        /// Deletes a cross-sell campground
        /// </summary>
        /// <param name="crossSellCampground">Cross-sell identifier</param>
        void DeleteCrossSellCampground(CrossSellCampground crossSellCampground);

        /// <summary>
        /// Gets cross-sell campgrounds by campground identifier
        /// </summary>
        /// <param name="campgroundId1">The first campground identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Cross-sell campgrounds</returns>
        IList<CrossSellCampground> GetCrossSellCampgroundsByCampgroundId1(int campgroundId1, bool showHidden = false);

        /// <summary>
        /// Gets cross-sell campgrounds by campground identifier
        /// </summary>
        /// <param name="campgroundIds">The first campground identifiers</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Cross-sell campgrounds</returns>
        IList<CrossSellCampground> GetCrossSellCampgroundsByCampgroundIds(int[] campgroundIds, bool showHidden = false);

        /// <summary>
        /// Gets a cross-sell campground
        /// </summary>
        /// <param name="crossSellCampgroundId">Cross-sell campground identifier</param>
        /// <returns>Cross-sell campground</returns>
        CrossSellCampground GetCrossSellCampgroundById(int crossSellCampgroundId);

        /// <summary>
        /// Inserts a cross-sell campground
        /// </summary>
        /// <param name="crossSellCampground">Cross-sell campground</param>
        void InsertCrossSellCampground(CrossSellCampground crossSellCampground);

        /// <summary>
        /// Updates a cross-sell campground
        /// </summary>
        /// <param name="crossSellCampground">Cross-sell campground</param>
        void UpdateCrossSellCampground(CrossSellCampground crossSellCampground);


        #endregion

    }
}
