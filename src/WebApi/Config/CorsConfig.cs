namespace WebApi.Config
{
    /// <summary>
    /// 跨域配置
    /// </summary>
    public static class CorsConfig
    {
        public static void AddCorsConfig(this IServiceCollection Services)
        {
            #region Cors跨域配置
            Services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    policyBuilder =>
                    {
                        policyBuilder
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .WithMethods("GET", "POST", "PUT", "DELETE");
                    });
            });
            #endregion
        }
    }
}
