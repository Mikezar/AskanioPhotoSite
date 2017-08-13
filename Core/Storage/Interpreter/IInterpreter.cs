using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AskanioPhotoSite.Core.Entities;

namespace AskanioPhotoSite.Core.Storage.Interpreter
{
    public interface IInterpreter<TEntity>
    {
        IEnumerable<TEntity> InterpreteToEntity(string source, TEntity entity);
        IEnumerable<string> InterpreteToString(IEnumerable<TEntity> entities, int maxId);
        Dictionary<int, string> InterpreteToString(IEnumerable<TEntity> entities);
    }
}
