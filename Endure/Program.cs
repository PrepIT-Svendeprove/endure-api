using Endure.Constants;
using Endure.Dispatcher.Mqtt;
using Endure.Dispatcher.RabbitMQ;
using Endure.Endpoints;
using Endure.Middleware;
using Endure.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var authConfigSection = builder.Configuration.GetSection("Auth");

        options.Authority = authConfigSection["Authority"] ?? throw new Exception("Missing Auth:Authority");
        options.Audience = authConfigSection["Audience"] ?? throw new Exception("Missing Auth:Audience");

        //if (builder.Environment.IsDevelopment())
            options.RequireHttpsMetadata = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            RoleClaimType = ClaimTypes.Role
        };
    });

// Register all of the policies
builder.Services.AddAuthorizationBuilder()
    .AddPolicy(PolicyConstants.CLIMATE_READ, x => x.RequireRole(PolicyConstants.CLIMATE_READ))
    .AddPolicy(PolicyConstants.CLIMATE_MODIFY, x => x.RequireRole(PolicyConstants.CLIMATE_MODIFY))
    .AddPolicy(PolicyConstants.STORAGEUNIT_READ, x => x.RequireRole(PolicyConstants.STORAGEUNIT_READ))
    .AddPolicy(PolicyConstants.STORAGEUNIT_MODIFY, x => x.RequireRole(PolicyConstants.STORAGEUNIT_MODIFY))
    .AddPolicy(PolicyConstants.AUDIT_READ, x => x.RequireRole(PolicyConstants.AUDIT_READ))
    .AddPolicy(PolicyConstants.WAREHOUSE_MODIFY, x => x.RequireRole(PolicyConstants.WAREHOUSE_MODIFY))
    .AddPolicy(PolicyConstants.WAREHOUSE_READ, x => x.RequireRole(PolicyConstants.WAREHOUSE_READ));

builder.Services.AddTransient<TraceMiddleware>();

builder.Services.RegisterRabbitMqPublisherExtensions(builder.Configuration);
builder.Services.RegisterMqttPublisherExtensions(builder.Configuration);
builder.Services.RegisterServices(builder.Configuration);

var app = builder.Build();

// Keep this for both development and product, as we will need it to demonstrate all of the endpoints available.
app.MapOpenApi();

app.UseMiddleware<TraceMiddleware>();

app.UseAuthentication()
   .UseAuthorization();

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
