using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace AskanioPhotoSite.Core.Storage.Queries
{
    public interface IQuery<TEntity> 
    {
        QueryType QueryType { get; set; }

        ICollection<TEntity> Entities { get; set; }

        ActionType ActionType { get; set; }

        ICollection<int> Keys { get; set; }

    }
}
