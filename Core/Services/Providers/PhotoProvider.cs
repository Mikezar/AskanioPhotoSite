using AskanioPhotoSite.Data.Entities;
using AskanioPhotoSite.Data.Repositories;
using AskanioPhotoSite.Data.Storage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AskanioPhotoSite.Core.Services.Providers
{
    public class PhotoProvider : BaseProvider<Photo>
    {
        private readonly IRepository<Photo> _repositoryPhoto;

        public PhotoProvider(IStorage storage) : base (storage)
        {
            _repositoryPhoto = _storage.GetRepository<Photo>();
        }

        public override IEnumerable<Photo> GetAll()
        {
            return _repositoryPhoto.GetAll().ToList();
        }

        public override Photo GetOne(int id)
        {
            return GetAll().FirstOrDefault(x => x.Id == id);
        }

        public override Photo AddOne(Photo entity)
        {
            return _repositoryPhoto.AddOne(entity);
        }

        public override Photo[] AddMany(Photo[] entities)
        {
            return _repositoryPhoto.AddMany(entities);
        }

        public override Photo UpdateOne(Photo entity)
        {
            return _repositoryPhoto.UpdateOne(entity);
        }

        public override void DeleteOne(int id)
        {
            _repositoryPhoto.DeleteOne(id);
        }

        public override void Commit()
        {
            base.Commit();
        }

        public override void DeleteMany(int[] ids)
        {
            _repositoryPhoto.DeleteMany(ids);
        }
    }
}
