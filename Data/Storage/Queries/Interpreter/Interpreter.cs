using System;
using System.Collections.Generic;
using System.Linq;
using AskanioPhotoSite.Data.Storage.Transactions;

namespace AskanioPhotoSite.Data.Storage.Queries.Interpreter
{
    public class Interpreter<TEntity> : IInterpreter<TEntity>
    {
        public IEnumerable<TEntity> InterpreteToEntity(string[] lines)
        {
            var list = new List<TEntity>();

            foreach (var line in lines)
            {
                var fields = line.Split(Processor<TEntity>.Field);
                var apex = fields.Count();
                var entity = (TEntity)Activator.CreateInstance(typeof(TEntity));
                var properties = entity.GetType().GetProperties();
                for (int i = 0; i < apex; i++)
                {
                    Type type = properties[i].PropertyType;
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                        properties[i].SetValue(entity, null);
                    else
                        properties[i].SetValue(entity, Convert.ChangeType(fields[i], properties[i].PropertyType), null);
                }
                list.Add(entity);
            }

            return list;
        }

        public IEnumerable<string> InterpreteToString(IEnumerable<TEntity> entities, int maxId)
        {
                int counter = ++maxId;
                var result = new List<string>();

                foreach(var entity in entities)
                {

                    entity.GetType().GetProperty("Id").SetValue(entity, counter);
                    counter++;
                    var properties = entity.GetType().GetProperties();
                    var values = properties.Select(x => x.GetValue(entity)).ToArray();
                    var entityString = String.Join($"{Processor<TEntity>.Field}", values.Select(c => c).ToArray());
                    result.Add(entityString);
                }

            return result;
        }

        public Dictionary<int, string> InterpreteToString(IEnumerable<TEntity> entities)
        {
                var result = new Dictionary<int, string>();

                foreach (var entity in entities)
                {
                    var properties = entity.GetType().GetProperties();
                    var values = properties.Select(x => x.GetValue(entity)).ToArray();
                    var entityString = String.Join($"{Processor<TEntity>.Field}", values.Select(c => c).ToArray());
                    result.Add((int)entity.GetType().GetProperty("Id").GetValue(entity), entityString);
                }

                return result;
        } 
    }
}
