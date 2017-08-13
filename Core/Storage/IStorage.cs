using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AskanioPhotoSite.Core.Entities;
using AskanioPhotoSite.Core.Repositories;
using AskanioPhotoSite.Core.Storage.Queries;

namespace AskanioPhotoSite.Core.Storage
{
   public interface IStorage
   {
       IDictionary<object, object> GetEntities { get; }
       IRepository<TEntity> GetRepository<TEntity>() where TEntity : Entity;
       IQueryResult<TEntity> Execute<TEntity>(IQuery<TEntity> query);
   }
}
