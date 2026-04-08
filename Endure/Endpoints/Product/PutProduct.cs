using Endure.Service.Models.Dto.ProductDtos;
using Endure.Service.Models.Enums;
using Endure.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Endure.Endpoints.Product;

public class PutProduct
{
    [EndpointName("UpdateProduct")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType<List<string>>(StatusCodes.Status400BadRequest, Description = "The entity were not updated, returns a list of statuscodes that indicates what went wrong.")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Description = "The entity succesfully has been updated.")]
    public static async Task<IResult> UpdateProductAsync(
            [FromServices] IProductService productService,
            [FromBody] ProductDto product
        )
    {
        try
        {
            var result = await productService.UpdateProductAsync(product);

            if (result is not { ServiceResult: ServiceResult.Failed })
                return Results.BadRequest(result.StatusCodes);

            return Results.NoContent();
        }
        catch(Exception ex)
        {
            return Results.InternalServerError();
        }
    }
}
