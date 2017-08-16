using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using System.Web.Routing;
using AskanioPhotoSite.Core.Entities;
using AskanioPhotoSite.Core.Models;
using AskanioPhotoSite.Core.Services;
using AskanioPhotoSite.WebUI.Models;
using Microsoft.Ajax.Utilities;

namespace AskanioPhotoSite.WebUI.Controllers
{
    [Auth]
    [Authorize(Users = "askanio")]
    public class ManagementController : Controller
    {
        private readonly BaseService<Album> _albumService;

        public ManagementController(BaseService<Album> albumService)
        {
            _albumService = albumService;
        }

        // GET: Management
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AlbumIndex()
        {
            var model = new AlbumModel()
            {
                Albums = _albumService.GetAll().ToList()
            };

            return View(model);
        }

        public ActionResult PhotoIndex()
        {
            return View();
        }


        public ActionResult AddAlbum()
        {
            var model = new EditAlbumModel()
            {
                ParentAlbum = new Album(),
                ParentAlbums = _albumService.GetSelectListItem()
            };
            return View("EditAlbum", model);
        }

        public ActionResult EditAlbum(int id)
        {
            var album = _albumService.GetOne(id);

            if (album == null) throw new NullReferenceException();

            var model = new EditAlbumModel()
            {
                Id = album.Id,
                ParentAlbum = album.ParentId == 0 ? new Album() : _albumService.GetOne(album.ParentId),
                DescriptionRu = album.DescriptionRu,
                DescriptionEng = album.DescriptionEng,
                TitleRu = album.TitleRu,
                TitleEng = album.TitleEng,
                ParentAlbums = _albumService.GetSelectListItem()
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult EditAlbum(EditAlbumModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.Id == 0)
                {
                    var added = _albumService.AddOne(model);
                    model.Id = added.Id;
                    ViewBag.Success = $"Альбом {model.TitleRu} был успешно добавлен";
                }
                else
                {
                    _albumService.UpdateOne(model);
                    ViewBag.Success = $"Альбом {model.TitleRu} был успешно обновлен";
                }
            }

            model.ParentAlbums = _albumService.GetSelectListItem();

            return View(model);
        }

        [HttpPost]
        public ActionResult DeleteAlbum(int id)
        {
            try
            {
                _albumService.DeleteOne(id);

                return Json(MyAjaxHelper.GetSuccessResponse());
            }
            catch(Exception exception)
            {
                return Json(MyAjaxHelper.GetErrorResponse(exception.Message));
            }
        }
    }
}