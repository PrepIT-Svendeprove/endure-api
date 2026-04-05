using Endure.Service.Dto.DietaryRestrictionTypeDtos;
using Endure.Service.Enums;
using Endure.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Endure.Endpoints.DietaryRestrictionsType;

public class PostDietaryRestrictionType
{
    [EndpointName("CreateDietaryRestrictionType")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public static async Task<IResult> CreateDietaryRestrictionsTypeAsync(
            IDietaryRestrictionTypeService dietaryRestrictionTypeService,
            CreateDietaryRestrictionTypeDto entity
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
