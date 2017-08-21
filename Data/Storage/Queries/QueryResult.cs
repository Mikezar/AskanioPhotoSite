using System;
using System.Collections.Generic;
using AskanioPhotoSite.Data.Storage.Transactions;

namespace AskanioPhotoSite.Data.Storage.Queries
{
    public class QueryResult<TEntity> : IQueryResult<TEntity>
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public Exception Exception { get; set; }
        public IEnumerable<TEntity> Result { get; set; }
        public TransactionServiceInfo ServiceInfo { get; set; }
    }
}
