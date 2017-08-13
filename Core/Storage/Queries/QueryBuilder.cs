using System.Collections.Generic;

namespace AskanioPhotoSite.Core.Storage.Queries
{
    public static class QueryBuilder<TEntity>
    {
        public static IQuery<TEntity> GetAll()
        {
            return new Query<TEntity>()
            {
                QueryType = QueryType.Read,
                ActionType = ActionType.Select
            };
        }

        public static IQuery<TEntity> AddOne(TEntity entity)
        {
            return new Query<TEntity>()
            {
                QueryType = QueryType.Write,
                Entities = new List<TEntity>()
                {
                    entity
                },
                ActionType = ActionType.Add
            };
        }


        public static IQuery<TEntity> AddMany(TEntity[] entities)
        {
            return new Query<TEntity>()
            {
                QueryType = QueryType.Write,
                Entities = entities,
                ActionType = ActionType.Add
            };
        }

        public static IQuery<TEntity> UpdateOne(TEntity entity)
        {
            return new Query<TEntity>()
            {
                QueryType = QueryType.Write,
                Entities = new List<TEntity>()
                {
                    entity
                },
                ActionType = ActionType.Update
            };
        }

        public static IQuery<TEntity> UpdateMany(TEntity[] entities)
        {
            return new Query<TEntity>()
            {
                QueryType = QueryType.Write,
                Entities = entities,
                ActionType = ActionType.Update
            };
        }

        public static IQuery<TEntity> DeleteOne(int key)
        {
            return new Query<TEntity>()
            {
                QueryType = QueryType.Write,
                ActionType = ActionType.Delete,
                Keys = new List<int>()
                {
                    key
                }
            };
        }

        public static IQuery<TEntity> DeleteMany(int[] keys)
        {
            return new Query<TEntity>()
            {
                QueryType = QueryType.Write,
                ActionType = ActionType.Delete,
                Keys = keys
            };
        }
    }
}
