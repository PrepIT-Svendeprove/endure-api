using Endure.Constants;

namespace Endure.Endpoints.Warehouse;

/// <summary>
/// Groups all of the warehouse endpoints with the route prefix /warehouse.
/// </summary>
public static class MapWarehouseEndpoints
{
    public static RouteGroupBuilder MapWarehouseApiRoutes(this RouteGroupBuilder route)
    {
        var group = route.MapGroup("/warehouse");
        group.WithTags("Warehouse");

        group.MapGet("/{warehouseId}/subwarehouse/count", GetWarehouse.GetSubWarehouseCountByWarehouseIdAsync).RequireAuthorization(PolicyConstants.WAREHOUSE_READ);
        group.MapGet("/root", GetWarehouse.GetRootWarehouseAsync).RequireAuthorization(PolicyConstants.WAREHOUSE_READ);
        group.MapGet("/", GetWarehouse.GetAllWarehousesAsync).RequireAuthorization(PolicyConstants.WAREHOUSE_READ);

        group.MapPut("/", PutWarehouse.UpdateWarehouseAsync).RequireAuthorization(PolicyConstants.WAREHOUSE_MODIFY);

        group.MapDelete("/{id}", DeleteWarehouse.DeleteWarehouseAsync).RequireAuthorization(PolicyConstants.WAREHOUSE_MODIFY);

        return route;
    }
}
