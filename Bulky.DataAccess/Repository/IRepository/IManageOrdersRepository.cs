using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IManageOrdersRepository : IRepository<ManageOrders>
    {
        ManageOrders Get(int id);
        void Update(ManageOrders obj);
    }
}
