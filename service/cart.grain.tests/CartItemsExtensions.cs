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
            Assert.Catch<ArgumentNullException>(cartItems.Validate);
        }

        [Test]
        public void Null_as_Cartitems_collection_is_invalid()
        {
            var cartItems = new CartItems { Items = null };
            Assert.Catch<ArgumentNullException>(cartItems.Validate);
        }

        [Test]
        public void Zero_id_is_invalid()
        {
            var items = new Dictionary<int, int> { { 10, 5 }, { 0, 10 }, { 3, 4 } };
            var cartItems = new CartItems { Items = items };
            Assert.Catch<ArgumentException>(cartItems.Validate);
        }

        [Test]
        public void Negative_id_is_invalid()
        {
            var items = new Dictionary<int, int> { { 10, 5 }, { -5, 10 }, { 3, 4 } };
            var cartItems = new CartItems { Items = items };
            Assert.Catch<ArgumentException>(cartItems.Validate);
        }

        [Test]
        public void Zero_quantity_is_invalid()
        {
            var items = new Dictionary<int, int> { { 10, 5 }, { 4, 0 }, { 3, 4 } };
            var cartItems = new CartItems { Items = items };
            Assert.Catch<ArgumentException>(cartItems.Validate);
        }

        [Test]
        public void Negative_quantity_is_invalid()
        {
            var items = new Dictionary<int, int> { { 10, 5 }, { 4, -1 }, { 3, 4 } };
            var cartItems = new CartItems { Items = items };
            Assert.Catch<ArgumentException>(cartItems.Validate);
        }

        [Test]
        public void Wellformed_CartItems_pass()
        {
            var items = new Dictionary<int, int> { { 10, 5 }, { 4, 3 }, { 3, 4 } };
            var cartItems = new CartItems { Items = items };
            cartItems.Validate();
        }
    }
}