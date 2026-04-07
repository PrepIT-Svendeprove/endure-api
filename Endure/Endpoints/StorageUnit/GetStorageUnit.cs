using Endure.Service.Models.Filters;
using Endure.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Endure.Endpoints.StorageUnit;

public class GetStorageUnit
{
    [EndpointName("GetStorageUnitsByParentId")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Description = "Could not parse the parameter to a guid.")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Description = "There was not found any storageunits with the parentid of the query parameter.")]
    [ProducesResponseType(StatusCodes.Status200OK, Description = "Succesfully retrieved a list of storageunits with the parentid of the query parameter.")]
    public static async Task<IResult> GetStorageUnitsByParentIdAsync(
            [FromServices] IStorageUnitService storageUnitService,
            [FromRoute] string id
        )
    {
        try
        {
            if (!Guid.TryParse(id, out Guid parsedId))
                return Results.UnprocessableEntity("Could not parse identifier to Guid.");

            var result = await storageUnitService.GetStorageUnitsByParentIdAsync(parsedId);

            if (result.Count <= 0)
                return Results.NoContent();

            return Results.Ok(result);
        }
        catch
        {
            return Results.InternalServerError();
        }
    }

    [EndpointName("GetPaginatedStorageUnits")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Description = "Could not parse the parameter to a guid.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public static async Task<IResult> GetPaginatedStorageUnitsAsync(
            [FromServices] IStorageUnitService storageUnitService,
            [AsParameters] StorageUnitPaginatedFilter id
        )
    {
        try
        {
            var result = await storageUnitService.GetPaginatedStorageUnitsAsync(id);

            if (result.Count <= 0)
                return Results.NoContent();

            return Results.Ok(result);
        }
        catch
        {
            return Results.InternalServerError();
        }
    }

    [EndpointName("GetStorageUnitById")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Description = "Could not parse the parameter to a guid.")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public static async Task<IResult> GetStorageUnitByIdAsync(
            [FromServices] IStorageUnitService storageUnitService,
            [FromRoute] string id
        )
    {
        try
        {
            if (!Guid.TryParse(id, out Guid parsedId))
                return Results.UnprocessableEntity("Could not parse identifier to Guid.");

            var result = await storageUnitService.GetStorageUnitByIdAsync(parsedId);

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
