using System.Linq.Dynamic.Core;

using Application.DTO;

using Domain.AggregateRoots;
using Domain.Entities;

namespace Application.ApplicationServices
{

    /// <summary>
    /// 基础服务接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IBaseService<TEntity, TKey> where TEntity : Entity<TKey>, IAggregateRoot
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="seachParams"></param>
        /// <returns></returns>
        Task<PagedResult<TEntity>> GetPagedResult(SeachParams seachParams);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<TEntity> AddEntity(TEntity model);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> DeleteEntity(TKey id);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<TEntity> EditEntity(TEntity model);
    }
}
