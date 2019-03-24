using System;
using System.Linq;

namespace cart.grain
{
    public static class CartItemsExtension
    {
        // here we check:
        // - something must be provided
        // - no null item in the list
        // - no invalid Id (must be > 0)
        // - no invalid Quantity (must be > 0)
        public static void Validate(this CartItems items)
        {
            if (null == items) throw new ArgumentNullException(nameof(items));
            if (null == items.Items) throw new ArgumentNullException(nameof(CartItems.Items));
            if (items.Items.Any(x => x.Key <= 0)) throw new ArgumentException("Invalid id provided");
            if (items.Items.Any(x => x.Value <= 0)) throw new ArgumentException("Invalid quantity provided");
        }
    }
}
