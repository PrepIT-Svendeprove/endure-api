using Endure.Service.Models.Enums;
using Endure.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Endure.Endpoints.DietaryRestrictionType;

public class DeleteDietaryRestrictionType
{
    [EndpointName("DeleteDietaryRestrictionType")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Description = "Could not parse the parameter to a guid.")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public static async Task<IResult> DeleteDietaryRestrictionTypesAsync(
            [FromServices] IDietaryRestrictionTypeService dietaryRestrictionTypeService,
            [FromRoute] string id
        )
    {
        try
        {
            if (!Guid.TryParse(id, out Guid parsedId))
                return Results.UnprocessableEntity("Could not parse the identifier to a Guid.");

            var result = await dietaryRestrictionTypeService.SoftDeleteEntity(parsedId);

            if (result is ServiceResult.Success)
                return Results.NoContent();

            return Results.BadRequest();
        }
        catch
        {
            return Results.InternalServerError();
        }
    }
}
