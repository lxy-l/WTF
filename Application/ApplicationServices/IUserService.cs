using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Entities;

namespace Application.ApplicationServices
{
    public interface IUserService
    {
        Task<List<User>> GetUsers();

        Task<User> AddUser(User model);

        Task<User> DeleteUser(int id);

        Task<User> EditUser(User model);
    }
}
