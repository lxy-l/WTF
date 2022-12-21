using AuthService.Extensions;
using AuthService.Seed;

using HealthChecks.UI.Client;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;

using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

//DbContextÅäÖÃ
builder.Services.AddDbContextConfig(builder.Configuration);
//½¡¿µ¼ì²éÅäÖÃ
builder.Services.AddHealthCheckConfig(builder.Configuration);
//LogÅäÖÃ
builder.Services.AddLogConfig(builder.Configuration);
//ConsulÅäÖÃ
builder.Services.AddConsulConfig(builder.Configuration);
//IdentityServerÅäÖÃ
builder.Services.AddIdentityServerConfig(builder.Configuration);

builder.Services.AddControllersWithViews();
//builder.Services.AddRazorPages();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.EnsureSeedData().ConfigureAwait(false);
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

app.Run();
