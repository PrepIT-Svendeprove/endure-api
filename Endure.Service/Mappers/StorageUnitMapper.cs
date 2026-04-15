using Endure.Data.Models;
using Endure.Dispatcher.RabbitMQ.EventMessage.StorageUnit;
using Endure.Service.Models.Dto.StorageUnitDtos;
using Endure.Service.Models.Dto.StorageUnitDtose;

namespace Endure.Service.Mappers;

internal static class StorageUnitMapper
{
    internal static StorageUnit MapToStorageUnit(this CreateStorageUnitDto entity, Guid warehouseId)
    {
        return new StorageUnit
        {
            Name = entity.Name,
            ShortName = entity.ShortName,
            Description = entity.Description,
            ParentId = entity.ParentId,
            StorageType = entity.StorageType,
            IsSlot = entity.IsSlot,
            WarehouseId = warehouseId
        };
    }

    public static StorageUnit MapToStorageUnit(this StorageUnitCreatedEventMessage entity)
    {
        return new StorageUnit
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
        };
    }

    public static StorageUnit MapToStorageUnit(this UpdateStorageUnitDto entity)
    {
        return new StorageUnit
        {
            Id = entity.Id,
            Name = entity.Name,
            ShortName = entity.ShortName,
            Description = entity.Description,
            ParentId = entity.ParentId,
            StorageType = entity.StorageType,
            IsSlot = entity.IsSlot
        };
    }

    public static StorageUnit MapToStorageUnit(this StorageUnitUpdatedEventMessage entity)
    {
        return new StorageUnit
        {
            Id = entity.Id,
            Name = entity.Name,
            ShortName = entity.ShortName,
            Description = entity.Description,
            ParentId = entity.ParentId,
            WarehouseId = entity.WarehouseId,
            StorageType = entity.StorageType,
            IsSlot = entity.IsSlot,
            UpdatedAt = entity.UpdatedAt,
            CreatedAt = entity.CreatedAt,
        };
    }

    internal static IQueryable<SelectStorageUnitDto> MapToSelectStorageDto(this IQueryable<StorageUnit> query)
    {
        return query.Select(x => new SelectStorageUnitDto
        {
            Id = x.Id,
            Name = x.Name
        });
    }

    internal static IQueryable<StorageUnitDto> MapToStorageUnitDto(this IQueryable<StorageUnit> query)
    {
        return query.Select(x => new StorageUnitDto
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            ShortName = x.ShortName,
            IsSlot = x.IsSlot,
            StorageType = x.StorageType,
            ParentId = x.ParentId,
            HasContent = x.Products.Any(x => !x.IsDeleted) || x.ChildStorageUnits.Any(x => !x.IsDeleted)
        });
    }
}
