using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AskanioPhotoSite.Data.Entities;
using AskanioPhotoSite.Data.Storage;

namespace AskanioPhotoSite.Core.Services
{
    public sealed class TagService : BaseService<Tag>
    {
        public TagService(IStorage storage) : base (storage) { }

        public override IEnumerable<Tag> GetAll()
        {
            return _storage.GetRepository<Tag>().GetAll().ToList();
        }

        public override Tag[] AddMany(object[] obj)
        {
            throw new NotImplementedException();
        }

        public override Tag AddOne(object obj)
        {
            throw new NotImplementedException();
        }

        public override void DeleteOne(int id)
        {
            throw new NotImplementedException();
        }

        public override Tag GetOne(int id)
        {
            throw new NotImplementedException();
        }

        public override Tag UpdateOne(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
