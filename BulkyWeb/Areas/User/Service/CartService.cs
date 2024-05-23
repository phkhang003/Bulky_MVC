

using BulkyBook.Models;
using BulkyBookWeb.Areas.Admin.Controllers;
using BulkyBookWeb.Areas.User.Service;

namespace BulkyBookWeb.Areas.User.CartService
{
    public class CartService
    {
        private readonly ISession _session;
        private const string CartSessionKey = "Cart";

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _session = httpContextAccessor.HttpContext.Session;
        }

        public Cart GetCart()
        {
            var cart = _session.GetObjectFromJson<Cart>(CartSessionKey) ?? new Cart();
            return cart;
        }

        public void SaveCart(Cart cart)
        {
            _session.SetObjectAsJson(CartSessionKey, cart);
        }
    }
}
