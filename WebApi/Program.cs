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


#region �����������
builder.Services.AddHealthChecks()
    .AddSqlServer(userSqlServerConnectionString,name:"UserSqlServer")
    .AddSqlServer(authSqlServerConnectionString,name:"AuthSqlServer")
    .AddTcpHealthCheck((x) => { x.AddHost("localhost", 5341); },"Seq");
#endregion

#region ���ݿ�����
builder.Services.AddDbContextPool<DbContext, UserDbContext>(options =>
    options.UseSqlServer(userSqlServerConnectionString)
);

builder.Services.AddDbContextPool<ApplicationDbContext>(options =>
    options.UseSqlServer(authSqlServerConnectionString)
);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
#endregion

#region ��־����
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddSeq(builder.Configuration.GetSection("Seq"));
});
#endregion

#region ��֤����
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

#region Cors��������
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

#region ��������
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
    #region XML�ĵ�
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

    #region Swagger��Ȩ��֤
    config.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "JWT��Ȩ(���ݽ�������ͷ�н��д���) ֱ�����¿�������Bearer {token}��ע������֮����һ���ո�\"",
        Name = "Authorization",//jwtĬ�ϵĲ�������
        In = ParameterLocation.Header,//jwtĬ�ϴ��Authorization��Ϣ��λ��(����ͷ��)
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
