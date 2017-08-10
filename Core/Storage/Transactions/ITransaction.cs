using System;
using AskanioPhotoSite.Core.Storage.Queries;

namespace AskanioPhotoSite.Core.Storage.Transactions
{
   public interface ITransaction<TEntity, TKey> : IDisposable
   {
       IQueryResult Read(IQuery<TEntity, TKey> query);
       IQueryResult Write(IQuery<TEntity, TKey> query);
       void Commit();
       void RollBack();
   }
}
