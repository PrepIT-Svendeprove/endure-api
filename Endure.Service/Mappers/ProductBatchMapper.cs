using Endure.Data.Models;
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
            Product = x.Product.MapToProductDto()
        });
    }

    internal static ProductBatch MapToProductBatch(this CreateProductBatchDto entity)
    {
        return new ProductBatch
        {
            BestBefore = entity.BestBefore,
            Count = entity.Count,
            ProductId = entity.ProductId,
            StorageUnitId = entity.StorageUnitId,
            WarehouseId = entity.WarehouseId
        };
    }
}
