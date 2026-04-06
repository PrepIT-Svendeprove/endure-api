using Endure.Data.Models;
using Endure.Service.Dto.DietaryRestrictionDtos;

namespace Endure.Service.Mappers;

internal static class DietaryRestrictionMapper
{
    public static IQueryable<DietaryRestrictionDto> MapToDietaryRestrictionDto(this IQueryable<DietaryRestriction> entity)
    {
        return entity.Select(x => new DietaryRestrictionDto
        {
            Id = x.Id,
            Cpr = x.HashedCpr,
            DietaryRestrictionType = x.DietaryRestrictionType.MapToDietaryRestrictionTypeDto()
        });
    }

    public static DietaryRestriction MapToDietaryRestrition(this CreateDietaryRestrictionDto entity)
    {
        return new DietaryRestriction
        {
            HashedCpr = entity.Cpr,
            DietaryRestrictionTypeId = entity.DietaryRestrictionTypeId
        };
    }
}
