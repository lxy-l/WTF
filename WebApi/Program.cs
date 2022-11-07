using System.Text.Json;
using System.Text;

using Application.ApplicationServices;

using Domain.Repository;

using Infrastructure.Context;
using Infrastructure.Repository;
using Infrastructure.UnitOfWork;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using NetCore.AutoRegisterDi;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


var userSqlServerConnectionString = builder.Configuration.GetConnectionString("UserSqlServer");
var authSqlServerConnectionString = builder.Configuration.GetConnectionString("AuthSqlServer");

builder.Services.AddHealthChecks()
    .AddSqlServer(userSqlServerConnectionString,name:"UserSqlServer")
    .AddSqlServer(authSqlServerConnectionString,name:"AuthSqlServer")
    .AddDbContextCheck<DbContext>("DbContext")
    //.AddRedis("")
    ;

builder.Services.AddDbContextPool<DbContext,UserDbContext>(options =>
    options.UseSqlServer(userSqlServerConnectionString)
);

builder.Services.AddDbContextPool<ApplicationDbContext>(options =>
    options.UseSqlServer(authSqlServerConnectionString)
);


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

builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();

builder.Services.AddTransient(typeof(IRepositoryAsync<,>), typeof(RepositoryAsync<,>));
builder.Services.AddTransient(typeof(IUserRepositoryAsync<,,>), typeof(UserRepositoryAsync<,,>));

builder.Services
    .RegisterAssemblyPublicNonGenericClasses(Assembly.GetAssembly(typeof(Application.Register)))
    .Where(x => x.Name.EndsWith("Service"))
    .AsPublicImplementedInterfaces();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationInsightsTelemetry();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    //app.UseMigrationsEndPoint();
    app.UseStatusCodePagesWithReExecute("/Log/{0}");
}
else if (app.Environment.IsStaging())
{

}
else if(app.Environment.IsProduction())
{

}

app.UseHealthChecks("/hc",new HealthCheckOptions
{ 
    Predicate=_=>true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecks("/liveness", new HealthCheckOptions
{
    Predicate = r => r.Name.Contains("self")
});

app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
