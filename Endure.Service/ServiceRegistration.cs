using Endure.Service.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Endure.Service;

public static class ServiceRegistration
{
    /// <summary>
    /// Registers all of the services.
    /// </summary>
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IRequestContext, RequestContext>();

        services.AddScoped<IDietaryRestrictionTypeService, DietaryRestrictionTypeService>();
        services.AddScoped<IWarehouseService, WarehouseService>();
        services.AddScoped<IAuditLogService, AuditLogService>();

        return services;
    }
}
