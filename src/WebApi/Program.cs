using System.Text.Json.Serialization;

using HealthChecks.UI.Client;

using Infrastructure.Context;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;

using OpenIddict.Abstractions;

using WebApi.Config;
using WebApi.Extend;

using static OpenIddict.Abstractions.OpenIddictConstants;

var builder = WebApplication.CreateBuilder(args);

//½¡¿µ¼ì²éÅäÖÃ
builder.Services.AddHealthCheckConfig(builder.Configuration);
//DBContextÅäÖÃ
builder.Services.AddDbContextConfig(builder.Configuration);
//LogÅäÖÃ
builder.Services.AddLogConfig(builder.Configuration);
//ÊÚÈ¨ÈÏÖ¤ÅäÖÃ
builder.Services.AddIdentityConfig(builder.Configuration);
//¿çÓòÅäÖÃ
builder.Services.AddCorsConfig();
//»ù´¡·þÎñ×¢Èë
builder.Services.AddServicesConfig();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateTimeOffsetJsonConverter());
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApplicationInsightsTelemetry();

//SwaggerÎÄµµÅäÖÃ
builder.Services.AddSwaggerConfig();
//ConsulÅäÖÃ
builder.Services.AddConsulConfig(builder.Configuration);
;

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
await using (var scope = app.Services.CreateAsyncScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    //await context.Database.EnsureCreatedAsync();

    await CreateApplicationsAsync();
    await CreateScopesAsync();

    async Task CreateApplicationsAsync()
    {
        var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

        if (await manager.FindByClientIdAsync("WebApi") is null)
        {
            await manager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = "WebApi",
                RedirectUris =
                {
                    new Uri("http://localhost:5059/")
                },
                Permissions =
                {
                    Permissions.Endpoints.Authorization,
                    Permissions.Endpoints.Token,
                    Permissions.GrantTypes.AuthorizationCode,
                    Permissions.ResponseTypes.Code,
                    Permissions.Scopes.Email,
                    Permissions.Scopes.Profile,
                    Permissions.Scopes.Roles,
                    Permissions.Prefixes.Scope + "api1",
                    Permissions.Prefixes.Scope + "api2"
                }
            });
        }

        if (await manager.FindByClientIdAsync("resource_server_1") is null)
        {
            await manager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = "resource_server_1",
                ClientSecret = "846B62D0-DEF9-4215-A99D-86E6B8DAB342",
                Permissions =
                {
                    Permissions.Endpoints.Introspection
                }
            });
        }

        // Note: no client registration is created for resource_server_2
        // as it uses local token validation instead of introspection.
    }

    async Task CreateScopesAsync()
    {
        var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictScopeManager>();

        if (await manager.FindByNameAsync("api1") is null)
        {
            await manager.CreateAsync(new OpenIddictScopeDescriptor
            {
                Name = "api1",
                Resources =
                {
                    "resource_server_1"
                }
            });
        }

        if (await manager.FindByNameAsync("api2") is null)
        {
            await manager.CreateAsync(new OpenIddictScopeDescriptor
            {
                Name = "api2",
                Resources =
                {
                    "resource_server_2"
                }
            });
        }
    }
}
app.UseAuthentication();
app.UseOpenIddictValidation();
app.UseAuthorization();

app.MapControllers();

app.Run();
