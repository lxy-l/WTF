using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Domain.Core.Models;
using EFCore.BulkExtensions;

using Infrastructure.Core.Extend;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Infrastructure.Core.Repository;

/// <summary>
/// 基础仓储实现
/// </summary>
public class EFCoreRepositoryAsync<TEntity, TKey> : IEFCoreRepositoryAsync<TEntity, TKey>
    where TEntity : AggregateRoot<TKey>
    where TKey : struct
{
    /// <summary>
    /// 数据上下文
    /// </summary>
    private readonly DbContext _dbContext;

    /// <summary>
    /// DbSet
    /// </summary>
    private DbSet<TEntity> DbSet => _dbContext.Set<TEntity>();

    public EFCoreRepositoryAsync(DbContext context)
    {
        _dbContext = context;
    }

    #region 查询

    public async Task<IQueryable<TEntity>> GetQueryableAsync() => await Task.Run(DbSet.AsQueryable).ConfigureAwait(false);

    public async Task<IQueryable<TEntity>> GetQueryAsync(
        Expression<Func<TEntity, bool>>? expression = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        bool ignoreQueryFilters = false)
    {
        IQueryable<TEntity> query = (await GetQueryableAsync()).AsNoTracking();
        query = query.WhereIf(expression != null, expression);
        if (ignoreQueryFilters)
        {
            query = query.IgnoreQueryFilters();
        }

        return orderBy != null ? orderBy(query) : query;
    }

    public async Task<IQueryable<TEntity>> GetQueryIncludeAsync(
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>> include,
        Expression<Func<TEntity, bool>>? expression = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        bool ignoreQueryFilters = false)
    {
        IQueryable<TEntity> query = (await GetQueryableAsync().ConfigureAwait(false)).AsNoTracking();
        
        query = include(query);
        query = query.WhereIf(expression != null, expression);

        if (ignoreQueryFilters)
        {
            query = query.IgnoreQueryFilters();
        }

        return orderBy != null ? orderBy(query) : query;
    }

    public async Task<IQueryable<TEntity>> GetDynamicQueryAsync(string? filter=null, string? sort=null,string? include=null)
    {
        IQueryable<TEntity> query = (await GetQueryableAsync()).AsNoTracking();
        if (!string.IsNullOrWhiteSpace(include))
        {
            //TODO 动态include多表联查

        }
        if (!string.IsNullOrWhiteSpace(filter))
        {
            var lambda = LinqExtend.BuildFilterLambda<TEntity>(filter);
            query = query.Where(lambda);
        }
        if (!string.IsNullOrWhiteSpace(sort))
        {
            query = query.OrderBy(sort);
        }
        return query;
    }

    public async Task<TEntity?> FindByIdAsync(TKey id) => await DbSet.FindAsync(id).ConfigureAwait(false);

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null) => predicate switch
    {
        null => await (await GetQueryableAsync()).AsNoTracking().AnyAsync().ConfigureAwait(false),
        _ => await (await GetQueryableAsync()).AsNoTracking().AnyAsync(predicate).ConfigureAwait(false)
    };

    public async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>>? expression) => expression switch
    {
        null => await (await GetQueryableAsync()).SingleAsync().ConfigureAwait(false),
        _ => await (await GetQueryableAsync()).SingleAsync(expression).ConfigureAwait(false)
    };

    public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? expression = null) => expression switch
    {
        null => await (await GetQueryableAsync()).FirstOrDefaultAsync().ConfigureAwait(false),
        _ => await (await GetQueryableAsync()).FirstOrDefaultAsync(expression).ConfigureAwait(false)
    };

    public async Task<long> CountAsync(Expression<Func<TEntity, bool>>? expression = null) => expression switch
    {
        null => await DbSet.CountAsync().ConfigureAwait(false),
        _ => await DbSet.CountAsync(expression).ConfigureAwait(false)
    };

    #endregion


    #region 新增

    public async Task InsertAsync(TEntity entity) => await DbSet.AddAsync(entity).ConfigureAwait(false);

    public async Task BatchInsertAsync(List<TEntity> entities) => await _dbContext.BulkInsertAsync(entities).ConfigureAwait(false);

    #endregion


    #region 修改

    public Task UpdateAsync(TEntity entity) => Task.Run(() => DbSet.Update(entity));

    public async Task UpdateAsync(IList<TEntity> entities) => await _dbContext.BulkUpdateAsync(entities).ConfigureAwait(false);

    #endregion


    #region 删除

    public Task DeleteAsync(TEntity entity) => Task.Run(() => DbSet.Remove(entity));

    public async Task<int> DeleteAsync(Expression<Func<TEntity, bool>>? expression) => expression switch
    {
        null => await DbSet.BatchDeleteAsync().ConfigureAwait(false),
        _ => await DbSet.Where(expression).BatchDeleteAsync().ConfigureAwait(false)
    };

    public async Task DeleteAsync(IList<TEntity> entities) => await _dbContext.BulkDeleteAsync(entities).ConfigureAwait(false);


    #endregion
}