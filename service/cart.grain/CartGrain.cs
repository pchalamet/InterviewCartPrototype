using System;
using System.Threading.Tasks;
using Orleans;

namespace cart.grain
{
    public class CartGrain : Grain, ICart
    {
        private ICartService _cartService;

        public CartGrain(ICartService cartService)
        {
            _cartService = cartService;
        }

        public Task<CartItems> Add(CartItems items)
        {
            _cartService.Add(items);
            return Task.FromResult(_cartService.Content);
        }

        public Task<CartItems> Remove(CartItems items)
        {
            _cartService.Remove(items);
            return Task.FromResult(_cartService.Content);
        }

        public Task Clear()
        {
            _cartService.Clear();
            return Task.CompletedTask;
        }
    }
}
