using System;
using AskanioPhotoSite.Core.Storage.Queries;

namespace AskanioPhotoSite.Core.Storage.Transactions
{
   public interface ITransaction<TEntity> : IDisposable 
   {
       IQueryResult<TEntity> Read(IQuery<TEntity> query);
       IQueryResult<TEntity> Write(IQuery<TEntity> query);
       void Commit();
       void RollBack();
   }
}
