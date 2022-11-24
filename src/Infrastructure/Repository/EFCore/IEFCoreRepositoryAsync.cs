using System.Linq.Expressions;

using Domain.AggregateRoots;
using Domain.Repository;

using Microsoft.EntityFrameworkCore.Query;

namespace Infrastructure.Repository
{
    /// <summary>
    /// EFCore通用仓储
    /// </summary>
    public interface IEFCoreRepositoryAsync<TEntity, TKey> : IRepositoryAsync<TEntity, TKey>
        where TEntity : AggregateRoot<TKey>
    {
        /// <summary>
        /// 查询列表（根据Lambda条件筛选）
        /// </summary>
        /// <param name="expression">筛选条件</param>
        /// <param name="orderBy">排序</param>
        /// <param name="include">EF包含查询</param>
        /// <param name="ignoreQueryFilters">是否禁用筛选器</param>
        /// <returns></returns>
        Task<IQueryable<TEntity>> GetQueryAsync(
        Expression<Func<TEntity, bool>>? expression = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            bool ignoreQueryFilters = false);
    }
}
