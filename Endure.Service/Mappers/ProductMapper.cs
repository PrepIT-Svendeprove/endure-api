using Endure.Data.Models;
using Endure.Dispatcher.RabbitMQ.EventMessage.Product;
using Endure.Service.Models.Dto.ProductDtos;

namespace Endure.Service.Mappers;

internal static class ProductMapper
{
    internal static Product MapToProduct(this CreateProductDto product)
    {
        return new Product
        {
            EAN = product.EAN,
            Name = product.Name,
            Description = product.Description
        };
    }

    internal static ProductDto MapToProductDto(this Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            EAN = product.EAN,
            Name = product.Name,
            Description = product.Description
        };
    }

    internal static Product MapToProduct(this ProductCreateEventMessage message)
    {
        return new Product
        {
            Id = message.Id,
            EAN = message.Ean,
            Name = message.Name,
            Description = message.Description,
            CreatedAt = message.CreatedAt,
            UpdatedAt = message.UpdatedAt,
            WarehouseId = message.WarehouseId
        };
    }

    internal static Product MapToProduct(this ProductUpdateEventMessage message)
    {
        return new Product
        {
            Id = message.Id,
            EAN = message.Ean,
            Name = message.Name,
            Description = message.Description,
            CreatedAt = message.CreatedAt,
            UpdatedAt = message.UpdatedAt,
            WarehouseId = message.WarehouseId
        };
    }

    internal static Product MapToProduct(this UpdateProductDto product)
    {
        return new Product
        {
            Id = product.Id,
            Name = product.Name,
            EAN = product.EAN,
            Description = product.Description
        };
    }

    internal static ProductUpdateEventMessage MapToProductUpdateEventMessage(this Product message)
    {
        return new ProductUpdateEventMessage
        {
            Id = message.Id,
            Ean = message.EAN,
            Name = message.Name,
            Description = message.Description,
            CreatedAt = message.CreatedAt,
            UpdatedAt = message.UpdatedAt,
            WarehouseId = message.WarehouseId
        };
    }

    internal static ProductCreateEventMessage MapToProductCreateEventMessage(this Product message)
    {
        return new ProductCreateEventMessage
        {
            Id = message.Id,
            Ean = message.EAN,
            Name = message.Name,
            Description = message.Description,
            CreatedAt = message.CreatedAt,
            UpdatedAt = message.UpdatedAt,
            WarehouseId = message.WarehouseId
        };
    }

    internal static IQueryable<ProductDto> MapToProductDto(this IQueryable<Product> query)
    {
        return query.Select(x => new ProductDto
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            EAN = x.EAN
        });
    }
}
