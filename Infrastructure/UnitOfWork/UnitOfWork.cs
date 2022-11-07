using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Domain.Entities;
using Domain.Repository;

using EFCore.BulkExtensions;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.UnitOfWork
{
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
            //throw new Exception("自定义异常");
            return await _dbContext.SaveChangesAsync(true,cancellationToken);
        }

        public async Task BulkCommitAsync()
        {
            await _dbContext.BulkSaveChangesAsync(new BulkConfig { BatchSize=5000});
        }

        public async Task RollBackAsync()
        {
            await _dbContext.Database.RollbackTransactionAsync();
        }
    }
}
