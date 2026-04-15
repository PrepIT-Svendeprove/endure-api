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

        group.MapGet("/subwarehouse", GetWarehouse.GetSubWarehousesAsync);
        group.MapGet("/{warehouseId}/subwarehouse/count", GetWarehouse.GetSubWarehouseCountByWarehouseIdAsync);
        group.MapGet("/root", GetWarehouse.GetRootWarehouseAsync);
        group.MapGet("/{id}", GetWarehouse.GetAllWarehousesByIdAsync);
        group.MapGet("/", GetWarehouse.GetAllWarehousesAsync);
        
        group.MapPost("/", PostWarehouse.CreateWarehouseAsync);

        group.MapPut("/", PutWarehouse.UpdateWarehouseAsync);

        group.MapDelete("/{id}", DeleteWarehouse.DeleteWarehouseAsync);

        return route;
    }
}
