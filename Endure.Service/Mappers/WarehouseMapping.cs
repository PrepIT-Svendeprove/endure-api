using Endure.Data.Models;
using Endure.Service.Dto.Warehouse;

namespace Endure.Service.Mappers;

internal static class WarehouseMapping
{
    public static IQueryable<WarehouseDto> MapToWarehouseDto(this IQueryable<Warehouse> entity)
    {
        return entity.Select(warehouse => new WarehouseDto
        {
            Id = warehouse.Id,
            Name = warehouse.Name,
            CreatedAt = DateTimeOffset.FromUnixTimeSeconds(warehouse.CreatedAt),
            LastUpdatedAt = DateTimeOffset.FromUnixTimeSeconds(warehouse.UpdatedAt),
            IsRoot = warehouse.IsRoot,
            ParentWarehouseId = warehouse.ParentWarehouseId,
        });
    }

    public static Warehouse MapToWarehouse(this CreateWarehouseDto entity)
    {
        return new Warehouse
        {
            Name = entity.Name,
            ShortName = entity.ShortName
        };
    }

    public static WarehouseDto MapToWarehouseDto(this Warehouse entity)
    {
        return new WarehouseDto
        {
            Id = entity.Id,
            Name = entity.Name,
            ShortName = entity.ShortName,
            CreatedAt = DateTimeOffset.FromUnixTimeSeconds(entity.CreatedAt),
            IsRoot = entity.IsRoot,
            LastUpdatedAt = DateTimeOffset.FromUnixTimeSeconds(entity.UpdatedAt),
            ParentWarehouseId = entity.ParentWarehouseId
        };
    }

    public static Warehouse MapToWarehouse(this UpdateWarehouseDto entity)
    {
        return new Warehouse
        {
            Id = entity.Id,
            Name = entity.Name,
            ShortName = entity.ShortName,
            IsRoot = entity.IsRoot,
            ParentWarehouseId = entity.ParentWarehouseId
        };
    }
}
