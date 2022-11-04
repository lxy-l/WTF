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


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, 
    options => builder.Configuration.Bind("JwtSettings", options));

builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();

builder.Services.AddTransient(typeof(IRepositoryAsync<,>), typeof(RepositoryAsync<,>));

// TODO: 后续改为程序集注入
builder.Services.AddTransient<IUserService,UserService>();



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

app.UseHealthChecks("/hc",new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{ 
    Predicate=_=>true,
    ResponseWriter =WriteResponse 
});

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

Task WriteResponse(HttpContext context, HealthReport result)
{
    context.Response.ContentType = "application/json; charset=utf-8";

    var options = new JsonWriterOptions
    {
        Indented = true
    };

    using (var stream = new MemoryStream())
    {
        using (var writer = new Utf8JsonWriter(stream, options))
        {
            writer.WriteStartObject();
            writer.WriteString("status", result.Status.ToString());
            writer.WriteStartObject("results");
            foreach (var entry in result.Entries)
            {
                writer.WriteStartObject(entry.Key);
                writer.WriteString("status", entry.Value.Status.ToString());
                writer.WriteString("description", entry.Value.Description);
                writer.WriteStartObject("data");
                foreach (var item in entry.Value.Data)
                {
                    writer.WritePropertyName(item.Key);
                    JsonSerializer.Serialize(
                        writer, item.Value, item.Value?.GetType() ??
                        typeof(object));
                }
                writer.WriteEndObject();
                writer.WriteEndObject();
            }
            writer.WriteEndObject();
            writer.WriteEndObject();
        }

        var json = Encoding.UTF8.GetString(stream.ToArray());

        return context.Response.WriteAsync(json);
    }
}
