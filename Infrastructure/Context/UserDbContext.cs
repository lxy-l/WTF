using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context
{

    public class UserDbContext:DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options)
         : base(options)
        {
        }

        public DbSet<User>? Users { get; set; }
    }


}
