using BulkyBook.Models;
using System.Collections.Generic;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IOrderDetailRepository : IRepository<OrderDetail>
    {
        IEnumerable<OrderDetail> GetAllWithProducts(int orderId);
    }
}
