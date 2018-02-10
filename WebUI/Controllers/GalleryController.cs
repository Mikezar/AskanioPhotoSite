using System.Web.Mvc;
using System.Linq;
using AskanioPhotoSite.Core.Models;
using AskanioPhotoSite.Core.Services.Abstract;
using AskanioPhotoSite.Core.Helpers;
using AskanioPhotoSite.Core.Services.Extensions;
using System.Collections.Generic;
using System;

namespace AskanioPhotoSite.WebUI.Controllers
{
    public class GalleryController : BaseController
    {
        private readonly IAlbumService _albumService;
        private readonly IPhotoService _photoService;
        private readonly ITagService _tagService;

        public GalleryController(IAlbumService albumService, IPhotoService photoService, ITagService tagService)
        {
            _albumService = albumService;
            _photoService = photoService;
            _tagService = tagService;
        }

        public ActionResult Index()
        {
            return View(_albumService.FillModel());
        }

        public ActionResult Album(int id)
        {
            var album = _albumService.GetOne(id);

            var childs = _albumService.GetAll().Where(x => x.ParentId == id);

            if (childs.Count() > 0)
            {
                var model = new GalleryViewModel()
                {
                    Albums = childs.Select(x => new GalleryAlbumModel()
                    {
                        Id = x.Id,
                        Title = CultureHelper.IsEnCulture() ? x.TitleEng : x.TitleRu,
                        Description = CultureHelper.IsEnCulture() ? x.DescriptionEng : x.DescriptionRu,
                        Cover = x.CoverPath
                    }),
                    ParentAlbums = _albumService.GetAll().BuildGraph(album)
                };

                return View("~/Views/Gallery/Index.cshtml", model);
            }
            else
            {
                var photos = _photoService.GetAll().Where(x => x.AlbumId == id).ToList();

                if (photos.Count() > 0)
                {
                    var model = new GalleryPhotoListModel()
                    {
                        AlbumTitle = CultureHelper.IsEnCulture() ? album.TitleEng : album.TitleRu,
                        Photos = photos.OrderByDescending(x => x.Order).Select(x => new GalleryPhotoModel()
                        {
                            Id = x.Id,
                            Title = CultureHelper.IsEnCulture() ? x.TitleEng : x.TitleRu,
                            Thumbnail = x.ThumbnailPath,
                            Photo = x.PhotoPath,
                            Description = CultureHelper.IsEnCulture() ? x.DescriptionEng : x.DescriptionRu,

                        }),
                        Albums = _albumService.GetAll().BuildGraph(album)
                    };

                    return View($"~/Views/Gallery/AlbumRenderPattern{(album.ViewPattern == default(int) ? 1 : album.ViewPattern)}.cshtml", model);
                }
                else
                    return View($"~/Views/Gallery/AlbumRenderPattern1.cshtml", new GalleryPhotoListModel());
            }
        }

        public ActionResult ViewPhotoPartial(int id, bool includeTag = false)
        {
            var photo = _photoService.GetOne(id);

            var model = new GalleryPhotoViewModel()
            {
                Id = photo.Id,
                Title = CultureHelper.IsEnCulture() ? photo.TitleEng : photo.TitleRu,
                Path = photo.PhotoPath,
                Description = CultureHelper.IsEnCulture() ? photo.DescriptionEng : photo.DescriptionRu,
                IncludeTag = includeTag
            };

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult ViewPhotoPartial(int id, bool isNext, bool includeTag = false)
        {
            var model = _photoService.GetNextPrevPhoto(id, isNext, includeTag);
            return PartialView(model);
        }

        public ActionResult Tag(IList<int> ids)
        {
            var model = _tagService.ShowPhotoByTag(ids);
            return View("~/Views/Gallery/TagRenderPattern.cshtml", model);
        }
    }
}