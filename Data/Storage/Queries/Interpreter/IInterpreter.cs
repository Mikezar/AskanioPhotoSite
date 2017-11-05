using System.Collections.Generic;

namespace AskanioPhotoSite.Data.Storage.Queries.Interpreter
{
    public interface IInterpreter<TEntity>
    {
        IEnumerable<TEntity> InterpreteToEntity(string[] lines);
        IEnumerable<string> InterpreteToString(IEnumerable<TEntity> entities, int maxId);
        Dictionary<int, string> InterpreteToString(IEnumerable<TEntity> entities);
    }
}
