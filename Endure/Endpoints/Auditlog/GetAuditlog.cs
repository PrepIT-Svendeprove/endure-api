using Endure.Service.Dto.AuditLog;
using Endure.Service.Filters;
using Endure.Service.Services.AuditLog;
using Microsoft.AspNetCore.Mvc;

namespace Endure.Endpoints.Auditlog;

/// <summary>
/// Contains all of the API methods that is used for GET requests for Auditlog.
/// </summary>
public class GetAuditlog
{
    /// <summary>
    /// Retrives a single auditlog.
    /// </summary>
    [EndpointName("GetAuditlog")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<AuditLogDto>(StatusCodes.Status200OK)]
    public static async Task<IResult> GetAuditlogAsync(
            IAuditLogService auditlogService, 
            [FromQuery] Guid id
        )
    {
        try
        {
            var auditLog = await auditlogService.GetAuditLogAsync(id);

            if (auditLog == null)
                return Results.NotFound();

            return Results.Ok(auditLog);
        }
        catch
        {
            return Results.InternalServerError();
        }
    }

    /// <summary>
    /// Retrives a paginated list of auditlogs, from both the root and sub-warehouses.
    /// </summary>
    [EndpointName("GetPaginatedAuditlogs")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType<List<AuditLogDto>>(StatusCodes.Status200OK)]
    public static async Task<IResult> GetPaginatedAuditlogsAsync(
            IAuditLogService auditlogService,
            [AsParameters] AuditlogPaginatedFilter filter
        )
    {
        try
        {
            return Results.Ok(await auditlogService.GetPaginatedAuditLogAsync(filter));
        }
        catch
        {
            return Results.InternalServerError();
        }
    }
}
