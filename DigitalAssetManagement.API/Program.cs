using DigitalAssetManagement.API;
using DigitalAssetManagement.Infrastructure;
using DigitalAssetManagement.Infrastructure.Common;
using Hangfire;
var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAPI(builder.Configuration)
    .AddInfrastructure(builder.Configuration);

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

app.UseCors();

app.UseExceptionHandler();

app.UseHangfireDashboard("/dashboard");

app.Run();
