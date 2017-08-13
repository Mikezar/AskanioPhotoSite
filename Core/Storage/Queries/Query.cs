using System.Collections.Generic;

namespace AskanioPhotoSite.Core.Storage.Queries
{
    public class Query<TEntity>: IQuery<TEntity>
    {
        public QueryType QueryType { get; set; }

        public ICollection<TEntity> Entities { get; set; }

        public ActionType ActionType { get; set; }

        public ICollection<int> Keys { get; set; }
    }
}
