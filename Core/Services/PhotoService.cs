using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AskanioPhotoSite.Core.Entities;
using AskanioPhotoSite.Core.Storage;

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
