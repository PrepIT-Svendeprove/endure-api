using Endure.Data.Configuration;
using Endure.Service;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.RegisterServices();
builder.Services.ConfigureDatabaseContext(builder.Configuration.GetConnectionString("EndureDbConnection") ?? throw new NullReferenceException("Could not get EndureDbConnection string."));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
    app.MapOpenApi();


app.MapScalarApiReference("/api-docs");
app.UseHttpsRedirection();

app.Run();
