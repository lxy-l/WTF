using System.Text;
using System.Text.Json.Serialization;

using Application.Core.ApplicationServices;

using Domain.Core.Repository;

using HealthChecks.UI.Client;

using Infrastructure.Context;
using Infrastructure.Core.Repository;
using Infrastructure.Core.UnitOfWork;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using Scrutor;

using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

var basePath = AppDomain.CurrentDomain.BaseDirectory;
var userSqlServerConnectionString = builder.Configuration.GetConnectionString("UserSqlServer");
//var authSqlServerConnectionString = builder.Configuration.GetConnectionString("AuthSqlServer");


#region 健康检查配置
builder.Services.AddHealthChecks()
    .AddSqlServer(userSqlServerConnectionString, name: "UserSqlServer")
    //.AddSqlServer(authSqlServerConnectionString, name: "AuthSqlServer")
    .AddTcpHealthCheck((x) => { x.AddHost("localhost", 5341); }, "Seq");
#endregion

#region 数据库配置
builder.Services.AddDbContextPool<DbContext, UserDbContext>(options =>
    options.UseSqlServer(userSqlServerConnectionString)
);

//builder.Services.AddDbContextPool<ApplicationDbContext>(options =>
//    options.UseSqlServer(authSqlServerConnectionString)
//);

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
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["JwtSettings:Audience"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(
                                builder.Configuration["JwtSettings:Secret"] ??
                                throw new Exception("未配置密钥"))),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });
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

builder.Services.AddTransient(typeof(IEFCoreRepositoryAsync<,>), typeof(EFCoreRepositoryAsync<,>));

builder.Services.AddTransient(typeof(IBaseService<,>), typeof(BaseService<,>));

builder.Services.Scan(scan => scan
    .FromAssembliesOf(typeof(Application.Register),typeof(Application.Core.Register))
    .AddClasses(classes => classes.Where(c => c.Name.EndsWith("Service")))
    .UsingRegistrationStrategy(RegistrationStrategy.Throw)
    .AsImplementedInterfaces()
    .WithTransientLifetime());
//builder.Services
//    .RegisterAssemblyPublicNonGenericClasses(Assembly.GetAssembly(typeof(Application.Register)))
//    .Where(x => x.Name.EndsWith("Service"))
//    .AsPublicImplementedInterfaces();
#endregion

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        //解决Json层级太深，循环依赖问题
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(config =>
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
