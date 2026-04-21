using Endure.Data;
using Endure.Data.Models;
using Endure.Service.Mappers;
using Endure.Service.Models.Dto.StorageUnitDtos;
using Endure.Service.Models.Dto.StorageUnitDtose;
using Endure.Service.Models.Enums;
using Endure.Service.Models.Filters;
using Endure.Service.Models.Results;
using Endure.Service.Models.StatusCodes;
using Endure.Service.Services.Dispatcher;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Endure.Service.Services;

internal sealed class StorageUnitService(
        IWarehouseService warehouseService,
        IDispatcherStorageUnitService dispatcherStorageUnitService,
        DatabaseContext context
    )
    : BaseService<StorageUnit>(context), IStorageUnitService
{
    private readonly IWarehouseService _warehouseService = warehouseService;
    private readonly IDispatcherStorageUnitService _dispatcherStorageUnitService = dispatcherStorageUnitService;

    protected override async Task<ServiceResult> SoftDeleteEntity(Guid id, Expression<Func<StorageUnit, bool>>? predicate = null)
    {
        if (await _context.ProductBatch.AnyAsync(x => x.StorageUnitId == id && !x.IsDeleted))
            return ServiceResult.Failed;

        return await _dispatcherStorageUnitService.DeleteStorageUnitAsync(id);
    }

    public async Task<Result> CreateStorageUnitAsync(CreateStorageUnitDto entity)
    {
        // Ensure that the parent that is trying to be added, is actually eligible as a parent.
        if (entity.ParentId.HasValue && !await IsStorageUnitEligibleAsParentAsync(entity.ParentId.Value))
            return Result.Failed([StorageUnitStatusCodes.PARENT_NOT_ELIGIBLE]);

        var rootId = await _warehouseService.GetRootWarehouseIdAsync();

        var mappedEntity = entity.MapToStorageUnit(rootId);
        mappedEntity.WarehouseId = rootId;

        return await _dispatcherStorageUnitService.CreateStorageUnitAsync(mappedEntity) ? Result.Success() : Result.Failed([]);
    }

    public async Task<Result> UpdateStorageUnitAsync(UpdateStorageUnitDto entity)
    {
        var dbEntity = await _context.StorageUnit.FirstOrDefaultAsync(x => x.Id == entity.Id && !x.IsDeleted && x.Warehouse.IsRoot);

        if (dbEntity is null)
            return Result.Failed([StorageUnitStatusCodes.ENTITY_MISSING]);

        // If the storageunit has any sub- units, then they should not be allowed to update the slot value.
        if (dbEntity.IsSlot && !entity.IsSlot && await HasSubStorageUnitsAsync(entity.Id))
            return Result.Failed([StorageUnitStatusCodes.CONTAINING_SUBUNITS]);

        // Check if the ParentId has changed, and if it has ensured that the parent is actually eligible as a parent.
        if (!dbEntity.ParentId.HasValue && entity.ParentId.HasValue && dbEntity.ParentId != entity.ParentId && !await IsStorageUnitEligibleAsParentAsync(entity.ParentId.Value))
            return Result.Failed([StorageUnitStatusCodes.PARENT_NOT_ELIGIBLE]);

        var mappedEntity = entity.MapToStorageUnit();
        mappedEntity.WarehouseId = await _warehouseService.GetRootWarehouseIdAsync();

        return await _dispatcherStorageUnitService.UpdateStorageUnitAsync(mappedEntity) ? Result.Success() : Result.Failed([]);
    }

    public async Task<bool> HasSubStorageUnitsAsync(Guid id)
    {
        return await _context
                .StorageUnit
                .AnyAsync(x => x.ParentId == id && !x.IsDeleted);
    }

    public async Task<List<StorageUnitDto>> GetStorageUnitsByParentIdAsync(Guid id)
    {
        return await _context
                .StorageUnit
                .Where(x => x.ParentId == id && !x.IsDeleted && !x.ParentStorageUnit.IsDeleted)
                .MapToStorageUnitDto()
                .ToListAsync();
    }

    public async Task<int> GetStorageUnitCountAsync(Guid warehouseId)
    {
        return await _context
                .StorageUnit
                .Where(x => x.WarehouseId == warehouseId && !x.IsDeleted)
                .CountAsync();
    }

    public async Task<StorageUnitDto?> GetStorageUnitByIdAsync(Guid id, Guid warehouseId)
    {
        return await _context
                .StorageUnit
                .Where(x => x.Id == id && x.WarehouseId == warehouseId && !x.IsDeleted)
                .MapToStorageUnitDto()
                .FirstOrDefaultAsync();
    }

    public async Task<PaginatedResult<StorageUnitDto>> GetPaginatedStorageUnitsAsync(StorageUnitPaginatedFilter filter)
    {
        var context = MakePaginatedQuery(filter)
            .OrderByDescending(x => x.Name)
            .ThenBy(x => x.ShortName)
            .Where(x => x.WarehouseId == filter.WarehouseId);

        var maxPages = (await context.CountAsync() / filter.Take) + 1;

        return new PaginatedResult<StorageUnitDto>(await context.MapToStorageUnitDto().ToListAsync(), maxPages);
    }

    private async Task<bool> IsStorageUnitEligibleAsParentAsync(Guid id)
    {
        return await _context
                .StorageUnit
                .Where(x => x.Id == id && !x.IsDeleted && x.Warehouse.IsRoot)
                .AnyAsync(x => !x.IsSlot);
    }

    public async Task<List<SelectStorageUnitDto>> GetSelectStorageUnitAsync(Guid warehouseId, bool allowSlot)
    {
        var context = _context
                .StorageUnit
                .Where(x => x.WarehouseId == warehouseId && !x.IsDeleted);

        if (!allowSlot)
            context = context.Where(x => !x.IsSlot);

        return await context
                .MapToSelectStorageDto()
                .ToListAsync();
    }
}

public interface IStorageUnitService : IBaseService
{
    /// <summary>
    /// Creates a new storage units, for the root warehouse.
    /// </summary>
    Task<Result> CreateStorageUnitAsync(CreateStorageUnitDto entity);
    Task<PaginatedResult<StorageUnitDto>> GetPaginatedStorageUnitsAsync(StorageUnitPaginatedFilter filter);
    Task<List<SelectStorageUnitDto>> GetSelectStorageUnitAsync(Guid warehouseId, bool allowSlot);
    Task<StorageUnitDto?> GetStorageUnitByIdAsync(Guid id, Guid warehouseId);
    Task<int> GetStorageUnitCountAsync(Guid warehouseId);
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
