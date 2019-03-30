using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using cart.grain;
using System.Net;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace silo
{
    public class Program
    {
        public static int Main(string[] args)
        {
            return RunMainAsync().Result;
        }

        private static async Task<int> RunMainAsync()
        {
            try
            {
                var host = await StartSilo();
                Console.WriteLine("Press ENTER to exit");
                Console.ReadLine();

                await host.StopAsync();
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 1;
            }
        }

        private static async Task<ISiloHost> StartSilo()
        {
            // define the cluster configuration
            var builder = new SiloHostBuilder()
                .ConfigureEndpoints(siloPort: 11111, gatewayPort: 30000, hostname: Dns.GetHostName())
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "OrleansCluster";
                    options.ServiceId = "CartWebApi";
                })
                .AddMemoryGrainStorageAsDefault()
                .UseConsulClustering(gatewayOptions =>
                {
                    gatewayOptions.Address = new Uri("http://consul:8500/");
                })
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(cart.grain.CartGrain).Assembly).WithReferences())
                .ConfigureServices(svc => svc.AddSingleton<ICartService, CartService>())
                .ConfigureLogging(logging => logging.AddConsole());

            var host = builder.Build();
            await host.StartAsync();
            return host;
        }
    }
}