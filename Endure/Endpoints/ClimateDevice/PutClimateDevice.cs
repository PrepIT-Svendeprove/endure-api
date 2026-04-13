using Endure.Service.Models.Dto.ClimateDeviceDtos;
using Endure.Service.Models.Enums;
using Endure.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Endure.Endpoints.ClimateDevice;

public class PutClimateDevice
{
    [EndpointName("UpdateClimateDevice")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "The entity could not be updated.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public static async Task<IResult> UpdateClimateDeviceAsync(
            [FromServices] IClimateDeviceService climateDeviceService,
            [FromBody] UpdateClimateDeviceDto device
        )
    {
        try
        {
            var result = await climateDeviceService.UpdateClimateDevice(device);

            if (result.ServiceResult is ServiceResult.Success)
                return Results.NoContent();

            return Results.BadRequest();
        }
        catch(Exception ex)
        {
            return Results.InternalServerError();
        }
    }
}
