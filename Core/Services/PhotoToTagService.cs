using System;
using System.Collections.Generic;
using System.Linq;
using AskanioPhotoSite.Data.Entities;
using AskanioPhotoSite.Data.Storage;
using AskanioPhotoSite.Core.Models;

namespace AskanioPhotoSite.Core.Services
{
   public class PhotoToTagService : BaseService<PhotoToTag>
    {
        public PhotoToTagService(IStorage storage) : base (storage) { }

        public override PhotoToTag[] AddMany(object[] obj)
        {
            throw new NotImplementedException();
        }

        public override PhotoToTag AddOne(object obj)
        {
            throw new NotImplementedException();
        }

        public override void DeleteOne(int id)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<PhotoToTag> GetAll()
        {
            return _storage.GetRepository<PhotoToTag>().GetAll().ToList();
        }

        public override PhotoToTag GetOne(int id)
        {
            throw new NotImplementedException();
        }

        public override PhotoToTag UpdateOne(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
