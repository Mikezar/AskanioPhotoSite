using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AskanioPhotoSite.Core.Entities;

namespace AskanioPhotoSite.Core.Storage
{
    public interface ICache
    {
        bool IsActual { get; set; }

        ICollection<TEntity> GetEntities<TEntity>();
        void AddEntity<TEntity>(IEnumerable<TEntity> entities);
        void Reset();
    }
}
