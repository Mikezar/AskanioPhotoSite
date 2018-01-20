using System.Collections.Generic;
using AskanioPhotoSite.Data.Storage;

namespace AskanioPhotoSite.Core.Services.Providers
{
    public abstract class BaseProvider<TEntity>
    {
        protected readonly IStorage _storage;

        public BaseProvider(IStorage storage)
        {
            _storage = storage;
        }

        public abstract IEnumerable<TEntity> GetAll();

        public abstract TEntity GetOne(int id);

        public abstract TEntity AddOne(TEntity entity);

        public abstract TEntity[] AddMany(TEntity[] entities);

        public abstract TEntity UpdateOne(TEntity entity);

        public abstract void DeleteOne(int id);

        public abstract void DeleteMany(int[] ids);

        public virtual void Commit()
        {
            _storage.Commit();
        }
    }
}
