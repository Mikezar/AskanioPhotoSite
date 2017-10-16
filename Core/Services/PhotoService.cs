using System;
using System.Collections.Generic;
using System.Linq;
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

            var photos = new List<Photo>();
            var photoToTags = new List<PhotoToTag>();


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

            var photoToTags = photoToTagRepository.GetAll();

            var relatedTagIds = photoToTags.GetRelatedTags(photo.Id).Select(x => x.Id).ToArray();

            if (relatedTagIds.Length > 0)
            {
                photoToTagRepository.DeleteMany(relatedTagIds);
                _storage.Commit(); //TODO: продумать
            }

         

            photoToTagRepository.AddMany(photo.RelatedTagIds
                .Select(x => new PhotoToTag()
                {
                    Id = 0,
                    PhotoId = photo.Id,
                    TagId = x
                }).ToArray()
            );



            var updated = _storage.GetRepository<Photo>().UpdateOne(entity);
            _storage.Commit();

            return updated;
        }

        public override void DeleteOne(int id)
        {
            var photoToTagRepository = _storage.GetRepository<PhotoToTag>();

            var photoToTags = photoToTagRepository.GetAll().Where(x => x.PhotoId == id).ToList();

            if (photoToTags.Count() > 0)
            {
                photoToTagRepository.DeleteMany(photoToTags.Select(x => x.Id).ToArray());
            }

            _storage.GetRepository<Photo>().DeleteOne(id);
            _storage.Commit();
        }

        public void GetIt()
        {

        }
    }
}
