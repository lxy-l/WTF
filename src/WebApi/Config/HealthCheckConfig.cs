namespace WebApi.Config;

/// <summary>
/// 健康检测配置
/// </summary>
public static class HealthCheckConfig
{
    public static void AddHealthCheckConfig(this IServiceCollection Services,IConfiguration Configuration)
    {
        #region 健康检查配置
        Services.AddHealthChecks()
        .AddSqlServer(Configuration.GetConnectionString("UserSqlServer"), name: "UserSqlServer")
        //.AddSqlServer(authSqlServerConnectionString, name: "AuthSqlServer")
        .AddTcpHealthCheck((x) => { x.AddHost("localhost", 5341); }, "Seq");
        #endregion
    }

}
