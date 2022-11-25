using System.Linq.Dynamic.Core;

using Application.DTO;

using Domain.AggregateRoots;
using Domain.Repository;

using Infrastructure.Linq;
using Infrastructure.Repository;

namespace Application.ApplicationServices;

public class BaseService<TEntity, TKey> : BaseInclude, IBaseService<TEntity, TKey> 
    where TEntity : AggregateRoot<TKey>
    where TKey : struct
{
    protected readonly IUnitOfWork UnitOfWork;
    protected readonly IEFCoreRepositoryAsync<TEntity, TKey> BaseRep;

    public override List<string>? Table 
    { 
        get 
        {
            return new List<string> { "UserInfo"};
        } 
    }

    public BaseService(IEFCoreRepositoryAsync<TEntity, TKey> userRep, IUnitOfWork unitOfWork)
    {
        BaseRep = userRep;
        UnitOfWork = unitOfWork;
    }

    public async Task<TEntity> AddEntity(TEntity model)
    {
        await BaseRep.InsertAsync(model).ConfigureAwait(false);
        await UnitOfWork.CommitAsync().ConfigureAwait(false);
        return model;
    }

    public async Task<TEntity> DeleteEntity(TKey id)
    {
        TEntity? model = await BaseRep.FindByIdAsync(id).ConfigureAwait(false);
        if (model is null)
        {
            throw new Exception("未找到实体信息！");
        }
        await BaseRep.DeleteAsync(model).ConfigureAwait(false);
        await UnitOfWork.CommitAsync().ConfigureAwait(false);
        return model;
    }

    public async Task<TEntity> EditEntity(TEntity model)
    {
        if (default(TKey).Equals(model.Id))
        {
            throw new Exception("主键错误！");
        }
        bool IsExist= await BaseRep
            .AnyAsync(x=>x.Id.Equals(model.Id))
            .ConfigureAwait(false);
        if (!IsExist)
        {
            throw new Exception("未找到实体信息！");
        }
        await BaseRep.UpdateAsync(model).ConfigureAwait(false);
        await UnitOfWork.CommitAsync().ConfigureAwait(false);
        return model;
    }

    public async Task<TEntity?> GetModelById(TKey id)
    {
        return await BaseRep.FindByIdAsync(id).ConfigureAwait(false);
    }

    public async Task<PagedResult<TEntity>> GetPagedResult(SearchParams searchParams)
    {

        /*
         * 动态查询，根据SearchParams参数进行筛选排序
         */

        var query = await BaseRep.GetQueryableAsync().ConfigureAwait(false);

        if (Table?.Any()??false)
        {
            //TODO 多表联查
            //query.IncludeIf
        }

        if (!string.IsNullOrWhiteSpace(searchParams.Filters))
        {
            var lambda = LinqQuery.BuildFilterLambda<TEntity>(searchParams.Filters);
            query = query.Where(lambda);
        }
        if (!string.IsNullOrWhiteSpace(searchParams.Sort))
        {
            query = query.OrderBy(searchParams.Sort);
        }

        return query.PageResult(searchParams.Page, searchParams.PageSize);
    }
}