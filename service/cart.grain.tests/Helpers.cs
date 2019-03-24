using System;
using System.Linq;

namespace cart.grain.tests
{
    public static class Helpers
    {
        public static CartItems Build(params (int, int)[] list)
        {
            var items = list.ToDictionary(x => x.Item1, x => x.Item2);
            return new CartItems(items);
        }
    }
}
