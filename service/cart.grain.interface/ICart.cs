using System;
using Orleans;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace cart.grain
{
    [Serializable]
    public class CartItems
    {
        public Dictionary<string, int> Items = new Dictionary<string, int>();

        public static CartItems Empty = new CartItems();
    }

    public interface ICart : IGrainWithIntegerKey
    {
        Task<CartItems> Add(CartItems items);
        Task<CartItems> Remove(CartItems items);
        Task Clear();
    }
}
