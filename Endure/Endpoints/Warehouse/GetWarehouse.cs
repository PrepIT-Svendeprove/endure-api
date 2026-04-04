using Endure.Service.Dto.Warehouse;
using Endure.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Endure.Endpoints.Warehouse;

/// <summary>
/// Defines all of the Warehouse GET Endpoints.
/// </summary>
public class GetWarehouse
{
    [EndpointName("GetRootWarehouse")]
    [EndpointDescription("Retrieves the root warehouse.")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<WarehouseDto>(StatusCodes.Status200OK)]
    public static async Task<IResult> GetRootWarehouseAsync(IWarehouseService warehouseService)
    {
        try
        {
            var warehouse = await warehouseService.GetCurrentRootWarehouseAsync();

            if (warehouse is null)
                return Results.NotFound();

            return Results.Ok(warehouse);
        }
        catch
        {
            return Results.InternalServerError();
        }
    }

    [EndpointName("GetSubWarehouses")]
    [EndpointDescription("Retrieves all warehouses that is not the current root warehouse.")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status204NoContent, Description = "The request were sucessful, but there was not found any sub warehouse.")]
    [ProducesResponseType<List<WarehouseDto>>(StatusCodes.Status200OK)]
    public static async Task<IResult> GetSubWarehousesAsync(IWarehouseService warehouseService)
    {
        try
        {
            var warehouse = await warehouseService.GetAllSubWarehousesAsync();

            if (warehouse is { Count: <= 0 })
                return Results.NoContent();

            return Results.Ok(warehouse);
        }
        catch
        {
            return Results.InternalServerError();
        }
    }

    [EndpointName("GetWarehouseFromId")]
    [EndpointDescription("Retrieves a warehouse and subwarehouses")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status204NoContent, Description = "The request were sucessful, but there was not found any warehouse with the identifier.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public static async Task<IResult> GetAllWarehousesByIdAsync(
            IWarehouseService warehouseService,
            [FromQuery] Guid id
        )
    {
        try
        {
            var warehouses = await warehouseService.GetAllByIdAsync(id);

            if (warehouses.Count <= 0)
                return Results.NoContent();

            return Results.Ok(warehouses);
        }
        catch
        {
            return Results.InternalServerError();
        }
    }

    [EndpointName("GetWarehouses")]
    [EndpointDescription("Retrieves all warehouses with the parent id.")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status204NoContent, Description = "The request were sucessful, but there was not founda ny warehouses with the parent id.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public static async Task<IResult> GetAllWarehousesByParentIdAsync(
            IWarehouseService warehouseService,
            [FromQuery] Guid id
        )
    {
        try
        {
            var warehouses = await warehouseService.GetAllByParentIdAsync(id);

            if (warehouses.Count <= 0)
                return Results.NoContent();

            return Results.Ok(warehouses);
        }
        catch
        {
            return Results.InternalServerError();
        }
    }
}
