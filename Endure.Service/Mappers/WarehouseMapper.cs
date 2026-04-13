using Endure.Data.Models;
using Endure.Dispatcher.RabbitMQ.EventMessage.Warehouse;
using Endure.Service.Models.Dto.WarehouseDtos;

namespace Endure.Service.Mappers;

internal static class WarehouseMapper
{
    public static IQueryable<WarehouseDto> MapToWarehouseDto(this IQueryable<Warehouse> entity)
    {
        return entity.Select(warehouse => new WarehouseDto
        {
            Id = warehouse.Id,
            Name = warehouse.Name,
            CreatedAt = DateTimeOffset.FromUnixTimeSeconds(warehouse.CreatedAt),
            UpdatedAt = DateTimeOffset.FromUnixTimeSeconds(warehouse.UpdatedAt),
            IsRoot = warehouse.IsRoot,
            ParentWarehouseId = warehouse.ParentId,
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

    public static Warehouse MapToWarehouse(this UpdateWarehouseDto entity)
    {
        return new Warehouse
        {
            Id = entity.Id,
            Name = entity.Name,
            ShortName = entity.ShortName,
            ParentId = entity.ParentId
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
            UpdatedAt = DateTimeOffset.FromUnixTimeSeconds(entity.UpdatedAt),
            ParentWarehouseId = entity.ParentId
        };
    }

    public static Warehouse MapToWarehouse(this WarehouseUpdatedEventMessage message)
    {
        return new Warehouse
        {
            Id = message.Id,
            Name = message.Name,
            ShortName = message.ShortName,
            CreatedAt = message.CreatedAt,
            UpdatedAt = message.UpdatedAt,
            ParentId = message.ParentId,
        };
    }
}
