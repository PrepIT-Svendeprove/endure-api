namespace Endure.Endpoints.Product;

public static class MapProductEndpoints
{
    public static RouteGroupBuilder MapProductApiRoutes(this RouteGroupBuilder route)
    {
        var group = route.MapGroup("/product").WithTags("Product");

        group.MapGet("/paginated", GetProduct.GetPaginatedProductsAsync);
        group.MapGet("/{id}", GetProduct.GetProductByIdAsync);

        group.MapPost("/", PostProduct.CreateProductAsync);

        group.MapPut("/", PutProduct.UpdateProductAsync);

        group.MapDelete("/{id}", DeleteProduct.DeleteProductAsync);

        return route;
    }
}
