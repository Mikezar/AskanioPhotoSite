using System.Web.Mvc;
using System.Web.Security;
using System.Linq;
using System;
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
            var albums = _albumService.GetAll();

            var model = new GalleryViewModel()
            {
                Albums = albums.Where(t => t.ParentId == 0).Select(x => new GalleryAlbumModel()
                {
                    Id = x.Id,
                    TitleRu = x.TitleRu,
                    TitleEng = x.TitleEng,
                    Cover = albums.Where(f => f.ParentId == x.Id).SingleOrDefault(r =>
                    {
                        var childs = albums.Where(f => f.ParentId == x.Id);
                        return childs.ElementAt(new Random().Next(0, childs.Count())).CoverPath != null;
                    })?.CoverPath
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
                        Photos = photos.OrderBy(x => x.Order).Select(x => new GalleryPhotoModel()
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
            var currentPhoto = _photoService.GetOne(id);

            Photo[] photos;

            if (includeTag)
            {
                var tags = _photoToTagService.GetAll();
                var photoIds = tags.Where(x => 
                    tags.GetRelatedTags(currentPhoto.Id).Select(g => g.TagId).Contains(x.TagId)
                ).Select(x => x.PhotoId);
                photos = _photoService.GetAll().Where(x => photoIds.Contains(x.Id)).ToArray();
            }
            else
                photos = _photoService.GetAll().Where(x => x.AlbumId == currentPhoto.AlbumId).ToArray();

            Photo photo = new Photo();
            for (int i = 0; i < photos.Length; i++)
            {
                if (photos[i].Id == id)
                {
                    if (isNext)
                    {
                        if (i + 1 >= photos.Length)
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
                IncludeTag = includeTag
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