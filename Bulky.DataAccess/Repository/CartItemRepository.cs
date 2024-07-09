using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.DataAcess.Data;

namespace BulkyBook.DataAccess.Repository
{
    public class CartItemRepository : Repository<CartItem>, ICartItemRepository
    {
        private readonly ApplicationDbContext _db;

        public CartItemRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(CartItem obj)
        {
            _db.CartItems.Update(obj);
        }
    }
}
