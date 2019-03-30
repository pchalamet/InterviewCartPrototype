using cart.grain;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace webapi.Controllers.Tests
{
    [TestFixture]
    public class CartController
    {
        [Test]
        public async Task Add_Ok()
        {
            var repo = new Moq.MockRepository(Moq.MockBehavior.Strict);

            var cartId = 42;

            var items = new System.Collections.Generic.Dictionary<string, int>()
            {
                { "article1", 3 }
            };
            var queryItems = new webapi.Controllers.CartItems { Items = items };
            var cartItems = new cart.grain.CartItems { Items = items };

            // initialize grain
            var cartGrain = repo.Create<ICart>();
            cartGrain.Setup(x => x.Add(It.Is<cart.grain.CartItems>(o => o.Items["article1"] == 3))).Returns(Task.FromResult((CartItemsStatusCode.Ok, cartItems)));

            // initialize grain factory
            var grainFactory = repo.Create<IGrainFactory>();
            grainFactory.Setup(x => x.GetGrain<ICart>(cartId)).Returns(cartGrain.Object);


            var controller = new webapi.Controllers.CartController(grainFactory.Object);
            var response = await controller.Add(cartId, queryItems) as OkObjectResult;
            Assert.AreEqual(200, response.StatusCode);
            var result = (webapi.Controllers.CartItems)response.Value;
            Assert.AreEqual(1, result.Items.Count);
            Assert.AreEqual(3, result.Items["article1"]);

            repo.VerifyAll();
        }

        [Test]
        public async Task Add_BadRequest_InvalidQuantity()
        {
            var repo = new Moq.MockRepository(Moq.MockBehavior.Strict);

            var cartId = 42;

            var items = new System.Collections.Generic.Dictionary<string, int>()
            {
                { "article1", 0 }
            };
            var queryItems = new webapi.Controllers.CartItems { Items = items };
            var cartItems = new cart.grain.CartItems { Items = items };

            // initialize grain
            var cartGrain = repo.Create<ICart>();
            cartGrain.Setup(x => x.Add(It.IsAny<cart.grain.CartItems>())).Returns(Task.FromResult((CartItemsStatusCode.InvalidQuantity, cartItems)));

            // initialize grain factory
            var grainFactory = repo.Create<IGrainFactory>();
            grainFactory.Setup(x => x.GetGrain<ICart>(cartId)).Returns(cartGrain.Object);


            var controller = new webapi.Controllers.CartController(grainFactory.Object);
            var response = await controller.Add(cartId, queryItems) as BadRequestObjectResult;
            Assert.AreEqual(400, response.StatusCode);

            repo.VerifyAll();
        }



        [Test]
        public async Task Remove_Ok()
        {
            var repo = new Moq.MockRepository(Moq.MockBehavior.Strict);

            var cartId = 42;

            var items = new System.Collections.Generic.Dictionary<string, int>()
            {
                { "article1", 3 }
            };
            var queryItems = new webapi.Controllers.CartItems { Items = items };
            var cartItems = new cart.grain.CartItems { Items = items };
            var emptyCartItems = new cart.grain.CartItems();

            // initialize grain
            var cartGrain = repo.Create<ICart>();
            cartGrain.Setup(x => x.Remove(It.Is<cart.grain.CartItems>(o => o.Items["article1"] == 3))).Returns(Task.FromResult((CartItemsStatusCode.Ok, emptyCartItems)));

            // initialize grain factory
            var grainFactory = repo.Create<IGrainFactory>();
            grainFactory.Setup(x => x.GetGrain<ICart>(cartId)).Returns(cartGrain.Object);


            var controller = new webapi.Controllers.CartController(grainFactory.Object);
            var response = await controller.Remove(cartId, queryItems) as OkObjectResult;
            Assert.AreEqual(200, response.StatusCode);
            var result = (webapi.Controllers.CartItems)response.Value;
            Assert.AreEqual(0, result.Items.Count);

            repo.VerifyAll();
        }

        [Test]
        public async Task Remove_BadRequest_InvalidQuantity()
        {
            var repo = new Moq.MockRepository(Moq.MockBehavior.Strict);

            var cartId = 42;

            var items = new System.Collections.Generic.Dictionary<string, int>()
            {
                { "article1", 0 }
            };
            var queryItems = new webapi.Controllers.CartItems { Items = items };
            var cartItems = new cart.grain.CartItems { Items = items };

            // initialize grain
            var cartGrain = repo.Create<ICart>();
            cartGrain.Setup(x => x.Remove(It.IsAny<cart.grain.CartItems>())).Returns(Task.FromResult((CartItemsStatusCode.InvalidQuantity, cartItems)));

            // initialize grain factory
            var grainFactory = repo.Create<IGrainFactory>();
            grainFactory.Setup(x => x.GetGrain<ICart>(cartId)).Returns(cartGrain.Object);


            var controller = new webapi.Controllers.CartController(grainFactory.Object);
            var response = await controller.Remove(cartId, queryItems) as BadRequestObjectResult;
            Assert.AreEqual(400, response.StatusCode);

            repo.VerifyAll();
        }



        [Test]
        public async Task Clear_Ok()
        {
            var repo = new Moq.MockRepository(Moq.MockBehavior.Strict);

            var cartId = 42;

            // initialize grain
            var cartGrain = repo.Create<ICart>();
            cartGrain.Setup(x => x.Clear()).Returns(Task.CompletedTask);

            // initialize grain factory
            var grainFactory = repo.Create<IGrainFactory>();
            grainFactory.Setup(x => x.GetGrain<ICart>(cartId)).Returns(cartGrain.Object);


            var controller = new webapi.Controllers.CartController(grainFactory.Object);
            var response = await controller.Clear(cartId) as OkResult;
            Assert.AreEqual(200, response.StatusCode);

            repo.VerifyAll();
        }

    }
}