using Endure.Data.Configuration;
using Endure.Endpoints;
using Endure.Middleware;
using Endure.Service;
using Endure.Service.Options;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddTransient<TraceMiddleware>();

builder.Services.Configure<CprHashingOptions>(builder.Configuration.GetSection(CprHashingOptions.SectionName));

builder.Services.RegisterServices();
builder.Services.ConfigureDatabaseContext(builder.Configuration.GetConnectionString("EndureDbConnection") ?? throw new NullReferenceException("Could not get EndureDbConnection string."));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.MapMinimalApiRoutes();

app.MapScalarApiReference("/api-docs", options =>
{
    options.WithTitle("Endure API Docs")
            .WithTheme(ScalarTheme.Moon)
            .WithClassicLayout()
            .ForceLightMode();
});

app.UseHttpsRedirection();

app.Run();
