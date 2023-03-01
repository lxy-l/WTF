using Domain.Core.Repository;

using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Core.Context
{
    /// <summary>
    /// EfCore实现UnitofWork模式
    /// </summary>
    public class BaseDbContext : DbContext, IUnitOfWork
    {
        public BaseDbContext(DbContextOptions options) : base(options)
        {
        }

        protected BaseDbContext()
        {
        }

        public async Task BulkCommitAsync(CancellationToken cancellationToken = default)
        {
            await this.BulkSaveChangesAsync(cancellationToken: cancellationToken);
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            return await SaveChangesAsync(cancellationToken);
        }
    }
}
