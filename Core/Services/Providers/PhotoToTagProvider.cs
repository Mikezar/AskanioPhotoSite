using AskanioPhotoSite.Data.Entities;
using AskanioPhotoSite.Data.Repositories;
using AskanioPhotoSite.Data.Storage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AskanioPhotoSite.Core.Services.Providers
{
    public class PhotoToTagProvider : BaseProvider<PhotoToTag>
    {
        private readonly IRepository<PhotoToTag> _repositoryTag;

        public PhotoToTagProvider(IStorage storage) : base (storage)
        {
            _repositoryTag = storage.GetRepository<PhotoToTag>();
        }

        public override PhotoToTag[] AddMany(PhotoToTag[] entities)
        {
            return _repositoryTag.AddMany(entities);
        }

        public override PhotoToTag AddOne(PhotoToTag entity)
        {
            return _repositoryTag.AddOne(entity);
        }

        public override void DeleteOne(int id)
        {
            _repositoryTag.DeleteOne(id);
        }

        public override IEnumerable<PhotoToTag> GetAll()
        {
            return _repositoryTag.GetAll().ToList();
        }

        public override PhotoToTag GetOne(int id)
        {
            return GetAll().FirstOrDefault(x => x.Id == id);
        }

        public override PhotoToTag UpdateOne(PhotoToTag entity)
        {
            return _repositoryTag.UpdateOne(entity);
        }

        public override void DeleteMany(int[] ids)
        {
            _repositoryTag.DeleteMany(ids);
        }

        public override void Commit()
        {
            base.Commit();
        }
    }
}
