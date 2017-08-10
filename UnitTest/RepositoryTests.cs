using AskanioPhotoSite.Core.Entities;
using AskanioPhotoSite.Core.Repositories;
using AskanioPhotoSite.Core.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AskanioPhotoSite.UnitTest
{
    [TestClass]
    public class RepositoryTests
    {
        [TestMethod]
        public void GetAllTest()
        {
            var storage = new Storage();

            var repository = storage.GetRepository<Album, int>();

            var data = repository.GetAll();

            Assert.IsNotNull(data);
        }
    }
}
