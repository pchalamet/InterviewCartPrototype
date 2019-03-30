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

    public enum CartItemsStatusCode
    {
        Ok,
        InvalidId,
        InvalidQuantity,
        InvalidArguments,
    }

    public interface ICart : IGrainWithIntegerKey
    {
        Task<(CartItemsStatusCode, CartItems)> Add(CartItems items);
        Task<(CartItemsStatusCode, CartItems)> Remove(CartItems items);
        Task Clear();
    }
}
