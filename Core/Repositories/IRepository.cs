using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AskanioPhotoSite.Core.Entities;
using AskanioPhotoSite.Core.Storage;

namespace AskanioPhotoSite.Core.Repositories
{
    public interface IRepository<TEntity> 
    {
        IEnumerable<TEntity> GetAll();
        TEntity AddOne(TEntity entity);
        TEntity[] AddMany(TEntity[] entities);
        TEntity UpdateOne(TEntity entity);
        TEntity[] UpdateMany(TEntity[] entities);
        void DeleteOne(int key);
        void DeleteMany(int [] keys);
    }
}
