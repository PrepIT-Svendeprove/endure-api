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
            [FromRoute] string id
        )
    {
        try
        {
            if (!Guid.TryParse(id, out Guid parsedId))
                return Results.UnprocessableEntity("Could not parse the identifier to a Guid.");

            var result = await productService.GetProductById(parsedId);

            if (result is null)
                return Results.BadRequest();

            return Results.Ok(result);
        }
        catch
        {
            return Results.InternalServerError();
        }
    }

    [EndpointName("GetTop10Products")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Description = "Could not parse the parameter to a guid.")]
    [ProducesResponseType<List<Top10ProductDto>>(StatusCodes.Status200OK)]
    public static async Task<IResult> GetTop10ProductsAsync(
            [FromServices] IProductService productService,
            [FromRoute] string id
        )
    {
        try
        {
            if (Guid.TryParse(id, out Guid parsedId))
                return Results.UnprocessableEntity();

            return Results.Ok(await productService.GetTop10ProductsInWarehouseId(parsedId));
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
