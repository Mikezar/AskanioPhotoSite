using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AskanioPhotoSite.Data.Entities;
using AskanioPhotoSite.Data.Storage;
using AskanioPhotoSite.Core.Models;
using AskanioPhotoSite.Core.Services.Extensions;

namespace AskanioPhotoSite.Core.Services
{
    public class PhotoService : BaseService<Photo>
    {
        public PhotoService(IStorage storage) : base (storage) { }

        public override IEnumerable<Photo> GetAll()
        {
            return _storage.GetRepository<Photo>().GetAll().ToList();
        }

        public override Photo GetOne(int id)
        {
            return GetAll().FirstOrDefault(x => x.Id == id);
        }

        public override Photo AddOne(object obj)
        {
            throw new NotImplementedException();
        }
    
        public override Photo[] AddMany(object[] obj)
        {
            var model = (PhotoUploadModel[])obj;

            var repository = _storage.GetRepository<Photo>();
            var photoToTagRepository = _storage.GetRepository<PhotoToTag>();
            var attributeRepository = _storage.GetRepository<Watermark>();

            var photos = new List<Photo>();
            var photoToTags = new List<PhotoToTag>();
            var attributes = new List<Watermark>();

            foreach (var photo in model)
            {
                var entity = new Photo()
                {
                    Id = 0,
                    AlbumId = photo.Album?.Id ?? 0,
                    TitleRu = photo.TitleRu,
                    TitleEng = photo.TitleEng,
                    DescriptionRu = photo.DescriptionRu,
                    DescriptionEng = photo.DescriptionEng,
                    PhotoPath = photo.PhotoPath,
                    ThumbnailPath = photo.ThumbnailPath,
                    FileName = photo.FileName,
                    CreationDate = photo.CreationDate,
                    ShowRandom = photo.ShowRandom
                };

                if (photo.ImageAttributes != null)
                {
                    attributes.Add(new Watermark()
                    {
                        Id = 0,
                        PhotoId = photo.Id,
                        IsWatermarkApplied = photo.ImageAttributes.IsWatermarkApplied,
                        IsWatermarkBlack = photo.ImageAttributes.IsWatermarkBlack,
                        IsSignatureApplied = photo.ImageAttributes.IsSignatureApplied,
                        IsSignatureBlack = photo.ImageAttributes.IsSignatureBlack,
                        IsWebSiteTitleApplied = photo.ImageAttributes.IsWebSiteTitleApplied,
                        IsWebSiteTitleBlack = photo.ImageAttributes.IsWebSiteTitleBlack,
                        IsRightSide = photo.ImageAttributes.IsRightSide
                    });
                }
                foreach (var tagId in photo.RelatedTagIds ?? new int[0])
                {
                    var photoToTag = new PhotoToTag()
                    {
                        Id = 0,
                        PhotoId = photo.Id,
                        TagId = tagId
                    };
                    photoToTags.Add(photoToTag);
                }

                photos.Add(entity);
            }

            var updatedPhotos = repository.AddMany(photos.ToArray());
            photoToTagRepository.AddMany(photoToTags.ToArray());
            attributeRepository.AddMany(attributes.ToArray());

           _storage.Commit();

            return updatedPhotos;
        }

        public override Photo UpdateOne(object obj)
        {
            var photo = (PhotoUploadModel)obj;

            var photoToTagRepository = _storage.GetRepository<PhotoToTag>();

            var entity = new Photo()
            {
                Id = photo.Id,
                AlbumId = photo.Album.Id,
                TitleRu = photo.TitleRu,
                TitleEng = photo.TitleEng,
                DescriptionRu = photo.DescriptionRu,
                DescriptionEng = photo.DescriptionEng,
                PhotoPath = photo.PhotoPath,
                ThumbnailPath = photo.ThumbnailPath,
                FileName = photo.FileName,
                CreationDate = photo.CreationDate,
                ShowRandom = photo.ShowRandom
            };

            var watermark = new Watermark()
            {
                IsRightSide = photo.ImageAttributes.IsRightSide,
                IsSignatureApplied = photo.ImageAttributes.IsSignatureApplied,
                IsSignatureBlack = photo.ImageAttributes.IsSignatureBlack,
                IsWatermarkApplied = photo.ImageAttributes.IsWatermarkApplied,
                IsWatermarkBlack = photo.ImageAttributes.IsWatermarkBlack,
                IsWebSiteTitleApplied = photo.ImageAttributes.IsWebSiteTitleApplied,
                IsWebSiteTitleBlack = photo.ImageAttributes.IsWebSiteTitleBlack,
                PhotoId = photo.ImageAttributes.PhotoId,
                Id = photo.ImageAttributes.Id
            };
            var photoToTags = photoToTagRepository.GetAll();

            var relatedTagIds = photoToTags.GetRelatedTags(photo.Id).Select(x => x.Id).ToArray();

            if (relatedTagIds.Length > 0)
            {
                photoToTagRepository.DeleteMany(relatedTagIds);
                _storage.Commit(); //TODO: продумать
            }


            if (photo.RelatedTagIds != null)
            {
                photoToTagRepository.AddMany(photo.RelatedTagIds
                    .Select(x => new PhotoToTag()
                    {
                        Id = 0,
                        PhotoId = photo.Id,
                        TagId = x
                    }).ToArray()
                );
            }
            _storage.GetRepository<Watermark>().UpdateOne(watermark);
            var updated = _storage.GetRepository<Photo>().UpdateOne(entity);
            _storage.Commit();

            return updated;
        }

        public override void DeleteOne(int id)
        {
            var photoToTagRepository = _storage.GetRepository<PhotoToTag>();
            var watermarkRepository = _storage.GetRepository<Watermark>();
            var photo = GetOne(id);
            var photoToTags = photoToTagRepository.GetAll().Where(x => x.PhotoId == id).ToList();

            var watermark = watermarkRepository.GetAll().FirstOrDefault(x => x.PhotoId == id);

            if (photoToTags.Count() > 0)
            {
                photoToTagRepository.DeleteMany(photoToTags.Select(x => x.Id).ToArray());
            }
            if (watermark != null)
                watermarkRepository.DeleteOne(watermark.Id);

            _storage.GetRepository<Photo>().DeleteOne(id);
            _storage.Commit();

            System.IO.File.Delete(HttpContext.Current.Server.MapPath(photo.PhotoPath));
            System.IO.File.Delete(HttpContext.Current.Server.MapPath(photo.ThumbnailPath));
        }

        public void GetIt()
        {

        }
    }
}
