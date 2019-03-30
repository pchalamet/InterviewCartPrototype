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
        // apply operation on cart with provided items
        // rules are as follow:
        // - adding an article increase its quantity (if unknown initial quantiy is 0)
        // - removing an article decrease its quantity until it's getting lower than 0 - then it's removed
        // - removing an unknown article is allowed and is a no-op
        // - max quantity is int.MaxValue
        private CartItems Update(CartItems items, CartItems opItems, Func<long, long, long> op)
        {
#if DEBUG
            if (items.Validate() != CartItemsStatusCode.Ok) throw new ArgumentException();
            if (opItems.Validate() != CartItemsStatusCode.Ok) throw new ArgumentException();
#endif

            var newItems = new CartItems { Items = new Dictionary<string, int>(items.Items) };
            foreach (var item in opItems.Items)
            {
                newItems.Items.TryGetValue(item.Key, out var qty);

                // take care of overflow and underflow
                var newQty = op(qty, item.Value);
                newQty = Math.Min(newQty, int.MaxValue);
                newQty = Math.Max(newQty, int.MinValue);

                if (newQty <= 0)
                {
                    newItems.Items.Remove(item.Key);
                }
                else
                {
                    newItems.Items[item.Key] = (int)newQty;
                }
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
