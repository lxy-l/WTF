namespace WebApi.Extensions;

/// <summary>
/// 日志配置
/// </summary>
public static class LogConfig
{
    /// <summary>
    /// 添加Seq
    /// </summary>
    /// <param name="Services"></param>
    /// <param name="Configuration"></param>
    public static void AddSeqLogConfig(this IServiceCollection Services, IConfiguration Configuration)
    {
        var config = Configuration.GetSection("Seq");
        if (config.GetChildren().Any())
        {
            Services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddSeq(config);
            });
        }
    }
}
