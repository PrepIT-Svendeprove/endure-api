using Endure.Data;
using Endure.Data.Models;
using Endure.Service.Enums;
using Microsoft.EntityFrameworkCore;

namespace Endure.Service.Services;

public abstract class BaseService<T>(DatabaseContext context) : IBaseService
    where T : BaseModel
{
    protected readonly DatabaseContext _context = context;

    public async Task<ServiceResult> SoftDeleteEntity(Guid id)
    {
        var result = await _context
            .DietaryRestriction
            .Where(x => x.Id == id && !x.IsDeleted)
            .ExecuteUpdateAsync(
                x => x.SetProperty(y => y.IsDeleted, true)
            );

        if (result > 0)
            return ServiceResult.Success;

        return ServiceResult.Failed;
    }
}

public interface IBaseService
{
    Task<ServiceResult> SoftDeleteEntity(Guid id);
}
