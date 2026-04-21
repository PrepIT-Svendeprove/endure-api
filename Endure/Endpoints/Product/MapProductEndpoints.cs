using Endure.Constants;

namespace Endure.Endpoints.Product;

public static class MapProductEndpoints
{
    public static RouteGroupBuilder MapProductApiRoutes(this RouteGroupBuilder route)
    {
        var group = route.MapGroup("/product").WithTags("Product");

        group.MapGet("/{warehouseId}/paginated", GetProduct.GetPaginatedProductsAsync).RequireAuthorization(PolicyConstants.WAREHOUSE_READ);
        group.MapGet("/available", GetProduct.GetAvailableProductsAsync).RequireAuthorization(PolicyConstants.WAREHOUSE_READ);
        group.MapGet("/{warehouseId}/{productId}", GetProduct.GetProductByIdAsync).RequireAuthorization(PolicyConstants.WAREHOUSE_READ);
        group.MapGet("/{warehouseId}/count", GetProduct.GetProductCountAsync).RequireAuthorization(PolicyConstants.WAREHOUSE_READ);

        group.MapPost("/", PostProduct.CreateProductAsync).RequireAuthorization(PolicyConstants.WAREHOUSE_MODIFY);

        group.MapPut("/", PutProduct.UpdateProductAsync).RequireAuthorization(PolicyConstants.WAREHOUSE_MODIFY);

        group.MapDelete("/{id}", DeleteProduct.DeleteProductAsync).RequireAuthorization(PolicyConstants.WAREHOUSE_MODIFY);

        return route;
    }
}
