namespace AuthService.Extensions;

/// <summary>
/// 日志配置
/// </summary>
public static class LogConfig
{
    public static void AddLogConfig(this IServiceCollection Services, IConfiguration Configuration)
    {
        #region 日志配置
        Services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddSeq(Configuration.GetSection("Seq"));
        });
        #endregion
    }
}
