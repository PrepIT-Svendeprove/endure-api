using Endure.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Endure.Endpoints.Warehouse;

public class DeleteWarehouse
{
    [EndpointName("DeleteWarehouse")]
    [EndpointDescription("Soft deletes a warehouse.")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public static async Task<IResult> DeleteWarehouseAsync(
            IWarehouseService warehouseService,
            [FromQuery] Guid id
        )
    {
        try
        {
            return await warehouseService.DeleteWarehouseAsync(id) ? Results.Ok() : Results.BadRequest();
        }
        catch
        {
            return Results.InternalServerError();
        }
    }
}
