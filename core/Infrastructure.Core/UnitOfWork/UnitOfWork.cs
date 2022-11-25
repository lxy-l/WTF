using Domain.Core.Repository;

using EFCore.BulkExtensions;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Core.UnitOfWork;

/// <summary>
/// 工作单元实现
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _dbContext;

    public UnitOfWork(DbContext context)
    {
        _dbContext = context;
    }


    public async Task<int> CommitAsync(CancellationToken cancellationToken = default,bool Enable = false)
    {
        if (Enable)
        {
            /**
             * EFCore.BulkExtensions组件批量提交
             */
             await _dbContext.BulkSaveChangesAsync(cancellationToken :cancellationToken);
        }
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}