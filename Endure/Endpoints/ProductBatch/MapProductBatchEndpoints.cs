using Endure.Constants;

namespace Endure.Endpoints.ProductBatch;

public static class MapProductBatchEndpoints
{
    public static RouteGroupBuilder MapProductBatchApiRoutes(this RouteGroupBuilder route)
    {
        var group = route.MapGroup("/productbatch").WithTags("ProductBatch");

        group.MapGet("/paginatedbyproduct", GetProductBatch.GetPaginatedProductsByProductIdAsync).RequireAuthorization(PolicyConstants.WAREHOUSE_READ);
        group.MapGet("/paginatedbystorage", GetProductBatch.GetPaginatedProductsByStorageUnitIdAsync).RequireAuthorization(PolicyConstants.WAREHOUSE_READ);
        group.MapGet("/paginatedbywarehouse", GetProductBatch.GetPaginatedProductsByWarehouseIdAsync).RequireAuthorization(PolicyConstants.WAREHOUSE_READ);
        group.MapGet("/{warehouseId}/count", GetProductBatch.GetProductBatchCountAsync).RequireAuthorization(PolicyConstants.WAREHOUSE_READ);

        group.MapPost("/", PostProductBatch.CreateProductBatchAsync).RequireAuthorization(PolicyConstants.WAREHOUSE_MODIFY);

        group.MapDelete("/{id}", DeleteProductBatch.DeleteProductBatchAsync).RequireAuthorization(PolicyConstants.WAREHOUSE_MODIFY);

        return route;
    }
}
