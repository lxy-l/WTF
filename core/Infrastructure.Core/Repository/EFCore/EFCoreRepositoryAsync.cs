using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

using Domain.Core.Models;

using EFCore.BulkExtensions;

using Infrastructure.Core.Extend;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Infrastructure.Core.Repository.EFCore;

/// <summary>
/// 基础仓储实现
/// </summary>
public class EfCoreRepositoryAsync<TEntity, TKey> : IEfCoreRepositoryAsync<TEntity, TKey>
    where TEntity : Entity<TKey> , IAggregateRoot
    where TKey : struct
{
    private readonly DbContext _dbContext;
    private DbSet<TEntity> DbSet => _dbContext.Set<TEntity>();
    //public IUnitOfWork UnitOfWork => (IUnitOfWork)_dbContext;

    public EfCoreRepositoryAsync(DbContext context) => _dbContext = context;

    #region 查询

    //public async Task<IQueryable<TEntity>> GetQueryable() => await Task.Run(DbSet.AsQueryable).ConfigureAwait(false);

    public IQueryable<TEntity> GetQueryable() => DbSet.AsQueryable();

    public IQueryable<TEntity> GetQuery(
        Expression<Func<TEntity, bool>>? expression = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        bool ignoreQueryFilters = false)
    {
        IQueryable<TEntity> query = DbSet.AsNoTracking();
        query = query.WhereIf(expression != null, expression);
        if (ignoreQueryFilters)
        {
            query = query.IgnoreQueryFilters();
        }

        return orderBy != null ? orderBy(query) : query.OrderBy(x => x.Id);
    }

    public IQueryable<TEntity> GetQueryInclude(
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>> include,
        Expression<Func<TEntity, bool>>? expression = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        bool ignoreQueryFilters = false)
    {
        IQueryable<TEntity> query = DbSet.AsNoTracking();

        query = include(query);
        query = query.WhereIf(expression != null, expression);

        if (ignoreQueryFilters)
        {
            query = query.IgnoreQueryFilters();
        }

        return orderBy != null ? orderBy(query) : query.OrderBy(x => x.Id);
    }

    public IQueryable<TEntity> GetQueryInclude(string includeProperties, Expression<Func<TEntity, bool>>? expression = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, bool ignoreQueryFilters = false)
    {
        IQueryable<TEntity> query = DbSet.AsNoTracking();

        query = query.WhereIf(expression != null, expression);

        foreach (var includeProperty in includeProperties.Split
            (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        if (orderBy != null)
        {
            return orderBy(query);
        }
        else
        {
            return query.OrderBy(x => x.Id);
        }
    }

    public IQueryable<TEntity> GetDynamicQuery(string? filter = null, string? sort = null, string[]? include = null)
    {
        IQueryable<TEntity> query = DbSet.AsNoTracking();
        if (include?.Any() ?? false)
        {
            foreach (var table in include)
            {
                query = query.Include(table);
            }
        }
        if (!string.IsNullOrWhiteSpace(filter))
        {
            var lambda = LinqExtend.BuildFilterLambda<TEntity>(filter);
            query = query.Where(lambda);
        }
        return string.IsNullOrWhiteSpace(sort) ? query.OrderBy(nameof(IEntity<TKey>.Id)) : query.OrderBy(sort);
    }

    public async ValueTask<TEntity?> FindByIdsAsync(object[] ids, CancellationToken cancellationToken = default) => await DbSet.FindAsync(ids, cancellationToken: cancellationToken).ConfigureAwait(false);

    public async ValueTask<TEntity?> FindByIdAsync(TKey id) => await DbSet.FindAsync(id).ConfigureAwait(false);

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default) => predicate switch
    {
        null => await DbSet.AsNoTracking().AnyAsync(cancellationToken).ConfigureAwait(false),
        _ => await DbSet.AsNoTracking().AnyAsync(predicate, cancellationToken).ConfigureAwait(false)
    };

    public async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>>? expression, CancellationToken cancellationToken = default) => expression switch
    {
        null => await DbSet.SingleAsync(cancellationToken).ConfigureAwait(false),
        _ => await DbSet.SingleAsync(expression, cancellationToken).ConfigureAwait(false)
    };

    public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? expression = null, CancellationToken cancellationToken = default) => expression switch
    {
        null => await DbSet.FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false),
        _ => await DbSet.FirstOrDefaultAsync(expression, cancellationToken).ConfigureAwait(false)
    };

    public async Task<long> CountAsync(Expression<Func<TEntity, bool>>? expression = null, CancellationToken cancellationToken = default) => expression switch
    {
        null => await DbSet.CountAsync(cancellationToken).ConfigureAwait(false),
        _ => await DbSet.CountAsync(expression, cancellationToken).ConfigureAwait(false)
    };

    #endregion


    #region 新增

    public async Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default) => await DbSet.AddAsync(entity, cancellationToken).ConfigureAwait(false);

    public async Task BulkInsertAsync(IList<TEntity> entities, BulkConfig? bulkConfig = null, CancellationToken cancellationToken = default)
    {
        //TODO 替换EFCore.BulkExtensions;
        await _dbContext.BulkInsertAsync(entities, cancellationToken: cancellationToken);
    }

    #endregion


    #region 修改

    public void Update(TEntity entity) => DbSet.Update(entity);

    public async Task UpdateAsync(IList<TEntity> entities, BulkConfig? bulkConfig = null, CancellationToken cancellationToken = default)
        => await _dbContext.BulkUpdateAsync(entities, bulkConfig, cancellationToken: cancellationToken).ConfigureAwait(false);

    #endregion


    #region 删除

    public void Delete(TEntity entity) => DbSet.Remove(entity);

    public async Task<int> DeleteAsync(Expression<Func<TEntity, bool>>? expression, CancellationToken cancellationToken = default) => expression switch
    {
        null => await DbSet.BatchDeleteAsync(cancellationToken).ConfigureAwait(false),
        _ => await DbSet.Where(expression).BatchDeleteAsync(cancellationToken).ConfigureAwait(false)
    };

    public async Task DeleteAsync(IList<TEntity> entities, BulkConfig? bulkConfig = null, CancellationToken cancellationToken = default) => await _dbContext.BulkDeleteAsync(entities, bulkConfig, cancellationToken: cancellationToken).ConfigureAwait(false);

    public void Delete(IList<TEntity> entities)
    {
        _dbContext.RemoveRange(entities);
    }
    #endregion
}