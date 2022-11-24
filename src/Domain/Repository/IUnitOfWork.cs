namespace Domain.Repository;

/// <summary>
/// 工作单元接口
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// 提交更改
    /// </summary>
    /// <param name="Enable">是否开启批量提交</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> CommitAsync(CancellationToken cancellationToken = default, bool Enable = false);
}