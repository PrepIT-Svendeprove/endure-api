namespace Endure.Endpoints.ClimateDevice;

public static class MapClimateDeviceEndpoints
{
    public static RouteGroupBuilder MapClimateDeviceApiRoutes(this RouteGroupBuilder route)
    {
        var group = route.MapGroup("/climatedevice").WithTags("ClimateDevice");

        group.MapGet("/available", GetClimateDevice.GetAvailableClimateDeviceAsync);
        group.MapGet("/paginated", GetClimateDevice.GetPaginatedClimateDevicesAsync);
        group.MapGet("/{warehouseId}/count", GetClimateDevice.GetClimateDeviceCountAsync);
        group.MapGet("/{warehouseId}/{storageunitId}", GetClimateDevice.GetClimateDevicesByStorageIdAsync);

        group.MapPost("/", PostClimateDevice.CreateClimateDeviceAsync);

        group.MapPut("/", PutClimateDevice.UpdateClimateDeviceAsync);

        group.MapDelete("/{id}", DeleteClimateDevice.DeleteClimateDeviceAsync);

        return route;
    }
}
