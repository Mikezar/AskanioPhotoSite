using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AskanioPhotoSite.Core.Entities;
using AskanioPhotoSite.Core.Repositories;
using AskanioPhotoSite.Core.Storage;

namespace AskanioPhotoSite.Core.Services
{
    public abstract class BaseService<TEntity>
    {
        protected readonly IStorage _storage;

        public BaseService(IStorage storage)
        {
            _storage = storage;
        }

        public abstract IEnumerable<TEntity> GetAll();

        public abstract TEntity GetOne(int id);

        public abstract TEntity AddOne(object obj);

        public abstract TEntity UpdateOne(object obj);

        public abstract void DeleteOne(int id);

        public abstract IEnumerable<SelectListItem> GetSelectListItem();
       
    }
}
