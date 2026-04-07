using Endure.Service.Models.Enums;
using Endure.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Endure.Endpoints.DietaryRestriction;

public class DeleteDietaryRestriction
{
    [EndpointName("DeleteDietaryRestriction")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Description = "Could not parse the parameter to a guid.")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public static async Task<IResult> DeleteDietaryRestrictionAsync(
            [FromServices] IDietaryRestrictionService dietaryRestrictionService,
            [FromRoute] string id
        )
    {
        try
        {
            if (!Guid.TryParse(id, out Guid parsedId))
                return Results.UnprocessableEntity("Could not parse the identifier to a Guid.");

            var results = await dietaryRestrictionService.SoftDeleteEntity(parsedId);

            if (results is ServiceResult.Success)
                return Results.NoContent();

            return Results.BadRequest("");
        }
        catch
        {
            return Results.InternalServerError();
        }
    }
}
