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
    public class GalleryController : Controller
    {
        private readonly BaseService<Album> _albumService;

        private readonly BaseService<Photo> _photoService;

        public GalleryController(BaseService<Album> albumService, BaseService<Photo> photoService)
        {
            _albumService = albumService;
            _photoService = photoService;
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

            if(childs.Count() > 0)
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
                            Thumbnail = x.ThumbnailPath
                        })
                    };

                    return View(model);
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
    }
}