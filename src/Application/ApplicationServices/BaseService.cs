using System.Linq.Dynamic.Core;

using Application.DTO;

using Domain.AggregateRoots;
using Domain.Entities;
using Domain.Repository;

namespace Application.ApplicationServices;

public class BaseService<TEntity, TKey>
    : IBaseService<TEntity, TKey> where TEntity : Entity<TKey>, IAggregateRoot
{
    protected readonly IUnitOfWork UnitOfWork;
    protected readonly IRepositoryAsync<TEntity, TKey> BaseRep;

    public BaseService(IRepositoryAsync<TEntity, TKey> userRep, IUnitOfWork unitOfWork)
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
        BaseRep.Delete(model);
        await UnitOfWork.CommitAsync();
        return model;
    }

    public async Task<TEntity> EditEntity(TEntity model)
    {
        BaseRep.Update(model);
        await UnitOfWork.CommitAsync();
        return model;
    }

    public async Task<TEntity?> GetModelById(TKey id)
    {
        return await BaseRep.FindByIdAsync(id);
    }

    public async Task<PagedResult<TEntity>> GetPagedResult(SearchParams searchParams)
    {
        return await BaseRep.GetPagedResultAsync(page: searchParams.Page, pageSize: searchParams.PageSize);
    }
}