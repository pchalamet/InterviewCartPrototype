using client.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client
{

    public static class CartItemsExtensions
    {
        public static void Print(this Cart.CartItems @this, int taskId)
        {
            if (null == @this) return;

            var sb = new StringBuilder();
            var items = @this.Items.Select(x => $"{x.Key}: {x.Value}");
            var res = string.Join(",", items);
            Console.WriteLine($"{taskId} ==> {res}");
        }
    }

    public class Program
    {
        private static async Task Run(int taskId)
        {
            var cartClient = new Cart.Client("http://localhost:5000", new System.Net.Http.HttpClient());
            var rnd = new Random();
            for(var i=0; i<10; ++i)
            {
                // generate a random article/quantity
                var items = new Dictionary<string, int> { { $"article{rnd.Next(3)}", rnd.Next(5) } };
                var cartItems = new Cart.CartItems { Items = items };
                var cartId = rnd.Next(2);
                try
                {
                    switch (rnd.Next(3))
                    {
                        case 0:
                            var resAdd = await cartClient.AddAsync(cartId, cartItems);
                            resAdd.Print(taskId);
                            break;

                        case 1:
                            var resRem = await cartClient.AddAsync(cartId, cartItems);
                            resRem.Print(taskId);
                            break;

                        case 2:
                            await cartClient.ClearAsync(cartId);
                            break;

                        default:
                            throw new ApplicationException("Unexpected action");
                    }
                }
                catch(SwaggerException<CartError> ex)
                {
                    Console.WriteLine($"Error on #{taskId}: {ex.Result.Reason}");
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Unknown error on #{taskId}: {ex.Message}");
                }
            }
        }


        public static Task Main(string[] args)
        {
            var tasks = Enumerable.Range(1, 5).Select(Run);
            return Task.WhenAll(tasks);
        }
    }
}
