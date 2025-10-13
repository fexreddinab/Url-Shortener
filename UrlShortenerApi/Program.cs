
using System;
using Microsoft.EntityFrameworkCore;
using Prometheus;
using UrlShortenerApi.Data;

// Add the correct namespace for IUrlShortenerService and UrlShortenerService
using UrlShortenerApi.Services;
using UrlShortenerApi.Services.Abstractions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connString = builder.Configuration.GetConnectionString("DefaultConnection") 
                 ?? "Host=postgres;Database=urlshortener;Username=admin;Password=password";
builder.Services.AddDbContext<UrlShortenerDbContext>(options =>
    options.UseNpgsql(connString));

builder.Services.AddScoped<IUrlShortenerService, UrlShortenerService>();

var app = builder.Build();

// Enable Swagger UI. In containerized/production environments the app may not be in
// Development mode, so allow enabling Swagger via the ENABLE_SWAGGER env var.
var enableSwagger = Environment.GetEnvironmentVariable("ENABLE_SWAGGER");
if (string.IsNullOrEmpty(enableSwagger) || enableSwagger.Equals("true", StringComparison.OrdinalIgnoreCase))
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UrlShortenerApi v1"));
}

app.MapMetrics("/metrics");

// Simple middleware to increment request counter
app.Use(async (context, next) =>
{
    try
    {
        await next();
        var status = context.Response?.StatusCode.ToString() ?? "200";
        UrlShortenerApi.MetricRegistry.ApiRequests.WithLabels(context.Request.Path, context.Request.Method, status).Inc();
    }
    catch
    {
        UrlShortenerApi.MetricRegistry.ApiRequests.WithLabels(context.Request.Path, context.Request.Method, "500").Inc();
        throw;
    }
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
