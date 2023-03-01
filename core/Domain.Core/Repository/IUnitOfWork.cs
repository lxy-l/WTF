using System;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Core.Repository;

/// <summary>
/// 工作单元接口
/// </summary>
public interface IUnitOfWork:IDisposable
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
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task BulkCommitAsync(CancellationToken cancellationToken = default);
}