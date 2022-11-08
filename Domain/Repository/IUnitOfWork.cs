using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EFCore.BulkExtensions;

namespace Domain.Repository
{
    /// <summary>
    /// 工作单元接口
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// 提交更改
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> CommitAsync(CancellationToken cancellationToken = default);


        /// <summary>
        /// 批量提交
        /// </summary>
        /// <returns></returns>
        Task BulkCommitAsync(BulkConfig? bulkConfig = null);


        /// <summary>
        /// 回滚更改
        /// </summary>
        Task RollBackAsync();
    }
}
