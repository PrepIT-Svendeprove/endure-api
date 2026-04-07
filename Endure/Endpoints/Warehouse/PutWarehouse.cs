using Endure.Service.Dto.WarehouseDtos;
using Endure.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Endure.Endpoints.Warehouse;

public class PutWarehouse
{
    [EndpointName("UpdateWarehouse")]
    [EndpointSummary("Updates a specific warehouse.")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "The entity were not updated.")]
    [ProducesResponseType(StatusCodes.Status200OK, Description = "The entity were sucessfully updated.")]
    public static async Task<IResult> UpdateWarehouseAsync(
            [FromServices] IWarehouseService warehouseService,
            [FromBody] UpdateWarehouseDto entity
        )
    {
        try
        {
            var result = await warehouseService.UpdateWarehouseAsync(entity);

            return result ? Results.Ok() : Results.BadRequest();
        }
        catch
        {
            return Results.InternalServerError();
        }
    }
}
