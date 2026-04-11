using Endure.Data.Configuration;
using Endure.Service.Services;
using Endure.Service.Services.Dispatcher;
using Endure.Service.Services.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Endure.Service;

public static class ServiceRegistration
{
    /// <summary>
    /// Registers all of the services.
    /// </summary>
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfigurationManager config)
    {
        services.ConfigureDatabaseContext(config.GetConnectionString("DefaultConnection") ?? throw new NullReferenceException("Could not get DefaultConnection string."));

        services.AddScoped<IRequestContext, RequestContext>();

        services.AddScoped<ICprCryptoService, CprCryptoService>();
        services.AddScoped<IDietaryRestrictionTypeService, DietaryRestrictionTypeService>();
        services.AddScoped<IDietaryRestrictionService, DietaryRestrictionService>();
        services.AddScoped<IProductBatchService, ProductBatchService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IStorageUnitService, StorageUnitService>();
        services.AddScoped<IWarehouseService, WarehouseService>();
        services.AddScoped<IAuditLogService, AuditLogService>();

        services.AddScoped<IDispatcherAuditLogService, DispatcherAuditLogService>();

        // Internal services
        services.AddScoped<IInternalProductBatchService, InternalProductBatchService>();

        return services;
    }
}
