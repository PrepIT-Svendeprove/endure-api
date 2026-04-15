namespace Endure.Endpoints.ProductBatch;

public static class MapProductBatchEndpoints
{
    public static RouteGroupBuilder MapProductBatchApiRoutes(this RouteGroupBuilder route)
    {
        var group = route.MapGroup("/productbatch").WithTags("ProductBatch");

        group.MapGet("/{id}", GetProductBatch.GetProductBatchByIdAsync);
        group.MapGet("/paginatedbyproduct", GetProductBatch.GetPaginatedProductsByProductIdAsync);
        group.MapGet("/paginatedbystorage", GetProductBatch.GetPaginatedProductsByStorageUnitIdAsync);
        group.MapGet("/paginatedbywarehouse", GetProductBatch.GetPaginatedProductsByWarehouseIdAsync);
        group.MapGet("/{id}/top10", GetProductBatch.GetTop10ProductsAsync);

        group.MapPost("/", PostProductBatch.CreateProductBatchAsync);

        group.MapDelete("/{id}", DeleteProductBatch.DeleteProductBatchAsync);

        return route;
    }
}
