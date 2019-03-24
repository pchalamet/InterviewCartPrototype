using System;
using System.Collections.Generic;

namespace cart.grain
{
    public interface ICartService
    {
        // Add new items to the cart
        CartItems Add(CartItems items, CartItems addItems);

        // Remove items in the cart
        CartItems Remove(CartItems items, CartItems removeItems);
    }


    public class CartService : ICartService
    {
        private CartItems Update(CartItems items, CartItems opItems, Func<int, int, int> op)
        {
            items.Validate();
            opItems.Validate();

            var newItems = new CartItems { Items = new Dictionary<int, int>(items.Items) };
            foreach (var item in opItems.Items)
            {
                if (newItems.Items.TryGetValue(item.Key, out var qty))
                {
                    qty = op(qty, item.Value);
                    if (qty <= 0)
                    {
                        newItems.Items.Remove(item.Key);
                    }
                    else
                    {
                        newItems.Items[item.Key] = qty;
                    }
                }
                else
                    newItems.Items.Add(item.Key, item.Value);
            }

            return newItems;
        }

        public CartItems Add(CartItems items, CartItems addItems)
        {
            return Update(items, addItems, (x, y) => x + y);
        }

        public CartItems Remove(CartItems items, CartItems removeItems)
        {
            return Update(items, removeItems, (x, y) => x - y);
        }
    }
}
