using Domain.AggregateRoots;
using Domain.Entities;

namespace Domain.Repository
{
    /// <summary>
    /// 自定义用户仓储
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    /// <typeparam name="TView">自定义视图</typeparam>
    /// <typeparam name="Tkey">主键</typeparam>
    public interface IUserRepositoryAsync<TEntity,TView, Tkey> : IRepositoryAsync<TEntity, Tkey> where TEntity : Entity<Tkey>,IAggregateRoot
    {
        /// <summary>
        /// 自定义视图查询
        /// </summary>
        /// <returns>自定义视图</returns>
        Task<TView> GetView();
    }
}
