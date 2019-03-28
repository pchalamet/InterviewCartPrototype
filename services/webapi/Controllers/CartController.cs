using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using cart.grain;
using Microsoft.AspNetCore.Mvc;
using Orleans;

namespace webapi.Controllers
{
    /// <summary>
    /// Describe the cart to be added or the cart itself
    /// </summary>
    public class CartItems
    {
        /// <summary>
        /// Describe an item in the cart:
        /// - Key is identifier of the article
        /// - Value is quantity of the article
        /// </summary>
        public Dictionary<string, int> Items;
    }

    /// <summary>
    /// This is the main interface to interact with the cart.
    /// NOTE: There is no security for the moment and cart id should be linked to user instead of id (ie: id must be implicit)
    ///       This is something to be reworked once main interface is stabilized.
    /// </summary>
    [Route("api/[controller]/[action]/{id}")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IClusterClient _clusterClient;

        public CartController(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
        }

        /// <summary>
        /// Add the specified items into the cart.
        /// </summary>
        /// <returns>Cart after items addition</returns>
        /// <param name="id">Cart identifier.</param>
        /// <param name="items">Items to add.</param>
        [HttpPost]
        public async Task<CartItems> Add(long id, [FromBody] CartItems items)
        {
            var grain = _clusterClient.GetGrain<ICart>(id);

            var cartItems = new cart.grain.CartItems { Items = items.Items };
            var newSvcCart = await grain.Add(cartItems);

            var newCart = new CartItems() { Items = newSvcCart.Items };
            return newCart;
        }

        /// <summary>
        /// Remove the specified items from cart.
        /// </summary>
        /// <returns>Cart after items removal</returns>
        /// <param name="id">Cart identifier.</param>
        /// <param name="items">Items to remove</param>
        [HttpPost]
        public async Task<CartItems> Remove(long id, [FromBody] CartItems items)
        {
            var grain = _clusterClient.GetGrain<ICart>(id);

            var cartItems = new cart.grain.CartItems { Items = items.Items };
            var newSvcCart = await grain.Remove(cartItems);

            var newCart = new CartItems() { Items = newSvcCart.Items };
            return newCart;
        }

        /// <summary>
        /// Clear the specified cart.
        /// </summary>
        /// <returns></returns>
        /// <param name="id">Cart identifier.</param>
        [HttpDelete]
        public async Task Clear(long id)
        {
            var grain = _clusterClient.GetGrain<ICart>(id);
            await grain.Clear();
        }
    }
}
