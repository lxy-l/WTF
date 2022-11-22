using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Service
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        /// <summary>
        /// 生成JwtToken
        /// </summary>
        /// <param name="claims">声明</param>
        /// <returns></returns>
        public JwtTokenViewModel GetJwtToken(List<Claim> claims)
        {
            string? secretKey = _configuration["JwtSettings:Secret"];
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new Exception("未配置JWT密钥！");
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            double ExpireSeconds = double.Parse(_configuration["JwtSettings:ExpireSeconds"]??
                throw new Exception("未配置token过期时间！"));
            var time = TimeSpan.FromSeconds(ExpireSeconds);
            var token = new JwtSecurityToken
            (
                issuer: _configuration["JwtSettings:ValidIssuer"],
                audience: _configuration["JwtSettings:ValidAudience"],
                claims: claims, 
                expires: DateTime.Now.Add(time),
                signingCredentials: creds
            );
            var access_token = new JwtSecurityTokenHandler().WriteToken(token);
            return new JwtTokenViewModel
            {
                AccessToken = access_token,
                Expires = time.TotalSeconds,
                TokenType = "Bearer"
            };

        }

        /// <summary>
        /// 获取用户id
        /// </summary>
        /// <param name="user">当前用户</param>
        /// <returns></returns>
        public string? GetUserId(ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.SerialNumber)?.Value;
        }

        /// <summary>
        /// 解析JwtToken
        /// </summary>
        /// <param name="jwtStr">token字符串</param>
        /// <returns></returns>
        public JwtSecurityToken SerializeJwt(string jwtStr)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(jwtStr);
            return jwtToken;
        }
    }
}
