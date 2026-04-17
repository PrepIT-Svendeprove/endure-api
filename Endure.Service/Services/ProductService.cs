using Endure.Data;
using Endure.Data.Models;
using Endure.Service.Mappers;
using Endure.Service.Models.Dto.ProductDtos;
using Endure.Service.Models.Enums;
using Endure.Service.Models.Filters;
using Endure.Service.Models.Results;
using Endure.Service.Models.StatusCodes;
using Endure.Service.Services.Dispatcher;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Endure.Service.Services;

internal class ProductService(
        IDispatcherProductService dispatcherProductService,
        IWarehouseService warehouseService,
        DatabaseContext context
    )
    : BaseService<Product>(context), IProductService
{
    private readonly IWarehouseService _warehouseService = warehouseService;
    private readonly IDispatcherProductService _dispatcherProductService = dispatcherProductService;

    protected override async Task<ServiceResult> SoftDeleteEntity(Guid id, Expression<Func<Product, bool>>? predicate = null)
    {
        // If the product id is being relied on, we should not be able to remove that product.
        if (await _context.ProductBatch.AnyAsync(x => x.ProductId == id && !x.IsDeleted))
            return ServiceResult.Failed;

        return await _dispatcherProductService.DeleteProductAsync(id);
    }

    public async Task<Result> CreateProductAsync(CreateProductDto entity)
    {
        // Check if there already exists a product with the same EAN.
        if (await _context.Product.AnyAsync(x => x.EAN == entity.EAN && !x.IsDeleted))
            return Result.Failed([ProductStatusCodes.EAN_EXISTS]);

        var warehouseRootId = await _warehouseService.GetRootWarehouseIdAsync();

        var mappedEntity = entity.MapToProduct();
        mappedEntity.WarehouseId = warehouseRootId;

        return await _dispatcherProductService.CreateProductAsync(mappedEntity) ? Result.Success() : Result.Failed([]);
    }

    public async Task<Result> UpdateProductAsync(UpdateProductDto entity)
    {
        var dbEntity = await _context.Product.FirstOrDefaultAsync(x => x.Id == entity.Id && !x.IsDeleted);

        if (dbEntity is null)
            return Result.Failed([ProductStatusCodes.ENTITY_MISSING]);

        var mappedEntity = entity.MapToProduct();
        mappedEntity.WarehouseId = dbEntity.WarehouseId;

        return await _dispatcherProductService.UpdateProductAsync(mappedEntity) ? Result.Success() : Result.Failed([]);
    }

    public async Task<PaginatedResult<ProductDto>> GetPaginatedProducts(ProductPaginatedFilter filter, Guid warehouseId)
    {
        var context = MakePaginatedQuery(filter)
            .OrderByDescending(x => x.Name)
            .ThenBy(x => x.EAN)
            .Where(x => x.WarehouseId == warehouseId);

        var maxPages = await context.CountAsync();

        return new PaginatedResult<ProductDto>(await context.MapToProductDto().ToListAsync(), maxPages);
    }

    public async Task<ProductDto?> GetProductById(Guid id)
    {
        return await _context
                .Product
                .Where(x => x.Id == id && !x.IsDeleted)
                .MapToProductDto()
                .FirstOrDefaultAsync();
    }

    public async Task<List<Top10ProductDto>> GetTop10ProductsInWarehouseId(Guid warehouseId)
    {
        return await _context
                .Product
                .OrderByDescending(x => x.ProductBatches.Count)
                .Where(x => x.WarehouseId == warehouseId)
                .Take(10)
                .MapToTop10ProductDto()
                .ToListAsync();
    }

    public async Task<List<SelectProductDto>> GetAvailableProductsAsync()
    {
        var rootWarehouseId = await _warehouseService.GetRootWarehouseIdAsync();

        return await _context
                .Product
                .OrderByDescending(x => x.Name)
                .MapToSelectProductDto()
                .ToListAsync();
    }
}

public interface IProductService : IBaseService
{
    /// <summary>
    /// Creates a new product.
    /// </summary>
    Task<Result> CreateProductAsync(CreateProductDto entity);

    /// <summary>
    /// Updates an exisiting product.
    /// </summary>
    Task<Result> UpdateProductAsync(UpdateProductDto entity);

    /// <summary>
    /// Retrieves a paginated list of products.
    /// </summary>
    Task<PaginatedResult<ProductDto>> GetPaginatedProducts(ProductPaginatedFilter filter, Guid warehouseId);

    /// <summary>
    /// Retrives a single product, by ID.
    /// </summary>
    Task<ProductDto?> GetProductById(Guid id);

    /// <summary>
    /// Retrieves the 10 products with the most product batches.
    /// </summary>
    /// <returns></returns>
    Task<List<Top10ProductDto>> GetTop10ProductsInWarehouseId(Guid warehouseId);
    Task<List<SelectProductDto>> GetAvailableProductsAsync();
}
