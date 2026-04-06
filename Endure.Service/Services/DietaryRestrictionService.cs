using Endure.Data;
using Endure.Data.Models;
using Endure.Service.Dto.DietaryRestrictionDtos;
using Endure.Service.Enums;
using Endure.Service.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Endure.Service.Services;

internal class DietaryRestrictionService(DatabaseContext context, ICprCryptoService cprCryptoService)
    : BaseService<DietaryRestriction>(context), IDietaryRestrictionService
{
    private readonly ICprCryptoService _cprCryptoService = cprCryptoService;

    public async Task<ServiceResult> CreateDietaryRestrictionAsync(CreateDietaryRestrictionDto entity)
    {
        try
        {
            entity.Cpr = _cprCryptoService.Hash(entity.Cpr);
            var mappedEntity = entity.MapToDietaryRestrition();

            await _context.AddAsync(mappedEntity);

            return await _context.SaveChangesAsync() > 0 ? ServiceResult.Success : ServiceResult.Failed;
        }
        catch
        {
            return ServiceResult.Failed;
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
}

public interface IDietaryRestrictionService : IBaseService
{
    Task<ServiceResult> CreateDietaryRestrictionAsync(CreateDietaryRestrictionDto entity);
    Task<List<DietaryRestrictionDto>> GetDietaryRestrictionsByCpr(string cpr);
}