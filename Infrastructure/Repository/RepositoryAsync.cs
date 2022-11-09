using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

using Domain.AggregateRoots;
using Domain.Entities;
using Domain.Repository;

using EFCore.BulkExtensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Infrastructure.Repository
{
    /// <summary>
    /// 基础仓储实现
    /// </summary>
    public class RepositoryAsync<TEntity, Tkey> : IRepositoryAsync<TEntity, Tkey> where TEntity : Entity<Tkey>, IAggregateRoot
    {
        /// <summary>
        /// 数据上下文
        /// </summary>
        protected readonly DbContext _dbContext;

        /// <summary>
        /// DbSet
        /// </summary>
        public virtual DbSet<TEntity> _dbSet => _dbContext.Set<TEntity>();

        public RepositoryAsync(DbContext context)
        {
            _dbContext = context;
        }

        #region 查询

        public IQueryable<TEntity> GetQuery() => _dbSet.AsQueryable();

        public async Task<List<TEntity>> GetQueryAsync(
            Expression<Func<TEntity, bool>>? expression = null, 
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            bool ignoreQueryFilters = false)
        {
            IQueryable<TEntity> query = GetQuery().AsNoTracking();

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

            return orderBy != null ? await orderBy(query).ToListAsync() : await query.ToListAsync();
        }

        public async Task<TEntity?> FindByIdAsync(Tkey id) => await _dbSet.FindAsync(id);

        public async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>>? expression) => expression switch
        {
            null => await GetQuery().SingleAsync(),
            _ => await GetQuery().SingleAsync(expression)
        };

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? expression = null) => expression switch
        {
            null => await GetQuery().FirstOrDefaultAsync(),
            _ => await GetQuery().FirstOrDefaultAsync(expression)
        };

        public async Task<long> CountAsync(Expression<Func<TEntity, bool>>? expression = null) => expression switch
        {
            null => await _dbSet.CountAsync(),
            _ => await _dbSet.CountAsync(expression)
        };

        public Task<PagedResult<TEntity>> GetPagedResultAsync(
            Expression<Func<TEntity, bool>>? expression = null, 
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, 
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, 
            int page = 1, int pageSize = 20, bool ignoreQueryFilters = false)
        {
            IQueryable<TEntity> query = GetQuery().AsNoTracking();

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

            return orderBy != null
                ? Task.FromResult(orderBy(query).PageResult(page, pageSize))
                : Task.FromResult(query.OrderBy(o=>o.Id).PageResult(page, pageSize));
        }

        public Task<PagedResult<TResult>> GetPagedResultBySelectAsync<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>>? expression = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            int page = 0, int pageSize = 20,bool ignoreQueryFilters = false)
        {
            IQueryable<TEntity> query = GetQuery().AsNoTracking();

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

            return orderBy != null
                ? Task.FromResult(orderBy(query).Select(selector).PageResult(page, pageSize))
                : Task.FromResult(query.OrderBy(o => o.Id).Select(selector).PageResult(page, pageSize));
        }

        #endregion


        #region 新增

        public async Task InsertAsync(TEntity entity) => await _dbSet.AddAsync(entity);

        public async Task BatchInsertAsync(List<TEntity> entities) => await _dbContext.BulkInsertAsync(entities);

        #endregion


        #region 修改

        public void Update(TEntity entity) => _dbSet.Update(entity);

        public async Task UpdateAsync(IList<TEntity> entities) => await _dbContext.BulkUpdateAsync(entities);
        
        #endregion


        #region 删除

        public void Delete(TEntity entity) => _dbSet.Remove(entity);

        public async Task<int> DeleteAsync(Expression<Func<TEntity, bool>>? expression) => expression switch
        {
            null => await _dbSet.BatchDeleteAsync(),
            _ => await _dbSet.Where(expression).BatchDeleteAsync()
        };

        public async Task DeleteAsync(IList<TEntity> entities) => await _dbContext.BulkDeleteAsync(entities);

        #endregion
    }
}
