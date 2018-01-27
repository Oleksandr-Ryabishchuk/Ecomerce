using Domain.Abstract;
using Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebUI.Controllers;

namespace UnitTests
{
    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        public void Index_Contains_All_Things()
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
            AdminController controller = new AdminController(mock.Object);

            List<Thing> result = ((IEnumerable<Thing>)controller.Index().ViewData.Model).ToList();

            Assert.AreEqual(result.Count(), 5);
            Assert.AreEqual(result[0].Name, "Thing1");
            Assert.AreEqual(result[1].Name, "Thing2");

        }

        [TestMethod]
        public void Can_Edit_Things()
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
            AdminController controller = new AdminController(mock.Object);

            Thing thing1 = controller.Edit(1).ViewData.Model as Thing;
            Thing thing2 = controller.Edit(2).ViewData.Model as Thing;
            Thing thing3 = controller.Edit(3).ViewData.Model as Thing;

            Assert.AreEqual(1, thing1.ThingId);
            Assert.AreEqual(2, thing2.ThingId);
            Assert.AreEqual(3, thing3.ThingId);
        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_Thing()
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
            AdminController controller = new AdminController(mock.Object);

            Thing result = controller.Edit(7).ViewData.Model as Thing;

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            Mock<IThingRepository> mock = new Mock<IThingRepository>();
            AdminController controller = new AdminController(mock.Object);

            Thing thing = new Thing { Name = "Test" };

            ActionResult result = controller.Edit(thing);

            mock.Verify(m => m.SaveThing(thing));

            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Can_Save_InValid_Changes()
        {
            Mock<IThingRepository> mock = new Mock<IThingRepository>();
            AdminController controller = new AdminController(mock.Object);

            Thing thing = new Thing { Name = "Test" };

            controller.ModelState.AddModelError("error", "error");

            ActionResult result = controller.Edit(thing);

            mock.Verify(m => m.SaveThing(It.IsAny<Thing>()), Times.Never());

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
    }
}
