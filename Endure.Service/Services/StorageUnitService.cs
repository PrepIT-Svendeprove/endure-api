using Endure.Data;
using Endure.Data.Models;
using Endure.Service.Mappers;
using Endure.Service.Models.Dto.StorageUnitDtos;
using Endure.Service.Models.Enums;
using Endure.Service.Models.Filters;
using Microsoft.EntityFrameworkCore;

namespace Endure.Service.Services;

internal sealed class StorageUnitService(DatabaseContext context, IWarehouseService warehouseService)
    : BaseService<StorageUnit>(context), IStorageUnitService
{
    private readonly IWarehouseService _warehouseService = warehouseService;

    public async Task<ServiceResult> CreateStorageUnitAsync(CreateStorageUnitDto entity)
    {
        // Ensure that the parent that is trying to be added, is actually eligible as a parent.
        if (entity.ParentStorageUnitId is Guid id && !await IsStorageUnitEligibleAsParentAsync(id))
            return ServiceResult.Failed;

        var rootId = await _warehouseService.GetRootWarehouseIdAsync();

        var mappedEntity = entity.MapToStorageUnit(rootId);

        await _context.AddAsync(mappedEntity);

        return await _context.SaveChangesAsync() > 0 ? ServiceResult.Success : ServiceResult.Failed;
    }

    public async Task<bool> HasSubStorageUnitsAsync(Guid id)
    {
        return await _context
                .StorageUnit
                .AnyAsync(x => x.ParentStorageUnitId == id && !x.IsDeleted);
    }

    public async Task<List<StorageUnitDto>> GetStorageUnitsByParentIdAsync(Guid id)
    {
        return await _context
                .StorageUnit
                .Where(x => x.ParentStorageUnitId == id && !x.IsDeleted && !x.ParentStorageUnit.IsDeleted)
                .MapToStorageUnitDto()
                .ToListAsync();
    }

    public async Task<StorageUnitDto?> GetStorageUnitByIdAsync(Guid id)
    {
        return await _context
                .StorageUnit
                .Where(x => x.Id == id && !x.IsDeleted)
                .MapToStorageUnitDto()
                .FirstOrDefaultAsync();
    }

    public async Task<List<StorageUnitDto>> GetPaginatedStorageUnitsAsync(StorageUnitPaginatedFilter filter)
    {
        var context = MakePaginatedQuery(filter)
            .OrderByDescending(x => x.Name)
            .ThenBy(x => x.ShortName);

        return await context
                .MapToStorageUnitDto()
                .ToListAsync();
    }

    private async Task<bool> IsStorageUnitEligibleAsParentAsync(Guid id)
    {
        return await _context
                .StorageUnit
                .Where(x => x.Id == id && !x.IsDeleted)
                .AnyAsync(x => !x.IsSlot);
    }
}

public interface IStorageUnitService : IBaseService
{
    /// <summary>
    /// Creates a new storage units, for the root warehouse.
    /// </summary>
    Task<ServiceResult> CreateStorageUnitAsync(CreateStorageUnitDto entity);
    Task<List<StorageUnitDto>> GetPaginatedStorageUnitsAsync(StorageUnitPaginatedFilter filter);
    Task<StorageUnitDto?> GetStorageUnitByIdAsync(Guid id);
    Task<List<StorageUnitDto>> GetStorageUnitsByParentIdAsync(Guid id);

    /// <summary>
    /// Checks if the <paramref name="id" /> is a parent for any non-deleted storage units.
    /// </summary>
    Task<bool> HasSubStorageUnitsAsync(Guid id);
}
