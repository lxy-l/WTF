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

        public async Task<int> Commit(CancellationToken cancellationToken = default)
        {
            throw new Exception("自定义异常");
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task BulkCommit()
        {
            await _dbContext.BulkSaveChangesAsync();
        }

        public async Task RollBack()
        {
            await _dbContext.Database.RollbackTransactionAsync();
        }
    }
}
