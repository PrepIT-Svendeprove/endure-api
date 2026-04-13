using Endure.Service.Models.Filters;
using Endure.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Endure.Endpoints.ClimateDevice;

public class GetClimateDevice
{
    [EndpointName("GetPaginatedClimateDevices")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public static async Task<IResult> GetPaginatedClimateDevicesAsync(
            [FromServices] IClimateDeviceService climateDeviceService,
            [AsParameters] ClimateDevicePaginatedFilter filter
        )
    {
        try
        {
            return Results.Ok(await climateDeviceService.GetPaginatedClimateDevice(filter));
        }
        catch(Exception ex)
        {
            return Results.InternalServerError();
        }
    }
}
