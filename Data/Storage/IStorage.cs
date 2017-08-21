using System.Collections.Generic;
using AskanioPhotoSite.Data.Entities;
using AskanioPhotoSite.Data.Repositories;
using AskanioPhotoSite.Data.Storage.Queries;
using AskanioPhotoSite.Data.Storage.Transactions;

namespace AskanioPhotoSite.Data.Storage
{
   public interface IStorage
   {
       ICache Cache { get; }
       IRepository<TEntity> GetRepository<TEntity>() where TEntity : Entity;
       IQueryResult<TEntity> Execute<TEntity>(IQuery<TEntity> query);
       IEnumerable<TEntity> GetDataFromCache<TEntity>();
       void AddToPool<TEntity>(TransactionServiceInfo info);
       ICache GetCache();
       void Commit();
       void UpdateCache(ICache cache);
       void MarkAsModified();
   }
}
