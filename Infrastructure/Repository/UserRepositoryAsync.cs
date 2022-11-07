using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Domain.AggregateRoots;
using Domain.Entities;
using Domain.Repository;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class UserRepositoryAsync<TEntity, TView, Tkey> : RepositoryAsync<TEntity, Tkey>,
        IUserRepositoryAsync<TEntity, TView, Tkey> where TEntity : Entity<Tkey>, IAggregateRoot
    {
        public UserRepositoryAsync(DbContext context) : base(context)
        {

        }

        public Task<TView> GetView()
        {
            throw new NotImplementedException();
        }
    }
}
