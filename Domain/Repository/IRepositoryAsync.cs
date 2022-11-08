using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

using Domain.AggregateRoots;
using Domain.Entities;

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
        /// <returns></returns>
        Task<List<TEntity>> GetListAsync(int page = 1, int pageSize = 20, Expression<Func<TEntity, bool>>? expression = null);


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


        //TODO 分页条件查询

        Task<PagedResult<TEntity>> GetPagedResultAsync(int page = 1, int pageSize = 20, Expression<Func<TEntity, bool>>? expression = null);


        #endregion


        #region 新增


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task InsertAsync(TEntity entity);

        /// <summary>
        /// 批量添加
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
        void UpdateAsync(TEntity entity);


        //TODO 批量更新，根据条件更新

        /// <summary>
        /// 根据条件更新
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        //Task<int> BatchUpdateAsync(Expression<Func<TEntity,bool>> expression);

        #endregion


        #region 删除


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        void DeleteAsync(TEntity entity);

        //TODO 批量删除

        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(Expression<Func<TEntity,bool>> expression);

        #endregion


    }
}
