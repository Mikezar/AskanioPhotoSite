using System;
using System.Collections.Generic;

namespace AskanioPhotoSite.Core.Storage.Queries
{
    public interface IQueryResult
    {
        bool IsSuccess { get; set; }

        string ErrorMessage { get; set; }

        Exception Exception { get; set; }

        IEnumerable<object> Result { get; set; }
    }
}
