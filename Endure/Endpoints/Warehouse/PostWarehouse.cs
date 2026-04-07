using Endure.Service.Dto.WarehouseDtos;
using Endure.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Endure.Endpoints.Warehouse;

public class PostWarehouse
{
    [EndpointName("CreateWarehoues")]
    [EndpointSummary("Creates a new warehouse, and creates it as the sub-warehouse of the current root warehouse.")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "The entity were not created.")]
    [ProducesResponseType<WarehouseDto>(StatusCodes.Status200OK, Description = "The entity were sucess fully created.")]
    public static async Task<IResult> CreateWarehouseAsync(
            [FromServices] IWarehouseService warehouseService,
            [FromBody] CreateWarehouseDto entity
        )
    {
        try
        {
            var result = await warehouseService.CreateWarehouseAsync(entity);

            if (result is null)
                return Results.BadRequest();

            return Results.Ok(result);
        }
        catch
        {
            return Results.InternalServerError();
        }
    }
}
