using Endure.Data.Models;
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
