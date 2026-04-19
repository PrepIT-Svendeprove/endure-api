using Endure.Service.Models.Dto.ProductDtos;
using Endure.Service.Models.Filters;
using Endure.Service.Models.Results;
using Endure.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Endure.Endpoints.Product;

public class GetProduct
{
    [EndpointName("GetPaginatedProducts")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Description = "Could not parse the parameter to a guid.")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Description = "No entities were found")]
    [ProducesResponseType<PaginatedResult<ProductDto>>(StatusCodes.Status200OK, Description = "Returns a list of entities")]
    public static async Task<IResult> GetPaginatedProductsAsync(
            [FromServices] IProductService productService,
            [FromRoute] string warehouseId,
            [AsParameters] ProductPaginatedFilter filter 
        )
    {
        try
        {
            if (!Guid.TryParse(warehouseId, out Guid parsedId))
                return Results.InternalServerError();

            var result = await productService.GetPaginatedProducts(filter, parsedId);

            return Results.Ok(result);
        }
        catch(Exception ex)
        {
            return Results.InternalServerError();
        }
    }

    [EndpointName("GetProductById")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Description = "Could not parse the parameter to a guid.")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "The entity were not found.")]
    [ProducesResponseType<ProductDto>(StatusCodes.Status200OK, Description = "Succesfully found the requested entity.")]
    public static async Task<IResult> GetProductByIdAsync(
            [FromServices] IProductService productService,
            [FromRoute] string warehouseId,
            [FromRoute] string productId
        )
    {
        try
        {
            if (!Guid.TryParse(productId, out Guid parsedProductId) || !Guid.TryParse(warehouseId, out Guid parsedWarehouseId))
                return Results.UnprocessableEntity("Could not parse the identifier to a Guid.");

            var result = await productService.GetProductById(parsedProductId, parsedWarehouseId);

            if (result is null)
                return Results.BadRequest();

            return Results.Ok(result);
        }
        catch
        {
            return Results.InternalServerError();
        }
    }

    [EndpointName("GetProductCountInWarehouse")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    public static async Task<IResult> GetProductCountAsync(
            [FromServices] IProductService productService,
            [FromRoute] string warehouseId
        )
    {
        try
        {
            if (!Guid.TryParse(warehouseId, out Guid parsedWarehouseId))
                return Results.UnprocessableEntity();

            return Results.Ok(await productService.GetProductCountAsync(parsedWarehouseId));
        }
        catch(Exception ex) 
        {
            return Results.InternalServerError();
        }
    }

    [EndpointName("GetAvailableProducts")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType<List<SelectProductDto>>(StatusCodes.Status200OK)]
    public static async Task<IResult> GetAvailableProductsAsync(
            [FromServices] IProductService productService
        )
    {
        try
        {
            return Results.Ok(await productService.GetAvailableProductsAsync());
        }
        catch (Exception ex)
        {
            return Results.InternalServerError();
        }
    }
}
