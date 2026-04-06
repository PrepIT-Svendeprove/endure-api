using Endure.Endpoints.Auditlog;
using Endure.Endpoints.DietaryRestriction;
using Endure.Endpoints.DietaryRestrictionType;
using Endure.Endpoints.Warehouse;
using Endure.Middleware;

namespace Endure.Endpoints;

public static class MinimalApiConfiguration
{
    public static WebApplication MapMinimalApiRoutes(this WebApplication app)
    {
        app.UseMiddleware<TraceMiddleware>();

        var appGroup = app.MapGroup("/api");

        appGroup.MapDietaryRestrictionsTypeApiRoutes();
        appGroup.MapDietaryRestrictionsApiRoutes();
        appGroup.MapWarehouseApiRoutes();
        appGroup.MapAuditlogApiRoutes();

        return app;
    }
}
