using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using AskanioPhotoSite.Core.Services.Extensions;
using AskanioPhotoSite.Core.Models;
using AskanioPhotoSite.Core.Services;
using AskanioPhotoSite.Data.Entities;
using AskanioPhotoSite.WebUI.Helpers;
using AskanioPhotoSite.Core.Enums;
using AskanioPhotoSite.Core.Helpers;
using AskanioPhotoSite.WebUI.Properties;
using AskanioPhotoSite.Core.Infrastructure.ImageHandler;
namespace AskanioPhotoSite.WebUI.Controllers
{
    [Auth]
    [Authorize(Users = "askanio")]
    public class ManagementController : BaseController
    {
        private readonly BaseService<Album> _albumService;

        private readonly BaseService<Photo> _photoService;

        private readonly BaseService<Tag> _tagService;

        private readonly BaseService<PhotoToTag> _photoToTagService;

        private readonly BaseService<TextAttributes> _textAttrService;

        private readonly BaseService<Watermark> _watermarkService;

        private readonly IImageProcessor _imageProcessor;


        public ManagementController(BaseService<Album> albumService, BaseService<Photo> photoService, 
            BaseService<Tag> tagService, BaseService<PhotoToTag> photoToTagService, 
            BaseService<TextAttributes> textAttrService, BaseService<Watermark> watermarkService,
            IImageProcessor imageProcessor)
        {
            _albumService = albumService;
            _photoService = photoService;
            _tagService = tagService;
            _photoToTagService = photoToTagService;
            _textAttrService = textAttrService;
            _watermarkService = watermarkService;
            _imageProcessor = imageProcessor;
        }

        public ActionResult Index()
        {
            var text = _textAttrService.GetAll().FirstOrDefault();

            if (text != null)
            {
                var model = new TextAttributeModel()
                {
                    Id = text.Id,
                    WatermarkFont = text.WatermarkFont,
                    WatermarkFontSize = text.WatermarkFontSize,
                    WatermarkText = text.WatermarkText,
                    SignatureFont = text.SignatureFont,
                    SignatureFontSize = text.SignatureFontSize,
                    SignatureText = text.SignatureText,
                    StampFont = text.StampFont,
                    StampFontSize = text.StampFontSize,
                    StampText = text.StampText
                };

                return View(model);
            }
            else
                return View(new TextAttributeModel()
                {
                    WatermarkFont = "Bell MT",
                    WatermarkFontSize = 60,
                    WatermarkText = "AlexSilver.Photo@gmail.com",
                    SignatureFont = "Edwardian Script ITC",
                    SignatureFontSize = 43,
                    SignatureText = "© Alexander Serebryakov",
                    StampFont = "Bell MT",
                    StampFontSize = 45,
                    StampText = "www.askanio.ru"
                });
        }


        #region Работа с настройками текста и шрифтов

        [HttpPost]
        public ActionResult UpdateTextAttribute(TextAttributeModel model)
        {
            try
            {
                if (model.Id == 0)
                {
                    var updated = _textAttrService.AddOne(model);
                    model.Id = updated.Id;
                    ViewBag.Success = $"Настройки текста и шрифтов были успешно добавлены";
                    return View("Index", model);
                }
                else
                {
                    _textAttrService.UpdateOne(model);
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
                var textSets = _textAttrService.GetAll().ToList();

                if (textSets.Count() > 0)
                {

                    foreach (var text in textSets)
                    {
                        _textAttrService.DeleteOne(text.Id);
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
                    RelativePhotoCount = _photoToTagService.GetAll().GetRelatedPhotoCount(_photoService.GetAll(), x.Id)
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

                ReturnUrl = Url.Action("EditAlbum")
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
                {
                    album.CoverPath = cover;
                }

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
            model.RelatedTagIds = _photoToTagService.GetAll().GetRelatedTags(photo.Id).Select(x => x.TagId).ToArray();
            model.AllTags = _tagService.GetAll().GetSelectListItem(model.RelatedTagIds);

            return PartialView("~/Views/Management/_EditUploadPhoto.cshtml", model);
        }

        [HttpPost]
        public ActionResult EditPhoto(PhotoUploadModel model)
        {
            try
            {
                _photoService.UpdateOne(model);

                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }

            }
            catch(Exception exception)
            {
                Log.RegisterError(exception);
            }
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
                Log.RegisterError(exception);
                return Json(MyAjaxHelper.GetErrorResponse(exception.Message));
            }
        }


        [HttpPost]
        public JsonResult ChangeOrder(PhotoSortModel model)
        {
            try
            {
                var photos = _photoService.GetAll();
                var currentPhoto = photos.FirstOrDefault(x => x.Id == model.CurrentId);
                var swappedPhoto = photos.FirstOrDefault(x => x.Id == model.SwappedId);

                currentPhoto.Order = currentPhoto.Order == default(int) ? model.CurrentId : currentPhoto.Order;
                swappedPhoto.Order = swappedPhoto.Order == default(int) ? model.SwappedId : swappedPhoto.Order;

                int temp = currentPhoto.Order;
                currentPhoto.Order = swappedPhoto.Order;
                swappedPhoto.Order = temp;

                 _photoService.UpdateOne(currentPhoto);
                 _photoService.UpdateOne(swappedPhoto);

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
            var model = Session["Uploads"] != null ? Session["Uploads"] as PhotoUploadListModel : new PhotoUploadListModel();
            int maxId = 0;
            int order = 0;
            var photos = _photoService.GetAll().ToList();

            // Если в БД уже есть фотографии, значит получаем макс. Id фотографии
            if (photos.Count > 0)
            {
                maxId = photos.Max(x => x.Id);
                order = photos.Max(x => x.Order);
            }

            // Проверяем, имеются ли в сессии уже загруженные фотогарфии, если да, то берем за макс Id значение из сессии
            if (model.Uploads.Any())
            {
                maxId = model.Uploads.Max(x => x.Id);
                order = model.Uploads.Max(x => x.Order);
            }

            foreach (var file in files)
            {
                var filename = $"photo_AS-S{++maxId}";

                var photoUploadModel = new PhotoUploadModel()
                {
                    Id = maxId,
                    FileName = filename,
                    PhotoPath = Settings.Default.PhotoPath + filename + Path.GetExtension(file.FileName).ToLower(),
                    ThumbnailPath = Settings.Default.ThumbPath + filename + "s" + Path.GetExtension(file.FileName).ToLower(),
                    CreationDate = TimeZone.CurrentTimeZone.ToLocalTime(DateTime.Now),
                    ShowRandom = false,
                    IsForBackground = false,
                    ImageAttributes = listModel.ImageAttributes,
                    Album = _albumService.GetOne(listModel.AlbumId),
                    Order = ++order
                };

                if (file.ContentLength < 4048576)
                {
                    if (file != null)
                    {
                        _imageProcessor.CreateThumbnail(file, 350, 350, filename);
                        listModel.ImageAttributes.PhotoId = photoUploadModel.Id;
                        file.SaveAs(Server.MapPath(photoUploadModel.PhotoPath));                
                        model.Uploads.Add(photoUploadModel);
                        Session["Uploads"] = model;
                    }
                }
            }
            model.Albums = _albumService.GetAll().Select(x => new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.TitleRu
            });

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
            photo.RelatedTagIds = _photoToTagService.GetAll().GetRelatedTags(photo.Id).Select(x => x.TagId).ToArray();
          
            return PartialView("~/Views/Management/_EditUploadPhoto.cshtml", photo);
        }

        [HttpPost]
        public ActionResult EditUploadPhoto(PhotoUploadModel model)
        {
            try
            {
                var list = Session["Uploads"] as PhotoUploadListModel;


                for (int i = 0; i < list.Uploads.Count; i++)
                {
                    if (list.Uploads[i].Id == model.Id) list.Uploads[i] = model;
                }

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