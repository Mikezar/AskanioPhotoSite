using System;
using System.Collections.Generic;
using AskanioPhotoSite.Core.Entities;

namespace AskanioPhotoSite.Core.Storage.Queries
{
    public interface IQueryResult<TEntity>
    {
        bool IsSuccess { get; set; }

        string ErrorMessage { get; set; }

        Exception Exception { get; set; }

        IEnumerable<TEntity> Result { get; set; }
    }
}
