using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

using Domain.AggregateRoots;

using EFCore.BulkExtensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Infrastructure.Repository;

/// <summary>
/// 基础仓储实现
/// </summary>
public class EFCoreRepositoryAsync<TEntity, TKey> :
    IEFCoreRepositoryAsync<TEntity, TKey>
    where TEntity : AggregateRoot<TKey>
{
    /// <summary>
    /// 数据上下文
    /// </summary>
    private readonly DbContext _dbContext;//TODO 改为异步获取

    /// <summary>
    /// DbSet
    /// </summary>
    private DbSet<TEntity> DbSet => _dbContext.Set<TEntity>();

    public EFCoreRepositoryAsync(DbContext context)
    {
        _dbContext = context;
    }

    #region 查询

    public async Task<IQueryable<TEntity>> GetQueryableAsync() => await Task.Run(DbSet.AsQueryable);

    public async Task<IQueryable<TEntity>> GetQueryAsync(
        Expression<Func<TEntity, bool>>? expression = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        bool ignoreQueryFilters = false)
    {
        IQueryable<TEntity> query = (await GetQueryableAsync()).AsNoTracking();

        if (expression != null)
        {
            query = query.Where(expression);
        }

        if (ignoreQueryFilters)
        {
            query = query.IgnoreQueryFilters();
        }

        return orderBy != null ? orderBy(query) : query;
    }

    public async Task<IQueryable<TEntity>> GetQueryAsync(
        Expression<Func<TEntity, bool>>? expression = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool ignoreQueryFilters = false)
    {
        IQueryable<TEntity> query = (await GetQueryableAsync()).AsNoTracking();

        if (include != null)
        {
            query = include(query);
        }

        if (expression != null)
        {
            query = query.Where(expression);
        }

        if (ignoreQueryFilters)
        {
            query = query.IgnoreQueryFilters();
        }

        return orderBy != null ? orderBy(query) : query;
    }

    public async Task<TEntity?> FindByIdAsync(TKey id) => await DbSet.FindAsync(id);

    public async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>>? expression) => expression switch
    {
        null => await (await GetQueryableAsync()).SingleAsync(),
        _ => await (await GetQueryableAsync()).SingleAsync(expression)
    };

    public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? expression = null) => expression switch
    {
        null => await (await GetQueryableAsync()).FirstOrDefaultAsync(),
        _ => await (await GetQueryableAsync()).FirstOrDefaultAsync(expression)
    };

    public async Task<long> CountAsync(Expression<Func<TEntity, bool>>? expression = null) => expression switch
    {
        null => await DbSet.CountAsync(),
        _ => await DbSet.CountAsync(expression)
    };

    #endregion


    #region 新增

    public async Task InsertAsync(TEntity entity) => await DbSet.AddAsync(entity);

    public async Task BatchInsertAsync(List<TEntity> entities) => await _dbContext.BulkInsertAsync(entities);

    #endregion


    #region 修改

    public Task UpdateAsync(TEntity entity) => Task.Run(() => DbSet.Update(entity));

    public async Task UpdateAsync(IList<TEntity> entities) => await _dbContext.BulkUpdateAsync(entities);

    #endregion


    #region 删除

    public Task DeleteAsync(TEntity entity) => Task.Run(() => DbSet.Remove(entity));

    public async Task<int> DeleteAsync(Expression<Func<TEntity, bool>>? expression) => expression switch
    {
        null => await DbSet.BatchDeleteAsync(),
        _ => await DbSet.Where(expression).BatchDeleteAsync()
    };

    public async Task DeleteAsync(IList<TEntity> entities) => await _dbContext.BulkDeleteAsync(entities);

    #endregion
}