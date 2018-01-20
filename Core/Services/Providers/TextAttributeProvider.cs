using AskanioPhotoSite.Data.Entities;
using AskanioPhotoSite.Data.Repositories;
using AskanioPhotoSite.Data.Storage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AskanioPhotoSite.Core.Services.Providers
{
    public class TextAttributeProvider : BaseProvider<TextAttributes>
    {
        private readonly IRepository<TextAttributes> _repositoryAttr;

        public TextAttributeProvider(IStorage storage) : base (storage)
        {
            _repositoryAttr = _storage.GetRepository<TextAttributes>();
        }

        public override IEnumerable<TextAttributes> GetAll()
        {
            return _repositoryAttr.GetAll().ToList();
        }

        public override TextAttributes GetOne(int id)
        {
            return GetAll().FirstOrDefault(x => x.Id == id);
        }

        public override TextAttributes[] AddMany(TextAttributes[] entities)
        {
            return _repositoryAttr.AddMany(entities);
        }

        public override TextAttributes AddOne(TextAttributes entity)
        {
            return _repositoryAttr.AddOne(entity);
        }

        public override void DeleteOne(int id)
        {
            _repositoryAttr.DeleteOne(id);
        }

        public override TextAttributes UpdateOne(TextAttributes entity)
        {
            return _repositoryAttr.UpdateOne(entity);
        }

        public override void Commit()
        {
            base.Commit();
        }

        public override void DeleteMany(int[] ids)
        {
            _repositoryAttr.DeleteMany(ids);
        }
    }
}
