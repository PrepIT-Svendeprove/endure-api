using Endure.Constants;

namespace Endure.Endpoints.StorageUnit;

public static class MapStorageUnitEndpoints
{
    public static RouteGroupBuilder MapStorageUnitApiRoutes(this RouteGroupBuilder route)
    {
        var group = route.MapGroup("/storageunit");
        group.WithTags("StorageUnit");

        group.MapGet("/{warehouseId}/{id}/", GetStorageUnit.GetStorageUnitByIdAsync).RequireAuthorization(PolicyConstants.STORAGEUNIT_READ);
        group.MapGet("/paginated", GetStorageUnit.GetPaginatedStorageUnitsAsync).RequireAuthorization(PolicyConstants.STORAGEUNIT_READ);
        group.MapGet("/parent/{id}", GetStorageUnit.GetStorageUnitsByParentIdAsync).RequireAuthorization(PolicyConstants.STORAGEUNIT_READ);
        group.MapGet("/{warehouseId}/count", GetStorageUnit.GetStorageUnitCountAsync).RequireAuthorization(PolicyConstants.STORAGEUNIT_READ);
        group.MapGet("/{warehouseId}/select", GetStorageUnit.GetSelectStorageUnitAsync).RequireAuthorization(PolicyConstants.STORAGEUNIT_READ);


        group.MapPost("/", PostStorageUnit.CreateStorageUnitAsync).RequireAuthorization(PolicyConstants.STORAGEUNIT_MODIFY);

        group.MapPut("/", PutStorageUnit.UpdateStorageUnitAsync).RequireAuthorization(PolicyConstants.STORAGEUNIT_MODIFY);

        group.MapDelete("/{id}", DeleteStorageUnit.DeleteStorageUnitAsync).RequireAuthorization(PolicyConstants.STORAGEUNIT_MODIFY);

        return route;
    }
}
