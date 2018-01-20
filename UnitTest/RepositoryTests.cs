using System.Collections.Generic;
using System.Linq;
using AskanioPhotoSite.Data.Entities;
using AskanioPhotoSite.Data.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AskanioPhotoSite.Core.Services.Providers;

namespace AskanioPhotoSite.UnitTest
{
    [TestClass]
    public class RepositoryTests
    {
        [TestMethod]
        public void GetAllFromCacheTest()
        {

            Storage storage = new Storage();
            BaseProvider<Album> service = new AlbumProvider(storage);

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
