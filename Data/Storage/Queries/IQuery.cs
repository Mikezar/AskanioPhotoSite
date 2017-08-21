using System.Collections.Generic;

namespace AskanioPhotoSite.Data.Storage.Queries
{
    public interface IQuery<TEntity> 
    {
        QueryType QueryType { get; set; }

        ICollection<TEntity> Entities { get; set; }

        ActionType ActionType { get; set; }

        ICollection<int> Keys { get; set; }

    }
}
