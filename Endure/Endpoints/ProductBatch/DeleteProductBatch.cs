using Endure.Service.Models.Enums;
using Endure.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Endure.Endpoints.ProductBatch;

public class DeleteProductBatch
{
	[EndpointName("DeleteProductBatch")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Description = "Could not parse the parameter to a guid.")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public static async Task<IResult> DeleteProductBatchAsync(
			[FromServices] IProductBatchService productBatchService,
			[FromRoute] string id
		)
    {
		try
		{
			if (!Guid.TryParse(id, out Guid parsedId))
				return Results.UnprocessableEntity("Could not parse the identifier to a Guid.");

			var result = await productBatchService.SoftDeleteEntity(parsedId);

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
