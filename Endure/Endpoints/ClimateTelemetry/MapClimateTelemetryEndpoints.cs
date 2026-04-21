using Endure.Constants;

namespace Endure.Endpoints.ClimateTelemetry;

public static class MapClimateTelemetryEndpoints
{
    public static RouteGroupBuilder MapClimateTelemetryApiRoutes(this RouteGroupBuilder route)
    {
        var group = route.MapGroup("/climatetelemetry").WithTags("ClimateTelemetry");

        group.MapGet("/range", GetClimateTelemetry.GetDateRangeClimateTelemetryAsync).RequireAuthorization(PolicyConstants.CLIMATE_READ);

        return route;
    }
}
