using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Application.DTO;

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.ApplicationServices
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ValueTask<JwtTokenViewModel> CreateJwtTokenAsync(List<Claim> claims)
        {
            string? secretKey = _configuration["JwtSettings:Secret"] ??
                    throw new Exception("未配置JWT密钥！");

            var creds = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                SecurityAlgorithms.HmacSha256);
            double expireSeconds = double.Parse(_configuration["JwtSettings:ExpireSeconds"] ?? "3600");

            var token = new JwtSecurityToken
            (
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddSeconds(expireSeconds),
                signingCredentials: creds
            );
            var tokenModel = new JwtTokenViewModel
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                Expires = expireSeconds,
                TokenType = "Bearer"
            };
            return ValueTask.FromResult(tokenModel);
        }

        public ValueTask<JwtTokenViewModel> CreateJwtTokenAsync(string id, string name)
        {
            List<Claim> claims=new List<Claim> {
                    new Claim(ClaimTypes.Name,name),
                    new Claim(JwtRegisteredClaimNames.Jti, id),
                    new Claim(ClaimTypes.SerialNumber, id)
            };
            string ? secretKey = _configuration["JwtSettings:Secret"] ??
                throw new Exception("未配置JWT密钥！");
            var creds = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                SecurityAlgorithms.HmacSha256);
            double expireSeconds = double.Parse(_configuration["JwtSettings:ExpireSeconds"] ?? "3600");

            var token = new JwtSecurityToken
            (
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddSeconds(expireSeconds),
                signingCredentials: creds
            );
            var tokenModel = new JwtTokenViewModel
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                Expires = expireSeconds,
                TokenType = "Bearer"
            };
            return ValueTask.FromResult(tokenModel);
        }

        public ValueTask<string?> GetUserIdAsync(ClaimsPrincipal user)
        {
            var id = user.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.SerialNumber)?.Value;
            return ValueTask.FromResult(id);
        }

        public ValueTask<JwtSecurityToken> SerializeJwtAsync(string jwtStr)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(jwtStr);
            return ValueTask.FromResult(jwtToken);
        }
    }
}
