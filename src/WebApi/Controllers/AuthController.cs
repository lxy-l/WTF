using Application.ApplicationServices;
using Application.DTO;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    /// <summary>
    /// 认证接口
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJwtTokenService _tokenService;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthController(
            IJwtTokenService tokenService, 
            SignInManager<IdentityUser> signInManager, 
            UserManager<IdentityUser> userManager)
        {
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userManager = userManager;
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
                //TODO 妈的搞不成换组件
                //var result= await _signInManager
                //    .PasswordSignInAsync(model.Username,model.Password,true,true);
                //if (result.Succeeded)
                //{

                //}
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
                var result= await _userManager.CreateAsync(model);
                return Ok(result);
            }
            return BadRequest();
        }
    }
}
