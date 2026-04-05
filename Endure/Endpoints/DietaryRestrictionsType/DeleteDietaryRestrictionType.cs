using Endure.Service.Enums;
using Endure.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Endure.Endpoints.DietaryRestrictionsType;

public class DeleteDietaryRestrictionType
{
    [EndpointName("DeleteDietaryRestrictionType")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public static async Task<IResult> DeleteDietaryRestrictionTypesAsync(
            IDietaryRestrictionTypeService dietaryRestrictionTypeService,
            [FromQuery] Guid id
        )
    {
        try
        {
            var result = await dietaryRestrictionTypeService.DeleteDietaryRestrictionTypeAsync(id);

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
