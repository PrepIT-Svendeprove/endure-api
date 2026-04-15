using Endure.Dispatcher.Mqtt;
using Endure.Dispatcher.RabbitMQ;
using Endure.Endpoints;
using Endure.Middleware;
using Endure.Service;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddTransient<TraceMiddleware>();

builder.Services.RegisterRabbitMqPublisherExtensions(builder.Configuration);
builder.Services.RegisterMqttPublisherExtensions(builder.Configuration);
builder.Services.RegisterServices(builder.Configuration);

var app = builder.Build();

// Keep this for both development and product, as we will need it to demonstrate all of the endpoints available.
app.MapOpenApi();

app.MapMinimalApiRoutes();

app.MapScalarApiReference("/api-docs", options =>
{
    options.WithTitle("Endure API Docs")
            .WithTheme(ScalarTheme.Moon)
            .WithClassicLayout()
            .ForceLightMode();
});

if (builder.Environment.IsProduction())
    app.UseHttpsRedirection();

app.Run();
