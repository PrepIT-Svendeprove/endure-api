using Endure.Service.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Endure.Endpoints.DietaryRestriction;

public class GetDietaryRestriction
{

    [EndpointName("GetDietaryRestrictions")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Description = "Could not parse the parameter to CPR number, regex: [0-9]{10}.")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Description = "There was not found any entities with the CPR number.")]
    [ProducesResponseType(StatusCodes.Status200OK, Description = "Succesfully retrieved a list of dietaryrestriction.")]
    public static async Task<IResult> GetDietaryRestrictionByCprAsync(
            [FromServices] IDietaryRestrictionService dietaryRestrictionService,
            [FromRoute] string cpr
        )
    {
        if (!Regex.IsMatch(cpr, "^[0-9]{10}$"))
            return Results.UnprocessableEntity("The provided CPR number is not in a valid format. Regex: [0-9]{10}");

        try
        {
            var entities = await dietaryRestrictionService.GetDietaryRestrictionsByCpr(cpr);

            if (entities.Count <= 0)
                return Results.NoContent();

            return Results.Ok(entities);
        }
        catch
        {
            return Results.InternalServerError();
        }
    }
}
