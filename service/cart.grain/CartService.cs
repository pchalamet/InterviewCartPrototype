using System;
using System.Collections.Generic;

namespace cart.grain
{
    public interface ICartService
    {
        // Add new items to the cart
        void Add(CartItems items);

        // Remove items in the cart
        void Remove(CartItems items);

        // Get rid of everything
        void Clear();

        // Current cart items
        CartItems Content { get; }
    }


    // WARNING: this class is not threadsafe - owner is responsible of correctly synchronizing access
    public class CartService : ICartService
    {
        // NOTE: we can afford having a mutable field & instance here since we are strictly serialized per instance
        Dictionary<int, int> _currentItems = new Dictionary<int, int>();

        private void Update(CartItems items, Func<int, int, int> op)
        {
            items.Validate();
            foreach (var item in items.Items)
            {
                if (_currentItems.TryGetValue(item.Key, out var qty))
                {
                    qty = op(qty, item.Value);
                    if (qty <= 0)
                    {
                        _currentItems.Remove(item.Key);
                    }
                    else
                    {
                        _currentItems[item.Key] = qty;
                    }
                }
                else
                    _currentItems.Add(item.Key, item.Value);
            }
        }

        public void Add(CartItems items)
        {
            Update(items, (x, y) => x + y);
        }

        public void Remove(CartItems items)
        {
            Update(items, (x, y) => x - y);
        }

        public void Clear()
        {
            _currentItems.Clear();
        }

        public CartItems Content
        {
            get => new CartItems(_currentItems);
        }
    }
}
