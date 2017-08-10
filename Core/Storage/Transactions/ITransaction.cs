using System;
using AskanioPhotoSite.Core.Storage.Queries;

namespace AskanioPhotoSite.Core.Storage.Transactions
{
   public interface ITransaction<TEntity, TKey> : IDisposable
   {
       IQueryResult<TEntity> Read(IQuery<TEntity, TKey> query);
       IQueryResult<TEntity> Write(IQuery<TEntity, TKey> query);
       void Commit();
       void RollBack();
   }
}
