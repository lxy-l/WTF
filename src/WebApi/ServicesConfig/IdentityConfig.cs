using Infrastructure.Context;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.ServicesConfig;

/// <summary>
/// 授权认证配置
/// </summary>
public static class IdentityConfig
{
    public static void AddIdentityConfig(this IServiceCollection Services, IConfiguration Configuration)
    {
        #region 认证配置

        Services.AddOpenIddict()
            .AddCore(options =>
            {
                options.UseEntityFrameworkCore()
                       .UseDbContext<ApplicationDbContext>();
            }).AddServer(options =>
            {
                options.SetAuthorizationEndpointUris("/connect/authorize", "/connect/authorize/callback")
                .SetDeviceEndpointUris("/device")
                .SetIntrospectionEndpointUris("/connect/introspect")
                .SetLogoutEndpointUris("/connect/logout")
                .SetRevocationEndpointUris("/connect/revocat")
                .SetTokenEndpointUris("/connect/token")
                .SetUserinfoEndpointUris("/connect/userinfo")
                .SetVerificationEndpointUris("/connect/verify");
                //options.AllowAuthorizationCodeFlow();
                options.AllowAuthorizationCodeFlow()
                .AllowHybridFlow()
                .AllowImplicitFlow()
                .AllowPasswordFlow()
                .AllowClientCredentialsFlow()
                .AllowRefreshTokenFlow()
                .AllowDeviceCodeFlow()
                .AllowNoneFlow();
                options.RegisterScopes("openid", "email", "profile", "phone", "roles", "address", "offline_access");
                options.AddEncryptionKey(new SymmetricSecurityKey(
                    Convert.FromBase64String("DRjd/GnduI3Efzen9V9BvbNUfc/VKgXltV7Kbk9sMkY=")));
                options.AddDevelopmentSigningCertificate();
                options.UseAspNetCore()
                    .EnableAuthorizationEndpointPassthrough()
                    .EnableTokenEndpointPassthrough()
                    .EnableUserinfoEndpointPassthrough()
                    .EnableLogoutEndpointPassthrough()
                    .EnableVerificationEndpointPassthrough()
                    .EnableStatusCodePagesIntegration()
                    .DisableTransportSecurityRequirement();
            }).AddValidation(options =>
            {
                options.AddAudiences("Roy");
                options.UseLocalServer();
                options.UseAspNetCore();
            });
        Services.AddAuthorization();

        //微软自带Jwt认证
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
        #endregion
    }
}
