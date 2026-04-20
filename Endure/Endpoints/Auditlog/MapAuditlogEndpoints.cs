namespace Endure.Endpoints.Auditlog;

public static class MapAuditlogEndpoints
{
    public static RouteGroupBuilder MapAuditlogApiRoutes(this RouteGroupBuilder route)
    {
        var group = route.MapGroup("/auditlog");
        group.WithTags("Auditlog");

        // GET routes
        group.MapGet("/{id}", GetAuditlog.GetAuditlogAsync);
        group.MapGet("/paginated", GetAuditlog.GetPaginatedAuditlogsAsync);

        return route;
    }
}
