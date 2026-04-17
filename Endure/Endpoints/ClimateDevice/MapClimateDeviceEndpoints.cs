namespace Endure.Endpoints.ClimateDevice;

public static class MapClimateDeviceEndpoints
{
    public static RouteGroupBuilder MapClimateDeviceApiRoutes(this RouteGroupBuilder route)
    {
        var group = route.MapGroup("/climatedevice").WithTags("ClimateDevice");

        group.MapGet("/select", GetClimateDevice.GetSelectClimateDevicesAsync);
        group.MapGet("/available", GetClimateDevice.GetAvailableClimateDeviceAsync);
        group.MapGet("/paginated", GetClimateDevice.GetPaginatedClimateDevicesAsync);
        group.MapGet("/{warehouseId}/count", GetClimateDevice.GetClimateDeviceCountAsync);
        group.MapGet("/{warehouseId}/{storageunitId}/storageunit", GetClimateDevice.GetClimateDevicesByStorageIdAsync);
        group.MapGet("/{warehouseId}/{climateDeviceId}", GetClimateDevice.GetClimateDeviceAsync);

        group.MapPost("/", PostClimateDevice.CreateClimateDeviceAsync);

        group.MapPut("/", PutClimateDevice.UpdateClimateDeviceAsync);
        group.MapPut("/{climateDeviceId}/updatestorageunit", PutClimateDevice.UpdateClimateDeviceStorageUnitAsync);

        group.MapDelete("/{id}", DeleteClimateDevice.DeleteClimateDeviceAsync);

        return route;
    }
}
