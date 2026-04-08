using Endure.Service.Services;
using Endure.Service.Services.Internal;
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

        services.AddScoped<ICprCryptoService, CprCryptoService>();
        services.AddScoped<IDietaryRestrictionTypeService, DietaryRestrictionTypeService>();
        services.AddScoped<IDietaryRestrictionService, DietaryRestrictionService>();
        services.AddScoped<IProductBatchService, ProductBatchService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IStorageUnitService, StorageUnitService>();
        services.AddScoped<IWarehouseService, WarehouseService>();
        services.AddScoped<IAuditLogService, AuditLogService>();

        // Internal services
        services.AddScoped<IInternalProductBatchService, InternalProductBatchService>();

        return services;
    }
}
