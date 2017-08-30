using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.ModelBinding;
using System.Web.Mvc;
using AskanioPhotoSite.Core.Services.Extensions;
using AskanioPhotoSite.Core.Models;
using AskanioPhotoSite.Core.Services;
using AskanioPhotoSite.Data.Entities;
using AskanioPhotoSite.WebUI.Models;
using AskanioPhotoSite.Core.Helpers;
namespace AskanioPhotoSite.WebUI.Controllers
{
    [Auth]
    [Authorize(Users = "askanio")]
    public class ManagementController : Controller
    {
        private readonly BaseService<Album> _albumService;

        private readonly BaseService<Photo> _photoService;

        private readonly BaseService<Tag> _tagService;

        public ManagementController(BaseService<Album> albumService, BaseService<Photo> photoService, BaseService<Tag> tagService)
        {
            _albumService = albumService;
            _photoService = photoService;
            _tagService = tagService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AlbumIndex()
        {
            var model = new AlbumListModel()
            {
                Albums = _albumService.GetAll().ToList()
            };

            return View(model);
        }

        public ActionResult PhotoIndex()
        {
            var model = new PhotoListModel()
            {
                Photos = _photoService.GetAll().Select(x => new PhotoModel()
                {
                    Id = x.Id,
                    DescriptionEng = x.DescriptionEng,
                    DescriptionRu = x.DescriptionRu,
                    PhotoPath = x.PhotoPath,
                    ThumbnailPath = x.ThumbnailPath,
                    TitleRu = x.TitleRu,
                    TitleEng = x.TitleEng,
                    Album = x.AlbumId == 0 ? new Album() : _albumService.GetOne(x.AlbumId)
                }).Where(x => x.Album.Id == 0).ToList()
            };

            return View(model);
        }

        #region Работа с тэгами

        public ActionResult TagIndex()
        {
            return View();
        }

        #endregion

        #region Работа с альбомами



        public ActionResult AddAlbum()
        {
            var model = new EditAlbumModel()
            {
                ParentAlbum = new Album(),
                ParentAlbums = _albumService.GetAll().GetSelectListItem()
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
                ParentAlbums = _albumService.GetAll().GetSelectListItem().Where(x => x.Value != album.Id.ToString())
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
                    if (model.ParentAlbum.Id != 0) model.ParentAlbum = _albumService.GetOne(model.ParentAlbum.Id);
                    ViewBag.Success = $"Альбом {model.TitleRu} был успешно добавлен";
                }
                else
                {
                    if (model.ParentAlbum.Id != 0) model.ParentAlbum = _albumService.GetOne(model.ParentAlbum.Id);
                    _albumService.UpdateOne(model);
                    ViewBag.Success = $"Альбом {model.TitleRu} был успешно обновлен";
                }
            }

            model.ParentAlbums = _albumService.GetAll().GetSelectListItem().Where(x => x.Value != model.Id.ToString());

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
            catch (Exception exception)
            {
                return Json(MyAjaxHelper.GetErrorResponse(exception.Message));
            }
        }

        #endregion

        #region Работа с фотографиями

        public ActionResult EditPhoto(int id)
        {
            var photo = _photoService.GetOne(id);

            var model = new PhotoUploadModel()
            {
                Id = photo.Id,
                PhotoPath = photo.PhotoPath,
                ThumbnailPath = photo.ThumbnailPath,
                CreationDate = photo.CreationDate,
                FileName = photo.FileName,
                TitleRu = photo.TitleRu,
                TitleEng = photo.TitleEng,
                DescriptionEng = photo.DescriptionEng,
                DescriptionRu = photo.DescriptionRu,
                Albums = _albumService.GetAll().GetSelectListItem(),
                Album = photo.AlbumId == 0 ? new Album() : _albumService.GetOne(id),
                Action = "EditPhoto"
            };

            return PartialView("~/Views/Management/_EditUploadPhoto.cshtml", model);
        }

        [HttpPost]
        public ActionResult EditPhoto(PhotoUploadModel model)
        {
            _photoService.UpdateOne(model);
            
            return RedirectToAction("PhotoIndex");
        }

        public ActionResult DeletePhoto(int id)
        {
            try
            {
                _photoService.DeleteOne(id);

                return Json(MyAjaxHelper.GetSuccessResponse());
            }
            catch (Exception exception)
            {
                return Json(MyAjaxHelper.GetErrorResponse(exception.Message));
            }
        }

        #endregion

        #region Загрузка и обработка фотографий



        [HttpGet]
        public ActionResult Upload()
        {
            var model = Session["Uploads"] as PhotoUploadListModel;

            return View(model ?? new PhotoUploadListModel());
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Upload(IEnumerable<HttpPostedFileBase> files)
        {
            var model = Session["Uploads"] != null ? Session["Uploads"] as PhotoUploadListModel : new PhotoUploadListModel();
            int maxId = 0;
            var photos = _albumService.GetAll().ToList();

            // Если в БД уже есть фотографии, значит получаем макс. Id фотографии
            if (photos.Count > 0) maxId = photos.Max(x => x.Id);

            // Проверяем, име.тся ли в сессии уже загруженные фотогарфии, если да, то берем за макс Id значение из сессии
            if (model.Uploads.Any()) maxId = model.Uploads.Max(x => x.Id);

            foreach (var file in files)
            {
                var filename = $"photo_AS-S{++maxId}";

                var photoUploadModel = new PhotoUploadModel()
                {
                    Id = maxId,
                    FileName = filename,
                    PhotoPath = "~/Content/Gallery/Photos/" + filename + Path.GetExtension(file.FileName).ToLower(),
                    ThumbnailPath = "~/Content/Gallery/Thumbs/" + filename + "s" + Path.GetExtension(file.FileName).ToLower(),
                    CreationDate = TimeZone.CurrentTimeZone.ToLocalTime(DateTime.Now)
                };

                if (file.ContentLength < 4048576)
                {
                    if (file != null)
                    {
                        ImageProcessor.CreateThumbnail(310, 210, file, photoUploadModel.ThumbnailPath);
                        ImageProcessor.ImageRotating(file, photoUploadModel.PhotoPath);

                        //if (IsWaterMark)
                        //{
                        //    ImageProcessing.ImageWatermarking(model.PhotoPath, file);
                        //}
                        //else
                        //{
                        //    ImageProcessing.ImageRotating(file, model.PhotoPath);
                        //}

                        model.Uploads.Add(photoUploadModel);

                        Session["Uploads"] = model;
                    }
                }
            }
       
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult SaveUploadPhoto()
        {
            var model = Session["Uploads"] as PhotoUploadListModel;

            try
            {

                _photoService.AddMany(model.Uploads.ToArray());

                Session["Uploads"] = null;

                return RedirectToAction("PhotoIndex");
            }
            catch
            {
                return PartialView("~/Views/Management/_Upload.cshtml", model);
            }
        }

        public ActionResult EditUploadPhoto(int id)
        {
            var model = Session["Uploads"] as PhotoUploadListModel;

            var photo = model.Uploads.Single(x => x.Id == id);

            photo.Action = "EditUploadPhoto";
            photo.Album = photo.Album ?? new Album();
            photo.Albums = _albumService.GetAll().GetSelectListItem();

            return PartialView("~/Views/Management/_EditUploadPhoto.cshtml", photo);
        }

        [HttpPost]
        public ActionResult EditUploadPhoto(PhotoUploadModel model)
        {
            var list = Session["Uploads"] as PhotoUploadListModel;


            for (int i = 0; i < list.Uploads.Count; i++)
            {
                if(list.Uploads[i].Id == model.Id) list.Uploads[i] = model;
            }

            Session["Uploads"] = list;

            return RedirectToAction("Upload");
        }

        [HttpPost]
        public ActionResult DeleteUploadPhoto(int id)
        {
            var model = Session["Uploads"] as PhotoUploadListModel;

            var photo = model.Uploads.Single(x => x.Id == id);

            System.IO.File.Delete(Server.MapPath(photo.PhotoPath));
            System.IO.File.Delete(Server.MapPath(photo.ThumbnailPath));
            model.Uploads.Remove(photo);

            return Json(MyAjaxHelper.GetSuccessResponse());
        }

        #endregion
    }
}