using System;
using System.IO;
using System.Text;

namespace AskanioPhotoSite.Data.Storage.Transactions
{
    public class Transaction : ITransaction
    {
        private string _filePath;

        private bool _disposed;

        private string[] _updated;

        private string _backUp;

        public Transaction(string filePath, string[] updated)
        {
            if(string.IsNullOrEmpty(filePath)) throw new ArgumentNullException("filePath");
            _filePath = filePath;
            _updated = updated;
            _backUp = _filePath + ".backup";
        }

        public string ReadStream()
        {
            using (var reader = File.Open(_filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                var buffer = new byte[reader.Length];
                reader.Read(buffer, 0, buffer.Length);
                return Encoding.Unicode.GetString(buffer);
            }
        }


        public void WriteStream()
        {
            using (var writer = File.Open(_filePath, FileMode.Truncate, FileAccess.ReadWrite, FileShare.None))
            {
                for (int i = 0; i < _updated.Length; i++)
                {
                    var bytes = Encoding.Unicode.GetBytes(_updated[i]);
                    writer.Write(bytes, 0, bytes.Length);
                }
            }
        }   

        public void BackUp()
        {
            if (!File.Exists(_backUp)) File.Copy(_filePath, _backUp);

        }

        public void Commit()
        {
            try
            {
                if (_updated == null) return;

                WriteStream();

                if (!File.Exists(_backUp))
                    File.Delete(_backUp);
            }
            catch (Exception exception)
            {
                RollBack();
                throw new Exception("Transaction failed.");
            }
        }

        public void RollBack()
        {
            File.Delete(_filePath);
            File.Copy(_backUp, _filePath);
            File.Delete(_backUp);
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
                _filePath = null;
                _backUp = null;
                _updated = null;
            }
            _disposed = true;
        }
    }
}
