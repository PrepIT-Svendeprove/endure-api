using Endure.Data.Models;
using Endure.Service.Dto.DietaryRestrictionTypeDtos;

namespace Endure.Service.Mappers;

internal static class DietaryRestrictionTypeMapper
{
    public static IQueryable<DietaryRestrictionTypeDto> MapToDietaryRestrictionDto(this IQueryable<DietaryRestrictionType> entity)
    {
        return entity.Select(x => new DietaryRestrictionTypeDto
        {
            Id = x.Id,
            Name = x.Name
        });
    }

    public static DietaryRestrictionType MapToDietaryRestrictionType(this CreateDietaryRestrictionTypeDto entity)
    {
        return new DietaryRestrictionType
        {
            Name = entity.Name,
            NormalizedName = entity.Name.ToUpper()
        };
    }

    public static DietaryRestrictionTypeDto MapToDietaryRestrictionTypeDto(this DietaryRestrictionType entity)
    {
        return new DietaryRestrictionTypeDto
        {
            Id = entity.Id,
            Name = entity.Name
        };
    }
}
