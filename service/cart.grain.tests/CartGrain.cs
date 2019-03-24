using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace cart.grain.tests
{
    [TestFixture]
    public class CartGrainTest
    {
        [Test]
        public async Task Grain_workflow()
        {
            var cartSvc = new CartService();

            var add1 = Helpers.Build((1, 10));
            var expected1 = Helpers.Build((1, 10));
            var remove2 = Helpers.Build((1, 2));
            var expected2 = Helpers.Build((1, 8));
            var add3 = Helpers.Build();
            var expected3 = Helpers.Build();
            var add4 = Helpers.Build((2, 3), (1, 5));
            var expected4 = Helpers.Build((2, 3), (1, 5));

            var grain = new CartGrain(cartSvc);
            var content1 = await grain.Add(add1);
            Assert.AreEqual(10, content1.Items[1]);
            Assert.AreEqual(1, content1.Items.Count);

            var content2 = await grain.Remove(remove2);
            Assert.AreEqual(8, content2.Items[1]);
            Assert.AreEqual(1, content2.Items.Count);

            await grain.Clear();

            var content3 = cartSvc.Content;
            Assert.AreEqual(0, content3.Items.Count);

            var content4 = await grain.Add(add4);
            Assert.AreEqual(5, content4.Items[1]);
            Assert.AreEqual(3, content4.Items[2]);
            Assert.AreEqual(2, content1.Items.Count);
        }
    }
}
