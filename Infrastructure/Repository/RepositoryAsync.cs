using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private bool disposedValue;

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
            await _dbContext.BulkSaveChangesAsync();
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
            return await Table.AsNoTracking().OrderByDescending(x=>x.CreateTime).Take(10).ToListAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                    _dbContext.Dispose();
                }

                // TODO: 释放未托管的资源(未托管的对象)并重写终结器
                // TODO: 将大型字段设置为 null
                disposedValue = true;
            }
        }

        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        // ~RepositoryAsync()
        // {
        //     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
