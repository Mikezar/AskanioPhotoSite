using System.Collections.Generic;
using System.Linq;
using AskanioPhotoSite.Core.Services;
using AskanioPhotoSite.Data.Entities;
using AskanioPhotoSite.Data.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AskanioPhotoSite.UnitTest
{
    [TestClass]
    public class RepositoryTests
    {
        [TestMethod]
        public void GetAllFromCacheTest()
        {

            Storage storage = new Storage();
            BaseService<Album> service = new AlbumService(storage);

            storage.Cache = new Cache();
            storage.Cache.IsActual = true;

            IEnumerable<Album> entity = new List<Album>()
            {
                new Album()
                {
                    Id = 1,
                    TitleRu = "TitleRu",
                    TitleEng = "TitleEng",
                    DescriptionRu = "DescriptionRu",
                    DescriptionEng = "DescripntionEng"
                }
            }.ToList();

            storage.Cache.AddEntity(entity);

            var data = service.GetAll();
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Count() == 1);
        }
    }
}
