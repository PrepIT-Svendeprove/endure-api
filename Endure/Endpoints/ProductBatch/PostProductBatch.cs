using Endure.Service.Models.Dto.ProductBatchDtos;
using Endure.Service.Models.Enums;
using Endure.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Endure.Endpoints.ProductBatch;

public class PostProductBatch
{
	[EndpointName("CreateProductBatch")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType<List<string>>(StatusCodes.Status400BadRequest, Description = "The entity were not created, returns a list of statuscodes that indicates what went wrong.")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Description = "The entity were created.")]
    public static async Task<IResult> CreateProductBatchAsync(
            [FromServices] IProductBatchService productBatchService,
            [FromBody] CreateProductBatchDto productBatch
        )
    {
		try
		{
			var result = await productBatchService.CreateProductBatch(productBatch);

			if (result is not { ServiceResult: ServiceResult.Success })
				return Results.BadRequest(result.StatusCodes);

			return Results.NoContent();
		}
		catch (Exception ex)
		{
			return Results.InternalServerError();
		}
    }
}
