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
       
       private IDictionary<object, ICollection<object>> Entities { get; set; } 

       public ICollection<TEntity> GetEntities<TEntity>()
       {
           return (ICollection<TEntity>)Entities[typeof(TEntity)];
       }

       public void AddEntity<TEntity>(ICollection<TEntity> entities)
       {
          // ICollection<object> converted = entities.Cast<object>().ToArray();
           Entities.Add(typeof(TEntity), (ICloneable<)entities);
       }

    }
}
