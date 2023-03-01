using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Domain.Core.Models;

namespace Domain.Core.Repository;

/*
 * where TEntity : IEntity<TKey> 应该约束为AggregateRoot
 * https://learn.microsoft.com/zh-cn/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-design#define-one-repository-per-aggregate
 * 对于每个聚合或聚合根，应创建一个存储库类。 你也许能够利用 C# 泛型来减少需要维护的具体类的总数（如本章后面所示）。 在基于域驱动设计 (DDD) 模式的微服务中，唯一应该用于更新数据库的渠道应是存储库。 这是因为它们与聚合根具有一对一的关系，聚合根控制着聚合的不变量和事务一致性。 可以通过其他渠道查询数据库（就像使用 CQRS 方法时一样），因为查询不会更改数据库的状态。 但是，事务区域（即更新）必须始终由存储库和聚合根控制。
 * 
 * 1.使用聚合(Aggregate)和聚合根(AggregateRoot)的概念，将相关的实体和值对象组织在一起，形成一个一致性边界。一次事务中，最多只能更改一个聚合的状态。若一次业务操作涉及多个聚合状态的更改，应采用领域事件异步修改相关的聚合，实现聚合间的解耦1。
 * 2.使用工作单元(Unit of Work)模式，将一个或多个仓储操作封装在一个工作单元对象中，由该对象负责跟踪所有需要持久化的变化，并在提交时执行相应的数据库操作。工作单元可以使用事务管理器(Transaction Manager)来协调不同数据源之间的事务2。
 * 3.使用分布式事务(Distributed Transaction)机制，如两阶段提交(2PC)，三阶段提交(3PC)，补偿事务(Compensating Transaction)，基于消息队列(MQ)或事件溯源(Event Sourcing)等方式，在不同数据源之间保证原子性、一致性、隔离性和持久性(ACID)。但这些机制通常会带来额外的复杂度、性能开销和可用性问题3。
 */
/// <summary>
/// 基础仓储接口
/// </summary>
public interface IRepositoryAsync<TEntity, in TKey>
    where TEntity : IEntity<TKey>
    where TKey : struct
{
    //TODO 考虑优化工作单元实现方法
    //IUnitOfWork UnitOfWork { get; }

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
    /// 查询列表2（包含子表查询）
    /// </summary>
    /// <param name="includeProperties">子表</param>
    /// <param name="expression">筛选条件</param>
    /// <param name="orderBy">排序</param>
    /// <param name="ignoreQueryFilters">是否禁用筛选器</param>
    /// <returns></returns>
    IQueryable<TEntity> GetQueryInclude(
        string includeProperties,
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