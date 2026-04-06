using Endure.Service.Dto.AuditLog;
using Endure.Service.Filters;
using Endure.Service.Services;
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
    [EndpointDescription("Retrieves a auditlog by id.")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<AuditLogDto>(StatusCodes.Status200OK)]
    public static async Task<IResult> GetAuditlogAsync(
            [FromServices] IAuditLogService auditlogService, 
            [FromRoute] string id
        )
    {
        try
        {
            if (!Guid.TryParse(id, out Guid parsedId))
                return Results.BadRequest("Could not parse the identifier to a Guid.");

            var auditLog = await auditlogService.GetAuditLogAsync(parsedId);

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
    [EndpointDescription("Retrieves auditlogs as a paginated list.")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<List<AuditLogDto>>(StatusCodes.Status200OK)]
    public static async Task<IResult> GetPaginatedAuditlogsAsync(
            [FromServices] IAuditLogService auditlogService,
            [AsParameters] AuditlogPaginatedFilter filter
        )
    {
        try
        {
            var auditLogs = await auditlogService.GetPaginatedAuditLogAsync(filter);

            if (auditLogs.Count <= 0)
                return Results.NoContent();

            return Results.Ok();
        }
        catch
        {
            return Results.InternalServerError();
        }
    }
}
