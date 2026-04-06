using Endure.Service.Dto.DietaryRestrictionDtos;
using Endure.Service.Enums;
using Endure.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Endure.Endpoints.DietaryRestriction;

public class PostDietaryRestriction
{
    [EndpointName("CreateDietaryRestriction")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public static async Task<IResult> CreateDietaryRestrictionAsync(
            [FromServices] IDietaryRestrictionService dietaryRestrictionService,
            [FromBody] CreateDietaryRestrictionDto entity
        )
    {
        try
        {
            var result = await dietaryRestrictionService.CreateDietaryRestrictionAsync(entity);

            if (result is ServiceResult.Success)
                return Results.Ok();

            return Results.BadRequest();
        }
        catch
        {
            return Results.InternalServerError();
        }
    }
}
