using System.Collections.Generic;

namespace AskanioPhotoSite.Data.Storage
{
    public interface ICache
    {
        bool IsActual { get; set; }

        ICollection<TEntity> GetEntities<TEntity>();
        void AddEntity<TEntity>(IEnumerable<TEntity> entities);
        void Reset();
    }
}
