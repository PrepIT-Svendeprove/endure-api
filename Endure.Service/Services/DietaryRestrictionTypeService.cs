using Endure.Data;
using Endure.Data.Models;
using Endure.Service.Dto.DietaryRestrictionTypeDtos;
using Endure.Service.Enums;
using Endure.Service.Filters;
using Endure.Service.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Endure.Service.Services;

internal class DietaryRestrictionTypeService(DatabaseContext context) 
    : BaseService<DietaryRestrictionType>(context), IDietaryRestrictionTypeService
{
    public async Task<ServiceResult> CreateDietaryRestrictionTypeAsync(CreateDietaryRestrictionTypeDto entity)
    {
        if (await _context.DietaryRestrictionType.AnyAsync(x => x.NormalizedName == entity.Name.ToUpper()))
            return ServiceResult.AlreadyExists;

        var mappedEntity = entity.MapToDietaryRestrictionType();

        await _context.AddAsync(mappedEntity);

        return await _context.SaveChangesAsync() > 0 ? ServiceResult.Success : ServiceResult.Failed;
    }

    public async Task<List<DietaryRestrictionTypeDto>> GetDietaryRestrictionTypesAsync(DietaryRestrictionTypeFilter filter)
    {
        var context = _context.DietaryRestrictionType
                            .OrderByDescending(x => x.UpdatedAt)
                            .ThenBy(x => x.CreatedAt)
                            .Take(filter.Take)
                            .Skip((filter.Page <= 0 ? 0 : filter.Page - 1) * filter.Take);

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
    Task<ServiceResult> CreateDietaryRestrictionTypeAsync(CreateDietaryRestrictionTypeDto entity);

    Task<List<DietaryRestrictionTypeDto>> GetDietaryRestrictionTypesAsync(DietaryRestrictionTypeFilter filter);
}
