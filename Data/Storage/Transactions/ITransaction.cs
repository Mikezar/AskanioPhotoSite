using System;

namespace AskanioPhotoSite.Data.Storage.Transactions
{
   public interface ITransaction : IDisposable
   {
       string ReadStream();
       void WriteStream();
       void Commit();
       void RollBack();
   }
}
