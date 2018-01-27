using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain.Entities;
using System.Linq;
using Moq;
using Domain.Abstract;
using WebUI.Controllers;
using System.Web.Mvc;
using WebUI.Models;

namespace UnitTests
{
    [TestClass]
    public class CartTests
    {


        [TestMethod]
        public void Can_Add_New_Lines()
        {
            //Organization
            Thing thing1 = new Thing { ThingId = 1, Name = "Thing1" };
            Thing thing2 = new Thing { ThingId = 2, Name = "Thing2" };

            Cart cart = new Cart();

            //Action
            cart.AddItem(thing1, 1);
            cart.AddItem(thing2, 1);
            List<CartLine> results = cart.Lines.ToList();

            //Statemant
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0].Thing, thing1);
            Assert.AreEqual(results[1].Thing, thing2);
        }

        [TestMethod]
        public void Can_Add_Quantity_For_Exsisting_Lines()
        {
            //Organization
            Thing thing1 = new Thing { ThingId = 1, Name = "Thing1" };
            Thing thing2 = new Thing { ThingId = 2, Name = "Thing2" };

            Cart cart = new Cart();

            //Action
            cart.AddItem(thing1, 1);
            cart.AddItem(thing2, 1);
            cart.AddItem(thing1, 5);
            List<CartLine> results = cart.Lines.OrderBy(c => c.Thing.ThingId).ToList();

            //Statemant
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0].Quantity, 6);
            Assert.AreEqual(results[1].Quantity, 1);
        }

        [TestMethod]
        public void Can_Remove_Line()
        {
            //Organization
            Thing thing1 = new Thing { ThingId = 1, Name = "Thing1" };
            Thing thing2 = new Thing { ThingId = 2, Name = "Thing2" };
            Thing thing3 = new Thing { ThingId = 3, Name = "Thing3" };


            Cart cart = new Cart();

            //Action
            cart.AddItem(thing1, 1);
            cart.AddItem(thing2, 1);
            cart.AddItem(thing1, 5);
            cart.AddItem(thing3, 2);
            cart.RemoveLine(thing2);

            //Statemant
            Assert.AreEqual(cart.Lines.Where(c => c.Thing == thing2).Count(), 0);
            Assert.AreEqual(cart.Lines.Count(), 2);


        }

        [TestMethod]
        public void Calculate_Cart_Total()
        {
            //Organization
            Thing thing1 = new Thing { ThingId = 1, Name = "Thing1", Price = 100 };
            Thing thing2 = new Thing { ThingId = 2, Name = "Thing2", Price = 55 };


            Cart cart = new Cart();

            //Action
            cart.AddItem(thing1, 1);
            cart.AddItem(thing2, 1);
            cart.AddItem(thing1, 5);
            decimal result = cart.ComputeTotalValue();

            //Statemant
            Assert.AreEqual(result, 655);

        }

        [TestMethod]
        public void Can_Clear_Contents()
        {
            //Organization
            Thing thing1 = new Thing { ThingId = 1, Name = "Thing1", Price = 100 };
            Thing thing2 = new Thing { ThingId = 2, Name = "Thing2", Price = 55 };


            Cart cart = new Cart();

            //Action
            cart.AddItem(thing1, 1);
            cart.AddItem(thing2, 1);
            cart.AddItem(thing1, 5);
            cart.Clear();

            //Statemant
            Assert.AreEqual(cart.Lines.Count(), 0);

        }


        //Adding element to cart
        [TestMethod]
        public void Can_Add_To_Cart()
        {
            Mock<IThingRepository> mock = new Mock<IThingRepository>();
            mock.Setup(m => m.Things).Returns(new List<Thing>{
                new Thing {ThingId = 1, Name = "Thing1", Kind = "Kind1"}
            }.AsQueryable());

            Cart cart = new Cart();

            CartController controller = new CartController(mock.Object, null);

            controller.AddToCart(cart, 1, null);

            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToList()[0].Thing.ThingId, 1);
        }
        //rerouting to cart page
        [TestMethod]
        public void Adding_Thing_To_Cart_Goes_To_Cart_Screen()
        {
            Mock<IThingRepository> mock = new Mock<IThingRepository>();
            mock.Setup(m => m.Things).Returns(new List<Thing>{
                new Thing {ThingId = 1, Name = "Thing1", Kind = "Kind1"}
            }.AsQueryable());

            Cart cart = new Cart();

            CartController controller = new CartController(mock.Object, null);

            RedirectToRouteResult result = controller.AddToCart(cart, 2, "myUrl");

            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }

        [TestMethod]
        public void Can_View_Cart_Contents()
        {
            Cart cart = new Cart();

            CartController target = new CartController(null, null);

            CartIndexViewModel result = (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;

            Assert.AreSame(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }

        [TestMethod]
        public void Cannot_Checkout_Empty_Cart()
        {
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            ShippingDetails shippingDetails = new ShippingDetails();

            CartController controller = new CartController(null, mock.Object);

            ViewResult result = controller.Checkout(cart, shippingDetails);

            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never());

            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Cannot_Checkout_InvalidShipping_Details()
        {
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            cart.AddItem(new Thing(), 1);           

            CartController controller = new CartController(null, mock.Object);
            controller.ModelState.AddModelError("error", "error");

            ViewResult result = controller.Checkout(cart, new ShippingDetails());

            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never());

            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Cannot_Checkout_Submit_Order()
        {
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            cart.AddItem(new Thing(), 1);

            CartController controller = new CartController(null, mock.Object);
         
            ViewResult result = controller.Checkout(cart, new ShippingDetails());

            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Once ());

            Assert.AreEqual("Completed", result.ViewName);
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }



    }
}
