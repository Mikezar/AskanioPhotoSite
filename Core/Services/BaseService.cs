using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AskanioPhotoSite.Core.Entities;
using AskanioPhotoSite.Core.Repositories;
using AskanioPhotoSite.Core.Storage;

namespace AskanioPhotoSite.Core.Services
{
    public abstract class BaseService<TEntity>
    {
        public ICache Cache { get; set; }
        
        private const string CacheKey = "AskanioCachedData";

        protected readonly IStorage _storage;

        public BaseService(IStorage storage)
        {
            _storage = storage;
        }

        public abstract IEnumerable<TEntity> GetAll();

        public ICache GetCache()
        {
            ICache cache = HttpContext.Current.Cache[CacheKey] as Cache;
            if (cache == null)
            {
                cache = new Cache();

                foreach (var key in _storage.GetEntities)
                {
                    var repository = (IRepository<object>)key.Value;
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
