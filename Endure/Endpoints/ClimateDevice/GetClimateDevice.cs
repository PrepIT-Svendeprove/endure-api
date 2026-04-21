using Endure.Service.Models.Dto.ClimateDeviceDtos;
using Endure.Service.Models.Filters;
using Endure.Service.Models.Results;
using Endure.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Endure.Endpoints.ClimateDevice;

public class GetClimateDevice
{
    [EndpointName("GetPaginatedClimateDevices")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType<PaginatedResult<ClimateDeviceDto>>(StatusCodes.Status200OK)]
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

    [EndpointName("GetClimateDevicesCount")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Description = "The parameter could not be converted into a guid.")]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    public static async Task<IResult> GetClimateDeviceCountAsync(
            [FromServices] IClimateDeviceService climateDeviceService,
            [FromRoute] string warehouseId 
        )
    {
        try
        {
            if (!Guid.TryParse(warehouseId, out Guid parsedId))
                return Results.UnprocessableEntity();

            return Results.Ok(await climateDeviceService.GetClimateDeviceCountAsync(parsedId));
        }
        catch(Exception ex)
        {
            return Results.InternalServerError();
        }
    }

    [EndpointName("GetClimateDevicesByStorageUnit")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Description = "The parameter could not be converted into a guid.")]
    [ProducesResponseType<List<ClimateDeviceDto>>(StatusCodes.Status200OK)]
    public static async Task<IResult> GetClimateDevicesByStorageIdAsync(
            [FromServices] IClimateDeviceService climateDeviceService,
            [FromRoute] string warehouseId,
            [FromRoute] string storageUnitId
        )
    {
        try
        {
            if (!Guid.TryParse(warehouseId, out Guid parsedWarehouseId) || !Guid.TryParse(storageUnitId, out Guid parsedStorageUnitId))
                return Results.UnprocessableEntity();

            return Results.Ok(await climateDeviceService.GetClimateDevicesByStorageUnitId(parsedWarehouseId, parsedStorageUnitId));
        }
        catch(Exception ex)
        {
            return Results.InternalServerError();
        }
    }


    [EndpointName("GetAvailableClimateDevice")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType<List<ClimateDeviceDto>>(StatusCodes.Status200OK)]
    public static async Task<IResult> GetAvailableClimateDeviceAsync(
            [FromServices] IClimateDeviceService climateDeviceService
        )
    {
        try
        {
            return Results.Ok(await climateDeviceService.GetAvailableClimateDevices());
        }
        catch(Exception ex)
        {
            return Results.InternalServerError();
        }
    }

    [EndpointName("GetClimateDevice")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Description = "The parameter could not be converted into a guid.")]
    [ProducesResponseType<ClimateDeviceDto>(StatusCodes.Status200OK)]
    public static async Task<IResult> GetClimateDeviceAsync(
            [FromServices] IClimateDeviceService climateDeviceService,
            [FromRoute] string warehouseId,
            [FromRoute] string climateDeviceId
        )
    {
        try
        {
            if (!Guid.TryParse(warehouseId, out Guid parsedWarehouseId) || !Guid.TryParse(climateDeviceId, out Guid parsedClimateId))
                return Results.UnprocessableEntity();

            return Results.Ok(await climateDeviceService.GetClimateDeviceAsync(parsedWarehouseId, parsedClimateId));
        }
        catch(Exception ex)
        {
            return Results.InternalServerError();
        }
    }

    [EndpointName("GetSelectClimateDevices")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType<List<SelectClimateDeviceDto>>(StatusCodes.Status200OK)]
    public static async Task<IResult> GetSelectClimateDevicesAsync(
            [FromServices] IClimateDeviceService climateDeviceService
        )
    {
        try
        {
            return Results.Ok(await climateDeviceService.GetSelectClimateDeviceDtoAsync());
        }
        catch(Exception ex)
        {
            return Results.InternalServerError();
        }
    }
}
