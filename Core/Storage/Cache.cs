using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AskanioPhotoSite.Core.Entities;

namespace AskanioPhotoSite.Core.Storage
{
   public class Cache : ICache
   {
       private bool _isActual = true;

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

    }
}
