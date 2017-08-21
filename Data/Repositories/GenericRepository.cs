using System.Collections.Generic;
using System.Linq;
using AskanioPhotoSite.Data.Storage;
using AskanioPhotoSite.Data.Storage.Queries;

namespace AskanioPhotoSite.Data.Repositories
{
    public class GenericRepository<TEntity> : IRepository<TEntity> 
    {
        private readonly IStorage _storage;

        public GenericRepository(IStorage storage)
        {
            _storage = storage;
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return _storage.GetDataFromCache<TEntity>();
        }

        public virtual TEntity AddOne(TEntity entity)
        {
            return _storage.Execute(QueryBuilder<TEntity>.AddOne(entity)).Result.SingleOrDefault();          
        }

        public virtual TEntity[] AddMany(TEntity[] entities)
        {
            return (TEntity[])_storage.Execute(QueryBuilder<TEntity>.AddMany(entities)).Result;
        }

        public virtual TEntity UpdateOne(TEntity entity)
        {
            return _storage.Execute(QueryBuilder<TEntity>.UpdateOne(entity)).Result.SingleOrDefault();
        }

        public virtual TEntity[] UpdateMany(TEntity[] entities)
        {
            return (TEntity[])_storage.Execute(QueryBuilder<TEntity>.UpdateMany(entities)).Result;
        }

        public virtual void DeleteOne(int key)
        {
             _storage.Execute(QueryBuilder<TEntity>.DeleteOne(key));
        }

        public virtual void DeleteMany(int[] keys)
        {
            _storage.Execute(QueryBuilder<TEntity>.DeleteMany(keys));
        }
    }
}
