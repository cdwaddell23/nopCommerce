using Nop.Core.Configuration;

namespace Nop.Core.Domain.Media
{
    public partial class CampgroundMediaSettings : MediaSettings, ISettings
    {
        /// <summary>
        /// Picture size of product picture thumbs displayed on catalog pages (e.g. category details page)
        /// </summary>
        public int CampgroundThumbPictureSize { get; set; }
        /// <summary>
        /// Picture size of the main product picture displayed on the product details page
        /// </summary>
        public int CampgroundDetailsPictureSize { get; set; }
        /// <summary>
        /// Picture size of the product picture thumbs displayed on the product details page
        /// </summary>
        public int CampgroundThumbPictureSizeOnCampgroundDetailsPage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int AssociatedCampgroundPictureSize { get; set; }
    }
}