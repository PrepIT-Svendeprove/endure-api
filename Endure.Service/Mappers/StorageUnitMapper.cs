using Endure.Data.Models;
using Endure.Service.Models.Dto.StorageUnitDtos;

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
            ParentId = entity.ParentStorageUnitId,
            StorageType = entity.StorageType,
            IsSlot = entity.IsSlot,
            WarehouseId = warehouseId
        };
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
            ParentStorageUnitId= x.ParentId
        });
    }
}
