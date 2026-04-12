using Endure.Data;
using Endure.Data.Models;
using Endure.Dispatcher.EventMessage.StorageUnit;
using Endure.Dispatcher.Publisher;
using Endure.Service.Mappers;
using Endure.Service.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace Endure.Service.Services.Dispatcher;

internal class DispatcherStorageUnitService(
        DatabaseContext context,
        IMessagePublisher messagePublisher,
        IWarehouseService warehouseService
    )
    : BaseDispatcherService<StorageUnit>(context, messagePublisher), IDispatcherStorageUnitService
{
    private readonly IWarehouseService _warehouseService = warehouseService;

    public async Task<bool> CreateStorageUnitAsync(StorageUnit entity)
    {
        // The entity already exists, so dont try to recreate it.
        if (await _context.StorageUnit.AnyAsync(x => x.Id == entity.Id && x.WarehouseId == entity.WarehouseId))
            return true;

        await _context.AddAsync(entity);

        var result = await _context.SaveChangesAsync() > 0;

        if (result)
            await SynchronizeWithParent(new StorageUnitCreatedEventMessage
            {
                Id = entity.Id,
                Name = entity.Name,
                ShortName = entity.ShortName,
                StorageType = entity.StorageType,
                Description = entity.Description,
                IsSlot = entity.IsSlot,
                ParentId = entity.ParentId,
                WarehouseId = entity.WarehouseId,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
            });

        return result;
    }

    public async Task<bool> CreateStorageUnitAsync(StorageUnitCreatedEventMessage message)
    {
        var mappedEntity = message.MapToStorageUnit();

        return await CreateStorageUnitAsync(mappedEntity);
    }

    public async Task<bool> UpdateStorageUnitAsync(StorageUnit entity)
    {
        var result = await _context
                .StorageUnit
                .Where(x => x.Id == entity.Id && x.WarehouseId == entity.WarehouseId)
                .ExecuteUpdateAsync(x =>
                    x.SetProperty(y => y.Name, entity.Name)
                     .SetProperty(y => y.ShortName, entity.ShortName)
                     .SetProperty(y => y.Description, entity.Description)
                     .SetProperty(y => y.ParentId, entity.ParentId)
                     .SetProperty(y => y.StorageType, entity.StorageType)
                     .SetProperty(y => y.IsSlot, entity.IsSlot)
                ) > 0;

        await SynchronizeWithParent(new StorageUnitUpdatedEventMessage
        {
            Id = entity.Id,
            Name = entity.Name,
            ShortName = entity.ShortName,
            Description = entity.Description,
            StorageType = entity.StorageType,
            ParentId = entity.ParentId,
            WarehouseId = entity.WarehouseId,
            IsSlot = entity.IsSlot,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
        });

        return result;
    }

    public async Task<bool> UpdateStorageUnitAsync(StorageUnitUpdatedEventMessage message)
    {
        var mappedEntity = message.MapToStorageUnit();

        return await UpdateStorageUnitAsync(mappedEntity);
    }

    public async Task<ServiceResult> DeleteStorageUnitAsync(Guid id)
    {
        var rootWarehouseId = await _warehouseService.GetRootWarehouseIdAsync();

        var entity = await _context.StorageUnit.FirstOrDefaultAsync(x => x.Id == id && x.WarehouseId == rootWarehouseId);

        if (entity is null)
            return ServiceResult.RelationNotFound;

        var result = await SoftDeleteEntity(id, x => x.WarehouseId == rootWarehouseId);

        if (result is ServiceResult.Success)
            await SynchronizeWithParent(
                new StorageUnitDeletedEventMessage
                {
                    Id = id,
                    WarehouseId = rootWarehouseId,
                    CreatedAt = entity.CreatedAt,
                    UpdatedAt = entity.UpdatedAt
                }
            );

        return result;
    }

    public async Task<bool> DeleteStorageUnitAsync(StorageUnitDeletedEventMessage message)
    {
        var result = await SoftDeleteEntity(message.Id, x => x.WarehouseId == message.WarehouseId) is ServiceResult.Success or ServiceResult.RelationNotFound or ServiceResult.NoChanges;

        if (result)
            await SynchronizeWithParent(message);

        return result;
    }
}

public interface IDispatcherStorageUnitService
{
    Task<bool> CreateStorageUnitAsync(StorageUnitCreatedEventMessage message);
    Task<bool> CreateStorageUnitAsync(StorageUnit entity);
    Task<ServiceResult> DeleteStorageUnitAsync(Guid id);
    Task<bool> DeleteStorageUnitAsync(StorageUnitDeletedEventMessage message);
    Task<bool> UpdateStorageUnitAsync(StorageUnitUpdatedEventMessage message);
    Task<bool> UpdateStorageUnitAsync(StorageUnit entity);
}
