using BulkyBook.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }
        ICartItemRepository CartItem { get; }
        IOrderDetailRepository OrderDetail { get; }
        IOrderRepository Order { get; }
        IManageOrdersRepository ManageOrders { get; }
        IApplicationUserRepository ApplicationUser { get; }
        void Save();
        Task SaveAsync();
    }
}
