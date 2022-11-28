using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using Application.Core.DTO;

namespace Application.Core.ApplicationServices
{
    /// <summary>
    /// JwtToken服务
    /// </summary>
    public interface IJwtTokenService
    {
        /// <summary>
        /// 生成JwtToken
        /// </summary>
        /// <remarks>需配置JwtSettings</remarks>
        /// <param name="claims">自定义声明</param>
        /// <returns></returns>
        ValueTask<JwtTokenViewModel> CreateJwtTokenAsync(List<Claim> claims);

        /// <summary>
        /// 生成JwtToken
        /// </summary>
        /// <remarks>简单的声明（包含用户标识和名称）</remarks>
        /// <param name="id">唯一标识</param>
        /// <param name="name">用户名</param>
        /// <returns></returns>
        ValueTask<JwtTokenViewModel> CreateJwtTokenAsync(string id,string name);

        /// <summary>
        /// 获取当前用户id
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        ValueTask<string?> GetUserIdAsync(ClaimsPrincipal user);

        /// <summary>
        /// 解析Token
        /// </summary>
        /// <param name="jwtStr"></param>
        /// <returns></returns>
        ValueTask<JwtSecurityToken> SerializeJwtAsync(string jwtStr);
    }
}
