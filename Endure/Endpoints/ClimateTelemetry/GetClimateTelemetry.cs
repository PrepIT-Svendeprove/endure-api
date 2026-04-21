using Endure.Service.Models.Dto.ClimateTelemetry;
using Endure.Service.Models.Filters;
using Endure.Service.Models.Results;
using Endure.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Endure.Endpoints.ClimateTelemetry;

public class GetClimateTelemetry
{
    [EndpointName("GetDateRangeClimateTelemetry")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType<List<ClimateTelemetryDto>>(StatusCodes.Status200OK)]
    public static async Task<IResult> GetDateRangeClimateTelemetryAsync(
            [FromServices] IClimateTelemetryService climateTelemetryService,
            [AsParameters] ClimateTelemetryDateRangeFilter filter
        )
    {
        try
        {
            return Results.Ok(await climateTelemetryService.GetClimateTelemetryFromDateRange(filter));
        }
        catch (Exception ex)
        {
            return Results.InternalServerError(ex);
        }
    }
}
