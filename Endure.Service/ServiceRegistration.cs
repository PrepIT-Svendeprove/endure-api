using Endure.Data.Configuration;
using Endure.Service.Services;
using Endure.Service.Services.Dispatcher;
using Endure.Service.Services.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Endure.Service;

public static class ServiceRegistration
{
    public static IServiceCollection RegisterWorkerServices(this IServiceCollection services, IConfigurationManager config)
    {
        services.ConfigureDatabaseContext(config.GetConnectionString("DefaultConnection") ?? throw new NullReferenceException("Could not get DefaultConnection string."));

        services.AddScoped<IDispatcherClimateDeviceService, DispatcherClimateDeviceService>();
        services.AddScoped<IDispatcherAuditLogService, DispatcherAuditLogService>();
        services.AddScoped<IDispatcherProductBatchService, DispatcherProductBatchService>();
        services.AddScoped<IDispatcherProductService, DispatcherProductService>();
        services.AddScoped<IDispatcherWarehouseService, DispatcherWarehouseService>();
        services.AddScoped<IDispatcherStorageUnitService, DispatcherStorageUnitService>();

        services.AddScoped<IWarehouseService, WarehouseService>();

        return services;
    }

    /// <summary>
    /// Registers all of the services.
    /// </summary>
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfigurationManager config)
    {
        services.RegisterWorkerServices(config);

        services.AddScoped<IRequestContext, RequestContext>();

        services.AddScoped<IClimateDeviceService, ClimateDeviceService>();
        services.AddScoped<IProductBatchService, ProductBatchService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IStorageUnitService, StorageUnitService>();
        services.AddScoped<IAuditLogService, AuditLogService>();

        // Internal services
        services.AddScoped<IInternalProductBatchService, InternalProductBatchService>();

        return services;
    }
}
