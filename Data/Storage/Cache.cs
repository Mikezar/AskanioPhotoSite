using System.Collections.Generic;

namespace AskanioPhotoSite.Data.Storage
{
   public class Cache : ICache
   {
       private bool _isActual;

       public bool IsActual
       {
           get { return _isActual; }
           set { _isActual = value; }
       }

       private IDictionary<object, IEnumerable<object>> Entities = new Dictionary<object, IEnumerable<object>>();

       public ICollection<TEntity> GetEntities<TEntity>()
       {
           return (ICollection<TEntity>)Entities[typeof(TEntity)];
       }

       public void AddEntity<TEntity>(IEnumerable<TEntity> entities)
       {
           Entities.Add(typeof(TEntity), (IEnumerable<object>)entities);
       }

       public void Reset()
       {
           Entities = new Dictionary<object, IEnumerable<object>>();
        }

    }
}
