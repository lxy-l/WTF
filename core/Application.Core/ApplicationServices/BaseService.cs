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
    protected IUnitOfWork UnitOfWork { get; }
    protected IEfCoreRepositoryAsync<TEntity, TKey> BaseRep { get; }

    public BaseService(IEfCoreRepositoryAsync<TEntity, TKey> userRep, IUnitOfWork unitOfWork)
    {
        BaseRep = userRep;
        UnitOfWork = unitOfWork;
    }

    public async Task<TEntity> AddEntity(TEntity model, CancellationToken cancellationToken = default)
    {
        await BaseRep.InsertAsync(model,cancellationToken).ConfigureAwait(false);
        await UnitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);
        return model;
    }

    public async Task BulkAddEntity(List<TEntity> models, CancellationToken cancellationToken)
    {
        await BaseRep.BulkInsertAsync(models,cancellationToken:cancellationToken).ConfigureAwait(false);
        await UnitOfWork.BulkCommitAsync(cancellationToken);
    }

    public async Task<TEntity> DeleteEntity(TKey id, CancellationToken cancellationToken = default)
    {
        TEntity? model = await BaseRep.FindByIdAsync(id,cancellationToken).ConfigureAwait(false) ?? throw new NotFoundException("未找到实体信息！");
        BaseRep.Delete(model);
        await UnitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);
        return model;
    }

    public async Task<TEntity> EditEntity(TEntity model, CancellationToken cancellationToken = default)
    {
        if (default(TKey).Equals(model.Id))
        {
            throw new NotFoundException("主键错误！");
        }
        bool isExist = await BaseRep
            .AnyAsync(x => x.Id.Equals(model.Id),cancellationToken)
            .ConfigureAwait(false);
        if (!isExist)
        {
            throw new NotFoundException("未找到实体信息！");
        }
        BaseRep.Update(model);
        //TODO 多对多关系编辑的时候会报错（关系表执行insert报主键重复）
        await UnitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);
        return model;
    }

    public async Task<TEntity?> GetModelById(TKey id, CancellationToken cancellationToken = default)
    {
        var entity = await BaseRep.FindByIdAsync(id, cancellationToken).ConfigureAwait(false) ?? throw new NotFoundException("未找到实体信息！");
        return entity;
    }

    public PagedResult<dynamic> GetPagedResult(SearchParams searchParams)
    {
        /*
         * 动态查询，根据SearchParams参数进行筛选排序
         */
        if (!string.IsNullOrWhiteSpace(searchParams.Includes))
        {
            Table = searchParams.Includes.Split(',');
        }
        var query = BaseRep.GetDynamicQuery(searchParams.Filters, searchParams.Sort, Table);
        return query.PageResult<dynamic>(searchParams.Page, searchParams.PageSize);
    }
}