using Endure.Data;
using Endure.Data.Models;
using Endure.Service.Mappers;
using Endure.Service.Models.Dto.DietaryRestrictionDtos;
using Endure.Service.Models.Enums;
using Endure.Service.Models.Results;
using Endure.Service.Models.StatusCodes;
using Microsoft.EntityFrameworkCore;

namespace Endure.Service.Services;

internal class DietaryRestrictionService(DatabaseContext context, ICprCryptoService cprCryptoService)
    : BaseService<DietaryRestriction>(context), IDietaryRestrictionService
{
    private readonly ICprCryptoService _cprCryptoService = cprCryptoService;

    public async Task<Result> CreateDietaryRestrictionAsync(CreateDietaryRestrictionDto entity)
    {
        try
        {
            entity.Cpr = _cprCryptoService.Hash(entity.Cpr);

            if (await HasDietaryRestrictionType(entity.Cpr, entity.DietaryRestrictionTypeId))
                return Result.Failed([DietaryRestrictionStatusCodes.ENTITY_ALREADY_EXISTS]);

            var mappedEntity = entity.MapToDietaryRestrition();

            await _context.AddAsync(mappedEntity);

            return await _context.SaveChangesAsync() > 0 ? Result.Success() : Result.Failed([]);
        }
        catch
        {
            return Result.Failed([]);
        }
    }

    public async Task<List<DietaryRestrictionDto>> GetDietaryRestrictionsByCpr(string cpr)
    {
        try
        {
            var hashedCpr = _cprCryptoService.Hash(cpr);

            return await _context
                .DietaryRestriction
                .Where(x => x.HashedCpr == hashedCpr && !x.IsDeleted)
                .MapToDietaryRestrictionDto()
                .ToListAsync();
        }
        catch
        {
            return [];
        }
    }

    private async Task<bool> HasDietaryRestrictionType(string hashedCpr, Guid dietaryRestrictionTypeId)
    {
        return await _context
                .DietaryRestriction
                .AnyAsync(x => x.DietaryRestrictionTypeId == dietaryRestrictionTypeId && x.HashedCpr == hashedCpr);
    }
}

public interface IDietaryRestrictionService : IBaseService
{
    Task<Result> CreateDietaryRestrictionAsync(CreateDietaryRestrictionDto entity);
    Task<List<DietaryRestrictionDto>> GetDietaryRestrictionsByCpr(string cpr);
}