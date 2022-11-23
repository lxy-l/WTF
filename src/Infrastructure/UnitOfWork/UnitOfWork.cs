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

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task BulkCommitAsync(BulkConfig? bulkConfig = null)
    {
        await _dbContext.BulkSaveChangesAsync(bulkConfig);
    }

    public async Task RollBackAsync()
    {
        await _dbContext.Database.RollbackTransactionAsync();
    }
}