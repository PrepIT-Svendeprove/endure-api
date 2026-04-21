using Endure.Constants;

namespace Endure.Endpoints.Auditlog;

public static class MapAuditlogEndpoints
{
    public static RouteGroupBuilder MapAuditlogApiRoutes(this RouteGroupBuilder route)
    {
        var group = route.MapGroup("/auditlog");
        group.WithTags("Auditlog");

        // GET routes
        group.MapGet("/{id}", GetAuditlog.GetAuditlogAsync).RequireAuthorization(PolicyConstants.AUDIT_READ);
        group.MapGet("/{warehouseId}/count", GetAuditlog.GetAuditLogCountAsync).RequireAuthorization(PolicyConstants.AUDIT_READ);
        group.MapGet("/paginated", GetAuditlog.GetPaginatedAuditlogsAsync).RequireAuthorization(PolicyConstants.AUDIT_READ);

        return route;
    }
}