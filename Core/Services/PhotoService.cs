using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AskanioPhotoSite.Data.Entities;
using AskanioPhotoSite.Data.Storage;
using AskanioPhotoSite.Core.Models;

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
            throw new NotImplementedException();
        }

        public override Photo AddOne(object obj)
        {
            throw new NotImplementedException();
        }
    
        public override Photo[] AddMany(object[] obj)
        {
            var model = (PhotoUploadModel[])obj;

            var repository = _storage.GetRepository<Photo>();

            var photos = new List<Photo>();

            foreach (var photo in model)
            {
                var entity = new Photo()
                {
                    Id = 0,
                    AlbumId = photo.Album.Id,
                    TitleRu = photo.TitleRu,
                    TitleEng = photo.TitleEng,
                    DescriptionRu = photo.DescriptionRu,
                    DescriptionEng = photo.DescriptionEng,
                    PhotoPath = photo.PhotoPath,
                    ThumbnailPath = photo.ThumbnailPath,
                    FileName = photo.FileName,
                    CreationDate = photo.CreationDate
                };

                photos.Add(entity);
            }

            var updatedPhotos =  _storage.GetRepository<Photo>().AddMany(photos.ToArray());

            _storage.Commit();

            return updatedPhotos;
        }

        public override Photo UpdateOne(object obj)
        {
            throw new NotImplementedException();
        }

        public override void DeleteOne(int id)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<SelectListItem> GetSelectListItem()
        {
            throw new NotImplementedException();
        }
    }
}
