using Endure.Service.Models.Dto.AuditLogDtos;
using Endure.Service.Models.Filters;
using Endure.Service.Models.Results;
using Endure.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Endure.Endpoints.Auditlog;

/// <summary>
/// Contains all of the API methods that is used for GET requests for Auditlog.
/// </summary>
public class GetAuditlog
{
    [EndpointName("GetAuditlog")]
    [EndpointDescription("Retrieves a auditlog by id.")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Description = "Could not parse the parameter to a guid.")]
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
                return Results.UnprocessableEntity("Could not parse the identifier to a Guid.");

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

    [EndpointName("GetPaginatedAuditlogs")]
    [EndpointDescription("Retrieves auditlogs as a paginated list.")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<PaginatedResult<AuditLogDto>>(StatusCodes.Status200OK)]
    public static async Task<IResult> GetPaginatedAuditlogsAsync(
            [FromServices] IAuditLogService auditlogService,
            [AsParameters] AuditlogPaginatedFilter filter
        )
    {
        try
        {
            var auditLogs = await auditlogService.GetPaginatedAuditLogAsync(filter);

            return Results.Ok();
        }
        catch
        {
            return Results.InternalServerError();
        }
    }
}
