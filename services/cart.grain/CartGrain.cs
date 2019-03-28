using System;
using System.Threading.Tasks;
using Orleans;
using Orleans.Providers;

namespace cart.grain
{
    [StorageProvider]
    public class CartGrain : Grain<CartItems>, ICart
    {
        private ICartService _cartService;

        public CartGrain(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<CartItems> Add(CartItems items)
        {
            base.State = _cartService.Add(base.State, items);
            await base.WriteStateAsync();
            return base.State;
        }

        public async Task<CartItems> Remove(CartItems items)
        {
            base.State = _cartService.Remove(base.State, items);
            await base.WriteStateAsync();
            return base.State;
        }

        public async Task Clear()
        {
            base.State = CartItems.Empty;
            await base.ClearStateAsync();
        }
    }
}
