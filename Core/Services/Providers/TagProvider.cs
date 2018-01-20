using AskanioPhotoSite.Data.Entities;
using AskanioPhotoSite.Data.Repositories;
using AskanioPhotoSite.Data.Storage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AskanioPhotoSite.Core.Services.Providers
{
    public class TagProvider : BaseProvider<Tag>
    {
        private readonly IRepository<Tag> _repositoryTag;

        public TagProvider(IStorage storage) : base (storage)
        {
            _repositoryTag = storage.GetRepository<Tag>();
        }

        public override Tag[] AddMany(Tag[] entities)
        {
            return _repositoryTag.AddMany(entities);   
        }

        public override Tag AddOne(Tag entity)
        {
            return _repositoryTag.AddOne(entity);
        }

        public override void DeleteMany(int[] ids)
        {
            _repositoryTag.DeleteMany(ids);
        }

        public override void DeleteOne(int id)
        {
            _repositoryTag.DeleteOne(id);
        }

        public override IEnumerable<Tag> GetAll()
        {
            return _repositoryTag.GetAll().ToList();
        }

        public override Tag GetOne(int id)
        {
            return GetAll().FirstOrDefault(x => x.Id == id);
        }

        public override Tag UpdateOne(Tag entity)
        {
            return _repositoryTag.UpdateOne(entity);
        }
    }
}
