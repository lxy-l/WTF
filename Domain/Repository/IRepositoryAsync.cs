using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

using Domain.AggregateRoots;
using Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Domain.Repository
{
    /// <summary>
    /// 基础仓储接口
    /// </summary>
    public interface IRepositoryAsync<TEntity,Tkey> where TEntity : Entity<Tkey>, IAggregateRoot
    {

        #region 查询

        /// <summary>
        /// 获取IQuery对象
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> GetQuery();


        /// <summary>
        /// 查询列表（根据条件筛选）
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="orderBy"></param>
        /// <param name="include"></param>
        /// <param name="disableTracking"></param>
        /// <param name="ignoreQueryFilters"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetQueryAsync(
            Expression<Func<TEntity, bool>>? expression = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            bool ignoreQueryFilters = false);

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity?> FindByIdAsync(Tkey id);

        /// <summary>
        /// 查询单个实体信息（根据条件筛选）
        /// </summary>
        /// <remarks>查询不到会抛出异常</remarks>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<TEntity> SingleAsync(Expression<Func<TEntity,bool>>? expression = null);


        /// <summary>
        /// 查询单个实体信息（根据条件筛选）
        /// </summary>
        /// <remarks>查询不到返回null</remarks>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity,bool>>? expression = null );


        /// <summary>
        /// 根据条件查询数量
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<long> CountAsync(Expression<Func<TEntity,bool>>? expression = null);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="orderBy"></param>
        /// <param name="include"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="ignoreQueryFilters"></param>
        /// <returns></returns>
        Task<PagedResult<TEntity>> GetPagedResultAsync(
            Expression<Func<TEntity, bool>>? expression = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            int page = 1,
            int pageSize = 20,
            bool ignoreQueryFilters = false);

        /// <summary>
        /// 分页查询（自定义Select字段）
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="selector"></param>
        /// <param name="predicate"></param>
        /// <param name="orderBy"></param>
        /// <param name="include"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="ignoreQueryFilters"></param>
        /// <returns></returns>
        Task<PagedResult<TResult>> GetPagedResultBySelectAsync<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>>? expression = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            int page = 1,
            int pageSize = 20,
            bool ignoreQueryFilters = false);


        #endregion


        #region 新增


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task InsertAsync(TEntity entity);

        /// <summary>
        /// 批量添加(BulkCommit)
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task BatchInsertAsync(List<TEntity> entities);

        #endregion



        #region 修改

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        void Update(TEntity entity);

        /// <summary>
        /// 批量修改(BulkCommit)
        /// </summary>
        /// <param name="entities"></param>
        Task UpdateAsync(IList<TEntity> entities);

        #endregion


        #region 删除


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        void Delete(TEntity entity);

        /// <summary>
        /// 批量删除(BulkCommit)
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task DeleteAsync(IList<TEntity> entities);

        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(Expression<Func<TEntity,bool>> expression);

        #endregion


    }
}
