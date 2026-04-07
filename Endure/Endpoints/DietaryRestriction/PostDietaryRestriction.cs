using Endure.Service.Models.Dto.DietaryRestrictionDtos;
using Endure.Service.Models.Enums;
using Endure.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Endure.Endpoints.DietaryRestriction;

public class PostDietaryRestriction
{
    [EndpointName("CreateDietaryRestriction")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType<List<string>>(StatusCodes.Status400BadRequest, Description = "The entity could not be created, returns a list of statuscodes that indicates what went wrong.")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Description = "Succesfully created the dietaryrestriction.")]
    public static async Task<IResult> CreateDietaryRestrictionAsync(
            [FromServices] IDietaryRestrictionService dietaryRestrictionService,
            [FromBody] CreateDietaryRestrictionDto entity
        )
    {
        try
        {
            var result = await dietaryRestrictionService.CreateDietaryRestrictionAsync(entity);

            if (result is not { ServiceResult: ServiceResult.Success })
                return Results.BadRequest(result.StatusCodes);

            return Results.NoContent();
        }
        catch
        {
            return Results.InternalServerError();
        }
    }
}
