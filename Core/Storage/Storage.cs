using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AskanioPhotoSite.Core.Entities;
using AskanioPhotoSite.Core.Repositories;
using AskanioPhotoSite.Core.Storage.Queries;
using AskanioPhotoSite.Core.Storage.Transactions;

namespace AskanioPhotoSite.Core.Storage
{
    public sealed class Storage : IStorage
    {
        private readonly IDictionary<object, object> _repositories;
        
        public ICache Cache { get; set; }

        private const string CacheKey = "AskanioCachedData";

        public Storage()
        {
            _repositories = new Dictionary<object, object>();
            _repositories.Add(typeof(Album), new GenericRepository<Album, int>(this));
            _repositories.Add(typeof(Photo), new GenericRepository<Photo, int>(this));
          //  Cache = GetCache();
        }

        public IRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : Entity
        {
            try
            {
                return (IRepository<TEntity, TKey>) _repositories[typeof(TEntity)];
            }
            catch(KeyNotFoundException)
            {
                throw new ApplicationException("Service locator failed to resolve to a finite instance");
            }
        }

        public IQueryResult<TEntity> Execute<TEntity, TKey>(IQuery<TEntity, TKey> query)
        {
            if (query.QueryType == QueryType.Read)
            {
                using (var transaction = new Transaction<TEntity, TKey>(new Interpreter<TEntity>()))
                {
                    var result = transaction.Read(query);

                    if (!result.IsSuccess)
                        throw new Exception(result.ErrorMessage);

                    return result;
                }
            }
            else 
            {
                using (var transaction = new Transaction<TEntity, TKey>(new Interpreter<TEntity>()))
                {
                    var result = transaction.Write(query);

                    if (!result.IsSuccess)
                    {
                        transaction.RollBack();
                        throw new Exception(result.ErrorMessage);
                    }

                    transaction.Commit();
                    return result;
                }
            }
        }

        public ICache GetCache()
        {
            ICache cache = HttpContext.Current.Cache[CacheKey] as Cache;
            if (cache == null)
            {
                cache = new Cache();

                foreach (var key in _repositories)
                {
                    var repository = (IRepository<Entity,int>)key.Value;
                    cache.AddEntity(repository.GetAll().ToList());
                }
                UpdateCache(cache);
            }

            return cache;
        }

        public void UpdateCache(ICache cache)
        {
            if (cache == null) throw new ArgumentNullException("cache");
            HttpContext.Current.Cache[CacheKey] = cache;
            Cache = cache;
        }

        public void MarkAsModified()
        {
            ICache cache = HttpContext.Current.Cache[CacheKey] as Cache;

            if (cache == null) cache = GetCache();

            cache.IsActual = false;

            UpdateCache(cache);
        }

    }
}
