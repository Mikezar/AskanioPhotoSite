using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using AskanioPhotoSite.Core.Entities;
using AskanioPhotoSite.Core.Storage.Interpreter;
using AskanioPhotoSite.Core.Storage.Queries;

namespace AskanioPhotoSite.Core.Storage.Transactions
{
    public class Transaction<TEntity, TKey> : ITransaction<TEntity, TKey>
    {
        private readonly IInterpreter<TEntity> _interpreter;
        private bool _disposed;

        public Transaction(IInterpreter<TEntity> interpreter)
        {
            _interpreter = interpreter;

        }
        public IQueryResult<TEntity> Read(IQuery<TEntity, TKey> query)
        {
            try
            {
                if (query.ActionType != ActionType.Select) throw new InvalidOperationException();
                
                    string type = typeof(TEntity).Name;
                    DirectoryInfo directory =
                        new DirectoryInfo(
                            Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\Core\App_Data")));

                    string path = $"{directory}\\{type}.txt";
                    if (!Directory.Exists(directory.ToString())) Directory.CreateDirectory(directory.ToString());

                    if (!File.Exists(path))File.Create(path);

                    var source = File.ReadAllText(path);
                
                return new QueryResult<TEntity>()
                {
                    IsSuccess = true,
                    Result =_interpreter.InterpreteToEntity(source, Activator.CreateInstance<TEntity>())
                };
            }
            catch (Exception exception)
            {
                return new QueryResult<TEntity>()
                {
                    IsSuccess = false,
                    Exception = exception,
                    ErrorMessage = exception.Message
                };
            }
        }

        public IQueryResult<TEntity> Write(IQuery<TEntity, TKey> query)
        {
            throw new NotImplementedException();
        }

        public void Commit()
        {
            throw new NotImplementedException();
        }

        public void RollBack()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            if (_disposed) return;

        }
    }
}
