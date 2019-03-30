using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using cart.grain;
using Microsoft.AspNetCore.Http;
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


    public enum CartErrorCode
    {
        InternalError = 1,
        InvalidItemId = 2,
        InvalidQuantity = 3,
        BadRequest = 4,
    }


    public class CartError
    {
        public CartErrorCode ErrorCode;
        public string Reason;
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
        private readonly IGrainFactory _grainFactory;

        public CartController(IGrainFactory grainFactory)
        {
            _grainFactory = grainFactory;
        }

        /// <summary>
        /// Add the specified items into the cart.
        /// </summary>
        /// <returns>Cart after items addition</returns>
        /// <param name="id">Cart identifier.</param>
        /// <param name="items">Items to add.</param>
        [HttpPost]
        [ProducesResponseType(typeof(CartItems), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CartError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add(long id, [FromBody] CartItems items)
        {
            try
            {
                var grain = _grainFactory.GetGrain<ICart>(id);

                var cartItems = new cart.grain.CartItems { Items = items.Items };
                var newSvcCart = await grain.Add(cartItems);

                var newCart = new CartItems() { Items = newSvcCart.Items };
                return Ok(newCart);
            }
            catch (Exception ex)
            {
                return base.StatusCode(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Remove the specified items from cart.
        /// </summary>
        /// <returns>Cart after items removal</returns>
        /// <param name="id">Cart identifier.</param>
        /// <param name="items">Items to remove</param>
        [HttpPost]
        [ProducesResponseType(typeof(CartItems), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Remove(long id, [FromBody] CartItems items)
        {
            try
            {
                var grain = _grainFactory.GetGrain<ICart>(id);

                var cartItems = new cart.grain.CartItems { Items = items.Items };
                var newSvcCart = await grain.Remove(cartItems);

                var newCart = new CartItems() { Items = newSvcCart.Items };
                return Ok(newCart);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Clear the specified cart.
        /// </summary>
        /// <returns></returns>
        /// <param name="id">Cart identifier.</param>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Clear(long id)
        {
            try
            {
                var grain = _grainFactory.GetGrain<ICart>(id);
                await grain.Clear();
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
