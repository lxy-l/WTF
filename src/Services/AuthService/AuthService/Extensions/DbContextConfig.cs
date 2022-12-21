using AuthService.Data;

using Microsoft.EntityFrameworkCore;

namespace WebApi.Extensions;

/// <summary>
/// 数据上下文配置
/// </summary>
public static class DbContextConfig
{
    public static void AddDbContextConfig(this IServiceCollection Services,IConfiguration Configuration)
    {
        #region 数据库配置

        Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));

        Services.AddDatabaseDeveloperPageExceptionFilter();
        #endregion
    }
}
