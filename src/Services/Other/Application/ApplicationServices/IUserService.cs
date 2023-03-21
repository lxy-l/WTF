using Crafty.Application.Core.DTO;
using Crafty.Domain.Core.Models;

using Domain.Entities;

namespace Application.ApplicationServices;

/// <summary>
/// 自定义用户信息查询
/// </summary>
public interface IUserService
{
    /// <summary>
    /// 获取全部用户包含用户信息
    /// </summary>
    /// <returns></returns>
    Task<IPagedList<User>> GetUserAndInfo(SearchParams search, CancellationToken cancellationToken = default);
}
