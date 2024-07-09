// IApplicationUserRepository.cs
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        ApplicationUser GetFirstOrDefault(Expression<Func<ApplicationUser, bool>> filter = null, string includeProperties = null);
    }
}
