using Endure.Service.Models.Dto.WarehouseDtos;
using Endure.Service.Models.Enums;
using Endure.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Endure.Endpoints.Warehouse;

public class PutWarehouse
{
    [EndpointName("UpdateWarehouse")]
    [EndpointSummary("Updates a specific warehouse.")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType<List<string>>(StatusCodes.Status400BadRequest, Description = "The entity were not updated, returns a list of statuscodes that indicates what went wrong.")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Description = "The entity were sucessfully updated.")]
    public static async Task<IResult> UpdateWarehouseAsync(
            [FromServices] IWarehouseService warehouseService,
            [FromBody] UpdateWarehouseDto entity
        )
    {
        try
        {
            var result = await warehouseService.UpdateWarehouseAsync(entity);

            if (result is not { ServiceResult: ServiceResult.Success })
                return Results.BadRequest(result.StatusCodes);

            return Results.NoContent();
        }
        catch
        {
            return Results.InternalServerError();
        }
    }
}
