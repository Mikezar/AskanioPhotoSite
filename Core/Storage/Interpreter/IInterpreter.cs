using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskanioPhotoSite.Core.Storage.Interpreter
{
    public interface IInterpreter<TEntity>
    {
        IEnumerable<TEntity> InterpreteToEntity(string source, TEntity entity);
    }
}
