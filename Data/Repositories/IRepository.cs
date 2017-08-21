using System.Collections.Generic;

namespace AskanioPhotoSite.Data.Repositories
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
