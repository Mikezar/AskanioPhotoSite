using AskanioPhotoSite.Data.Entities;
using AskanioPhotoSite.Data.Repositories;
using AskanioPhotoSite.Data.Storage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AskanioPhotoSite.Core.Services.Providers
{
    public class WatermarkProvider : BaseProvider<Watermark>
    {
        private readonly IRepository<Watermark> _repositoryWatermark;

        public WatermarkProvider(IStorage storage) : base (storage)
        {
            _repositoryWatermark = _storage.GetRepository<Watermark>();
        }

        public override IEnumerable<Watermark> GetAll()
        {
            return _repositoryWatermark.GetAll().ToList();
        }

        public override Watermark GetOne(int id)
        {
            return GetAll().FirstOrDefault(x => x.Id == id);
        }

        public override Watermark AddOne(Watermark entity)
        {
            return _repositoryWatermark.AddOne(entity);
        }

        public override Watermark[] AddMany(Watermark[] entities)
        {
            return _repositoryWatermark.AddMany(entities);
        }

        public override Watermark UpdateOne(Watermark entity)
        {
            return _repositoryWatermark.UpdateOne(entity);
        }

        public override void DeleteOne(int id)
        {
            _repositoryWatermark.DeleteOne(id);
        }

        public override void Commit()
        {
            base.Commit();
        }

        public override void DeleteMany(int[] ids)
        {
            throw new NotImplementedException();
        }
    }
}
