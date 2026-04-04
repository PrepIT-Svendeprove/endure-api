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

        group.MapGet("/", GetWarehouse.GetSubWarehousesAsync);
        group.MapGet("/root", GetWarehouse.GetRootWarehouseAsync);
        group.MapGet("/{id}", GetWarehouse.GetAllWarehousesByIdAsync);
        group.MapGet("/{id}/subwarehouses", GetWarehouse.GetAllWarehousesByParentIdAsync);
        
        group.MapPost("/create", PostWarehouse.CreateWarehouseAsync);

        group.MapPut("/update", PutWarehouse.UpdateWarehouseAsync);

        group.MapDelete("/{id}", DeleteWarehouse.DeleteWarehouseAsync);

        return route;
    }
}
