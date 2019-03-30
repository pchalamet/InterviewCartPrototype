using System;
using NUnit.Framework;
using System.Threading.Tasks;
using Orleans.TestingHost;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace cart.grain.tests
{
    [TestFixture]
    public class CartGrainTest : IDisposable
    {
        private readonly TestCluster _cluster;


        private class ClusterConfigurator : ISiloBuilderConfigurator
        {
            public void Configure(ISiloHostBuilder hostBuilder)
            {
                hostBuilder.AddMemoryGrainStorage("Default")
                           .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(cart.grain.CartGrain).Assembly).WithReferences())
                           .ConfigureServices(svc => svc.AddTransient<ICartService, CartService>());
            }
        }


        public CartGrainTest()
        {
            var builder = new TestClusterBuilder();
            builder.AddSiloBuilderConfigurator<ClusterConfigurator>();
            _cluster = builder.Build();
            _cluster.Deploy();
        }

        public void Dispose()
        {
            _cluster.StopAllSilos();
        }


        [Test]
        public async Task Grain_workflow()
        {
            var add1 = Helpers.Build(("A1", 10));
            var expected1 = Helpers.Build(("A1", 10));
            var remove2 = Helpers.Build(("A1", 2));
            var expected2 = Helpers.Build(("A1", 8));
            var add3 = Helpers.Build(("A2", 3), ("A1", 5));
            var expected3 = Helpers.Build(("A2", 3), ("A1", 5));

            var grain = _cluster.GrainFactory.GetGrain<ICart>(42);

            // expected cart:
            // - article1: 10
            var (status1, content1) = await grain.Add(add1);
            Assert.AreEqual(CartItemsStatusCode.Ok, status1);
            Assert.AreEqual(10, content1.Items["A1"]);
            Assert.AreEqual(1, content1.Items.Count);

            // expected cart:
            // - article1: 8
            var (status2, content2) = await grain.Remove(remove2);
            Assert.AreEqual(CartItemsStatusCode.Ok, status2);
            Assert.AreEqual(8, content2.Items["A1"]);
            Assert.AreEqual(1, content2.Items.Count);

            // expected cart:
            // - empty
            await grain.Clear();

            // expected cart:
            // - article1: 5
            // - article2: 3
            var (status3, content3) = await grain.Add(add3);
            Assert.AreEqual(CartItemsStatusCode.Ok, status3);
            Assert.AreEqual(5, content3.Items["A1"]);
            Assert.AreEqual(3, content3.Items["A2"]);
            Assert.AreEqual(2, content3.Items.Count);
        }
    }
}
