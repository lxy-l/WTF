using Domain.AggregateRoots;
using Domain.Entities;

namespace Application.ApplicationServices
{
    public interface IUserService<TEntity,TKey>:IBaseService<TEntity, TKey> where TEntity : Entity<TKey>, IAggregateRoot
    {
        Task BulkInsertUser(List<User> users);
    }
}
