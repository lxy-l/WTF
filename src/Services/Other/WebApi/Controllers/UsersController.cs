using Application.ApplicationServices;

using Crafty.Application.Core.ApplicationServices;
using Crafty.WebApi.Core.BaseController;

using Domain.Entities;

using Infrastructure.Context;

namespace WebApi.Controllers;

/// <summary>
/// 用户管理接口
/// </summary>
public class UsersController : BaseApiController<UserDbContext, User, int>
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService) : base(userService)
    {
        _userService = userService;
    }
}