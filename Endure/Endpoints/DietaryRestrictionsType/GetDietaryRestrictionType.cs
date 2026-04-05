using Endure.Service.Filters;
using Endure.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Endure.Endpoints.DietaryRestrictionsType;

public class GetDietaryRestrictionType
{
    [EndpointName("GetDietaryRestrictionTypes")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public static async Task<IResult> GetDietaryRestrictionTypesAsync(
            IDietaryRestrictionTypeService dietaryRestrictionTypeService,
            [AsParameters] DietaryRestrictionTypeFilter filter
        )
    {
        try
        {
            var dietaryRestrictionTypes = await dietaryRestrictionTypeService.GetDietaryRestrictionTypesAsync(filter);

            if (dietaryRestrictionTypes.Count > 0)
                return Results.Ok(dietaryRestrictionTypes);

            return Results.NoContent();
        }
        catch
        {
            return Results.InternalServerError();
        }
    }
}
