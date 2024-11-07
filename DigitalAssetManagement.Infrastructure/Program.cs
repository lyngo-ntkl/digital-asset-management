using DigitalAssetManagement.Cache;
using DigitalAssetManagement.Infrastructure;
using DigitalAssetManagement.Infrastructure.Common;
using DigitalAssetManagement.Infrastructure.Hangfire;
using DigitalAssetManagement.Infrastructure.Mapper;
using DigitalAssetManagement.Infrastructure.PostgreSQL;
using DigitalAssetManagement.Infrastructure.RabbitMQ;
using DigitalAssetManagement.UseCases;
using Hangfire;
var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddUseCases();
builder.Services.AddAPI(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddAuthorizationPolicy();
builder.Services.AddHelper();
builder.Services.AddHangfireConfiguration(builder.Configuration);
builder.Services.AddRabbitMQ();
builder.Services.AddRedis(builder.Configuration);
builder.Services.AddMappers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("cors", opts =>
    {
        opts.WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseCors("cors");

app.UseExceptionHandler();

app.UseHangfireDashboard("/dashboard");

app.Run();
