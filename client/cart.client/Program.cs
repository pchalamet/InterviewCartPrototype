using System;
using System.Collections.Generic;

namespace cart.client
{
    class Program
    {
        static void Main(string[] args)
        {
            var cart = new Cart.Client("http://localhost:5000", new System.Net.Http.HttpClient());
            var items = new Dictionary<int, int>() {
                { 1, 1 } };
            var cartItems = new Cart.CartItems() { Items = items };
        }
    }
}
