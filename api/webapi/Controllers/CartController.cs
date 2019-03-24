using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using cart.grain;
using Microsoft.AspNetCore.Mvc;
using Orleans;

namespace webapi.Controllers
{
    public class CartItems
    {
        public Dictionary<int, int> Items;
    }

    [Route("api/[controller]/[action]/{id}")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IClusterClient _clusterClient;

        public CartController(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
        }

        [HttpPost]
        public async Task<CartItems> Add(long id, [FromBody] CartItems items)
        {
            var grain = _clusterClient.GetGrain<ICart>(id);

            var cartItems = new cart.grain.CartItems { Items = items.Items };
            var newSvcCart = await grain.Add(cartItems);

            var newCart = new CartItems() { Items = newSvcCart.Items };
            return newCart;
        }

        [HttpPost]
        public async Task<CartItems> Remove(long id, [FromBody] CartItems items)
        {
            var grain = _clusterClient.GetGrain<ICart>(id);

            var cartItems = new cart.grain.CartItems { Items = items.Items };
            var newSvcCart = await grain.Remove(cartItems);

            var newCart = new CartItems() { Items = newSvcCart.Items };
            return newCart;
        }

        [HttpDelete]
        public async Task Clear(long id)
        {
            var grain = _clusterClient.GetGrain<ICart>(id);
            await grain.Clear();
        }
    }
}
