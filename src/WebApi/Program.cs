using System.Text.Json.Serialization;

using HealthChecks.UI.Client;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;

using WebApi.Config;
using WebApi.Extend;

var builder = WebApplication.CreateBuilder(args);

//�����������
builder.Services.AddHealthCheckConfig(builder.Configuration);
//DBContext����
builder.Services.AddDbContextConfig(builder.Configuration);
//Log����
builder.Services.AddLogConfig(builder.Configuration);
//��Ȩ��֤����
builder.Services.AddIdentityConfig(builder.Configuration);
//��������
builder.Services.AddCorsConfig();
//��������ע��
builder.Services.AddServicesConfig();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateTimeOffsetJsonConverter());
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApplicationInsightsTelemetry();

//Swagger�ĵ�����
builder.Services.AddSwaggerConfig();
//Consul����
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
