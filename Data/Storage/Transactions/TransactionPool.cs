using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace AskanioPhotoSite.Data.Storage.Transactions
{
    public class TransactionPool 
    {
        private Dictionary<Type, TransactionServiceInfo> _dictionary;
        private bool _disposed;
        private readonly DirectoryInfo _directory;

        public TransactionPool(DirectoryInfo directory = null)
        {
            _directory = directory;
            _dictionary = new Dictionary<Type, TransactionServiceInfo>();
            SetMarker(true);
        }

        public void Put<T>(TransactionServiceInfo info)
        {
            _dictionary.Add(typeof(T), info);
        }

        public TransactionServiceInfo Get<T>()
        {
            return _dictionary[typeof(T)];
        }

        public int Length => _dictionary.Count;

        public Dictionary<Type, TransactionServiceInfo> GetDictionary => _dictionary;


        public void RollBack()
        {
            foreach (var key in _dictionary)
            {
                using (var transaction = new Transaction(key.Value.FilePath, null))
                {
                    transaction.RollBack();
                }
            }
        }

        public void SetMarker(bool value)
        {
            DirectoryInfo directory = _directory ?? new DirectoryInfo(HttpContext.Current.Server.MapPath("~\\App_Data"));
            string path = $"{directory}\\Transaction.txt";

            string boolean = value ? "1" : "0";

            if (!File.Exists(path)) File.Create(path).Close();

            using (var transaction = new Transaction(path, new string[] {boolean}))
            {
                transaction.Commit();
            }
        }
    }
}
