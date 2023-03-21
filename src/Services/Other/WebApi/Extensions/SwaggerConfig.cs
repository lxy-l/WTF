using Crafty.WebApi.Core.Extensions;

using Microsoft.OpenApi.Models;

namespace WebApi.Extensions;

/// <summary>
/// Swagger配置
/// </summary>
public static class SwaggerConfig
{
    /// <summary>
    /// 添加Swagger配置
    /// </summary>
    /// <param name="Services"></param>
    public static void AddSwaggerService(this IServiceCollection Services)
    {
        var basePath = AppDomain.CurrentDomain.BaseDirectory;
        Services.AddSwaggerConfig();
        Services.AddSwaggerGen(config =>
        {
            //#region XML文档
            //string[] xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml");
            //foreach (var xmlFile in xmlFiles)
            //{
            //    config.IncludeXmlComments(xmlFile, true);
            //}
            //config.OperationFilter<AddResponseHeadersFilter>();
            //config.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
            //config.OperationFilter<SecurityRequirementsOperationFilter>();
            //#endregion

            #region Swagger授权认证
            config.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Description = "JWT Bearer授权",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });
            #endregion
        });
    }
}
