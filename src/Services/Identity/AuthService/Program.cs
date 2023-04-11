using AuthService.Extensions;
using AuthService.Seed;

using HealthChecks.UI.Client;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;

using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

//DbContext配置
builder.Services.AddDbContextConfig(builder.Configuration);
//健康检查配置
builder.Services.AddHealthCheckConfig(builder.Configuration);
//Log配置
builder.Services.AddLogConfig(builder.Configuration);
//Consul配置
builder.Services.AddConsulConfig(builder.Configuration);
//IdentityServer配置
builder.Services.AddIdentityServerConfig(builder.Configuration);
builder.Services.AddControllersWithViews();
//builder.Services.AddRazorPages();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    await app.EnsureSeedData();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseHealthChecks("/hc", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseRouting();

app.UseIdentityServer();
app.UseAuthorization();

app.MapDefaultControllerRoute();
app.MapRazorPages();

await app.RunAsync();
