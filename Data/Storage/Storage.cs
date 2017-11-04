using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using AskanioPhotoSite.Data.Entities;
using AskanioPhotoSite.Data.Repositories;
using AskanioPhotoSite.Data.Storage.Queries;
using AskanioPhotoSite.Data.Storage.Queries.Interpreter;
using AskanioPhotoSite.Data.Storage.Transactions;
using NLog;

namespace AskanioPhotoSite.Data.Storage
{
    public sealed class Storage : IStorage
    {
        private readonly Logger _log = LogManager.GetLogger("storage");
        private readonly IDictionary<object, object> _repositories;

        private object _locker = new object();
        public TransactionPool TransactionPool;

        public ICache Cache { get; set; }

        private readonly DirectoryInfo _directory;

        private const string CacheKey = "AskanioCachedData";

        public Storage()
        {
            _repositories = new Dictionary<object, object>();
            _repositories.Add(typeof(Album), new GenericRepository<Album>(this));
            _repositories.Add(typeof(Photo), new GenericRepository<Photo>(this));
            _repositories.Add(typeof(Tag), new GenericRepository<Tag>(this));
            _repositories.Add(typeof(PhotoToTag), new GenericRepository<PhotoToTag>(this));
            _repositories.Add(typeof(TextAttributes), new GenericRepository<TextAttributes>(this));
        }

        public Storage(DirectoryInfo directory) : this()
        {
            if (directory == null) throw new ArgumentNullException("directory");
            _directory = directory;       
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : Entity
        {
            try
            {
                return (IRepository<TEntity>) _repositories[typeof(TEntity)];
            }
            catch(KeyNotFoundException)
            {
                _log.Error("[STORAGE]:Service locator failed to resolve to a finite instance");
                throw new ApplicationException("Service locator failed to resolve to a finite instance");
            }
        }

        public IEnumerable<TEntity> GetDataFromCache<TEntity>()
        {
            if (Cache == null) Cache = GetCache();
            if (!Cache.IsActual) UpdateCache(Cache);

            return Cache.GetEntities<TEntity>();
        }

        public void AddToPool<TEntity>(TransactionServiceInfo info)
        {
            if(TransactionPool == null) TransactionPool = new TransactionPool(_directory);
            TransactionPool.Put<TEntity>(info);
        }

        public IQueryResult<TEntity> Execute<TEntity>(IQuery<TEntity> query)
        {
            if (query.QueryType == QueryType.Read)
            {
                using (var processor = new Processor<TEntity>(new Interpreter<TEntity>(), _directory))
                {
                    _log.Info(string.Format("[EXECUTE READ]: Reading data from {0}", typeof(TEntity).Name));

                    var result = processor.Read(query);

                    if (!result.IsSuccess)
                    {
                        _log.Error($"[EXECUTE READ]:{result.ErrorMessage}");
                        throw new Exception(result.ErrorMessage);
                    }

                    return result;
                }
            }
            else
            {
                using (var processor = new Processor<TEntity>(new Interpreter<TEntity>(), _directory))
                {
                    _log.Info(string.Format("[EXECUTE WRITE]: Pre-writing data to {0}", typeof(TEntity).Name));

                    var result = processor.Write(query);

                    if (!result.IsSuccess)
                    {
                        throw new Exception(result.ErrorMessage);
                    }

                    _log.Info(string.Format("[EXECUTE WRITE]: Add service info on {0} to transaction pool", typeof(TEntity).Name));

                    AddToPool<TEntity>(result.ServiceInfo);
                    return result;
                }
            }
        }

        public void Commit()
        {
            try
            {
                var dictionary = TransactionPool.GetDictionary;
                lock (_locker)
                {
                    _log.Info(string.Format("[COMMIT] START WRITING"));

                    foreach (var key in dictionary)
                    {
                        if (key.Value.Modified == null) return;

                        _log.Info(string.Format("[COMMIT] Data commit {0}", key.Value.FilePath));
                        using (var transaction = new Transaction(key.Value.FilePath, key.Value.Modified))
                        {
                            transaction.WriteStream();
                        }

                        if(File.Exists(key.Value.FilePath + ".backup")) File.Delete(key.Value.FilePath + ".backup");
                    }
                    MarkAsModified();
                    TransactionPool.SetMarker(false);
                    TransactionPool = null;
                }
            }
            catch (Exception exception)
            {
                TransactionPool = null;
                TransactionPool?.RollBack();

                _log.Error($"[COMMIT]:{exception.Message}");
                throw new Exception("Transaction failed.", exception);
            }
            
        }

        public ICache GetCache()
        {
            ICache cache = HttpContext.Current?.Cache[CacheKey] as Cache;
            if (cache == null)
            {
                cache = new Cache();

                UpdateCache(cache);
            }

            return cache;
        }

        public void UpdateCache(ICache cache)
        {
            if (cache == null) throw new ArgumentNullException("cache");

            cache.Reset();
            cache.AddEntity(Execute(new Query<Album>() { QueryType = QueryType.Read, ActionType = ActionType.Select }).Result.ToList());
            cache.AddEntity(Execute(new Query<Photo>() { QueryType = QueryType.Read, ActionType = ActionType.Select }).Result.ToList());
            cache.AddEntity(Execute(new Query<Tag>() { QueryType = QueryType.Read, ActionType = ActionType.Select }).Result.ToList());
            cache.AddEntity(Execute(new Query<PhotoToTag>() { QueryType = QueryType.Read, ActionType = ActionType.Select }).Result.ToList());
            cache.AddEntity(Execute(new Query<TextAttributes>() { QueryType = QueryType.Read, ActionType = ActionType.Select }).Result.ToList());
            cache.IsActual = true;
            HttpContext.Current.Cache[CacheKey] = cache;

            Cache = cache;
        }

        public void MarkAsModified()
        {
            ICache cache = HttpContext.Current?.Cache[CacheKey] as Cache;

            if (cache == null) cache = GetCache();

            cache.IsActual = false;

            HttpContext.Current.Cache[CacheKey] = cache;
        }
    }
}
