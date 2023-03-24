using System.Text.Json.Serialization;

using Application.ApplicationServices;

using Crafty.Application.Core.DependencyInjection;
using Crafty.Infrastructure.EFCore.DependencyInjection;
using Crafty.WebApi.Core.DependencyInjection;

using HealthChecks.UI.Client;

using Infrastructure.Context;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;

using WebApi.Extend;
using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

string sqlserverConn = builder.Configuration.GetConnectionString("UserSqlServer") ?? throw new ArgumentNullException();

//数据库配置
builder.Services.AddDbContextPool<DbContext, UserDbContext>(options =>
    options.UseSqlServer(sqlserverConn)
).AddUnitOfWork<UserDbContext>();

//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//健康检查配置
builder.Services.AddHealthChecks().AddSqlServer(sqlserverConn, name: "UserSqlServer");
//Log配置
builder.Services.AddSeqLogConfig(builder.Configuration);
//授权认证配置
builder.Services.AddIdentityConfig(builder.Configuration);
//跨域配置
builder.Services.AddCorsConfig();
//Swagger文档配置
builder.Services.AddSwaggerConfig();
//Consul配置
builder.Services.AddConsulConfig(builder.Configuration);
//仓储配置
builder.Services.AddEfCoreRepository();
//基础服务配置
builder.Services.AddBaseServices();
//其他服务
//builder.Services.AddOtherServices(new Type[]{ typeof(Application.Register) });
builder.Services.AddTransient<IUserService,UserService >();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateTimeOffsetJsonConverter());
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApplicationInsightsTelemetry();

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
    app.UseExceptionHandler("/Error");
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
app.UseAuthorization();

app.MapControllers();

app.Run();
