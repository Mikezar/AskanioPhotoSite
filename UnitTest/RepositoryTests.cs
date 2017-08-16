﻿using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
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
