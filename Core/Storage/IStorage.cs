using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AskanioPhotoSite.Core.Entities;
using AskanioPhotoSite.Core.Repositories;
using AskanioPhotoSite.Core.Storage.Queries;
using AskanioPhotoSite.Core.Storage.Transactions;

namespace AskanioPhotoSite.Core.Storage
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
