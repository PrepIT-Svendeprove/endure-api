using Endure.Service.Models.Dto.AuditLogDtos;
using Endure.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Endure.Endpoints.Auditlog;

/// <summary>
/// Contains all of the API methods that is used for POST requests for Auditlog.
/// </summary>
public class PostAuditlog
{
    /// <summary>
    /// Creates a new auditlog
    /// </summary>
    [EndpointName("CreateAuditlog")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public static async Task<IResult> CreateAuditLogAsync(
            [FromServices] IAuditLogService auditLogService,
            [FromBody] CreateAuditlogDto auditlog
        )
    {
        try
        {
            if (await auditLogService.CreateAuditLogAsync(auditlog))
                return Results.Ok();

            return Results.BadRequest();
        }
        catch(Exception e)
        {
            return Results.InternalServerError();
        }
    }
}
