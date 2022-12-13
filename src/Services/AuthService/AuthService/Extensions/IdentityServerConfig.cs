using AuthService.Data;

using IdentityServer7.EntityFramework;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Extensions
{
    /// <summary>
    /// ÈÏÖ¤ÅäÖÃ
    /// </summary>
    public static class IdentityServerConfig
    {
        public static void AddIdentityServerConfig(this IServiceCollection services,string connectionStr) 
        {
            services
                .AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // IdentityÅäÖÃ£ºhttps://learn.microsoft.com/zh-cn/aspnet/core/security/authentication/identity-configuration?view=aspnetcore-7.0
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;

                options.Lockout.DefaultLockoutTimeSpan= TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts= 5;
                options.Lockout.AllowedForNewUsers= true;

                options.SignIn.RequireConfirmedEmail=true;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            });

            //CookiesÅäÖÃ£ºhttps://learn.microsoft.com/zh-cn/dotnet/api/microsoft.aspnetcore.authentication.cookies.cookieauthenticationoptions?view=aspnetcore-7.0
            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                //options.Cookie.Name = "YourAppCookieName";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.LoginPath = "/Identity/Account/Login";
                // ReturnUrlParameter requires 
                //using Microsoft.AspNetCore.Authentication.Cookies;
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
            });



            //IdentityServerÅäÖÃ
            var migrationsAssembly = typeof(ApplicationDbContext).Assembly.GetName().Name;

            var builder = services
                .AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                    options.EmitStaticAudienceClaim = true;
                })
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(connectionStr,
                            sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(connectionStr, sql =>
                            sql.MigrationsAssembly(migrationsAssembly));
                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 3600;
                })
                .AddAspNetIdentity<IdentityUser>();

            builder.AddDeveloperSigningCredential();
        }
    }
}
