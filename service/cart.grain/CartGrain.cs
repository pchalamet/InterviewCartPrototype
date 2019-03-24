using System;
using System.Threading.Tasks;
using Orleans;
using Orleans.Providers;

namespace cart.grain
{
    public class CartGrain : Grain<CartItems>, ICart
    {
        private ICartService _cartService;

        public CartGrain(ICartService cartService)
        {
            _cartService = cartService;
            base.State = CartItems.Empty;
        }

        public Task<CartItems> Add(CartItems items)
        {
            base.State = _cartService.Add(base.State, items);
            return Task.FromResult(base.State);
        }

        public Task<CartItems> Remove(CartItems items)
        {
            base.State = _cartService.Remove(base.State, items);
            return Task.FromResult(base.State);
        }

        public Task Clear()
        {
            base.State = CartItems.Empty;
            return Task.CompletedTask;
        }
    }
}
