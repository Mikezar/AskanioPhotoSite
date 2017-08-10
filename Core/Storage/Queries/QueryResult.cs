using System;
using System.Collections.Generic;
using AskanioPhotoSite.Core.Entities;

namespace AskanioPhotoSite.Core.Storage.Queries
{
    public class QueryResult<TEntity> : IQueryResult<TEntity>
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public Exception Exception { get; set; }
        public IEnumerable<TEntity> Result { get; set; }
    }
}
