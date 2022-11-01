using Domain.Repository;

using Infrastructure.Context;
using Infrastructure.Repository;
using Infrastructure.UnitOfWork;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContextPool<UserDbContext>(options =>
    options.UseSqlServer(connectionString)
);

builder.Services.AddDbContext<DbContext, UserDbContext>();

builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();

builder.Services.AddTransient(typeof(IRepositoryAsync<,>), typeof(RepositoryAsync<,>));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationInsightsTelemetry();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseStatusCodePagesWithReExecute("/Log/{0}");
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();
