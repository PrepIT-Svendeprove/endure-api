using Endure.Endpoints.Auditlog;
using Endure.Endpoints.ClimateDevice;
using Endure.Endpoints.ClimateTelemetry;
using Endure.Endpoints.Product;
using Endure.Endpoints.ProductBatch;
using Endure.Endpoints.StorageUnit;
using Endure.Endpoints.Warehouse;
using Endure.Middleware;

namespace Endure.Endpoints;

public static class MinimalApiConfiguration
{
    public static WebApplication MapMinimalApiRoutes(this WebApplication app)
    {
        app.UseMiddleware<TraceMiddleware>();

        var appGroup = app.MapGroup("/api");

        appGroup.MapProductBatchApiRoutes();
        appGroup.MapClimateTelemetryApiRoutes();
        appGroup.MapProductApiRoutes();
        appGroup.MapClimateDeviceApiRoutes();
        appGroup.MapStorageUnitApiRoutes();
        appGroup.MapWarehouseApiRoutes();
        appGroup.MapAuditlogApiRoutes();

        return app;
    }
}
