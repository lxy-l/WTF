using System.Linq.Dynamic.Core;

using Application.Core.DTO;
using Application.Core.MyException;

using Domain.Core.Models;
using Domain.Core.Repository;
using Infrastructure.Core.Repository.EFCore;

namespace Application.Core.ApplicationServices;

public class BaseService<TEntity, TKey> : BaseInclude, IBaseService<TEntity, TKey>
    where TEntity : AggregateRoot<TKey>
    where TKey : struct
{
    protected readonly IUnitOfWork UnitOfWork;
    protected readonly IEfCoreRepositoryAsync<TEntity, TKey> BaseRep;

    public BaseService(IEfCoreRepositoryAsync<TEntity, TKey> userRep, IUnitOfWork unitOfWork)
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
            throw new NotFoundException("未找到实体信息！");
        }
        await BaseRep.DeleteAsync(model).ConfigureAwait(false);
        await UnitOfWork.CommitAsync().ConfigureAwait(false);
        return model;
    }

    public async Task<TEntity> EditEntity(TEntity model)
    {
        if (default(TKey).Equals(model.Id))
        {
            throw new NotFoundException("主键错误！");
        }
        bool isExist = await BaseRep
            .AnyAsync(x => x.Id.Equals(model.Id))
            .ConfigureAwait(false);
        if (!isExist)
        {
            throw new NotFoundException("未找到实体信息！");
        }
        await BaseRep.UpdateAsync(model).ConfigureAwait(false);

        //TODO 多对多关系编辑的时候会报错（关系表执行insert报主键重复）
        await UnitOfWork.CommitAsync().ConfigureAwait(false);
        return model;
    }

    public async Task<TEntity?> GetModelById(TKey id)
    {
        var entity = await BaseRep.FindByIdAsync(id).ConfigureAwait(false);
        if (entity is null)
        {
            throw new NotFoundException("未找到实体信息！");
        }
        return entity;
    }

    public async Task<PagedResult<dynamic>> GetPagedResult(SearchParams searchParams)
    {
        /*
         * 动态查询，根据SearchParams参数进行筛选排序
         */
        if (!string.IsNullOrWhiteSpace(searchParams.Includes))
        {
            Table = searchParams.Includes.Split(',');
        }
        var query = await BaseRep
            .GetDynamicQueryAsync(searchParams.Filters,searchParams.Sort,Table)
            .ConfigureAwait(false);
        return query.PageResult<dynamic>(searchParams.Page, searchParams.PageSize);
    }
}