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
    }
}