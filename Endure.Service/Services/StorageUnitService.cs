using Endure.Data;
using Endure.Data.Models;
using Endure.Service.Mappers;
using Endure.Service.Models.Dto.StorageUnitDtos;
using Endure.Service.Models.Filters;
using Endure.Service.Models.Results;
using Endure.Service.Models.StatusCodes;
using Microsoft.EntityFrameworkCore;

namespace Endure.Service.Services;

internal sealed class StorageUnitService(DatabaseContext context, IWarehouseService warehouseService)
    : BaseService<StorageUnit>(context), IStorageUnitService
{
    private readonly IWarehouseService _warehouseService = warehouseService;

    public async Task<Result> CreateStorageUnitAsync(CreateStorageUnitDto entity)
    {
        // Ensure that the parent that is trying to be added, is actually eligible as a parent.
        if (entity.ParentStorageUnitId.HasValue && !await IsStorageUnitEligibleAsParentAsync(entity.ParentStorageUnitId.Value))
            return Result.Failed([StorageUnitStatusCodes.PARENT_NOT_ELIGIBLE]);

        var rootId = await _warehouseService.GetRootWarehouseIdAsync();

        var mappedEntity = entity.MapToStorageUnit(rootId);

        await _context.AddAsync(mappedEntity);

        return await _context.SaveChangesAsync() > 0 ? Result.Success() : Result.Failed([]);
    }

    public async Task<Result> UpdateStorageUnitAsync(UpdateStorageUnitDto entity)
    {
        var dbEntity = await _context.StorageUnit.FirstOrDefaultAsync(x => x.Id == entity.Id && !x.IsDeleted && x.Warehouse.IsRoot);

        if (dbEntity is null)
            return Result.Failed([StorageUnitStatusCodes.ENTITY_MISSING]);

        // If the storageunit has any sub- units, then they should not be allowed to update the slot value.
        if (dbEntity.IsSlot && !entity.IsSlot && await HasSubStorageUnitsAsync(entity.Id))
            return Result.Failed([StorageUnitStatusCodes.CONTAINING_SUBUNITS]);

        // Check if the ParentStorageId has changed, and if it has ensure that the parent is actually eligible as a parent.
        if (!dbEntity.ParentStorageUnitId.HasValue && entity.ParentStorageUnitId.HasValue && dbEntity.ParentStorageUnitId != entity.ParentStorageUnitId && !await IsStorageUnitEligibleAsParentAsync(entity.ParentStorageUnitId.Value))
            return Result.Failed([StorageUnitStatusCodes.PARENT_NOT_ELIGIBLE]);

        return await _context
                .StorageUnit
                .Where(x => x.Id == entity.Id && !x.IsDeleted)
                .ExecuteUpdateAsync(x => 
                    x.SetProperty(y => y.Name, entity.Name)
                    .SetProperty(y => y.ShortName, entity.ShortName)
                    .SetProperty(y => y.Description, entity.Description)
                    .SetProperty(y => y.ParentStorageUnitId, entity.ParentStorageUnitId)
                    .SetProperty(y => y.StorageType, entity.StorageType)
                    .SetProperty(y => y.IsSlot, entity.IsSlot)
                ) > 0 ? Result.Success() : Result.Failed([]);
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
                .Where(x => x.Id == id && !x.IsDeleted && x.Warehouse.IsRoot)
                .AnyAsync(x => !x.IsSlot);
    }
}

public interface IStorageUnitService : IBaseService
{
    /// <summary>
    /// Creates a new storage units, for the root warehouse.
    /// </summary>
    Task<Result> CreateStorageUnitAsync(CreateStorageUnitDto entity);
    Task<List<StorageUnitDto>> GetPaginatedStorageUnitsAsync(StorageUnitPaginatedFilter filter);
    Task<StorageUnitDto?> GetStorageUnitByIdAsync(Guid id);
    Task<List<StorageUnitDto>> GetStorageUnitsByParentIdAsync(Guid id);

    /// <summary>
    /// Checks if the <paramref name="id" /> is a parent for any non-deleted storage units.
    /// </summary>
    Task<bool> HasSubStorageUnitsAsync(Guid id);

    /// <summary>
    /// Updates a storage unit.
    /// </summary>
    Task<Result> UpdateStorageUnitAsync(UpdateStorageUnitDto entity);
}
