using System;
using Orleans;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace cart.grain
{
    public class CartItems
    {
        public Dictionary<int, int> Items;

        public static CartItems Empty = new CartItems { Items = new Dictionary<int, int>() };
    }

    public interface ICart : IGrainWithIntegerKey
    {
        Task<CartItems> Add(CartItems items);
        Task<CartItems> Remove(CartItems items);
        Task Clear();
    }
}
