namespace Endure.Endpoints.Auditlog;

public static class MapAuditlogEndpoints
{


    public static RouteGroupBuilder MapAuditlogApiRoutes(this RouteGroupBuilder route)
    {
        var group = route.MapGroup("/auditlog");

        // GET routes
        group.MapGet("/", GetAuditlog.GetAuditlogAsync);
        group.MapGet("/paginated", GetAuditlog.GetPaginatedAuditlogsAsync);

        group.MapPost("/createauditlog", PostAuditlog.CreateAuditLogAsync);

        return route;
    }
}
