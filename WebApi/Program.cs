using System.Reflection;

using Domain.Repository;

using HealthChecks.UI.Client;

using Infrastructure.Context;
using Infrastructure.Repository;
using Infrastructure.UnitOfWork;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using NetCore.AutoRegisterDi;

using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

var basePath = AppDomain.CurrentDomain.BaseDirectory;
var userSqlServerConnectionString = builder.Configuration.GetConnectionString("UserSqlServer");
var authSqlServerConnectionString = builder.Configuration.GetConnectionString("AuthSqlServer");


#region 健康检查配置
builder.Services.AddHealthChecks()
    .AddSqlServer(userSqlServerConnectionString,name:"UserSqlServer")
    .AddSqlServer(authSqlServerConnectionString,name:"AuthSqlServer")
    .AddTcpHealthCheck((x) => { x.AddHost("localhost", 5341); },"Seq");
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
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

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
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .WithMethods("GET", "POST", "PUT", "DELETE");
        });
});
#endregion

#region 服务配置
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddTransient(typeof(IRepositoryAsync<,>), typeof(RepositoryAsync<,>));
builder.Services.AddTransient(typeof(IUserRepositoryAsync<,,>), typeof(UserRepositoryAsync<,,>));

builder.Services
    .RegisterAssemblyPublicNonGenericClasses(Assembly.GetAssembly(typeof(Application.Register)))
    .Where(x => x.Name.EndsWith("Service"))
    .AsPublicImplementedInterfaces();
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
        Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
        Name = "Authorization",//jwt默认的参数名称
        In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
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
else if(app.Environment.IsProduction())
{
    app.UseStatusCodePages();
}

app.UseHealthChecks("/hc",new HealthCheckOptions
{ 
    Predicate=_=>true,
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
