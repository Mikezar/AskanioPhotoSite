using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using AskanioPhotoSite.Core.Entities;
using AskanioPhotoSite.Core.Storage.Queries;
using AskanioPhotoSite.Core.Storage.Queries.Interpreter;

namespace AskanioPhotoSite.Core.Storage.Transactions
{
    public class Transaction<TEntity> : ITransaction<TEntity> 
    {
        private readonly IInterpreter<TEntity> _interpreter;
        private bool _disposed;

        public const char Line = (char)30;
        public const char Field = (char)31;

        private string FilePath { get; set; }

        private DirectoryInfo OptDirectory { get; set; }
        private string Source { get; set; }

        private string[] Modified { get; set; }

        public Transaction(IInterpreter<TEntity> interpreter, DirectoryInfo directory = null)
        {
            _interpreter = interpreter;
            OptDirectory = directory;

        }

        private void CreateIfNotExists(string type)
        {
            DirectoryInfo directory = OptDirectory ??  new DirectoryInfo(HttpContext.Current.Server.MapPath("..\\App_Data"));

            string path = $"{directory}\\{type}.txt";
             if (!Directory.Exists(directory.ToString())) Directory.CreateDirectory(directory.ToString());

            if (!File.Exists(path))
            {
                using (FileStream stream = File.Create(path)) { }
            }

            FilePath = path;
        }


        private void ReadStream()
        {
            using (var reader = File.OpenRead(FilePath))
            {
                var buffer = new byte[reader.Length];
                reader.Read(buffer, 0, buffer.Length);
                Source = Encoding.Unicode.GetString(buffer);
            }
        }


        private void WriteStream()
        {
                var bytes = Encoding.Unicode.GetBytes(String.Concat(Modified));
               
                File.WriteAllBytes(FilePath, bytes);
        }


        public IQueryResult<TEntity> Read(IQuery<TEntity> query)
        {
            CreateIfNotExists(typeof(TEntity).Name);

            try
            {
                if (query.ActionType != ActionType.Select) throw new InvalidOperationException();

                ReadStream();

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
                    Result =_interpreter.InterpreteToEntity(lines, Activator.CreateInstance<TEntity>())
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
            CreateIfNotExists(typeof(TEntity).Name);

            try
            {
                if (query.ActionType == ActionType.Select) throw new InvalidOperationException();

                ReadStream();

                BackUp();

                var lines = Source.Split(Line).Where(x => x != "\r\n" && !string.IsNullOrEmpty(x)).ToArray();

                if (query.ActionType == ActionType.Delete)
                {
                    if (string.IsNullOrEmpty(Source)) throw new ArgumentNullException("Source");

                    if (query.Keys == null) throw new ArgumentNullException("Keys");

                    Modified = lines.Where(x => query.Keys.Contains(Convert.ToInt32(x.Substring(0, x.IndexOf(Field)))) == false).ToArray();

                    return new QueryResult<TEntity>()
                    {
                        IsSuccess = true,
                    };
                }

                if(query.ActionType == ActionType.Add)
                {
                    if(query.Entities == null) throw new ArgumentNullException("Entities");

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
                        Result = query.Entities
                    };
                }
                if(query.ActionType == ActionType.Update)
                {
                    if (string.IsNullOrEmpty(Source)) throw new ArgumentNullException("Source");

                    if (query.Entities == null) throw new ArgumentNullException("Entities");

                    var updates = _interpreter.InterpreteToString(query.Entities);

                    Modified = lines.ToArray();

                    for (int i = 0; i < Modified.Length; i ++)
                    {
                        int id = Convert.ToInt32(Modified[i].Substring(0, Modified[i].IndexOf(Field)));
                     
                        if(updates.ContainsKey(id))
                        {
                            Modified[i] = updates[id];
                        }
                    }
                    Modified = Modified.Select(x => x + $"{Line}").ToArray();

                    return new QueryResult<TEntity>()
                    {
                        IsSuccess = true,
                        Result = query.Entities
                    };
                }

                throw new NullReferenceException();
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

        public void BackUp()
        {
            if (!File.Exists(FilePath + ".backup"))
            {
                File.Copy(FilePath, FilePath + ".backup");
            }
        }

        public void Commit()
        {
            try
            {
                if (Modified == null) return;

                WriteStream();
               // File.WriteAllLines(FilePath, Modified, Encoding.Unicode);
                File.Delete(FilePath + ".backup");
            }
            catch(Exception exception)
            {
                RollBack();
                throw new Exception("Transaction failed.");
            }
        }

        public void RollBack()
        {
            File.Delete(FilePath);
            File.Copy(FilePath + ".backup", FilePath);
            File.Delete(FilePath + ".backup");
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
