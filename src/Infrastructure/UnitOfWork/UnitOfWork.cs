using Domain.Repository;

using EFCore.BulkExtensions;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.UnitOfWork;

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


    public async Task<int> CommitAsync(bool Enable = false, CancellationToken cancellationToken = default)
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