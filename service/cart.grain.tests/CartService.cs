using System;
using NUnit.Framework;
using System.Linq;

namespace cart.grain.tests
{
    [TestFixture]
    public class CartServiceTests
    {
        [Test]
        public void Initial_state_is_empty()
        {
            var svc = new CartService();

            var op = Helpers.Build();
            svc.Add(op);

            var content = svc.Content;
            Assert.IsEmpty(content.Items);
        }

        [Test]
        public void Add_new_id()
        {
            var svc = new CartService();
            svc.Add(Helpers.Build((1, 5)));

            var content = svc.Content;
            Assert.AreEqual(5, content.Items[1]);
            Assert.AreEqual(1, content.Items.Count);
        }

        [Test]
        public void Add_items()
        {
            var svc = new CartService();

            svc.Add(Helpers.Build((1, 5)));
            svc.Add(Helpers.Build((2, 20)));

            var content = svc.Content;
            Assert.AreEqual(5, content.Items[1]);
            Assert.AreEqual(20, content.Items[2]);
            Assert.AreEqual(2, content.Items.Count);
        }

        [Test]
        public void Zero_quantity_is_discarded()
        {
            var svc = new CartService();

            svc.Add(Helpers.Build((1, 5)));
            svc.Add(Helpers.Build((1, 5), (2, 20)));
            svc.Remove(Helpers.Build((1, 10)));

            var content = svc.Content;
            Assert.AreEqual(20, content.Items[2]);
            Assert.AreEqual(1, content.Items.Count);
        }

        [Test]
        public void Clear_can_be_observed()
        {
            var svc = new CartService();

            svc.Add(Helpers.Build((1, 5)));
            svc.Add(Helpers.Build((2, 20)));
            svc.Clear();

            var content = svc.Content;
            Assert.IsEmpty(content.Items);
        }
    }
}
