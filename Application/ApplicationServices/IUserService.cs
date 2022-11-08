using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

using Application.DTO;

using Domain.Entities;

namespace Application.ApplicationServices
{
    public interface IUserService
    {
        Task<PagedResult<User>> GetUsers(SeachParams seachParams);

        Task<User> AddUser(User model);

        Task<User> DeleteUser(int id);

        Task<User> EditUser(User model);

        Task BulkInsertUser(List<User> users);
    }
}
