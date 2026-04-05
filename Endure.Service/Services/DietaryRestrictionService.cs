using Endure.Data;
using Endure.Service.Dto.DietaryRestrictionDtos;
using Endure.Service.Enums;

namespace Endure.Service.Services;

internal class DietaryRestrictionService(DatabaseContext context) : IDietaryRestrictionService
{
    private readonly DatabaseContext _context = context;

    public async Task<ServiceResult> CreateDietaryRestrictionAsync(CreateDietaryRestrictionDto entity)
    {

    }
}

public interface IDietaryRestrictionService
{

}