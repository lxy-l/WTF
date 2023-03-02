using System.Linq.Expressions;
using Domain.Core.Models;
using Domain.Core.Repository;
using EFCore.BulkExtensions;

using Microsoft.EntityFrameworkCore.Query;

namespace Infrastructure.Core.Repository.EFCore;

/// <summary>
/// EFCore通用仓储接口
/// </summary>
public interface IEfCoreRepositoryAsync<TEntity, in TKey> : IRepositoryAsync<TEntity, TKey> 
    where TEntity : IAggregateRoot
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
    IQueryable<TEntity> GetQueryInclude(
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>> include,
        Expression<Func<TEntity, bool>>? expression = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        bool ignoreQueryFilters = false);


    /// <summary>
    /// 批量添加
    /// </summary>
    /// <remarks>EFCore.BulkExtensions</remarks>
    /// <param name="entities">实体列表</param>
    /// <param name="bulkConfig">批量提交配置</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task BulkInsertAsync(IList<TEntity> entities, BulkConfig? bulkConfig = null, CancellationToken cancellationToken = default);


    /// <summary>
    /// 批量删除
    /// </summary>
    /// <remarks>EFCore.BulkExtensions</remarks>
    /// <param name="entities">实体列表</param>
    /// <param name="bulkConfig">批量删除配置</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DeleteAsync(IList<TEntity> entities, BulkConfig? bulkConfig = null, CancellationToken cancellationToken = default);


    /// <summary>
    /// 批量修改
    /// </summary>
    /// <remarks>EFCore.BulkExtensions</remarks>
    /// <param name="entities">实体列表</param>
    /// <param name="bulkConfig">批量修改配置</param>
    /// <param name="cancellationToken"></param>
    Task UpdateAsync(IList<TEntity> entities, BulkConfig? bulkConfig = null, CancellationToken cancellationToken = default);

}