using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Routing;
using System.Web.UI.WebControls;
using AskanioPhotoSite.Core.Entities;
using AskanioPhotoSite.Core.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AskanioPhotoSite.Core.Storage.Queries;
using AskanioPhotoSite.WebUI;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;


namespace AskanioPhotoSite.UnitTest
{
 
    [TestClass]
    public class StorageTests
    {
        public object obj = new object();

        [TestInitialize]
        public void Init()
        {

            HttpContext.Current = new HttpContext(new HttpRequest("", "http://localhost/", ""),
                new HttpResponse(new StringWriter()));

        }

        DirectoryInfo directory =
                   new DirectoryInfo(
                       Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\Core\App_Data")));

        [TestMethod]
        public void ExecuteSelectTest()
        {
            Storage storage = new Storage(directory);

           
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
            Storage storage = new Storage(directory);


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
          
            Storage storage = new Storage(directory);

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

            Assert.IsTrue(data.IsSuccess);
        }


        [TestMethod]
        public void ExecuteDeleteTest()
        {

            Storage storage = new Storage(directory);

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
