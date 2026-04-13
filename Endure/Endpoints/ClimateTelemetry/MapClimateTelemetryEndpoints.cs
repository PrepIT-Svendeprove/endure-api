namespace Endure.Endpoints.ClimateTelemetry;

public static class MapClimateTelemetryEndpoints
{
    public static RouteGroupBuilder MapClimateTelemetryApiRoutes(this RouteGroupBuilder route)
    {
        var group = route.MapGroup("/climatetelemetry").WithTags("ClimateTelemetry");

        group.MapGet("/paginated", GetClimateTelemetry.GetPaginatedClimateTelemetryAsync);

        return route;
    }
}
