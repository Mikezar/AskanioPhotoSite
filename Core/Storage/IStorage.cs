﻿using System;
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
       ICache Cache { get; }
       IRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : Entity;
       IQueryResult Execute<TEntity, TKey>(IQuery<TEntity, TKey> query);
       ICache GetCache();
       void UpdateCache(ICache cache);
       void MarkAsModified();
   }
}
