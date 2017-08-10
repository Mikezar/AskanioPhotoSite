using System.Collections.Generic;

namespace AskanioPhotoSite.Core.Storage.Queries
{
    public class Query<TEntity, TKey>: IQuery<TEntity, TKey>
    {
        public QueryType QueryType { get; set; }

        public ICollection<TEntity> Entities { get; set; }

        public ActionType ActionType { get; set; }

        public ICollection<TKey> Keys { get; set; }
    }
}
