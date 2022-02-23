using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks().AddCheck<ServiceHealth>("Health");
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        if (CustomSettings.AvailableAt > DateTime.Now)
            return Results.BadRequest();

        var forecast = Enumerable.Range(1, 5).Select(index =>
           new WeatherForecast
           (
               DateTime.Now.AddDays(index),
               Random.Shared.Next(-20, 55),
               summaries[Random.Shared.Next(summaries.Length)]
           ))
            .ToArray();
        return Results.Ok(forecast);
    })
.WithName("GetWeatherForecast");
app.MapHealthChecks("/ServiceStatus");
app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

internal class CustomSettings
{
    public static DateTime AvailableAt = DateTime.Now.AddSeconds(20);
}

public class ServiceHealth : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context=null, CancellationToken cancellationToken = new CancellationToken())
    {
        return Task.FromResult(CustomSettings.AvailableAt > DateTime.Now ? HealthCheckResult.Unhealthy() : HealthCheckResult.Healthy());
    }
}

