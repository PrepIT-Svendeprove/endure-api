using Endure.Service.Models.Dto.ClimateTelemetry;
using Endure.Service.Models.Filters;
using Endure.Service.Models.Results;
using Endure.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Endure.Endpoints.ClimateTelemetry;

public class GetClimateTelemetry
{
    [EndpointName("GetPaginatedClimateTelemetry")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType<PaginatedResult<ClimateTelemetryDto>>(StatusCodes.Status200OK)]
    public static async Task<IResult> GetPaginatedClimateTelemetryAsync(
            [FromServices] IClimateTelemetryService climateTelemetryService,
            [AsParameters] ClimateTelemetryPaginatedFilter filter
        )
    {
        try
        {
            return Results.Ok(await climateTelemetryService.GetPaginatedClimateTelemetryAsync(filter));
        }
        catch (Exception ex)
        {
            return Results.InternalServerError(ex);
        }
    }
}
