using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

using Domain.AggregateRoots;
using Domain.Entities;
using Domain.Repository;

using EFCore.BulkExtensions;

using Microsoft.EntityFrameworkCore;

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
        private readonly DbContext _dbContext;

        /// <summary>
        /// 模型
        /// </summary>
        public virtual DbSet<TEntity> _dbSet => _dbContext.Set<TEntity>();

        public RepositoryAsync(DbContext context)
        {
            _dbContext = context;
        }

        #region 查询

        public IQueryable<TEntity> GetQuery() => _dbSet.AsQueryable();

        public async Task<List<TEntity>> GetListAsync(int page=1,int pageSize=20, Expression<Func<TEntity, bool>>? expression=null)
        {
            if (expression == null)
            {
                return await _dbSet
                    .OrderBy(x => x.CreateTime)
                    .Page(page,pageSize)
                    .AsNoTracking()
                    .ToListAsync();
            }
            return await _dbSet
                .Where(expression)
                .OrderBy(x=>x.CreateTime)
                .Page(page,pageSize)
                .AsNoTracking()
                .ToListAsync();
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

        public Task<PagedResult<TEntity>> GetPagedResultAsync(int page = 1, int pageSize = 20, Expression<Func<TEntity, bool>>? expression = null)
        {
            if (expression==null)
            {
                return Task.FromResult(_dbSet.OrderBy(x => x.CreateTime).PageResult(page, pageSize));
            }
            return Task.FromResult(_dbSet
                .Where(expression)
                .OrderBy(x=>x.CreateTime)
                .PageResult(page, pageSize));
        }

        #endregion


        #region 新增
        public async Task InsertAsync(TEntity entity) => await _dbSet.AddAsync(entity);

        public async Task BatchInsertAsync(List<TEntity> entities) => await _dbContext.BulkInsertAsync(entities);

        #endregion


        #region 修改
        public void UpdateAsync(TEntity entity) => _dbSet.Update(entity);


        //public async Task<int> BatchUpdateAsync(Expression<Func<TEntity, bool>> expression)
        //{
        //    //return await Entity.BatchUpdateAsync();
        //}

        #endregion


        #region 删除

        public void DeleteAsync(TEntity entity) => _dbSet.Remove(entity);

        public async Task<int> DeleteAsync(Expression<Func<TEntity, bool>>? expression) => expression switch
        {
            null => await _dbSet.BatchDeleteAsync(),
            _ => await _dbSet.Where(expression).BatchDeleteAsync()
        };




        #endregion
    }
}
