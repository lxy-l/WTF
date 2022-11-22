using Application.ApplicationServices;

using Domain.Repository;

using HealthChecks.UI.Client;

using Infrastructure.Context;
using Infrastructure.Repository;
using Infrastructure.Service;
using Infrastructure.UnitOfWork;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

//using NetCore.AutoRegisterDi;

using Scrutor;

using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

var basePath = AppDomain.CurrentDomain.BaseDirectory;
var userSqlServerConnectionString = builder.Configuration.GetConnectionString("UserSqlServer");
var authSqlServerConnectionString = builder.Configuration.GetConnectionString("AuthSqlServer");


#region 健康检查配置
#pragma warning disable CS8604 // 引用类型参数可能为 null。
builder.Services.AddHealthChecks()
    .AddSqlServer(userSqlServerConnectionString, name: "UserSqlServer")
    .AddSqlServer(authSqlServerConnectionString, name: "AuthSqlServer")
    .AddTcpHealthCheck((x) => { x.AddHost("localhost", 5341); }, "Seq");
#pragma warning restore CS8604 // 引用类型参数可能为 null。
#endregion

#region 数据库配置
builder.Services.AddDbContextPool<DbContext, UserDbContext>(options =>
    options.UseSqlServer(userSqlServerConnectionString)
);

builder.Services.AddDbContextPool<ApplicationDbContext>(options =>
    options.UseSqlServer(authSqlServerConnectionString)
);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
#endregion

#region 日志配置
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddSeq(builder.Configuration.GetSection("Seq"));
});
#endregion

#region 认证配置
//builder.Services.AddIdentity<IdentityUser, IdentityRole>()
//    .AddEntityFrameworkStores<ApplicationDbContext>()
//    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 3;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
    options => builder.Configuration.Bind("JwtSettings", options));
#endregion

#region Cors跨域配置
builder.Services.AddCors(options =>
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

#region 服务配置

//TODO 可以考虑改用AutoFac注入

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddTransient(typeof(IRepositoryAsync<,>), typeof(RepositoryAsync<,>));

builder.Services.AddTransient(typeof(IBaseService<,>), typeof(BaseService<,>));
builder.Services.AddSingleton<IJwtTokenService, JwtTokenService>();
builder.Services.Scan(scan => scan
    .FromAssemblyOf<Application.Register>()
    .AddClasses(classes => classes.Where(c => c.Name.EndsWith("Service")))
    .UsingRegistrationStrategy(RegistrationStrategy.Throw)
    .AsImplementedInterfaces()
    .WithTransientLifetime());
//builder.Services
//    .RegisterAssemblyPublicNonGenericClasses(Assembly.GetAssembly(typeof(Application.Register)))
//    .Where(x => x.Name.EndsWith("Service"))
//    .AsPublicImplementedInterfaces();
#endregion

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(config =>
{
    #region XML文档
    config.OrderActionsBy(o => o.RelativePath);
    var xmlPath = Path.Combine(basePath, "WebApi.xml");
    config.IncludeXmlComments(xmlPath, true);
    var xmlModelPath = Path.Combine(basePath, "Domain.xml");
    config.IncludeXmlComments(xmlModelPath);
    var xmlApplicationPath = Path.Combine(basePath, "Application.xml");
    config.IncludeXmlComments(xmlApplicationPath);
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

builder.Services.AddApplicationInsightsTelemetry();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    //app.UseMigrationsEndPoint();
    //app.UseStatusCodePagesWithReExecute("/Log/{0}");
    //app.UseExceptionHandler("/Error");
}
else if (app.Environment.IsStaging())
{

}
else if (app.Environment.IsProduction())
{
    app.UseStatusCodePages();
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
