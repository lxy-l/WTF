using System.Text.Json.Serialization;

using HealthChecks.UI.Client;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;

using WebApi.Extend;
using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

//健康检查配置
builder.Services.AddHealthCheckConfig(builder.Configuration);
//DBContext配置
builder.Services.AddDbContextConfig(builder.Configuration);
//Log配置
builder.Services.AddLogConfig(builder.Configuration);
//授权认证配置
builder.Services.AddIdentityConfig(builder.Configuration);
//跨域配置
builder.Services.AddCorsConfig();
//基础服务注入
builder.Services.AddServicesConfig();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateTimeOffsetJsonConverter());
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApplicationInsightsTelemetry();

//Swagger文档配置
builder.Services.AddSwaggerConfig();
//Consul配置
builder.Services.AddConsulConfig(builder.Configuration);

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    //app.UseMigrationsEndPoint();
}
else if (app.Environment.IsStaging())
{

}
else if (app.Environment.IsProduction())
{
    //app.UseStatusCodePages();
    //app.UseExceptionHandler("/Error");
    //app.UseStatusCodePagesWithReExecute("/Error");
}

app.UseHealthChecks("/hc", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();

app.UseAuthentication();
//app.UseOpenIddictValidation();
app.UseAuthorization();

app.MapControllers();

app.Run();
