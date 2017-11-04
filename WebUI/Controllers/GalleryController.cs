using System.Web.Mvc;
using System.Web.Security;
using System.Linq;
using System.Collections.Generic;
using AskanioPhotoSite.Data.Entities;
using AskanioPhotoSite.Core.Services.Extensions;
using AskanioPhotoSite.Core.Models;
using AskanioPhotoSite.Core.Services;
using AskanioPhotoSite.WebUI.Helpers;

namespace AskanioPhotoSite.WebUI.Controllers
{
    public class GalleryController : BaseController
    {
        private readonly BaseService<Album> _albumService;

        private readonly BaseService<Photo> _photoService;

        private readonly BaseService<Tag> _tagService;

        private readonly BaseService<PhotoToTag> _photoToTagService;

        public GalleryController(BaseService<Album> albumService, BaseService<Photo> photoService,
                                 BaseService<PhotoToTag> photoToTagService, BaseService<Tag> tagService)
        {
            _albumService = albumService;
            _photoService = photoService;
            _photoToTagService = photoToTagService;
            _tagService = tagService;
        }

        public ActionResult Index()
        {
            var model = new GalleryViewModel()
            {
                Albums = _albumService.GetAll().Select(x => new GalleryAlbumModel()
                {
                    Id = x.Id,
                    TitleRu = x.TitleRu,
                    TitleEng = x.TitleEng,
                    Cover = x.CoverPath
                })
            };

            return View(model);
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
                        TitleRu = x.TitleRu,
                        TitleEng = x.TitleEng,
                        Cover = x.CoverPath
                    })
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
                        Photos = photos.Select(x => new GalleryPhotoModel()
                        {
                            Id = x.Id,
                            Thumbnail = x.ThumbnailPath,
                            Photo = x.PhotoPath,
                            Description = CultureHelper.IsEnCulture() ? x.DescriptionEng : x.DescriptionRu,

                        })
                    };

                    return View($"~/Views/Gallery/AlbumRenderPattern{album.ViewPattern ?? 1}.cshtml", model);
                }
                else
                    return View(new GalleryPhotoListModel());
            }
        }

        public ActionResult ViewPhotoPartial(int id)
        {
            var photo = _photoService.GetOne(id);

            var model = new GalleryPhotoViewModel()
            {
                Id = photo.Id,
                Title = CultureHelper.IsEnCulture() ? photo.TitleEng : photo.TitleRu,
                Path = photo.PhotoPath,
                Description = CultureHelper.IsEnCulture() ? photo.DescriptionEng : photo.DescriptionRu,
            };

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult ViewPhotoPartial(int id, bool isNext)
        {
            var photos = _photoService.GetAll().ToArray();

            Photo photo = new Photo();
            for (int i = 0; i < photos.Length; i++)
            {
                if (photos[i].Id == id)
                {
                    if (isNext)
                    {
                        if (i + 1 >= photos.Length - 1)
                        {
                            photo = photos[0];
                            break;
                        }
                        photo = photos[i + 1];
                    }
                    else
                    {
                        if (i - 1 < 0)
                        {
                            photo = photos[photos.Length - 1];
                            break;
                        }
                        photo = photos[i - 1];
                    }
                    break;
                }
            }

            var model = new GalleryPhotoViewModel()
            {
                Id = photo.Id,
                Title = CultureHelper.IsEnCulture() ? photo.TitleEng : photo.TitleRu,
                Path = photo.PhotoPath,
                Description = CultureHelper.IsEnCulture() ? photo.DescriptionEng : photo.DescriptionRu,
            };

            return PartialView(model);
        }

        public ActionResult Tag(int id)
        {
            var tag = _tagService.GetOne(id);
            var photos = _photoService.GetAll().Where(x =>
               _photoToTagService.GetAll().Where(t => t.TagId == id).Select(r => r.PhotoId).Contains(x.Id));

            if (photos.Count() > 0)
            {
                var model = new GalleryPhotoTagModel()
                {
                    TagName = CultureHelper.IsEnCulture() ? tag.TitleEng : tag.TitleRu,
                    Photos = photos.Select(x => new GalleryPhotoModel()
                    {
                        Id = x.Id,
                        Thumbnail = x.ThumbnailPath,
                        Photo = x.PhotoPath
                    })
                };

                return View("~/Views/Gallery/TagRenderPattern.cshtml", model);
            }
            else
                return View(new GalleryPhotoListModel());
        }
    }
}