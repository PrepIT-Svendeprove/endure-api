namespace Endure.Endpoints.Product;

public static class MapProductEndpoints
{
    public static RouteGroupBuilder MapProductApiRoutes(this RouteGroupBuilder route)
    {
        var group = route.MapGroup("/product").WithTags("Product");

        group.MapGet("/{warehouseId}/paginated", GetProduct.GetPaginatedProductsAsync);
        group.MapGet("/available", GetProduct.GetAvailableProductsAsync);
        group.MapGet("/{warehouseId}/{productId}", GetProduct.GetProductByIdAsync);
        group.MapGet("/{warehouseId}/count", GetProduct.GetProductCountAsync);

        group.MapPost("/", PostProduct.CreateProductAsync);

        group.MapPut("/", PutProduct.UpdateProductAsync);

        group.MapDelete("/{id}", DeleteProduct.DeleteProductAsync);

        return route;
    }
}
