using Crafty.Application.Core.ApplicationServices;

using Domain.Entities;

using Infrastructure.Context;

namespace Application.ApplicationServices;

/// <summary>
/// 自定义用户信息查询
/// </summary>
public interface IUserService:IBaseService<UserDbContext,User,int>
{
}
