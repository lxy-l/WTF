using Domain.Entities;
using Domain.Repository;

using EFCore.BulkExtensions;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    /// <summary>
    /// 基础仓储实现
    /// </summary>
    public class RepositoryAsync<TEntity, Tkey> : IRepositoryAsync<TEntity, Tkey> where TEntity : BaseEntity
    {
        /// <summary>
        /// 数据上下文
        /// </summary>
        private readonly DbContext _dbContext;

        /// <summary>
        /// 当前操作对象
        /// </summary>
        public DbSet<TEntity> Table => _dbContext.Set<TEntity>();

        public RepositoryAsync(DbContext context)
        {
            _dbContext = context;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual Task DeleteAsync(TEntity entity)
        {
            Table.Remove(entity);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<TEntity?> FindByIdAsync(Tkey id)
        {
            return await Table.FindAsync(id);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task InsertAsync(TEntity entity)
        {
            await Table.AddAsync(entity);
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual async Task InsertRangAsync(List<TEntity> entities)
        {
            await _dbContext.BulkInsertAsync(entities);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual Task UpdateAsync(TEntity entity)
        {
            Table.Update(entity);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<TEntity>> GetListAsync()
        {
            return await Table.AsNoTracking().OrderByDescending(x=>x.Id).Take(10).ToListAsync();
        }
    }
}
