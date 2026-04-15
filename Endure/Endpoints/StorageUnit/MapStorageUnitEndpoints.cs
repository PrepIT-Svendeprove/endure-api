namespace Endure.Endpoints.StorageUnit;

public static class MapStorageUnitEndpoints
{
    public static RouteGroupBuilder MapStorageUnitApiRoutes(this RouteGroupBuilder route)
    {
        var group = route.MapGroup("/storageunit");
        group.WithTags("StorageUnit");

        group.MapPost("/", PostStorageUnit.CreateStorageUnitAsync);

        group.MapPut("/", PutStorageUnit.UpdateStorageUnitAsync);

        group.MapGet("/{id}", GetStorageUnit.GetStorageUnitByIdAsync);
        group.MapGet("/paginated", GetStorageUnit.GetPaginatedStorageUnitsAsync);
        group.MapGet("/parent/{id}", GetStorageUnit.GetStorageUnitsByParentIdAsync);
        group.MapGet("/{warehouseId}/count", GetStorageUnit.GetStorageUnitCountAsync);
        group.MapGet("/{warehouseId}/select", GetStorageUnit.GetSelectStorageUnitAsync);

        group.MapDelete("/{id}", DeleteStorageUnit.DeleteStorageUnitAsync);

        return route;
    }
}
