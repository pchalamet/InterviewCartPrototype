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
        public static CartItemsStatusCode Validate(this CartItems items)
        {
            if (null == items) return CartItemsStatusCode.InvalidArguments;
            if (null == items.Items) return CartItemsStatusCode.InvalidArguments;
            if (items.Items.Any(x => x.Key == null)) return CartItemsStatusCode.InvalidId;
            if (items.Items.Any(x => x.Value <= 0)) return CartItemsStatusCode.InvalidQuantity;
            return CartItemsStatusCode.Ok;
        }
    }
}
