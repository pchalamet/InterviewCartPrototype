using NUnit.Framework;
using cart.grain;
using System.Collections.Generic;
using System;

namespace cart.grain.tests
{
    [TestFixture]
    public class CartItemsExtensionsTests
    {
        [Test]
        public void Null_as_CartItems_is_invalid()
        {
            CartItems cartItems = null;
            Assert.AreEqual(CartItemsStatusCode.InvalidArguments, cartItems.Validate());
        }

        [Test]
        public void Null_as_Cartitems_collection_is_invalid()
        {
            var cartItems = new CartItems { Items = null };
            Assert.AreEqual(CartItemsStatusCode.InvalidArguments, cartItems.Validate());
        }

        [Test]
        public void Zero_quantity_is_invalid()
        {
            var items = new Dictionary<string, int> { { "A10", 5 }, { "A4", 0 }, { "A3", 4 } };
            var cartItems = new CartItems { Items = items };
            Assert.AreEqual(CartItemsStatusCode.InvalidQuantity, cartItems.Validate());
        }

        [Test]
        public void Negative_quantity_is_invalid()
        {
            var items = new Dictionary<string, int> { { "A10", 5 }, { "A4", -1 }, { "A3", 4 } };
            var cartItems = new CartItems { Items = items };
            Assert.AreEqual(CartItemsStatusCode.InvalidQuantity, cartItems.Validate());
        }

        [Test]
        public void Wellformed_CartItems_pass()
        {
            var items = new Dictionary<string, int> { { "A10", 5 }, { "A4", 3 }, { "A3", 4 } };
            var cartItems = new CartItems { Items = items };
            Assert.AreEqual(CartItemsStatusCode.Ok, cartItems.Validate());
        }
    }
}