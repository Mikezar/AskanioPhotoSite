using System.Collections.Generic;
using System.Linq;
using System.Web;
using AskanioPhotoSite.Data.Entities;
using AskanioPhotoSite.Core.Models;
using AskanioPhotoSite.Core.Services.Extensions;
using AskanioPhotoSite.Core.Services.Abstract;
using AskanioPhotoSite.Core.Convertors.Abstract;
using AskanioPhotoSite.Core.Services.Providers;
using System;
using AskanioPhotoSite.Core.Helpers;
using System.IO;
using System.Web.Mvc;
using AskanioPhotoSite.Core.Infrastructure.ImageHandler;

namespace AskanioPhotoSite.Core.Services.Concrete
{
    public class PhotoService : IPhotoService
    {
        private readonly IConverterFactory _factory;
        private readonly IPhotoConverter _converter;
        private readonly IWatermarkConverter _converterWatermark;
        private readonly ITagConverter _converterTag;
        private readonly BaseProvider<Photo> _providerPhoto;
        private readonly BaseProvider<Tag> _providerTag;
        private readonly BaseProvider<Watermark> _providerWatermark;
        private readonly BaseProvider<PhotoToTag> _providerPhotoToTag;
        private readonly BaseProvider<Album> _providerAlbum;

        public PhotoService(IConverterFactory factory, BaseProvider<Photo> provider,
            BaseProvider<Watermark> providerWatermark, BaseProvider<Tag> providerTag,
            BaseProvider<PhotoToTag> providerPhotoToTag, BaseProvider<Album> providerAlbum)
        {
            _providerPhoto = provider;
            _providerWatermark = providerWatermark;
            _providerPhotoToTag = providerPhotoToTag;
            _providerTag = providerTag;
            _factory = factory;
            _providerAlbum = providerAlbum;
            _converter = _factory.GetConverter<IPhotoConverter>();
            _converterWatermark = _factory.GetConverter<IWatermarkConverter>();
            _converterTag = _factory.GetConverter<ITagConverter>();
        }

        public IEnumerable<Photo> GetAll()
        {
            return _providerPhoto.GetAll().ToList();
        }

        public Photo GetOne(int id)
        {
            return _providerPhoto.GetOne(id);
        }

        public Photo[] AddMany(PhotoUploadModel[] models)
        {
            var photos = _providerPhoto.AddMany(_converter.ConvertToIEnumerable(models).ToArray());
            _providerWatermark.AddMany(
                _converterWatermark.ConvertToIEnumerable(
                    models.Where(x => x.ImageAttributes != null).Select(x => x.ImageAttributes).ToArray())
                    .ToArray());
            _providerPhotoToTag.AddMany(
                _converterTag.ConvertToIEnumerable(models).ToArray());

            _providerPhoto.Commit();

            return photos;
        }

        public Photo UpdateOne(Photo entity)
        {
            var photo = _providerPhoto.UpdateOne(entity);
            _providerPhoto.Commit();
            return photo;
        }

        public Photo UpdateOne(PhotoUploadModel model, HttpPostedFileBase file, IImageProcessor processor)
        {
            var photoToTags = _providerPhotoToTag.GetAll();
            var relatedTagIds = photoToTags.GetRelatedTags(model.Id).Select(x => x.Id).ToArray();

            if (relatedTagIds.Length > 0)
            {
                _providerPhotoToTag.DeleteMany(relatedTagIds);
                _providerPhoto.Commit(); //TODO: продумать
            }

            if(file != null)
            {
                if (file.ContentLength != 0 && file.ContentLength < 4048576)
                {
                    processor.CreateThumbnail(file, 350, 350, model.FileName);
                    file.SaveAs(HttpContext.Current.Server.MapPath(model.PhotoPath));
                }
            }

            var photos = _providerPhoto.UpdateOne(_converter.ConvertTo(model));
            _providerWatermark.UpdateOne(_converterWatermark.ConvertTo(model.ImageAttributes));

            if (model.RelatedTagIds != null)
            {
                _providerPhotoToTag.AddMany(model.RelatedTagIds
                    .Select(x => new PhotoToTag()
                    {
                        Id = 0,
                        PhotoId = model.Id,
                        TagId = x
                    }).ToArray()
                );
            }

            _providerPhoto.Commit();

            return photos;
        }

        public void DeleteOne(int id)
        {
            var photo = GetOne(id);
            var photoToTags = _providerPhotoToTag.GetAll().Where(x => x.PhotoId == id).ToList();
            var watermark = _providerWatermark.GetAll().FirstOrDefault(x => x.PhotoId == id);

            if (photoToTags.Count() > 0)
                _providerPhotoToTag.DeleteMany(photoToTags.Select(x => x.Id).ToArray());

            if (watermark != null)
                _providerWatermark.DeleteOne(watermark.Id);

            System.IO.File.Delete(HttpContext.Current?.Server?.MapPath(photo?.PhotoPath));
            System.IO.File.Delete(HttpContext.Current?.Server?.MapPath(photo?.ThumbnailPath));

            _providerPhoto.DeleteOne(id);
            _providerPhoto.Commit();
        }

        public void DeleteMany(int[] ids)
        {
            foreach (var id in ids)
                DeleteOne(id);
        }

        public IEnumerable<Photo> GetOrphans()
        {
            return GetAll().Where(r => r.AlbumId == 0);
        }

        public IEnumerable<Photo> GetRandomPhotos()
        {
            return GetAll().Where(x => x.ShowRandom == true);
        }

        public Photo GetRandomPhoto()
        {
            Random random = new Random();

            var photos = GetAll().Where(x => x.ShowRandom == true).ToList();

            if (photos.Count > 0)
                return photos[random.Next(0, photos.Count)];

            return null;
        }

        public IEnumerable<Photo> GetBackgroundPhotos()
        {
            return GetAll().Where(x => x.IsForBackground == true);
        }

        public GalleryPhotoViewModel GetNextPrevPhoto(int id, bool isNext, bool includeTag)
        {
            var currentPhoto = GetOne(id);

            Photo[] photos;

            if (includeTag)
            {
                var tags = _providerPhotoToTag.GetAll();
                var photoIds = tags.Where(x =>
                    tags.GetRelatedTags(currentPhoto.Id).Select(g => g.TagId).Contains(x.TagId)
                ).Select(x => x.PhotoId);
                photos = GetAll().Where(x => photoIds.Contains(x.Id)).ToArray();
            }
            else
                photos = GetAll().Where(x => x.AlbumId == currentPhoto.AlbumId).ToArray();

            Photo photo = new Photo();
            for (int i = 0; i < photos.Length; i++)
            {
                if (photos[i].Id == id)
                {
                    if (!isNext)
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

            return new GalleryPhotoViewModel()
            {
                Id = photo.Id,
                Title = CultureHelper.IsEnCulture() ? photo.TitleEng : photo.TitleRu,
                Path = photo.PhotoPath,
                Description = CultureHelper.IsEnCulture() ? photo.DescriptionEng : photo.DescriptionRu,
                IncludeTag = includeTag
            };

        }

        public void ChangeOrder(PhotoSortModel model)
        {
            var photos = GetAll();
            var currentPhoto = photos.FirstOrDefault(x => x.Id == model.CurrentId);
            var swappedPhoto = photos.FirstOrDefault(x => x.Id == model.SwappedId);

            currentPhoto.Order = currentPhoto.Order == default(int) ? model.CurrentId : currentPhoto.Order;
            swappedPhoto.Order = swappedPhoto.Order == default(int) ? model.SwappedId : swappedPhoto.Order;

            int temp = currentPhoto.Order;
            currentPhoto.Order = swappedPhoto.Order;
            swappedPhoto.Order = temp;

            _providerPhoto.UpdateOne(currentPhoto);
            _providerPhoto.Commit();
            _providerPhoto.UpdateOne(swappedPhoto);
            _providerPhoto.Commit();
        }

        public PhotoUploadListModel Upload(IEnumerable<HttpPostedFileBase> files, PhotoUploadListModel listModel, IImageProcessor processor)
        {
            var model = HttpContext.Current.Session["Uploads"] != null ? HttpContext.Current.Session["Uploads"] as PhotoUploadListModel : new PhotoUploadListModel();
            int maxId = 0;
            int order = 0;
            var photos = GetAll().ToList();

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
                    PhotoPath = "~/SysData/PhotoGallery/Photos/" + filename + Path.GetExtension(file.FileName).ToLower(),
                    ThumbnailPath = "~/SysData/PhotoGallery/Thumbs/" + filename + "s" + Path.GetExtension(file.FileName).ToLower(),
                    CreationDate = TimeZone.CurrentTimeZone.ToLocalTime(DateTime.Now),
                    ShowRandom = false,
                    IsForBackground = false,
                    ImageAttributes =  new ImageAttrModel()
                    {
                        Id = listModel.ImageAttributes.Id,
                        PhotoId = 0,
                        IsWatermarkApplied = listModel.ImageAttributes.IsWatermarkApplied,
                        IsWatermarkBlack = listModel.ImageAttributes.IsWatermarkBlack,
                        IsSignatureApplied = listModel.ImageAttributes.IsSignatureApplied,
                        IsSignatureBlack = listModel.ImageAttributes.IsSignatureBlack,
                        IsWebSiteTitleApplied = listModel.ImageAttributes.IsWebSiteTitleApplied,
                        IsWebSiteTitleBlack = listModel.ImageAttributes.IsWebSiteTitleBlack,
                        IsRightSide = listModel.ImageAttributes.IsRightSide
                    },
                    Album = _providerAlbum.GetOne(listModel.AlbumId),
                    Order = ++order
                };

                if (file.ContentLength < 4048576)
                {
                    if (file != null)
                    {
                        processor.CreateThumbnail(file, 350, 350, filename);
                        photoUploadModel.ImageAttributes.PhotoId = photoUploadModel.Id;
                        file.SaveAs(HttpContext.Current.Server.MapPath(photoUploadModel.PhotoPath));
                        model.Uploads.Add(photoUploadModel);
                        HttpContext.Current.Session["Uploads"] = model;
                    }
                }
            }
            model.Albums = _providerAlbum.GetAll().Select(x => new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.TitleRu
            });

            return model;
        }
    }
}
