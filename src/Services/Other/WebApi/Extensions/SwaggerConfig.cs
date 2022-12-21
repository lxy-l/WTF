using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace WebApi.Extensions;

/// <summary>
/// Swagger配置
/// </summary>
public static class SwaggerConfig
{
    public static void AddSwaggerConfig(this IServiceCollection Services)
    {
        var basePath = AppDomain.CurrentDomain.BaseDirectory;
        Services.AddSwaggerGen(config =>
        {
            #region XML文档
            config.OrderActionsBy(o => o.RelativePath);
            var xmlPath = Path.Combine(basePath, "WebApi.xml");
            config.IncludeXmlComments(xmlPath, true);
            var xmlPath1 = Path.Combine(basePath, "WebApi.Core.xml");
            config.IncludeXmlComments(xmlPath1, true);
            var xmlModelPath = Path.Combine(basePath, "Domain.xml");
            config.IncludeXmlComments(xmlModelPath);
            var xmlModelPath1 = Path.Combine(basePath, "Domain.Core.xml");
            config.IncludeXmlComments(xmlModelPath1);
            var xmlApplicationPath = Path.Combine(basePath, "Application.xml");
            config.IncludeXmlComments(xmlApplicationPath);
            var xmlApplicationPath1 = Path.Combine(basePath, "Application.Core.xml");
            config.IncludeXmlComments(xmlApplicationPath1);
            config.OperationFilter<AddResponseHeadersFilter>();
            config.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
            config.OperationFilter<SecurityRequirementsOperationFilter>();
            #endregion

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
