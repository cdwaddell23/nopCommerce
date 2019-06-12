using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Campgrounds.Services;
using Nop.Plugin.Widgets.Campgrounds.Models;
using Nop.Services.Media;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Framework.Security;
using Newtonsoft.Json;
using Nop.Plugin.Campgrounds.Data.Domain;

namespace Nop.Plugin.Widgets.Campgrounds.Controllers
{
    public partial class ReviewController : Controller
    {
        const string API_VERSION_1 = "camping/api/v1/";

        #region Fields
        private readonly ICampgroundService _campgroundService;
        private readonly IPictureService _pictureService;
        private readonly CampgroundSettings _campgroundSettings;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        #endregion

        #region Ctor
        public ReviewController(ICampgroundService campgroundService,
            IPictureService pictureService,
            CampgroundSettings campgroundSettings,
            IWorkContext workContext,
            IStoreContext storeContext)
        {
            this._campgroundService = campgroundService;
            this._pictureService = pictureService;
            this._campgroundSettings = campgroundSettings;
            this._workContext = workContext;
            this._storeContext = storeContext;
        }
        #endregion


        #region Utilities
        private int UpdateReview(Campground campground, int rating, string title, string reviewText)
        {
            var isApproved = !_campgroundSettings.CampgroundReviewsMustBeApproved;

            var campgroundReview = _campgroundService.GetCampgroundReviewByCustomer(customerId: _workContext.CurrentCustomer.Id, campgroundId: campground.Id);

            if (campgroundReview == null)
            {
                campgroundReview = new CampgroundReview
                {
                    CampgroundId = campground.Id,
                    CustomerId = _workContext.CurrentCustomer.Id,
                    Title = title,
                    ReviewText = reviewText,
                    Rating = rating,
                    HelpfulYesTotal = 0,
                    HelpfulNoTotal = 0,
                    IsApproved = isApproved,
                    CreatedOnUtc = DateTime.UtcNow,
                    StoreId = _storeContext.CurrentStore.Id,
                };
            }
            else
            {
                campgroundReview.Rating = rating;
                if (title?.Length > 0)
                    campgroundReview.Title = title;
                if (reviewText?.Length > 0)
                    campgroundReview.ReviewText = reviewText;
                campgroundReview.HelpfulYesTotal = 0;
                campgroundReview.HelpfulNoTotal = 0;
                campgroundReview.IsApproved = isApproved;
                campgroundReview.CreatedOnUtc = DateTime.UtcNow;
                campgroundReview.StoreId = _storeContext.CurrentStore.Id;
            }
            campground.CampgroundReviews.Add(campgroundReview);
            _campgroundService.UpdateCampground(campground);

            return campgroundReview.Id;
        }
        #endregion

        [HttpGet]
        [Route(API_VERSION_1 + "campgrounds/review/find-review/{campgroundId?}")]
        public virtual JsonResult FindReview(int campgroundId)
        {
            var campground = _campgroundService.GetCampgroundById(campgroundId);
            var campgroundReview = _campgroundService.GetCampgroundReviewByCustomer(customerId: _workContext.CurrentCustomer.Id, campgroundId: campgroundId);
            if (campgroundReview == null)
                return Json(new
                {
                    success = false,
                    message = "No Review Found",
                    name = campground != null ? campground.Name : string.Empty,
                    reviewid = 0,
                    rating = 0,
                    title = string.Empty,
                    description = string.Empty,
                    pictures = string.Empty,
                });

            List<string> pictures = new List<string>();
            foreach (var picture in campgroundReview.CampgroundPictures)
            {
                pictures.Add(_pictureService.GetPictureUrl(picture.Picture));
            }

            return Json(new
            {
                success = true,
                message = "Success",
                name = campgroundReview.Campground.Name,
                reviewid = campgroundReview.Id,
                rating = campgroundReview.Rating,
                title = campgroundReview.Title ?? string.Empty,
                description = campgroundReview.ReviewText ?? string.Empty,
                pictures = pictures,
            });
        }

        [HttpPost]
        [Route(API_VERSION_1 + "campground/review/add-review")]
        public virtual IActionResult AddReview([FromBody]object value)
        {
            var data = JsonConvert.DeserializeObject<CampgroundReviewRatingModel>(value.ToString());
            var campground = _campgroundService.GetCampgroundById(data.CampgroundId);
            if (campground == null || campground.Deleted || !campground.Published || !campground.AllowCampgroundReviews)
            {
                
                return Json(new
                {
                    success = false,
                    message = "Campground not found!",
                    name = 0,
                    reviewid = 0
                });
            }

            //var rating = model.AddCampgroundReview.Rating;
            if (data.Rating < 1 || data.Rating > 5)
                data.Rating = _campgroundSettings.DefaultCampgroundRatingValue;

            var reviewId = UpdateReview(campground, data.Rating, data.Title, data.ReviewText);

            return Json(new
            {
                success = true,
                message = "Success",
                name = campground.Name,
                reviewid = reviewId
            });
        }

        [HttpPost]
        [Route(API_VERSION_1 + "campgrounds/review/add-photo/{campgroundId?}/{reviewId?}")]
        public virtual IActionResult AddPhoto([FromForm]object value, int? campgroundId, int? reviewId)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.UploadPictures))
            //    return Json(new { success = false, error = "You do not have required permissions" }, "text/plain");
            //var data = JsonConvert.DeserializeObject<CampgroundReviewRatingModel>(value.ToString());
            var campground = _campgroundService.GetCampgroundById(campgroundId ?? 0);
            var campgroundReview = _campgroundService.GetCampgroundReviewById(reviewId ?? 0);

            var httpPostedFile = Request.Form.Files.FirstOrDefault();
            if (httpPostedFile == null)
            {
                return Json(new
                {
                    success = false,
                    message = "No file uploaded",
                    downloadGuid = Guid.Empty,
                });
            }

            var fileBinary = httpPostedFile.GetDownloadBits();

            var qqFileNameParameter = "qqfilename";
            var fileName = httpPostedFile.FileName;
            if (string.IsNullOrEmpty(fileName) && Request.Form.ContainsKey(qqFileNameParameter))
                fileName = Request.Form[qqFileNameParameter].ToString();
            //remove path (passed in IE)
            fileName = Path.GetFileName(fileName);

            var contentType = httpPostedFile.ContentType;

            var fileExtension = Path.GetExtension(fileName);
            if (!string.IsNullOrEmpty(fileExtension))
                fileExtension = fileExtension.ToLowerInvariant();

            //contentType is not always available 
            //that's why we manually update it here
            //http://www.sfsu.edu/training/mimetype.htm
            if (string.IsNullOrEmpty(contentType))
            {
                switch (fileExtension)
                {
                    case ".bmp":
                        contentType = MimeTypes.ImageBmp;
                        break;
                    case ".gif":
                        contentType = MimeTypes.ImageGif;
                        break;
                    case ".jpeg":
                    case ".jpg":
                    case ".jpe":
                    case ".jfif":
                    case ".pjpeg":
                    case ".pjp":
                        contentType = MimeTypes.ImageJpeg;
                        break;
                    case ".png":
                        contentType = MimeTypes.ImagePng;
                        break;
                    case ".tiff":
                    case ".tif":
                        contentType = MimeTypes.ImageTiff;
                        break;
                    default:
                        break;
                }
            }
            var SeoFilename = fileName.Replace(fileExtension, string.Empty) + "_" + (campgroundId ?? 0).ToString() + "_" + (reviewId ?? 0).ToString();
            var picture = _pictureService.InsertPicture(fileBinary, contentType, SeoFilename);

            var campgroundPicture = new CampgroundPicture
            {
                CampgroundId = campground.Id,
                PictureId = picture.Id,
                Review = campgroundReview,
                ReviewId = campgroundReview.Id
            };
            _campgroundService.InsertCampgroundPicture(campgroundPicture);

            campgroundReview.CampgroundPictures.Add(campgroundPicture);

                //when returning JSON the mime-type must be set to text/plain
                //otherwise some browsers will pop-up a "Save As" dialog.
            return Json(new
            {
                success = true,
                pictureId = picture.Id,
                imageUrl = _pictureService.GetPictureUrl(picture, 100)
            });
        }

        public IActionResult Error()
        {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }
    }
}

