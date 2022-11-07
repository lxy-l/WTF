using Domain.AggregateRoots;
using Domain.Entities;

namespace Domain.Repository
{
    /// <summary>
    /// 自定义用户仓储
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TView"></typeparam>
    /// <typeparam name="Tkey"></typeparam>
    public interface IUserRepositoryAsync<TEntity,TView, Tkey> : IRepositoryAsync<TEntity, Tkey> where TEntity : Entity<Tkey>,IAggregateRoot
    {
        /// <summary>
        /// 自定义视图查询
        /// </summary>
        /// <returns></returns>
        Task<TView> GetView();
    }
}
