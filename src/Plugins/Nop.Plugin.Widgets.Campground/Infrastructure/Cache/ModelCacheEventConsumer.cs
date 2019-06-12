using Nop.Core.Caching;
using Nop.Plugin.Campgrounds.Data.Domain;
using Nop.Core.Events;
using Nop.Services.Events;

namespace Nop.Plugin.Widgets.Campgrounds.Infrastructure.Cache
{
    /// <summary>
    /// Model cache event consumer (used for caching of presentation layer models)
    /// </summary>
    public partial class ModelCacheEventConsumer :
        //campground
        IConsumer<EntityInsertedEvent<Campground>>,
        IConsumer<EntityUpdatedEvent<Campground>>,
        IConsumer<EntityDeletedEvent<Campground>>

    {
        #region Fields

        private readonly CampgroundSettings _campgroundSettings;
        private readonly IStaticCacheManager _cacheManager;

        #endregion

        #region Ctor

        public ModelCacheEventConsumer(CampgroundSettings campgroundSettings, IStaticCacheManager cacheManager)
        {
            this._cacheManager = cacheManager;
            this._campgroundSettings = campgroundSettings;
        }

        #endregion

        #region Cache keys 

        /// <summary>
        /// Key for campgrounds on the search page
        /// </summary>
        /// <remarks>
        /// {0} : language id
        /// {1} : roles of the current user
        /// {2} : current store ID
        /// </remarks>
        public const string SEARCH_CAMPGROUND_MODEL_KEY = "Nop.plugin.widgets.campgrounds.search.campgrounds-{0}-{1}-{2}";
        public const string SEARCH_CAMPGROUNDS_PATTERN_KEY = "Nop.plugin.widgets.campgrounds.search.campgrounds";


        public const string CAMPGROUND_MENU_MODEL_KEY = "Nop.plugin.widgets.campgrounds.models.campgroundmenu-{0}";
        public const string CAMPGROUND_NUMBER_OF_SITES_KEY = "Nop.plugin.widgets.campgrounds.models.campgroundmenu-Sites-{0}";

        public const string CAMPGROUND_PARENTCATEGORY_KEY = "Nop.plugin.widgets.campgrounds.models.campgroundparentcategory-{0}";

        /// <summary>
        /// Key for specification attributes caching (campground details page)
        /// </summary>
        public const string SPEC_ATTRIBUTES_MODEL_KEY = "Nop.pres.admin.campground.specs";
        public const string SPEC_ATTRIBUTES_PATTERN_KEY = "Nop.pres.admin.campground.specs";


        /// <summary>
        /// Key for home page news
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// {1} : current store ID
        /// </remarks>
        public const string HOMEPAGE_BLOGMODEL_KEY = "Nop.pres.blog.homepage-{0}-{1}";
        public const string BLOG_PATTERN_KEY = "Nop.pres.blog";

        
        /// <summary>
        /// Key for "also camped" campground identifiers displayed on the campground details page
        /// </summary>
        /// <remarks>
        /// {0} : current campground id
        /// {1} : current store ID
        /// </remarks>
        public const string CAMPGROUNDS_ALSO_CAMPED_AT_KEY = "Nop.plugin.widgets.campgrounds.alsocampedat-{0}-{1}";
        public const string CAMPGROUNDS_ALSO_CAMPED_AT_PATTERN_KEY = "Nop.plugin.widgets.campgrounds.alsocampedat";

        /// <summary>
        /// Key for bestsellers identifiers displayed on the home page
        /// </summary>
        /// <remarks>
        /// {0} : current store ID
        /// </remarks>
        public const string HOMEPAGE_NEWCAMPGROUNDS_IDS_KEY = "Nop.pres.newcampgrounds.homepage-{0}";
        public const string HOMEPAGE_NEWCAMPGROUNDS_IDS_PATTERN_KEY = "Nop.pres.newcampgrounds.homepage";

        /// <summary>
        /// Key for "related" campground identifiers displayed on the campground details page
        /// </summary>
        /// <remarks>
        /// {0} : current campground id
        /// {1} : current store ID
        /// </remarks>
        public const string CAMPGROUND_RELATED_IDS_KEY = "Nop.plugin.widgets.campgrounds.related.campground-{0}-{1}";
        public const string CAMPGROUND_RELATED_IDS_PATTERN_KEY = "Nop.plugin.widgets.campgrounds.related.campground";


        public const string CAMPGROUND_NUMBER_OF_CAMPGROUNDS_MODEL_KEY = "Nop.plugin.widgets.campgrounds.campground.numberofcampgrounds-{0}-{1}-{2}";
        public const string CAMPGROUND_NUMBER_OF_CAMPGROUNDS_PATTERN_KEY = "Nop.plugin.widgets.campgrounds.campground.numberofcampgrounds";

        public const string CAMPGROUND_SPECS_MODEL_KEY = "Nop.plugin.widgets.campgrounds.campground.specs-{0}-{1}";
        public const string CAMPGROUND_SPECS_PATTERN_KEY = "Nop.plugin.widgets.campgrounds.campground.specs";
        public const string CAMPGROUND_SPECS_PATTERN_KEY_BY_ID = "Nop.plugin.widgets.campgrounds.campground.specs-{0}";


        /// <summary>
        /// Key for CampgroundCategoryModel caching
        /// </summary>
        /// <remarks>
        /// {0} : campground template id
        /// </remarks>
        public const string CAMPGROUND_TEMPLATE_MODEL_KEY = "Nop.plugin.widgets.campgrounds.campgroundtemplate-{0}";
        public const string CAMPGROUND_TEMPLATE_PATTERN_KEY = "Nop.plugin.widgets.campgrounds.campgroundtemplate";

        /// <summary>
        /// Key for CampgroundOverviewModel caching
        /// </summary>
        /// <remarks>
        /// {0} : campground template id
        /// </remarks>
        public const string CAMPGROUND_OVERVIEW_MODEL_KEY = "Nop.plugin.widgets.campgrounds.campgroundoverview-{0}";
        public const string CAMPGROUND_OVERVIEW_PATTERN_KEY = "Nop.plugin.widgets.campgrounds.campgroundoverview";

        /// <summary>
        /// Key for TopMenuModel caching
        /// </summary>
        /// <remarks>
        /// {0} : language id
        /// {1} : current store ID
        /// {2} : comma separated list of customer roles
        /// </remarks>
        public const string CAMPGROUND_TOP_MENU_MODEL_KEY = "Nop.plugin.widgets.campgrounds.campground.topmenu-{0}-{1}-{2}";

        /// <summary>
        /// Key for CampgroundBreadcrumbModel caching
        /// </summary>
        /// <remarks>
        /// {0} : campground id
        /// {1} : language id
        /// {2} : comma separated list of customer roles
        /// {3} : current store ID
        /// </remarks>
        public const string CAMPGROUND_BREADCRUMB_MODEL_KEY = "Nop.plugin.widgets.campgrounds.campground.breadcrumb-{0}-{1}-{2}-{3}";
        public const string CAMPGROUND_BREADCRUMB_PATTERN_KEY = "Nop.plugin.widgets.campgrounds.campground.breadcrumb";
        public const string CAMPGROUND_BREADCRUMB_PATTERN_KEY_BY_ID = "Nop.plugin.widgets.campgrounds.campground.breadcrumb-{0}-";

        /// <summary>
        /// Key for CampgroundTagModel caching
        /// </summary>
        /// <remarks>
        /// {0} : campground id
        /// {1} : language id
        /// {2} : current store ID
        /// </remarks>
        public const string CAMPGROUNDTAG_BY_CAMPGROUND_MODEL_KEY = "Nop.plugin.widgets.campgrounds.campgroundtag.bycampground-{0}-{1}-{2}";
        public const string CAMPGROUNDTAG_BY_CAMPGROUND_PATTERN_KEY = "Nop.plugin.widgets.campgrounds.campgroundtag.bycampground";

        /// <summary>
        /// Key for categories caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// </remarks>
        public const string CAMPGROUND_CATEGORIES_LIST_KEY = "Nop.pres.admin.campground.categories.list-{0}";
        public const string CAMPGROUND_CATEGORIES_LIST_PATTERN_KEY = "Nop.pres.admin.campground.categories.list";

        /// <summary>
        /// Key for categories caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// </remarks>
        public const string CAMPGROUND_CAMPGROUNDHOST_LIST_KEY = "Nop.pres.admin.campground.campgroundhost.list-{0}";
        public const string CAMPGROUND_CAMPGROUNDHOST_LIST_PATTERN_KEY = "Nop.pres.admin.campground.campgroundhost.list";

        /// <summary>
        /// Key for categories caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// </remarks>
        public const string CAMPGROUND_CAMPGROUNDTYPE_LIST_KEY = "Nop.pres.admin.campground.campgroundtype.list-{0}";
        public const string CAMPGROUND_CAMPGROUNDTYPE_LIST_PATTERN_KEY = "Nop.pres.admin.campground.campgroundtype.list";

        /// <summary>
        /// Key for campground picture caching on the campground details page
        /// </summary>
        /// <remarks>
        /// {0} : campground id
        /// {1} : picture size
        /// {2} : value indicating whether a default picture is displayed in case if no real picture exists
        /// {3} : language ID ("alt" and "title" can depend on localized campground name)
        /// {4} : is connection SSL secured?
        /// {5} : current store ID
        /// </remarks>
        public const string CAMPGROUND_DETAILS_PICTURES_MODEL_KEY = "Nop.plugin.widgets.campgrounds.campground.picture-{0}-{1}-{2}-{3}-{4}-{5}";
        public const string CAMPGROUND_DETAILS_PICTURES_PATTERN_KEY = "Nop.plugin.widgets.campgrounds.campground.picture";
        public const string CAMPGROUND_DETAILS_PICTURES_PATTERN_KEY_BY_ID = "Nop.plugin.widgets.campgrounds.campground.picture-{0}-";

        /// <summary>
        /// Key for default campground picture caching (all pictures)
        /// </summary>
        /// <remarks>
        /// {0} : campground id
        /// {1} : picture size
        /// {2} : isAssociatedCampground?
        /// {3} : language ID ("alt" and "title" can depend on localized campground name)
        /// {4} : is connection SSL secured?
        /// {5} : current store ID
        /// </remarks>
        public const string CAMPGROUND_DEFAULTPICTURE_MODEL_KEY = "Nop.plugin.widgets.campgrounds.campground.detailspictures-{0}-{1}-{2}-{3}-{4}-{5}";
        public const string CAMPGROUND_DEFAULTPICTURE_PATTERN_KEY = "Nop.plugin.widgets.campgrounds.campground.detailspictures";
        public const string CAMPGROUND_DEFAULTPICTURE_PATTERN_KEY_BY_ID = "Nop.plugin.widgets.campgrounds.campground.detailspictures-{0}-";

        /// <summary>
        /// Key for campground reviews caching
        /// </summary>
        /// <remarks>
        /// {0} : campground id
        /// {1} : current store ID
        /// </remarks>
        public const string CAMPGROUND_REVIEWS_MODEL_KEY = "Nop.plugin.widgets.campgrounds.campground.reviews-{0}-{1}";
        public const string CAMPGROUND_REVIEWS_PATTERN_KEY = "Nop.plugin.widgets.campgrounds.campground.reviews";
        public const string CAMPGROUND_REVIEWS_PATTERN_KEY_BY_ID = "Nop.plugin.widgets.campgrounds.campground.reviews-{0}-";

        /// <summary>
        /// Key for caching of a value indicating whether a campground has campground attributes
        /// </summary>
        /// <remarks>
        /// {0} : campground id
        /// </remarks>
        public const string CAMPGROUND_HAS_CAMPGROUND_ATTRIBUTES_KEY = "Nop.plugin.widgets.campgrounds.campground.hascampgroundattributes-{0}-";
        public const string CAMPGROUND_HAS_CAMPGROUND_ATTRIBUTES_PATTERN_KEY = "Nop.plugin.widgets.campgrounds.campground.hascampgroundattributes";
        public const string CAMPGROUND_HAS_CAMPGROUND_ATTRIBUTES_PATTERN_KEY_BY_ID = "Nop.plugin.widgets.campgrounds.campground.hascampgroundattributes-{0}-";

        /// <summary>
        /// Key for campground attribute picture caching on the campground details page
        /// </summary>
        /// <remarks>
        /// {0} : picture id
        /// {1} : is connection SSL secured?
        /// {2} : current store ID
        /// </remarks>
        public const string CAMPGROUNDATTRIBUTE_IMAGESQUARE_PICTURE_MODEL_KEY = "Nop.plugin.widgets.campgrounds.campgroundattribute.imagesquare.picture-{0}-{1}-{2}";
        public const string CAMPGROUNDATTRIBUTE_IMAGESQUARE_PICTURE_PATTERN_KEY = "Nop.plugin.widgets.campgrounds.campgroundattribute.imagesquare.picture";

        #endregion

        #region Methods

        //campgrounds
        public void HandleEvent(EntityInsertedEvent<Campground> eventMessage)
        {
            _cacheManager.RemoveByPattern(Nop.Web.Infrastructure.Cache.ModelCacheEventConsumer.SITEMAP_PATTERN_KEY);
        }
        public void HandleEvent(EntityUpdatedEvent<Campground> eventMessage)
        {
            _cacheManager.RemoveByPattern(Nop.Web.Infrastructure.Cache.ModelCacheEventConsumer.HOMEPAGE_BESTSELLERS_IDS_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDS_ALSO_CAMPED_AT_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUND_RELATED_IDS_PATTERN_KEY);
            _cacheManager.RemoveByPattern(Nop.Web.Infrastructure.Cache.ModelCacheEventConsumer.SITEMAP_PATTERN_KEY);
            _cacheManager.RemoveByPattern(string.Format(CAMPGROUND_REVIEWS_PATTERN_KEY_BY_ID, eventMessage.Entity.Id));
            _cacheManager.RemoveByPattern(CAMPGROUNDTAG_BY_CAMPGROUND_PATTERN_KEY);
        }
        public void HandleEvent(EntityDeletedEvent<Campground> eventMessage)
        {
            _cacheManager.RemoveByPattern(Nop.Web.Infrastructure.Cache.ModelCacheEventConsumer.HOMEPAGE_BESTSELLERS_IDS_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUNDS_ALSO_CAMPED_AT_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CAMPGROUND_RELATED_IDS_PATTERN_KEY);
            _cacheManager.RemoveByPattern(Nop.Web.Infrastructure.Cache.ModelCacheEventConsumer.SITEMAP_PATTERN_KEY);
        }
        #endregion

    }
}