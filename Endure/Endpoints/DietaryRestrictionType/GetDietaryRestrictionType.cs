using Endure.Service.Models.Filters;
using Endure.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Endure.Endpoints.DietaryRestrictionType;

public class GetDietaryRestrictionType
{
    [EndpointName("GetDietaryRestrictionTypes")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status204NoContent, Description = "There was not found any dietaryrestrictiontypes, with the current filter.")]
    [ProducesResponseType(StatusCodes.Status200OK, Description = "Succesfully retrieved a list of dietaryrestrictiontypes, according to the filter.")]
    public static async Task<IResult> GetDietaryRestrictionTypesAsync(
            [FromServices] IDietaryRestrictionTypeService dietaryRestrictionTypeService,
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
