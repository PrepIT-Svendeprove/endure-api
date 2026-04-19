namespace Endure.Endpoints.ProductBatch;

public static class MapProductBatchEndpoints
{
    public static RouteGroupBuilder MapProductBatchApiRoutes(this RouteGroupBuilder route)
    {
        var group = route.MapGroup("/productbatch").WithTags("ProductBatch");

        group.MapGet("/paginatedbyproduct", GetProductBatch.GetPaginatedProductsByProductIdAsync);
        group.MapGet("/paginatedbystorage", GetProductBatch.GetPaginatedProductsByStorageUnitIdAsync);
        group.MapGet("/paginatedbywarehouse", GetProductBatch.GetPaginatedProductsByWarehouseIdAsync);
        group.MapGet("/{warehouseId}/count", GetProductBatch.GetProductBatchCountAsync);

        group.MapPost("/", PostProductBatch.CreateProductBatchAsync);

        group.MapDelete("/{id}", DeleteProductBatch.DeleteProductBatchAsync);

        return route;
    }
}
