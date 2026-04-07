using Endure.Service.Models.Dto.DietaryRestrictionTypeDtos;
using Endure.Service.Models.Enums;
using Endure.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Endure.Endpoints.DietaryRestrictionType;

public class PostDietaryRestrictionType
{
    [EndpointName("CreateDietaryRestrictionType")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public static async Task<IResult> CreateDietaryRestrictionsTypeAsync(
            [FromServices] IDietaryRestrictionTypeService dietaryRestrictionTypeService,
            [FromBody] CreateDietaryRestrictionTypeDto entity
        )
    {
        try
        {
            var result = await dietaryRestrictionTypeService.CreateDietaryRestrictionTypeAsync(entity);

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
