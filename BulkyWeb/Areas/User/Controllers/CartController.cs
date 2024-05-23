using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBookWeb.Areas.User.CartService;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ShoppingCartContext _context;

        public CartController(ShoppingCartContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartItem>>> GetCartItems()
        {
            return await _context.CartItems.Include(c => c.Product).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<CartItem>> AddToCart(CartItem cartItem)
        {
            var product = await _context.Products.FindAsync(cartItem.ProductId);
            if (product == null)
            {
                return NotFound();
            }

            cartItem.Product = product;
            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCartItems), new { id = cartItem.Id }, cartItem);
        }
    }

}
