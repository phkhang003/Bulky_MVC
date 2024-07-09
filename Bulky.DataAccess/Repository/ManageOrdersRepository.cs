using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.DataAcess.Data;
using BulkyBook.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BulkyBook.DataAccess.Repository
{
    public class ManageOrderRepository : Repository<ManageOrders>, IManageOrdersRepository
    {
        private ApplicationDbContext _db;

        public ManageOrderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ManageOrders manageOrders)
        {
            var objFromDb = _db.ManageOrders.FirstOrDefault(c => c.Id == manageOrders.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = manageOrders.Name;
                objFromDb.PhoneNumber = manageOrders.PhoneNumber;
                objFromDb.Email = manageOrders.Email;
                objFromDb.Date = manageOrders.Date;
                objFromDb.Status = manageOrders.Status;
                objFromDb.Total = manageOrders.Total;
                _db.ManageOrders.Update(objFromDb);
            }
        }

        public new IEnumerable<ManageOrders> GetAll(Expression<Func<ManageOrders, bool>> filter = null, string includeProperties = null)
        {
            IQueryable<ManageOrders> query = _db.ManageOrders;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            return query.ToList();
        }

        public new ManageOrders GetFirstOrDefault(Expression<Func<ManageOrders, bool>> filter = null, string includeProperties = null)
        {
            IQueryable<ManageOrders> query = _db.ManageOrders;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            return query.FirstOrDefault();
        }

        public new void Remove(ManageOrders entity)
        {
            _db.ManageOrders.Remove(entity);
        }

        public new void RemoveRange(IEnumerable<ManageOrders> entity)
        {
            _db.ManageOrders.RemoveRange(entity);
        }

        public ManageOrders Get(int id)
        {
            return _db.ManageOrders.FirstOrDefault(c => c.Id == id);
        }
    }
}
