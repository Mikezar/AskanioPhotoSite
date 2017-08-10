using System.Collections.Generic;

namespace AskanioPhotoSite.Core.Storage.Queries
{
    public interface IQuery<TEntity, TKey>
    {
        QueryType QueryType { get; set; }

        ICollection<TEntity> Entities { get; set; }

        ActionType ActionType { get; set; }

        ICollection<TKey> Keys { get; set; }
    }
}
