using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AskanioPhotoSite.Core.Services.Extensions;
using AskanioPhotoSite.Core.Models;
using AskanioPhotoSite.Data.Entities;
using AskanioPhotoSite.WebUI.Helpers;
using AskanioPhotoSite.Core.Enums;
using AskanioPhotoSite.Core.Helpers;
using AskanioPhotoSite.Core.Infrastructure.ImageHandler;
using AskanioPhotoSite.Core.Services.Abstract;
using AskanioPhotoSite.Core.Convertors.Abstract;

namespace AskanioPhotoSite.WebUI.Controllers
{
    [Auth]
    [Authorize(Users = "askanio")]
    public class ManagementController : BaseController
    {
        private readonly IImageProcessor _imageProcessor;
        private readonly ITextAttributeService _attrService;
        private readonly IAlbumService _albumService;
        private readonly IPhotoService _photoService;
        private readonly ITagService _tagService;
        private readonly IConverterFactory _factory;
        private readonly ITextAttributeConverter _converterAttr;
        private readonly IWatermarkService _watermarkService;

        public ManagementController(ITextAttributeService attrService, IImageProcessor imageProcessor,
            IConverterFactory factory, IAlbumService albumService, IPhotoService photoService, 
            ITagService tagService, IWatermarkService watermarkService)
        {
            _imageProcessor = imageProcessor;
            _factory = factory;
            _converterAttr = _factory.GetConverter<ITextAttributeConverter>();
            _attrService = attrService;
            _albumService = albumService;
            _tagService = tagService;
            _photoService = photoService;
            _watermarkService = watermarkService;
        }

        public ActionResult Index()
        {
            var attr = _attrService.GetAll().FirstOrDefault();
            var model = _converterAttr.ConvertTo(attr) ??
                new TextAttributeModel()
                {
                    WatermarkFont = "Bell MT",
                    WatermarkFontSize = 60,
                    WatermarkText = "AlexSilver.Photo@gmail.com",
                    SignatureFont = "Edwardian Script ITC",
                    SignatureFontSize = 43,
                    SignatureText = "© Alexander Serebryakov",
                    StampFont = "Bell MT",
                    StampFontSize = 45,
                    StampText = "www.askanio.ru",
                    Alpha = 80
                    
                };
            return View(model);
        }


        #region Работа с настройками текста и шрифтов

        [HttpPost]
        public ActionResult UpdateTextAttribute(TextAttributeModel model)
        {
            try
            {
                if (model.Id == 0)
                {
                    var updated = _attrService.AddOne(model);
                    model.Id = updated.Id;
                    ViewBag.Success = $"Настройки текста и шрифтов были успешно добавлены";
                    return View("Index", model);
                }
                else
                {
                    _attrService.UpdateOne(model);
                    ViewBag.Success = $"Настройки текста и шрифтов были успешно обновлены";
                    return View("Index", model);
                }
            }
            catch(Exception exception)
            {
                Log.RegisterError(exception);
                return View("Index", model);
            }
        }

        public ActionResult SetToDefault()
        {
            try
            {
                var textSets = _attrService.GetAll().ToList();

                if (textSets.Count() > 0)
                {
                    foreach (var text in textSets)
                    {
                        _attrService.DeleteOne(text.Id);
                    } 
                }

                ViewBag.Success = $"Настройки текста и шрифтов были сброшены";
            }
            catch(Exception exception)
            {
                Log.RegisterError(exception);
            }

            return View("Index", new TextAttributeModel());
        }

        #endregion


        public ActionResult AlbumIndex()
        {
            var model = new AlbumListModel()
            {
                Albums = _albumService.GetAll().Select(x => new AlbumModel()
                {
                    Id = x.Id,
                    Title = x.TitleRu,
                    PhotoCount = _photoService.GetAll().GetPhotoCountInAlbum(x.Id)
                }).ToList()
            };

            return View(model);
        }

        public ActionResult PhotoIndex()
        {
            return View(new PhotoFilterManagement());
        }

        public ActionResult FilterPhoto(PhotoFilter filter = PhotoFilter.None)
        {
            var model = new PhotoListModel();

            try
            {
                switch (filter)
                {
                    case PhotoFilter.Random:
                        model.Photos = _photoService.GetRandomPhotos().InitPhotoListModel();
                        break;
                    case PhotoFilter.NoAlbum:
                        model.Photos = _photoService.GetOrphans().InitPhotoListModel();
                        break;
                    case PhotoFilter.Background:
                        model.Photos = _photoService.GetBackgroundPhotos().InitPhotoListModel();
                        break;
                    default:
                        model.Photos = _photoService.GetRandomPhotos().Union(_photoService.GetOrphans()).Union(_photoService.GetBackgroundPhotos()).InitPhotoListModel();
                        break;
                }
            }
            catch (Exception exception)
            {
                Log.RegisterError(exception);
            }
            return PartialView("~/Views/Management/_PhotoList.cshtml", model);
        }

        public ActionResult TagIndex()
        {
            var model = new TagListModel()
            {
                Tags = _tagService.GetAll().Select(x => new TagModel()
                {
                    Id = x.Id,
                    Title = x.TitleRu,
                    RelativePhotoCount = _tagService.GetRelatedPhotoCount(x.Id)
                }).ToList()
            };

            return View(model);
        }


        #region Работа с тэгами

        public ActionResult AddTag()
        {
            return View("_EditTag", new EditTagModel());
        }

        public ActionResult EditTag(int id)
        {
            var tag = _tagService.GetOne(id);

            var model = new EditTagModel()
            {
                Id = tag.Id,
                TitleEng = tag.TitleEng,
                TitleRu = tag.TitleRu
            };

            return PartialView("_EditTag", model);
        }

        [HttpPost]
        public ActionResult EditTag(EditTagModel model)
        {
            if (!ModelState.IsValid)
                return PartialView("_EditTag", model);

            try
            {
                if (model.Id == 0)
                    _tagService.AddOne(model);
                else
                    _tagService.UpdateOne(model);
            }
            catch(Exception exception)
            {
                Log.RegisterError(exception);
            }

            return RedirectToAction("TagIndex");
        }

        [HttpPost]
        public ActionResult DeleteTag(int id)
        {
            try
            {
                _tagService.DeleteOne(id);

                return Json(MyAjaxHelper.GetSuccessResponse());
            }
            catch (Exception exception)
            {
                Log.RegisterError(exception);
                return Json(MyAjaxHelper.GetErrorResponse(exception.Message));
            }
        }

        #endregion

        #region Работа с альбомами

        public ActionResult AddAlbum()
        {
            var model = new EditAlbumModel()
            {
                ParentAlbum = new Album(),
                ParentAlbums = _albumService.GetAvailableAlbumSelectList(_photoService.GetAll()),
                Photos = new PhotoListModel(),
                ViewPatterns = ViewPatternHelper.GetPatterns()
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
                ParentAlbums = _albumService.GetAvailableAlbumSelectList(_photoService.GetAll()).Where(x => x.Value != album.Id.ToString()),
                IsParent = _albumService.GetAll().isParent(album),
                ViewPattern =  album.ViewPattern,
                CoverPath = album.CoverPath,
                ViewPatterns = ViewPatternHelper.GetPatterns()
            };

            var photos = _photoService.GetAll().Where(x => x.AlbumId == model.Id);

            model.Photos = new PhotoListModel()
            {
                Photos = photos.Select(x => new PhotoModel()
                {
                    Id = x.Id,
                    DescriptionEng = x.DescriptionEng,
                    DescriptionRu = x.DescriptionRu,
                    PhotoPath = x.PhotoPath,
                    ThumbnailPath = x.ThumbnailPath,
                    TitleRu = x.TitleRu,
                    TitleEng = x.TitleEng,
                    Album = _albumService.GetOne(model.Id),
                    Order = x.Order == default(int) ? x.Id : x.Order
                }).OrderBy(x => x.Order).ToList(),

                ReturnUrl = Url.Action("EditAlbum"),
                ShowOrderArrows = true
            };

            return View(model);
        }

        [HttpPost]
        public JsonResult SetAlbumCover(int id, string cover)
        {
            try
            {
                var album = _albumService.GetOne(id);

                if (album != null)
                    album.CoverPath = cover;

                _albumService.UpdateOne(album);

                return Json(MyAjaxHelper.GetSuccessResponse());
            }
            catch (Exception exception)
            {
                Log.RegisterError(exception);
                return Json(MyAjaxHelper.GetErrorResponse(exception.Message));
            }
        }


        [HttpPost]
        public ActionResult EditAlbum(EditAlbumModel model)
        {
            try
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

                model.ParentAlbums = _albumService.GetAvailableAlbumSelectList(_photoService.GetAll()).Where(x => x.Value != model.Id.ToString());
                model.ViewPatterns = ViewPatternHelper.GetPatterns();
                model.Photos = new PhotoListModel()
                {
                    Photos = _photoService.GetAll().Where(x => x.AlbumId == model.Id).Select(x => new PhotoModel()
                    {
                        Id = x.Id,
                        DescriptionEng = x.DescriptionEng,
                        DescriptionRu = x.DescriptionRu,
                        PhotoPath = x.PhotoPath,
                        ThumbnailPath = x.ThumbnailPath,
                        TitleRu = x.TitleRu,
                        TitleEng = x.TitleEng,
                        Album = _albumService.GetOne(model.Id),
                        Order = x.Order
                    }).OrderBy(x => x.Order).ToList(),
                    ReturnUrl = Url.Action("EditAlbum")
                };
            }
            catch(Exception exception)
            {
                Log.RegisterError(exception);
            }

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
                Log.RegisterError(exception);
                return Json(MyAjaxHelper.GetErrorResponse(exception.Message));
            }
        }

        #endregion

        #region Работа с фотографиями

        public ActionResult EditPhoto(int id, string returnUrl = null)
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
                Albums = _albumService.GetAll().GetEndNodeAlbums().GetSelectListItem(),
                Album = photo.AlbumId == 0 ? new Album() : _albumService.GetOne(photo.AlbumId),
                Action = "EditPhoto",
                ReturnUrl = returnUrl,
                ShowRandom = photo.ShowRandom,
                IsForBackground = photo.IsForBackground,
                Order = photo.Order
            };

            var watermark = _watermarkService.GetAll().FirstOrDefault(x => x.PhotoId == photo.Id);
            model.ImageAttributes = new ImageAttrModel(watermark);
            model.RelatedTagIds = _tagService.GetRelatedTags(photo.Id);
            model.AllTags = _tagService.GetAll().GetSelectListItem(model.RelatedTagIds);

            return PartialView("~/Views/Management/_EditUploadPhoto.cshtml", model);
        }

        [HttpPost]
        public JsonResult EditPhoto(PhotoUploadModel model)
        {
            try
            {
                _photoService.UpdateOne(model);
                return Json(MyAjaxHelper.GetSuccessResponse());
            }
            catch(Exception exception)
            {
                Log.RegisterError(exception);
                return Json(MyAjaxHelper.GetErrorResponse(exception.Message));
            }
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
                Log.RegisterError(exception);
                return Json(MyAjaxHelper.GetErrorResponse(exception.Message));
            }
        }


        [HttpPost]
        public JsonResult ChangeOrder(PhotoSortModel model)
        {
            try
            {
                _photoService.ChangeOrder(model);

                return Json(MyAjaxHelper.GetSuccessResponse());
            }
            catch (Exception exception)
            {
                Log.RegisterError(exception);
                return Json(MyAjaxHelper.GetErrorResponse(exception.Message));
            }
        }

        #endregion

        #region Загрузка и обработка фотографий



        [HttpGet]
        public ActionResult Upload()
        {
            var model = Session["Uploads"] as PhotoUploadListModel;

            return View(model ?? new PhotoUploadListModel()
            {
                Albums = _albumService.GetAll().GetEndNodeAlbums().GetSelectListItem()
            });
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Upload(IEnumerable<HttpPostedFileBase> files, PhotoUploadListModel listModel)
        {
            var model = _photoService.Upload(files, listModel, _imageProcessor);
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
            catch(Exception exception)
            {
                Log.RegisterError(exception);
                return PartialView("~/Views/Management/_Upload.cshtml", model);
            }
        }

        public ActionResult EditUploadPhoto(int id)
        {
            var model = Session["Uploads"] as PhotoUploadListModel;

            var photo = model.Uploads.Single(x => x.Id == id);

            photo.Action = "EditUploadPhoto";
            photo.Album = photo.Album ?? new Album();
            photo.Albums = _albumService.GetAll().GetEndNodeAlbums().GetSelectListItem();
            photo.AllTags = _tagService.GetAll().GetSelectListItem(photo.RelatedTagIds);
          
            return PartialView("~/Views/Management/_EditUploadPhoto.cshtml", photo);
        }

        [HttpPost]
        public ActionResult EditUploadPhoto(PhotoUploadModel model)
        {
            try
            {
                var list = Session["Uploads"] as PhotoUploadListModel;

                for (int i = 0; i < list.Uploads.Count; i++)
                    if (list.Uploads[i].Id == model.Id) list.Uploads[i] = model;
                Session["Uploads"] = list;
            }
            catch(Exception exception)
            {
                Log.RegisterError(exception);
            }

            return RedirectToAction("Upload");
        }

        public ActionResult CancelUpload()
        {
            try
            {
                var list = Session["Uploads"] as PhotoUploadListModel;

                foreach (var photo in list.Uploads)
                {
                    System.IO.File.Delete(Server.MapPath(photo.PhotoPath));
                    System.IO.File.Delete(Server.MapPath(photo.ThumbnailPath));
                }

                Session["Uploads"] = null;
            }
            catch(Exception exception)
            {
                Log.RegisterError(exception);
            }

            return RedirectToAction("PhotoIndex");
        }

        [HttpPost]
        public ActionResult DeleteUploadPhoto(int id)
        {
            try
            {
                var model = Session["Uploads"] as PhotoUploadListModel;

                var photo = model.Uploads.Single(x => x.Id == id);

                System.IO.File.Delete(Server.MapPath(photo.PhotoPath));
                System.IO.File.Delete(Server.MapPath(photo.ThumbnailPath));
                model.Uploads.Remove(photo);

                return Json(MyAjaxHelper.GetSuccessResponse());
            }
            catch(Exception exception)
            {
                Log.RegisterError(exception);

                return Json(MyAjaxHelper.GetErrorResponse(exception.Message));
            }
        }

        #endregion
    }
}