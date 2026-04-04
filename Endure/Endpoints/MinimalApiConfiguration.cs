using Endure.Endpoints.Auditlog;
using Endure.Endpoints.Warehouse;
using Endure.Middleware;

namespace Endure.Endpoints;

public static class MinimalApiConfiguration
{
    public static WebApplication MapMinimalApiRoutes(this WebApplication app)
    {
        app.UseMiddleware<TraceMiddleware>();

        var appGroup = app.MapGroup("/api");

        appGroup.MapWarehouseApiRoutes();
        appGroup.MapAuditlogApiRoutes();

        return app;
    }
}
