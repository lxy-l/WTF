using System.Linq.Dynamic.Core;

using Application.Core.DTO;
using Domain.Core.Models;

namespace Application.Core.ApplicationServices;

/// <summary>
/// 基础服务接口
/// </summary>
/// <typeparam name="TEntity">实体</typeparam>
/// <typeparam name="TKey">主键</typeparam>
public interface IBaseService<TEntity, in TKey> 
    where TEntity : AggregateRoot<TKey>
    where TKey : struct
{
    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="searchParams">筛选条件模型</param>
    /// <returns></returns>
    Task<PagedResult<dynamic>> GetPagedResult(SearchParams searchParams);

    /// <summary>
    /// 根据主键获取单个实体
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns></returns>
    Task<TEntity?> GetModelById(TKey id);

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="model">实体模型</param>
    /// <returns></returns>
    Task<TEntity> AddEntity(TEntity model);

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns></returns>
    Task<TEntity> DeleteEntity(TKey id);

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="model">实体模型</param>
    /// <returns></returns>
    Task<TEntity> EditEntity(TEntity model);
}