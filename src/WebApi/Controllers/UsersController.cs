using Application.ApplicationServices;
using Application.Core.DTO;

using Domain.Entities;

using Microsoft.AspNetCore.Mvc;

using WebApi.Core.BaseController;

namespace WebApi.Controllers;

/// <summary>
/// 用户管理接口
/// </summary>
public class UsersController : BaseApiController<User, int>
{
    public IUserService _userService { get; set; }
    public UsersController(IUserService userService) : base(userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// 获取用户（包含用户信息多表联查示例）
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    [HttpGet("GetUserInfo")]
    public async Task<IActionResult> GetUserInfo([FromQuery]SearchParams search)
    {
        var list=await _userService.GetUserAndInfo(search);
        return Ok(list);
    }

}