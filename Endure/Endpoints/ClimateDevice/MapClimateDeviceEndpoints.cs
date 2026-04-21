using Endure.Constants;

namespace Endure.Endpoints.ClimateDevice;

public static class MapClimateDeviceEndpoints
{
    public static RouteGroupBuilder MapClimateDeviceApiRoutes(this RouteGroupBuilder route)
    {
        var group = route.MapGroup("/climatedevice").WithTags("ClimateDevice");

        group.MapGet("/select", GetClimateDevice.GetSelectClimateDevicesAsync).RequireAuthorization(PolicyConstants.CLIMATE_READ);
        group.MapGet("/paginated", GetClimateDevice.GetPaginatedClimateDevicesAsync).RequireAuthorization(PolicyConstants.CLIMATE_READ);
        group.MapGet("/{warehouseId}/count", GetClimateDevice.GetClimateDeviceCountAsync).RequireAuthorization(PolicyConstants.CLIMATE_READ);
        group.MapGet("/{warehouseId}/{storageunitId}/storageunit", GetClimateDevice.GetClimateDevicesByStorageIdAsync).RequireAuthorization(PolicyConstants.CLIMATE_READ);
        group.MapGet("/{warehouseId}/{climateDeviceId}", GetClimateDevice.GetClimateDeviceAsync).RequireAuthorization(PolicyConstants.CLIMATE_READ);

        group.MapPost("/", PostClimateDevice.CreateClimateDeviceAsync).RequireAuthorization(PolicyConstants.CLIMATE_MODIFY);

        group.MapPut("/", PutClimateDevice.UpdateClimateDeviceAsync).RequireAuthorization(PolicyConstants.CLIMATE_MODIFY);
        group.MapPut("/{climateDeviceId}/updatestorageunit", PutClimateDevice.UpdateClimateDeviceStorageUnitAsync).RequireAuthorization(PolicyConstants.CLIMATE_MODIFY);

        group.MapDelete("/{id}", DeleteClimateDevice.DeleteClimateDeviceAsync).RequireAuthorization(PolicyConstants.CLIMATE_MODIFY);

        return route;
    }
}
