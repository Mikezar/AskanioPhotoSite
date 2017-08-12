using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using AskanioPhotoSite.Core.Entities;
using AskanioPhotoSite.Core.Repositories;
using AskanioPhotoSite.Core.Services;
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
            BaseService<Album> service = new AlbumService(new Storage());

            service.Cache = new Cache();

            ICollection<Entity> entity = new List<Entity>()
            {
                new Album()
                {
                    Id = 1,
                    TitleRu = "TitleRu",
                    TitleEng = "TitleEng",
                    DescriptionRu = "DescriptionRu",
                    DescriptionEng = "DescripntionEng"
                }
            };

            service.Cache.AddEntity(entity);

            var data = service.GetAll();
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Count() > 1);
        }
    }
}
