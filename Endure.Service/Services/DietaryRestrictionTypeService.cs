using Endure.Data;
using Endure.Data.Models;
using Endure.Service.Mappers;
using Endure.Service.Models.Dto.DietaryRestrictionTypeDtos;
using Endure.Service.Models.Enums;
using Endure.Service.Models.Filters;
using Endure.Service.Models.Results;
using Endure.Service.Models.StatusCodes;
using Microsoft.EntityFrameworkCore;

namespace Endure.Service.Services;

internal class DietaryRestrictionTypeService(DatabaseContext context) 
    : BaseService<DietaryRestrictionType>(context), IDietaryRestrictionTypeService
{
    public async Task<Result> CreateDietaryRestrictionTypeAsync(CreateDietaryRestrictionTypeDto entity)
    {
        if (await _context.DietaryRestrictionType.AnyAsync(x => x.NormalizedName == entity.Name.ToUpper()))
            return Result.Failed([DietaryRestrictionTypeStatusCodes.ENTITY_ALREADY_EXISTS]);

        var mappedEntity = entity.MapToDietaryRestrictionType();

        await _context.AddAsync(mappedEntity);

        return await _context.SaveChangesAsync() > 0 ? Result.Success() : Result.Failed([]);
    }

    public async Task<List<DietaryRestrictionTypeDto>> GetDietaryRestrictionTypesAsync(DietaryRestrictionTypeFilter filter)
    {
        var context = MakePaginatedQuery(filter)
                .OrderByDescending(x => x.UpdatedAt)
                .ThenBy(x => x.CreatedAt)
                .AsQueryable();

        if (!string.IsNullOrEmpty(filter.Name))
            context = context.Where(x => x.NormalizedName.Contains(filter.Name.ToUpper()));

        return await context
                .Where(x => !x.IsDeleted)
                .MapToDietaryRestrictionDto()
                .ToListAsync();
    }
}

public interface IDietaryRestrictionTypeService : IBaseService
{
    /// <summary>
    /// Creates a new dietaryrestriction type.
    /// </summary>
    Task<Result> CreateDietaryRestrictionTypeAsync(CreateDietaryRestrictionTypeDto entity);

    Task<List<DietaryRestrictionTypeDto>> GetDietaryRestrictionTypesAsync(DietaryRestrictionTypeFilter filter);
}
