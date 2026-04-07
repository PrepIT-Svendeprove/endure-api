using Endure.Service.Models.Enums;
using Endure.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Endure.Endpoints.Warehouse;

public class DeleteWarehouse
{
    [EndpointName("DeleteWarehouse")]
    [EndpointSummary("Soft deletes a warehouse.")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Description = "Could not parse the parameter to a guid.")]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable, Description = "Cannot delete the root warehouse.")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "The entity has not been deleted.")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Description = "The entity has succesfully been deleted")]
    public static async Task<IResult> DeleteWarehouseAsync(
            [FromServices] IWarehouseService warehouseService,
            [FromRoute] string id
        )
    {
        try
        {
            if (!Guid.TryParse(id, out Guid parsedId))
                return Results.UnprocessableEntity("Could not parse the identifier to a Guid.");

            var rootWarehouseId = await warehouseService.GetRootWarehouseIdAsync();

            if (rootWarehouseId == parsedId)
                return Results.StatusCode(StatusCodes.Status406NotAcceptable);

            var result = await warehouseService.SoftDeleteEntity(parsedId);

            if (result is ServiceResult.Success)
                return Results.NoContent();

            return Results.BadRequest();
        }
        catch
        {
            return Results.InternalServerError();
        }
    }
}
