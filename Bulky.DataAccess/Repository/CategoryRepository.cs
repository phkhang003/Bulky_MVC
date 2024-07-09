// CategoryRepository.cs
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.DataAcess.Data;
using BulkyBook.Models;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public Category Get(int id)
        {
            return _db.Categories.FirstOrDefault(c => c.Id == id);
        }

        public void Update(Category obj)
        {
            _db.Categories.Update(obj);
        }
    }
}
