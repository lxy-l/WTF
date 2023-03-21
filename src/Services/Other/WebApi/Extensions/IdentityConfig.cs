using Infrastructure.Context;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Extensions;

/// <summary>
/// 授权认证配置
/// </summary>
public static class IdentityConfig
{
    public static void AddIdentityConfig(this IServiceCollection Services, IConfiguration Configuration)
    {
        //Services.AddIdentity<IdentityUser, IdentityRole>()
        //    .AddEntityFrameworkStores<ApplicationDbContext>()
        //    .AddDefaultTokenProviders();

        //Services.Configure<IdentityOptions>(options =>
        //{
        //    options.Password.RequiredLength = 6;
        //    options.Password.RequiredUniqueChars = 3;
        //    options.Password.RequireNonAlphanumeric = false;
        //    options.Password.RequireLowercase = false;
        //    options.Password.RequireUppercase = false;
        //});
        //Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //    .AddJwtBearer(options =>
        //    {
        //        options.TokenValidationParameters = new TokenValidationParameters
        //        {
        //            ValidateIssuer = true,
        //            ValidIssuer = Configuration["JwtSettings:Issuer"],
        //            ValidateAudience = true,
        //            ValidAudience = Configuration["JwtSettings:Audience"],
        //            ValidateIssuerSigningKey = true,
        //            IssuerSigningKey = new SymmetricSecurityKey(
        //                Encoding.UTF8.GetBytes(
        //                    Configuration["JwtSettings:Secret"] ??
        //                    throw new Exception("未配置密钥"))),
        //            ValidateLifetime = true,
        //            ClockSkew = TimeSpan.Zero
        //        };
        //    });

        Services.AddAuthorization()
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = "https://localhost:7296";
                options.RequireHttpsMetadata = true;
                options.Audience = "WebApi";
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true
                };
            });
    }
}
