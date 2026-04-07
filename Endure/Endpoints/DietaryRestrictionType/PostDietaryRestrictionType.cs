using Endure.Service.Models.Dto.DietaryRestrictionTypeDtos;
using Endure.Service.Models.Enums;
using Endure.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Endure.Endpoints.DietaryRestrictionType;

public class PostDietaryRestrictionType
{
    [EndpointName("CreateDietaryRestrictionType")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType<List<string>>(StatusCodes.Status400BadRequest, Description = "The entity could not be created, returns a list of statuscodes that indicates what went wrong.")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Description = "The entity were succesfully created.")]
    public static async Task<IResult> CreateDietaryRestrictionsTypeAsync(
            [FromServices] IDietaryRestrictionTypeService dietaryRestrictionTypeService,
            [FromBody] CreateDietaryRestrictionTypeDto entity
        )
    {
        try
        {
            var result = await dietaryRestrictionTypeService.CreateDietaryRestrictionTypeAsync(entity);

            if (result is not { ServiceResult: ServiceResult.Success})
                return Results.BadRequest(result.StatusCodes);

            return Results.NoContent();
        }
        catch
        {
            return Results.InternalServerError();
        }
    }
}
