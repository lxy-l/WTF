using System.Linq.Expressions;

using Domain.Core;
using Domain.Core.Repository;

using Microsoft.EntityFrameworkCore.Query;

namespace Infrastructure.Core.Repository
{
    /// <summary>
    /// EFCore通用仓储
    /// </summary>
    public interface IEFCoreRepositoryAsync<TEntity, TKey> : IRepositoryAsync<TEntity, TKey> 
        where TEntity : AggregateRoot<TKey>
        where TKey : struct
    {
        /// <summary>
        /// 查询列表（包含子表查询）
        /// </summary>
        /// <param name="include">EF包含查询</param>
        /// <param name="expression">筛选条件</param>
        /// <param name="orderBy">排序</param>
        /// <param name="ignoreQueryFilters">是否禁用筛选器</param>
        /// <returns></returns>
        Task<IQueryable<TEntity>> GetQueryIncludeAsync(
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>> include,
            Expression<Func<TEntity, bool>>? expression = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            bool ignoreQueryFilters = false);

        /// <summary>
        /// 动态查询
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <param name="sort">排序条件</param>
        /// <param name="include">子表</param>
        /// <returns></returns>
        Task<IQueryable<TEntity>> GetDynamicQueryAsync(string? filter=null,string? sort=null, string? include = null);
    }
}
