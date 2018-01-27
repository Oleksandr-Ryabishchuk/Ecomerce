using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Domain.Abstract;
using Domain.Entities;
using System.Collections.Generic;
using WebUI.Controllers;
using System.Linq;
using System.Web.Mvc;
using WebUI.Models;
using WebUI.HtmlHelpers;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            Mock<IThingRepository> mock = new Mock<IThingRepository>();
            mock.Setup(m => m.Things).Returns(new List<Thing>
            {
                new Thing {ThingId = 1, Name = "Thing1" },
                new Thing {ThingId = 2, Name = "Thing2" },
                new Thing {ThingId = 3, Name = "Thing3" },
                new Thing {ThingId = 4, Name = "Thing4" },
                new Thing {ThingId = 5, Name = "Thing5" }
            });
            ThingsController controller = new ThingsController(mock.Object);
            controller.pageSize = 3;

            ThingsListViewModel result = (ThingsListViewModel)controller.List(null,2).Model;

            List<Thing> things = result.Things.ToList();
            Assert.IsTrue(things.Count == 2);
            Assert.AreEqual(things[0].Name, "Thing4");
            Assert.AreEqual(things[1].Name, "Thing5");

        }
        [TestMethod]
        public void Can_Generate_Page_Links()
        {//Organisation
            HtmlHelper myHelper = null;
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };
            Func<int, string> pageUrlDelegate = i => "Page" + i;
            //Action
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);
            //Statement
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
               + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
               + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
               result.ToString());

        }
        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            Mock<IThingRepository> mock = new Mock<IThingRepository>();
            mock.Setup(m => m.Things).Returns(new List<Thing>
            {
                new Thing {ThingId = 1, Name = "Thing1" },
                new Thing {ThingId = 2, Name = "Thing2" },
                new Thing {ThingId = 3, Name = "Thing3" },
                new Thing {ThingId = 4, Name = "Thing4" },
                new Thing {ThingId = 5, Name = "Thing5" }
            });
            ThingsController controller = new ThingsController(mock.Object);
            controller.pageSize = 3;

            ThingsListViewModel result = (ThingsListViewModel)controller.List(null,2).Model;

            PagingInfo pagingInfo = result.PagingInfo;
            Assert.AreEqual(pagingInfo.CurrentPage, 2);
            Assert.AreEqual(pagingInfo.ItemsPerPage, 3);
            Assert.AreEqual(pagingInfo.TotalItems, 5);
            Assert.AreEqual(pagingInfo.TotalPages, 2);
        }
        [TestMethod]
        public void Can_Filter_Things()
        {
            Mock<IThingRepository> mock = new Mock<IThingRepository>();
            mock.Setup(m => m.Things).Returns(new List<Thing>
            {
                new Thing {ThingId = 1, Name = "Thing1", Kind = "Kind1" },
                new Thing {ThingId = 2, Name = "Thing2", Kind = "Kind2" },
                new Thing {ThingId = 3, Name = "Thing3", Kind = "Kind1" },
                new Thing {ThingId = 4, Name = "Thing4", Kind = "Kind3" },
                new Thing {ThingId = 5, Name = "Thing5", Kind = "Kind2" }
            });
            ThingsController controller = new ThingsController(mock.Object);
            controller.pageSize = 3;

            List<Thing> result = ((ThingsListViewModel)controller.List("Kind2", 1).Model).Things.ToList();

            Assert.AreEqual(result.Count(), 2);
            Assert.IsTrue(result[0].Name == "Thing2" && result[0].Kind == "Kind2");
            Assert.IsTrue(result[1].Name == "Thing5" && result[1].Kind == "Kind2");
        }
        [TestMethod]
        public void Can_Create_Categories()
        {
            Mock<IThingRepository> mock = new Mock<IThingRepository>();
            mock.Setup(m => m.Things).Returns(new List<Thing>
            {
                new Thing {ThingId = 1, Name = "Thing1", Kind = "Kind1" },
                new Thing {ThingId = 2, Name = "Thing2", Kind = "Kind2" },
                new Thing {ThingId = 3, Name = "Thing3", Kind = "Kind1" },
                new Thing {ThingId = 4, Name = "Thing4", Kind = "Kind3" },
                new Thing {ThingId = 5, Name = "Thing5", Kind = "Kind2" }
            });
            NavController target = new NavController(mock.Object);

            List<string> result = ((IEnumerable<string>)target.Menu().Model).ToList();

            Assert.AreEqual(result.Count(), 3);
            Assert.AreEqual(result[0], "Kind1");
            Assert.AreEqual(result[1], "Kind2");
            Assert.AreEqual(result[2], "Kind3");
        }
        [TestMethod]
        public void Indicates_Selected_Kinds()
        {
            Mock<IThingRepository> mock = new Mock<IThingRepository>();
            mock.Setup(m => m.Things).Returns(new List<Thing>
            {
                new Thing {ThingId = 1, Name = "Thing1", Kind = "Kind1" },
                new Thing {ThingId = 2, Name = "Thing2", Kind = "Kind2" },
                new Thing {ThingId = 3, Name = "Thing3", Kind = "Kind1" },
                new Thing {ThingId = 4, Name = "Thing4", Kind = "Kind3" },
                new Thing {ThingId = 5, Name = "Thing5", Kind = "Kind2" }
            });
            NavController target = new NavController(mock.Object);

            string kindToSelect = "Kind2";

            string result = target.Menu(kindToSelect).ViewBag.SelectedKind; 

            Assert.AreEqual(kindToSelect ,result);
        }
        [TestMethod]
        public void Generate_Kind_Specific_Thing_Count()
        {
            Mock<IThingRepository> mock = new Mock<IThingRepository>();
            mock.Setup(m => m.Things).Returns(new List<Thing>
            {
                new Thing {ThingId = 1, Name = "Thing1", Kind = "Kind1" },
                new Thing {ThingId = 2, Name = "Thing2", Kind = "Kind2" },
                new Thing {ThingId = 3, Name = "Thing3", Kind = "Kind1" },
                new Thing {ThingId = 4, Name = "Thing4", Kind = "Kind3" },
                new Thing {ThingId = 5, Name = "Thing5", Kind = "Kind2" }
            });

            ThingsController controller = new ThingsController(mock.Object);
            controller.pageSize = 3;

            int res1 = ((ThingsListViewModel)controller.List("Kind1").Model).PagingInfo.TotalItems;
            int res2 = ((ThingsListViewModel)controller.List("Kind2").Model).PagingInfo.TotalItems;
            int res3 = ((ThingsListViewModel)controller.List("Kind3").Model).PagingInfo.TotalItems;
            int resAll = ((ThingsListViewModel)controller.List(null).Model).PagingInfo.TotalItems;

            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);
        }
    } 

    
}
