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
            var options = new TestClusterOptions();
            options.SiloBuilderConfiguratorTypes.Add(typeof(ClusterConfigurator).FullName);

            _cluster = new TestCluster(options, new List<IConfigurationSource>());
            _cluster.Deploy();
        }

        public void Dispose()
        {
            _cluster.StopAllSilos();
        }


        [Test]
        public async Task Grain_workflow()
        {
            var add1 = Helpers.Build((1, 10));
            var expected1 = Helpers.Build((1, 10));
            var remove2 = Helpers.Build((1, 2));
            var expected2 = Helpers.Build((1, 8));
            var add3 = Helpers.Build((2, 3), (1, 5));
            var expected3 = Helpers.Build((2, 3), (1, 5));

            var factory = _cluster.GrainFactory;
            var grain = factory.GetGrain<ICart>(42);

            var content1 = await grain.Add(add1);
            Assert.AreEqual(10, content1.Items[1]);
            Assert.AreEqual(1, content1.Items.Count);

            var content2 = await grain.Remove(remove2);
            Assert.AreEqual(8, content2.Items[1]);
            Assert.AreEqual(1, content2.Items.Count);

            await grain.Clear();

            var content3 = await grain.Add(add3);
            Assert.AreEqual(5, content3.Items[1]);
            Assert.AreEqual(3, content3.Items[2]);
            Assert.AreEqual(2, content3.Items.Count);
        }
    }
}
