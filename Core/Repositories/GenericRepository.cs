using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AskanioPhotoSite.Core.Entities;
using AskanioPhotoSite.Core.Storage;
using AskanioPhotoSite.Core.Storage.Queries;

namespace AskanioPhotoSite.Core.Repositories
{
    public class GenericRepository<TEntity,TKey> : IRepository<TEntity, TKey> where TEntity: Entity
    {
        private readonly IStorage _storage;
        private readonly ICache _cache;

        public GenericRepository(IStorage storage)
        {
            _storage = storage;
            _cache = storage.Cache;
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            if (_cache == null)
            {
                return _storage.Execute(QueryBuilder<TEntity, TKey>.GetAll()).Result;
            }

            return  _cache.GetEntities<TEntity>().AsEnumerable();
        }

        public virtual TEntity AddOne(TEntity entity)
        {
            return _storage.Execute(QueryBuilder<TEntity, TKey>.AddOne(entity)).Result.SingleOrDefault();          
        }

        public virtual TEntity[] AddMany(TEntity[] entities)
        {
            return (TEntity[])_storage.Execute(QueryBuilder<TEntity, TKey>.AddMany(entities)).Result;
        }

        public virtual TEntity UpdateOne(TEntity entity)
        {
            return _storage.Execute(QueryBuilder<TEntity, TKey>.UpdateOne(entity)).Result.SingleOrDefault();
        }

        public virtual TEntity[] UpdateMany(TEntity[] entities)
        {
            return (TEntity[])_storage.Execute(QueryBuilder<TEntity, TKey>.UpdateMany(entities)).Result;
        }

        public virtual void DeleteOne(TKey key)
        {
             _storage.Execute(QueryBuilder<TEntity, TKey>.DeleteOne(key));
        }

        public virtual void DeleteMany(TKey[] keys)
        {
            _storage.Execute(QueryBuilder<TEntity, TKey>.DeleteMany(keys));
        }
    }
}
