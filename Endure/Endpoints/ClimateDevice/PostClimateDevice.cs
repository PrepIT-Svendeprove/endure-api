using Endure.Service.Models.Dto.ClimateDeviceDtos;
using Endure.Service.Models.Enums;
using Endure.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Endure.Endpoints.ClimateDevice;

public class PostClimateDevice
{
    [EndpointName("CreateClimateDevice")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public static async Task<IResult> CreateClimateDeviceAsync(
            [FromServices] IClimateDeviceService climateDeviceService,
            [FromBody] CreateClimateDeviceDto device
        )
    {
        try
        {
            var result = await climateDeviceService.CreateClimateDevice(device);

            if (result.ServiceResult is ServiceResult.Success)
                return Results.NoContent();

            return Results.BadRequest();
        }
        catch
        {
            return Results.InternalServerError();
        }
    }
}
