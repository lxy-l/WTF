using Application.ApplicationServices;
using Application.Core.DTO;

using Domain.Entities;
using Domain.ValueObjects;

using Microsoft.AspNetCore.Mvc;

using WebApi.Core.BaseController;

namespace WebApi.Controllers;

/// <summary>
/// 用户管理接口
/// </summary>
public class UsersController : BaseApiController<User, int>
{
    private IUserService UserService { get; }
    public UsersController(IUserService userService) : base(userService)
    {
        UserService = userService;
    }

    /// <summary>
    /// 获取用户（包含用户信息多表联查示例）
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    [HttpGet("GetUserInfo")]
    public IActionResult GetUserInfo([FromQuery] SearchParams search)
    {
        var list = UserService.GetUserAndInfo(search);
        return Ok(list);
    }

    /// <summary>
    /// 批量添加
    /// </summary>
    /// <returns></returns>
    [HttpPost("BulkAddUser")]
    public async Task<IActionResult> BulkAddUserAsync()
    {
        List<User> users = new();
        long j = await UserService.Count();
        for (long i=j; i < j+5000000; i++)
        {
            users.Add(new User($"User{i}", $"password{i}", null, null, null, new Address("country" + i, "city" + i, "street" + i)));
        }
        await UserService.BulkAddEntity(users);
        return Ok();
    }

}