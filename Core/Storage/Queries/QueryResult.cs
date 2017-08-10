using System;
using System.Collections.Generic;

namespace AskanioPhotoSite.Core.Storage.Queries
{
    public class QueryResult : IQueryResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public Exception Exception { get; set; }
        public IEnumerable<object> Result { get; set; }
    }
}
