using System;
using Orleans;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace cart.grain
{
    public class CartItems
    {
        public CartItems(IReadOnlyDictionary<int, int> items)
        {
            Items = items;
        }

        public IReadOnlyDictionary<int, int> Items { get; private set; }
    }

    public interface ICart : IGrainWithGuidKey
    {
        Task<CartItems> Add(CartItems items);
        Task<CartItems> Remove(CartItems items);
        Task Clear();
    }
}
