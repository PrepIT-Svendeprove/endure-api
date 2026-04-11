using Endure.Service.Models.Dto.ProductDtos;
using Endure.Service.Models.Enums;
using Endure.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Endure.Endpoints.Product;

public class PostProduct
{
    [EndpointName("CreateProduct")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "The product were not created, returns a list of statuscodes that indicates what went wrong.")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Description = "The product were created.")]
    public static async Task<IResult> CreateProductAsync(
            [FromServices] IProductService productService,
            [FromBody] CreateProductDto product
        )
    {
        try
        {
            var result = await productService.CreateProductAsync(product);

            if (result is not { ServiceResult: ServiceResult.Success })
                return Results.BadRequest(result.StatusCodes);

            return Results.NoContent();
        }
        catch(Exception ex)
        {
            return Results.InternalServerError();
        }
    }
}
