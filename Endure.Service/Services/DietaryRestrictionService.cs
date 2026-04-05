using Endure.Data;

namespace Endure.Service.Services;

internal class DietaryRestrictionService(DatabaseContext context) : IDietaryRestrictionService
{
    private readonly DatabaseContext _context = context;

}

public interface IDietaryRestrictionService
{

}