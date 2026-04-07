using Endure.Data;
using Endure.Data.Models;
using Endure.Service.Models.Enums;
using Endure.Service.Models.Filters;
using Microsoft.EntityFrameworkCore;

namespace Endure.Service.Services;

public abstract class BaseService<T>(DatabaseContext context) : IBaseService
    where T : BaseModel
{
    protected readonly DatabaseContext _context = context;

    protected IQueryable<T> MakePaginatedQuery(BasePaginatedFilter filter)
        => _context
            .Set<T>()
            .Take(filter.Take)
            .Skip((filter.Page <= 0 ? 0 : filter.Page - 1) * filter.Take)
            .Where(x => !x.IsDeleted);

    public async Task<ServiceResult> SoftDeleteEntity(Guid id)
    {
        var result = await _context
            .Set<T>()
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
