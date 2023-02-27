using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Domain.Core.Models;

namespace Domain.Core.Repository;

/// <summary>
/// 基础仓储接口
/// </summary>
public interface IRepositoryAsync<TEntity, in TKey>
    where TEntity : AggregateRoot<TKey>
    where TKey : struct
{

    #region 查询

    /// <summary>
    /// 异步获取IQuery对象
    /// </summary>
    /// <returns></returns>
    IQueryable<TEntity> GetQueryable();


    /// <summary>
    /// 查询列表
    /// </summary>
    /// <param name="expression">筛选条件</param>
    /// <param name="orderBy">排序</param>
    /// <param name="ignoreQueryFilters">是否禁用筛选器</param>
    /// <returns></returns>
    IQueryable<TEntity> GetQuery(
        Expression<Func<TEntity, bool>>? expression = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        bool ignoreQueryFilters = false);


    /// <summary>
    /// 动态查询(扩展)
    /// </summary>
    /// <param name="filter">筛选条件</param>
    /// <param name="sort">排序条件</param>
    /// <param name="include">子表</param>
    /// <returns></returns>
    IQueryable<TEntity> GetDynamicQuery(string? filter = null, string? sort = null, string[]? include = null);

    /// <summary>
    /// 根据主键查询
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns></returns>
    ValueTask<TEntity?> FindByIdAsync(TKey id);

    /// <summary>
    /// 根据主键查询
    /// </summary>
    /// <param name="ids">主键列表</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask<TEntity?> FindByIdsAsync(object[] ids, CancellationToken cancellationToken = default);

    /// <summary>
    /// 查询单个实体信息
    /// </summary>
    /// <remarks>查询不到会抛出异常</remarks>
    /// <param name="expression">筛选条件</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>>? expression = null, CancellationToken cancellationToken = default);


    /// <summary>
    /// 查询单个实体信息
    /// </summary>
    /// <remarks>查询不到返回null</remarks>
    /// <param name="expression">筛选条件</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? expression = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// 判断是否存在
    /// </summary>
    /// <param name="predicate">条件</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// 根据条件查询数量
    /// </summary>
    /// <param name="expression">筛选条件</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<long> CountAsync(Expression<Func<TEntity, bool>>? expression = null, CancellationToken cancellationToken = default);

    #endregion


    #region 新增

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="entity">实体</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task InsertAsync(TEntity entity, CancellationToken cancellationToken=default);

    #endregion



    #region 修改

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="entity">实体</param>
    /// <returns></returns>
    void Update(TEntity entity);

    #endregion


    #region 删除


    /// <summary>
    /// 删除
    /// </summary>
    /// <remarks>可以不用查询附加，直接删除</remarks>
    /// <param name="entity">实体</param>
    /// <returns></returns>
    void Delete(TEntity entity);

    /// <summary>
    /// 根据条件删除(扩展)
    /// </summary>
    /// <param name="expression">删除条件</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> DeleteAsync(Expression<Func<TEntity, bool>>? expression, CancellationToken cancellationToken = default);

    #endregion

}