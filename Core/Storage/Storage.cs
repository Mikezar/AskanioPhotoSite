using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AskanioPhotoSite.Core.Entities;
using AskanioPhotoSite.Core.Repositories;
using AskanioPhotoSite.Core.Storage.Interpreter;
using AskanioPhotoSite.Core.Storage.Queries;
using AskanioPhotoSite.Core.Storage.Transactions;

namespace AskanioPhotoSite.Core.Storage
{
    public sealed class Storage : IStorage
    {
        private readonly IDictionary<object, object> _repositories;
        

        public Storage()
        {
            _repositories = new Dictionary<object, object>();
            _repositories.Add(typeof(Album), new GenericRepository<Album>(this));
            _repositories.Add(typeof(Photo), new GenericRepository<Photo>(this));
        }


        public IDictionary<object, object> GetEntities => _repositories;

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : Entity
        {
            try
            {
                return (IRepository<TEntity>) _repositories[typeof(TEntity)];
            }
            catch(KeyNotFoundException)
            {
                throw new ApplicationException("Service locator failed to resolve to a finite instance");
            }
        }

        public IQueryResult<TEntity> Execute<TEntity>(IQuery<TEntity> query)
        {
            if (query.QueryType == QueryType.Read)
            {
                using (var transaction = new Transaction<TEntity>(new Interpreter<TEntity>()))
                {
                    var result = transaction.Read(query);

                    if (!result.IsSuccess)
                        throw new Exception(result.ErrorMessage);

                    return result;
                }
            }
            else 
            {
                using (var transaction = new Transaction<TEntity>(new Interpreter<TEntity>()))
                {
                    var result = transaction.Write(query);

                    if (!result.IsSuccess)
                    {
                        transaction.RollBack();
                        throw new Exception(result.ErrorMessage);
                    }

                    transaction.Commit();
                    return result;
                }
            }
        }
    }
}
