using Endure.Data.Models;
using Endure.Dispatcher.RabbitMQ.EventMessage.ProductBatch;
using Endure.Service.Models.Dto.ProductBatchDtos;

namespace Endure.Service.Mappers;

internal static class ProductBatchMapper
{
    internal static IQueryable<ProductBatchDto> MapToProductBatchDto(this IQueryable<ProductBatch> query)
    {
        return query.Select(x => new ProductBatchDto
        {
            Id = x.Id,
            BestBeforeUtc = DateTimeOffset.FromUnixTimeSeconds(x.BestBefore),
            Count = x.Count,
            Product = x.Product.MapToProductDto(),
            StorageUnit = x.StorageUnit.MapToSelectStorageUnitDto()
        });
    }

    internal static IQueryable<Top10ProductBatchDto> MapTotop10ProductBatchDto(this IQueryable<ProductBatch> query)
    {
        return query.Select(x => new Top10ProductBatchDto
        {
            Id = x.Id,
            ProductEAN = x.Product.EAN,
            BestBeforeUtc = DateTimeOffset.FromUnixTimeSeconds(x.BestBefore),
            Count = x.Count
        });
    }

    internal static ProductBatch MapToProductBatch(this CreateProductBatchDto entity, Guid warehouseId)
    {
        return new ProductBatch
        {
            BestBefore = entity.BestBeforeUtc,
            Count = entity.Count,
            ProductId = entity.ProductId,
            StorageUnitId = entity.StorageUnitId,
            WarehouseId = warehouseId
        };
    }

    internal static ProductBatch MapToProductBatch(this ProductBatchCreatedEventMessage entity)
    {
        return new ProductBatch
        {
            Id = entity.Id,
            BestBefore = entity.BestBefore,
            Count = entity.Count,
            UpdatedAt = entity.UpdatedAt,
            CreatedAt = entity.CreatedAt,
            IsDeleted = false,
            StorageUnitId = entity.StorageUnitId,
            WarehouseId = entity.WarehouseId,
            ProductId = entity.ProductId,
        };
    }
}
