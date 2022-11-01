using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Entities;

namespace Domain.Repository
{
    /// <summary>
    /// 基础仓储接口
    /// </summary>
    public interface IRepositoryAsync<TEntity,Tkey>: IDisposable where TEntity : BaseEntity
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task InsertAsync(TEntity entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task DeleteAsync(TEntity entity);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task UpdateAsync(TEntity entity);

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity?> FindByIdAsync(Tkey id);

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        Task<List<TEntity>> GetListAsync();


    }
}
