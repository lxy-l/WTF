using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Config;

/// <summary>
/// 数据上下文配置
/// </summary>
public static class DbContextConfig
{
    public static void AddDbContextConfig(this IServiceCollection Services,IConfiguration Configuration)
    {
        #region 数据库配置
        Services.AddDbContextPool<DbContext, UserDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("UserSqlServer"))
        );

        //builder.Services.AddDbContextPool<ApplicationDbContext>(options =>
        //    options.UseSqlServer(authSqlServerConnectionString)
        //);

        Services.AddDatabaseDeveloperPageExceptionFilter();
        #endregion
    }
}
