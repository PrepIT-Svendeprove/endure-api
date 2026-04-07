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
        group.MapGet("/{id}/fromparent", GetStorageUnit.GetStorageUnitsByParentIdAsync);

        group.MapDelete("/{id}", DeleteStorageUnit.DeleteStorageUnitAsync);

        return route;
    }
}
