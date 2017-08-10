using System;
using System.Collections.Generic;
using AskanioPhotoSite.Core.Entities;
using AskanioPhotoSite.Core.Storage.Queries;

namespace AskanioPhotoSite.Core.Storage.Transactions
{
    public class Transaction<TEntity, TKey> : ITransaction<TEntity, TKey>
    {
        private bool _disposed;

        public IQueryResult Read(IQuery<TEntity, TKey> query)
        {
            return new QueryResult()
            {
                IsSuccess = true,
                Result = new List<Album>()
                {
                    new Album()
                    {
                        Id = 1,
                        DescriptionEng = "Test",
                        DescriptionRu =  "Тест",
                        TitleEng = "Test",
                        TitleRu = "Тест"
                    }
                }
            };
        }

        public IQueryResult Write(IQuery<TEntity, TKey> query)
        {
            throw new NotImplementedException();
        }

        public void Commit()
        {
            throw new NotImplementedException();
        }

        public void RollBack()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            if (_disposed) return;

        }
    }
}
