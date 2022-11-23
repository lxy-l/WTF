using System.Linq.Dynamic.Core;

using Application.DTO;

using Domain.AggregateRoots;
using Domain.Repository;

using Infrastructure.Repository;

using Microsoft.EntityFrameworkCore;

namespace Application.ApplicationServices;

public class BaseService<TEntity, TKey> where TEntity : AggregateRoot<TKey>
{
    protected readonly IUnitOfWork UnitOfWork;
    protected readonly IEFCoreRepositoryAsync<TEntity,TKey> BaseRep;

    public BaseService(IEFCoreRepositoryAsync<TEntity,TKey> userRep, IUnitOfWork unitOfWork)
    {
        BaseRep = userRep;
        UnitOfWork = unitOfWork;
    }

    public async Task<TEntity> AddEntity(TEntity model)
    {
        await BaseRep.InsertAsync(model);
        await UnitOfWork.CommitAsync();
        return model;
    }

    public async Task<TEntity> DeleteEntity(TKey id)
    {
        TEntity? model = await BaseRep.FindByIdAsync(id);
        if (model is null)
        {
            throw new Exception("未找到实体信息！");
        }
        await BaseRep.DeleteAsync(model);
        await UnitOfWork.CommitAsync();
        return model;
    }

    public async Task<TEntity> EditEntity(TEntity model)
    {
        await BaseRep.UpdateAsync(model);
        await UnitOfWork.CommitAsync();
        return model;
    }

    public async Task<List<TEntity>> GetAll()
    {
        return await (await BaseRep.GetQueryableAsync()).ToListAsync();
    }

    public async Task<TEntity?> GetModelById(TKey id)
    {
        return await BaseRep.FindByIdAsync(id);
    }

    public async Task<PagedResult<TEntity>> GetPagedResult(SearchParams searchParams)
    {
        //TODO PageResult后期需要自己封装
        return (await BaseRep.GetQueryAsync())
            .PageResult(searchParams.Page,searchParams.PageSize);
    }
}