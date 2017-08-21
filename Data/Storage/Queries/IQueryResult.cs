using System;
using System.Collections.Generic;
using AskanioPhotoSite.Data.Storage.Transactions;

namespace AskanioPhotoSite.Data.Storage.Queries
{
    public interface IQueryResult<TEntity>
    {
        bool IsSuccess { get; set; }

        string ErrorMessage { get; set; }

        Exception Exception { get; set; }

        IEnumerable<TEntity> Result { get; set; }

        TransactionServiceInfo ServiceInfo { get; set; }
    }
}
