using Endure.Service.Models.Enums;
using Endure.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Endure.Endpoints.Product;

public class DeleteProduct
{
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Description = "Could not parse the parameter to a guid.")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "The entity could not be removed, this could be because the product is being relied on by another one or more productbatches.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public static async Task<IResult> DeleteProductAsync(
            [FromServices] IProductService productService,
            [FromRoute] string id
        )
    {
        try
        {
            if (!Guid.TryParse(id, out Guid parsedId))
                return Results.UnprocessableEntity("Could not parse the identifier to a Guid.");

            var result = await productService.SoftDeleteEntity(parsedId);

            if (result is not ServiceResult.Success)
                return Results.BadRequest();

            return Results.NoContent();
        }
        catch (Exception ex)
        {
            return Results.InternalServerError();
        }
    }
}
