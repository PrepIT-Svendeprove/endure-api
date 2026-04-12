using Endure.Data;
using Endure.Data.Models;
using Endure.Service.Models.Dto.ProductBatchDtos;
using Endure.Service.Models.Enums;
using Endure.Service.Models.Filters;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Endure.Service.Services;

public abstract class BaseService<T>(DatabaseContext context) : BaseService<T, Guid>(context)
    where T : BaseModel<Guid>;

public abstract class BaseService<T, TKey>(DatabaseContext context) : IBaseService
    where T : BaseModel<TKey>
    where TKey : notnull, IEquatable<TKey>
{
    protected readonly DatabaseContext _context = context;

    protected virtual IQueryable<T> MakePaginatedQuery(BasePaginatedFilter filter)
        => _context
            .Set<T>()
            .Take(filter.Take)
            .Skip((filter.Page <= 0 ? 0 : filter.Page - 1) * filter.Take)
            .Where(x => !x.IsDeleted);

    protected virtual async Task<ServiceResult> SoftDeleteEntity(TKey id, Expression<Func<T, bool>>? predicate = null)
    {
        var query = _context
            .Set<T>()
            .Where(x => x.Id == id && !x.IsDeleted);


        if (predicate is not null)
            query = query.Where(predicate);

        var result = await query
                        .ExecuteUpdateAsync(
                            x => x.SetProperty(y => y.IsDeleted, true)
                        );

        if (result > 0)
            return ServiceResult.Success;

        if (result == 0)
            return ServiceResult.NoChanges;

        return ServiceResult.Failed;
    }

    public async Task<ServiceResult> SoftDeleteEntity(TKey id)
        => await SoftDeleteEntity(id, null);

    public virtual async Task<bool> IsDeleted<TModel>(TKey id)
        where TModel : BaseModel<TKey>
        => !await _context.Set<TModel>().AnyAsync(x => x.Id == id && !x.IsDeleted);

    /// <summary>
    /// Checks if the warehouseId is a root warehouse, and if it has a parentId.
    /// </summary>
    protected async Task<bool> ShouldSynchronizeWithParent(Guid warehouseId)
    {
        return await _context.Warehouse.AnyAsync(x => x.Id == warehouseId && x.ParentId != null);
    }
}

public interface IBaseService
{
    Task<ServiceResult> SoftDeleteEntity(Guid id);
}
