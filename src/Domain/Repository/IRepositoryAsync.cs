using System.Linq.Expressions;

using Domain.AggregateRoots;

namespace Domain.Repository;

/// <summary>
/// 基础仓储接口
/// </summary>
public interface IRepositoryAsync<TEntity, in TKey> where TEntity : AggregateRoot<TKey>
{

    #region 查询

    ///// <summary>
    ///// 获取IQuery对象
    ///// </summary>
    ///// <returns>实体列表</returns>
    //IQueryable<TEntity> GetQuery();

    /// <summary>
    /// 异步获取IQuery对象
    /// </summary>
    /// <returns></returns>
    Task<IQueryable<TEntity>> GetQueryableAsync();


    /// <summary>
    /// 查询列表（根据Lambda条件筛选）
    /// </summary>
    /// <param name="expression">筛选条件</param>
    /// <param name="orderBy">排序</param>
    /// <param name="ignoreQueryFilters">是否禁用筛选器</param>
    /// <returns></returns>
    Task<IQueryable<TEntity>> GetQueryAsync(
        Expression<Func<TEntity, bool>>? expression = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        bool ignoreQueryFilters = false);

    /// <summary>
    /// 根据主键查询
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns></returns>
    Task<TEntity?> FindByIdAsync(TKey id);

    /// <summary>
    /// 查询单个实体信息（根据条件筛选）
    /// </summary>
    /// <remarks>查询不到会抛出异常</remarks>
    /// <param name="expression">筛选条件</param>
    /// <returns></returns>
    Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>>? expression = null);


    /// <summary>
    /// 查询单个实体信息（根据条件筛选）
    /// </summary>
    /// <remarks>查询不到返回null</remarks>
    /// <param name="expression">筛选条件</param>
    /// <returns></returns>
    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? expression = null);


    /// <summary>
    /// 根据条件查询数量
    /// </summary>
    /// <param name="expression">筛选条件</param>
    /// <returns></returns>
    Task<long> CountAsync(Expression<Func<TEntity, bool>>? expression = null);

    #endregion


    #region 新增


    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="entity">实体</param>
    /// <returns></returns>
    Task InsertAsync(TEntity entity);

    /// <summary>
    /// 批量添加(BulkCommit)
    /// </summary>
    /// <param name="entities">实体列表</param>
    /// <returns></returns>
    Task BatchInsertAsync(List<TEntity> entities);

    #endregion



    #region 修改

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="entity">实体</param>
    /// <returns></returns>
    Task UpdateAsync(TEntity entity);

    /// <summary>
    /// 批量修改
    /// </summary>
    /// <param name="entities">实体列表</param>
    Task UpdateAsync(IList<TEntity> entities);

    #endregion


    #region 删除


    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="entity">实体</param>
    /// <returns></returns>
    Task DeleteAsync(TEntity entity);

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="entities">实体列表</param>
    /// <returns></returns>
    Task DeleteAsync(IList<TEntity> entities);

    /// <summary>
    /// 根据条件删除
    /// </summary>
    /// <param name="expression">删除条件</param>
    /// <returns></returns>
    Task<int> DeleteAsync(Expression<Func<TEntity, bool>> expression);

    #endregion

}