using Endure.Service.Models.Dto.StorageUnitDtos;
using Endure.Service.Models.Enums;
using Endure.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Endure.Endpoints.StorageUnit;

public class PutStorageUnit
{

    [EndpointName("UpdateStorageUnit")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType<List<string>>(StatusCodes.Status400BadRequest, Description = "The entity could not be updated, returns a list of statuscodes that indicates what went wrong.")]
    [ProducesResponseType(StatusCodes.Status200OK, Description = "The entity has succesfully been updated.")]
    public static async Task<IResult> UpdateStorageUnitAsync(
            [FromServices] IStorageUnitService storageUnitService,
            [FromBody] UpdateStorageUnitDto entity
        )
    {
        try
        {
            var result = await storageUnitService.UpdateStorageUnitAsync(entity);

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
