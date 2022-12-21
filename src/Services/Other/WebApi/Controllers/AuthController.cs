using Application.Core.ApplicationServices;
using Application.Core.DTO;
using Application.DTO;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

/// <summary>
/// 认证接口
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IJwtTokenService _tokenService;

    public AuthController(
        IJwtTokenService tokenService)
    {
        _tokenService = tokenService;
    }

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("Login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Login(LoginModel model)
    {
        if (ModelState.IsValid)
        {
            JwtTokenViewModel token = await _tokenService.CreateJwtTokenAsync("1", model.Username);
            return Ok(token);
            //return Unauthorized(result);
        }
        return BadRequest();
    }

    /// <summary>
    /// 注册
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("Register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Register(IdentityUser model)
    {
        if (ModelState.IsValid)
        {
            await Task.Delay(1000);
            return Ok(model);
        }
        return BadRequest();
    }
}