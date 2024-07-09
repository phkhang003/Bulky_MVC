using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAcess.Data;
using BulkyBook.Models;
using Microsoft.AspNetCore.Identity;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _db;

    public UnitOfWork(ApplicationDbContext db)
    {
        _db = db;

        Category = new CategoryRepository(_db);
        Product = new ProductRepository(_db);
        CartItem = new CartItemRepository(_db);
        OrderDetail = new OrderDetailRepository(_db);
        Order = new OrderRepository(_db);
        ManageOrders = new ManageOrderRepository(_db);
        ApplicationUser = new ApplicationUserRepository(_db);
    }

    public ICategoryRepository Category { get; private set; }
    public IProductRepository Product { get; private set; }
    public ICartItemRepository CartItem { get; private set; }
    public IOrderDetailRepository OrderDetail { get; private set; }
    public IOrderRepository Order { get; private set; }
    public IManageOrdersRepository ManageOrders { get; private set; }
    public IApplicationUserRepository ApplicationUser { get; private set; }

    public void Save()
    {
        _db.SaveChanges();
    }

    public async Task SaveAsync()
    {
        await _db.SaveChangesAsync();
    }

    public void Dispose()
    {
        _db.Dispose();
    }
}
