using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using AskanioPhotoSite.Data.Storage.Queries;
using AskanioPhotoSite.Data.Storage.Queries.Interpreter;

namespace AskanioPhotoSite.Data.Storage.Transactions
{
    public class Processor<TEntity> : IProcessor<TEntity>, IDisposable
    {
        private readonly IInterpreter<TEntity> _interpreter;
        private object _lock = new object();
        private bool _disposed;
        public const char Line = (char)30;
        public const char Field = (char) 31;

        private DirectoryInfo OptDirectory { get; set; }

        private string FilePath { get; set; }
        private string Source { get; set; }
        private string[] Modified { get; set; }

        public Processor(IInterpreter<TEntity> interpreter, DirectoryInfo directory = null)
        {
            _interpreter = interpreter;
            OptDirectory = directory;

        }

        private void CreateIfNotExists(string type)
        {
            DirectoryInfo directory = OptDirectory ?? new DirectoryInfo(HttpContext.Current.Server.MapPath("~\\App_Data"));

            string path = $"{directory}\\{type}.txt";
            if (!Directory.Exists(directory.ToString())) Directory.CreateDirectory(directory.ToString());

            if (!File.Exists(path))
            {
                using (FileStream stream = File.Create(path)) { }
            }

            FilePath = path;
        }

        public IQueryResult<TEntity> Read(IQuery<TEntity> query)
        {
            try
            {
                CreateIfNotExists(typeof(TEntity).Name);

                using (var transaction = new Transaction(FilePath, null))
                {
                   Source = transaction.ReadStream();
                }

                if (query.ActionType != ActionType.Select) throw new InvalidOperationException();

                if (string.IsNullOrEmpty(Source))
                {
                    return new QueryResult<TEntity>()
                    {
                        IsSuccess = true,
                        Result = new List<TEntity>()
                    };
                }

                var lines = Source.Split(Line).Where(x => x != "\r\n" && !string.IsNullOrEmpty(x)).ToArray();
                return new QueryResult<TEntity>()
                {
                    IsSuccess = true,
                    Result = _interpreter.InterpreteToEntity(lines, Activator.CreateInstance<TEntity>())
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


        public IQueryResult<TEntity> Write(IQuery<TEntity> query)
        {

            try
            {
                CreateIfNotExists(typeof(TEntity).Name);
                lock (_lock)
                {
                    if (query.ActionType == ActionType.Select) throw new InvalidOperationException();

                    using (var transaction = new Transaction(FilePath, null))
                    {
                        Source = transaction.ReadStream();
                        transaction.BackUp();
                    }

                    var lines = Source.Split(Line).Where(x => x != "\r\n" && !string.IsNullOrEmpty(x)).ToArray();

                    if (query.ActionType == ActionType.Delete)
                    {
                        if (string.IsNullOrEmpty(Source)) throw new ArgumentNullException("Source");

                        if (query.Keys == null) throw new ArgumentNullException("Keys");

                        Modified =
                            lines.Where(
                                    x =>
                                        query.Keys.Contains(Convert.ToInt32(x.Substring(0, x.IndexOf(Field)))) ==
                                        false)
                                .ToArray();

                        Modified = Modified.Select(x => x + $"{Line}").ToArray();
                        return new QueryResult<TEntity>()
                        {
                            IsSuccess = true,
                            ServiceInfo = new TransactionServiceInfo()
                            {
                                FilePath = FilePath,
                                Modified = Modified
                            }
                        };
                    }

                    if (query.ActionType == ActionType.Add)
                    {
                        if (query.Entities == null) throw new ArgumentNullException("Entities");

                        int maxId = 0;
                        if (lines.Length > 0)
                        {
                            var lstLine = lines.Last();
                            maxId = Convert.ToInt32(lstLine.Substring(0, lstLine.IndexOf(Field)));
                        }

                        Modified = lines.Concat(_interpreter.InterpreteToString(query.Entities, maxId)).ToArray();
                        Modified = Modified.Select(x => x + $"{Line}").ToArray();
                        return new QueryResult<TEntity>()
                        {
                            IsSuccess = true,
                            Result = query.Entities,
                            ServiceInfo = new TransactionServiceInfo()
                            {
                                FilePath = FilePath,
                                Modified = Modified
                            }

                        };
                    }
                    if (query.ActionType == ActionType.Update)
                    {
                        if (string.IsNullOrEmpty(Source)) throw new ArgumentNullException("Source");

                        if (query.Entities == null) throw new ArgumentNullException("Entities");

                        var updates = _interpreter.InterpreteToString(query.Entities);

                        Modified = lines.ToArray();

                        for (int i = 0; i < Modified.Length; i++)
                        {
                            int id = Convert.ToInt32(Modified[i].Substring(0, Modified[i].IndexOf(Field)));

                            if (updates.ContainsKey(id))
                            {
                                Modified[i] = updates[id];
                            }
                        }
                        Modified = Modified.Select(x => x + $"{Line}").ToArray();

                        return new QueryResult<TEntity>()
                        {
                            IsSuccess = true,
                            Result = query.Entities,
                            ServiceInfo = new TransactionServiceInfo()
                            {
                                FilePath = FilePath,
                                Modified = Modified
                            }
                        };
                    }

                    throw new NullReferenceException();
                }
            }
            catch (Exception exception)
            {
                using (var transaction = new Transaction(FilePath, null))
                {
                    transaction.RollBack();
                }
                return new QueryResult<TEntity>()
                {
                    IsSuccess = false,
                    Exception = exception,
                    ErrorMessage = exception.Message,
                    ServiceInfo = new TransactionServiceInfo()
                    {
                        FilePath = FilePath,
                        Modified = Modified
                    }
                };
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                FilePath = null;
                Source = null;
                Modified = null;
            }
            _disposed = true;
        }
    }
}
