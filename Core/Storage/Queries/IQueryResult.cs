using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using AskanioPhotoSite.Core.Entities;
using AskanioPhotoSite.Core.Storage.Transactions;

namespace AskanioPhotoSite.Core.Storage.Queries
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
