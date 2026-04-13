using System.Threading.Tasks.Dataflow;

namespace Endure.Endpoints.ClimateDevice;

public static class MapClimateDeviceEndpoints
{
    public static RouteGroupBuilder MapClimateDeviceApiRoutes(this RouteGroupBuilder route)
    {
        var group = route.MapGroup("/climatedevice").WithTags("ClimateDevice");

        group.MapGet("/paginated", GetClimateDevice.GetPaginatedClimateDevicesAsync);

        group.MapPost("/", PostClimateDevice.CreateClimateDeviceAsync);

        group.MapPut("/", PutClimateDevice.UpdateClimateDeviceAsync);

        group.MapDelete("/{id}", DeleteClimateDevice.DeleteClimateDeviceAsync);

        return route;
    }
}
