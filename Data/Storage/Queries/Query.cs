using System.Collections.Generic;

namespace AskanioPhotoSite.Data.Storage.Queries
{
    public class Query<TEntity>: IQuery<TEntity>
    {
        public QueryType QueryType { get; set; }

        public ICollection<TEntity> Entities { get; set; }

        public ActionType ActionType { get; set; }

        public ICollection<int> Keys { get; set; }

       // public override Type Type => typeof(TEntity);
    }

    //public abstract class Query 
    //{

    //   public abstract Type Type { get; }
    //}
}
