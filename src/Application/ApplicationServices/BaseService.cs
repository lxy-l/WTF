using Application.DTO;

using Domain.AggregateRoots;
using Domain.Repository;

using Infrastructure.Linq;
using Infrastructure.Repository;

using Microsoft.EntityFrameworkCore;

using System.Linq.Dynamic.Core;

namespace Application.ApplicationServices;

public class BaseService<TEntity, TKey> : BaseInclude, IBaseService<TEntity, TKey> where TEntity : AggregateRoot<TKey>
{
    protected readonly IUnitOfWork UnitOfWork;
    protected readonly IEFCoreRepositoryAsync<TEntity, TKey> BaseRep;

    public override List<string>? Table 
    { 
        get 
        {
            return new List<string> { "1","2"};
        } 
    }

    public BaseService(IEFCoreRepositoryAsync<TEntity, TKey> userRep, IUnitOfWork unitOfWork)
    {
        BaseRep = userRep;
        UnitOfWork = unitOfWork;
    }

    public async Task<TEntity> AddEntity(TEntity model, CancellationToken cancellationToken = default)
    {
        await BaseRep.InsertAsync(model).ConfigureAwait(false);
        await UnitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);
        return model;
    }

    public async Task<TEntity> DeleteEntity(TKey id, CancellationToken cancellationToken = default)
    {
        TEntity? model = await BaseRep.FindByIdAsync(id).ConfigureAwait(false);
        if (model is null)
        {
            throw new Exception("未找到实体信息！");
        }
        await BaseRep.DeleteAsync(model).ConfigureAwait(false);
        await UnitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);
        return model;
    }

    public async Task<TEntity> EditEntity(TEntity model, CancellationToken cancellationToken = default)
    {
        await BaseRep.UpdateAsync(model).ConfigureAwait(false);
        await UnitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);
        return model;
    }

    public async Task<List<TEntity>> GetAll(CancellationToken cancellationToken = default)
    {
        return await (await BaseRep.GetQueryableAsync().ConfigureAwait(false)).ToListAsync();
    }

    public async Task<TEntity?> GetModelById(TKey id, CancellationToken cancellationToken = default)
    {
        return await BaseRep.FindByIdAsync(id).ConfigureAwait(false);
    }

    public async Task<PagedResult<TEntity>> GetPagedResult(SearchParams searchParams, CancellationToken cancellationToken = default)
    {

        /*
         * 动态查询，根据SearchParams参数进行筛选排序
         */

        var query = await BaseRep.GetQueryableAsync().ConfigureAwait(false);

        if (Table.Any())
        {
            //TODO 多表联查
            //query.IncludeIf
        }

        if (!string.IsNullOrWhiteSpace(searchParams.Filters))
        {
            var lambda = LinqQuery.BuildLambda<TEntity>(searchParams.Filters);
            query = query.Where(lambda);
        }
        if (!string.IsNullOrWhiteSpace(searchParams.Sort))
        {
            query = query.OrderBy(searchParams.Sort);
        }

        return query.PageResult(searchParams.Page, searchParams.PageSize);
    }
}