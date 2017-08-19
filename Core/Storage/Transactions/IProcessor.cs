using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AskanioPhotoSite.Core.Storage.Queries;

namespace AskanioPhotoSite.Core.Storage.Transactions
{
    public interface IProcessor<TEntity>
    {
        IQueryResult<TEntity> Read(IQuery<TEntity> query);
        IQueryResult<TEntity> Write(IQuery<TEntity> query);
    }
}
