using System.Collections.Generic;

namespace AskanioPhotoSite.Core.Storage.Queries
{
    public static class QueryBuilder<TEntity, TKey>
    {
        public static IQuery<TEntity, TKey> GetAll()
        {
            return new Query<TEntity, TKey>()
            {
                QueryType = QueryType.Read,
                ActionType = ActionType.Select
            };
        }

        public static IQuery<TEntity, TKey> AddOne(TEntity entity)
        {
            return new Query<TEntity, TKey>()
            {
                QueryType = QueryType.Write,
                Entities = new List<TEntity>()
                {
                    entity
                },
                ActionType = ActionType.Add
            };
        }


        public static IQuery<TEntity, TKey> AddMany(TEntity[] entities)
        {
            return new Query<TEntity, TKey>()
            {
                QueryType = QueryType.Write,
                Entities = entities,
                ActionType = ActionType.Add
            };
        }

        public static IQuery<TEntity, TKey> UpdateOne(TEntity entity)
        {
            return new Query<TEntity, TKey>()
            {
                QueryType = QueryType.Write,
                Entities = new List<TEntity>()
                {
                    entity
                },
                ActionType = ActionType.Update
            };
        }

        public static IQuery<TEntity, TKey> UpdateMany(TEntity[] entities)
        {
            return new Query<TEntity, TKey>()
            {
                QueryType = QueryType.Write,
                Entities = entities,
                ActionType = ActionType.Update
            };
        }

        public static IQuery<TEntity, TKey> DeleteOne(TKey key)
        {
            return new Query<TEntity, TKey>()
            {
                QueryType = QueryType.Write,
                ActionType = ActionType.Delete,
                Keys = new List<TKey>()
                {
                    key
                }
            };
        }

        public static IQuery<TEntity, TKey> DeleteMany(TKey[] keys)
        {
            return new Query<TEntity, TKey>()
            {
                QueryType = QueryType.Write,
                ActionType = ActionType.Delete,
                Keys = keys
            };
        }
    }
}
