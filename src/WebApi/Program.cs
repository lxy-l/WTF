using System.Text.Json.Serialization;

using HealthChecks.UI.Client;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;

using WebApi.Config;
using WebApi.Extend;

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

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    //app.UseMigrationsEndPoint();
    //app.UseStatusCodePagesWithReExecute("/Error/{0}");

}
else if (app.Environment.IsStaging())
{

}
else if (app.Environment.IsProduction())
{
    //app.UseStatusCodePages();
    app.UseExceptionHandler("/Error");
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
