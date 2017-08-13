using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using AskanioPhotoSite.Core.Entities;
using AskanioPhotoSite.Core.Repositories;
using AskanioPhotoSite.Core.Services;
using AskanioPhotoSite.Core.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AskanioPhotoSite.Core.Storage.Queries;


namespace AskanioPhotoSite.UnitTest
{
    [TestClass]
    public class StorageTests
    {
        [TestMethod]
        public void ExecuteSelectTest()
        {
            Storage storage = new Storage();

            var query = new Query<Album>()
            {
                QueryType = QueryType.Read,
                ActionType = ActionType.Select
            };

            var data = storage.Execute(query);

            Assert.IsNotNull(data.Result);
        }

        [TestMethod]
        public void ExecuteDeleteTest()
        {
            Storage storage = new Storage();

            var query = new Query<Album>()
            {
                QueryType = QueryType.Write,
                ActionType = ActionType.Delete,
                Keys = new List<int>()
                {
                    3,
                }
            };

            var data = storage.Execute(query);

            Assert.IsTrue(data.IsSuccess);
        }


        [TestMethod]
        public void ExecuteAddTest()
        {
            Storage storage = new Storage();

            var query = new Query<Album>()
            {
                QueryType = QueryType.Write,
                ActionType = ActionType.Add,
                Entities = new List<Album>()
                {
                    new Album()
                    {
                        Id = 0,
                        ParentId = 0,
                        TitleEng = "Title",
                        TitleRu = "Название",
                        DescriptionEng = "Description",
                        DescriptionRu = "Описание"
                    }
                }
            };

            var data = storage.Execute(query);

            Assert.IsTrue(data.IsSuccess);
        }

        [TestMethod]
        public void ExecuteUpdateTest()
        {
            Storage storage = new Storage();

            var query = new Query<Album>()
            {
                QueryType = QueryType.Write,
                ActionType = ActionType.Update,
                Entities = new List<Album>()
                {
                    new Album()
                    {
                        Id = 2,
                        ParentId = 1,
                        TitleEng = "TitleUpdated",
                        TitleRu = "НазваниеUpdated",
                        DescriptionEng = "DescriptionUpdated",
                        DescriptionRu = "ОписаниеUpdated"
                    }
                }
            };

            var data = storage.Execute(query);

            Assert.IsTrue(data.IsSuccess);
        }
    }
}
