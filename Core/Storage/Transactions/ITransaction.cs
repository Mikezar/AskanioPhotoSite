using System;
using AskanioPhotoSite.Core.Storage.Queries;

namespace AskanioPhotoSite.Core.Storage.Transactions
{
   public interface ITransaction : IDisposable
   {
       string ReadStream();
       void WriteStream();
       void Commit();
       void RollBack();
   }
}
