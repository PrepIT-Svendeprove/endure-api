using Endure.Service.Models.Dto.ProductDtos;
using Endure.Service.Models.Filters;
using Endure.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Endure.Endpoints.Product;

public class GetProduct
{
    [EndpointName("GetPaginatedProducts")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status204NoContent, Description = "No entities were found")]
    [ProducesResponseType<List<ProductDto>>(StatusCodes.Status200OK, Description = "Returns a list of entities")]
    public static async Task<IResult> GetPaginatedProductsAsync(
            [FromServices] IProductService productService,
            [AsParameters] ProductPaginatedFilter filter 
        )
    {
        try
        {
            var result = await productService.GetPaginatedProducts(filter);

            if (result.Count <= 0)
                return Results.NoContent();

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
}
