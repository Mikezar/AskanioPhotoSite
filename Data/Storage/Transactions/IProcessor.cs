using AskanioPhotoSite.Data.Storage.Queries;

namespace AskanioPhotoSite.Data.Storage.Transactions
{
    public interface IProcessor<TEntity>
    {
        IQueryResult<TEntity> Read(IQuery<TEntity> query);
        IQueryResult<TEntity> Write(IQuery<TEntity> query);
    }
}
