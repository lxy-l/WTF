using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Extensions;

/// <summary>
/// 数据上下文配置
/// </summary>
public static class DbContextConfig
{
    public static void AddDbContextConfig(this IServiceCollection Services,IConfiguration Configuration)
    {
        Services.AddDbContextPool<DbContext, UserDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("UserSqlServer"))
        );

        Services.AddDatabaseDeveloperPageExceptionFilter();
    }
}
