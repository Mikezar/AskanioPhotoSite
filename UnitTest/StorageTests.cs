using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using AskanioPhotoSite.Core.Entities;
using AskanioPhotoSite.Core.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AskanioPhotoSite.Core.Storage.Queries;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;


namespace AskanioPhotoSite.UnitTest
{
 
    [TestClass]
    public class StorageTests
    {
        [TestInitialize]
        public void Init()
        {

            HttpContext.Current = new HttpContext(new HttpRequest("", "http://localhost/", ""),
                new HttpResponse(new StringWriter()));

        }

        public static DirectoryInfo directory =
                   new DirectoryInfo(
                       Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\Core\App_Data")));

        public static  Storage storage = new Storage(directory);

        [TestMethod]
        public void ExecuteSelectTest()
        {    
            var query = new Query<Album>()
            {
                QueryType = QueryType.Read,
                ActionType = ActionType.Select
            };

            var data = storage.Execute(query);

            Assert.IsNotNull(data.Result);
        }

        [TestMethod]
        public void ExecuteAddTest()
        {
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
            
            storage.Commit();

            Assert.IsTrue(data.IsSuccess);
        }

        [TestMethod]
        public void ExecuteUpdateTest()
        {
         
            var query = new Query<Album>()
            {
                QueryType = QueryType.Write,
                ActionType = ActionType.Update,
                Entities = new List<Album>()
                {
                    new Album()
                    {
                        Id = 1,
                        ParentId = 1,
                        TitleEng = "TitleUpdated",
                        TitleRu = "НазваниеUpdated",
                        DescriptionEng = "DescriptionUpdated",
                        DescriptionRu = "ОписаниеUpdated"
                    }
                }
            };

            var data = storage.Execute(query);

            storage.Commit();

            Assert.IsTrue(data.IsSuccess);
        }


        [TestMethod]
        public void ExecuteDeleteTest()
        {
            var query = new Query<Album>()
            {
                QueryType = QueryType.Write,
                ActionType = ActionType.Delete,
                Keys = new List<int>()
                {
                    1,
                }
            };

            var data = storage.Execute(query);

            storage.Commit();

            Assert.IsTrue(data.IsSuccess);
        }

        [TestMethod]
        public void CharTest()
        {
            char Line = (char)30;
            char Field = (char)31;

            Assert.IsNotNull(Field);
        }  
    }
}
